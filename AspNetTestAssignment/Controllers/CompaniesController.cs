using AspNetTestAssignment.Constants;
using AspNetTestAssignment.DataBase;
using AspNetTestAssignment.Interfaces;
using AspNetTestAssignment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AspNetTestAssignment.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ILogger<CompaniesController> _logger;

        public CompaniesController(ILogger<CompaniesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult List()
        {
            return View(model: TableType.Companies.ToString());
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            Company? model;

            using (CompaniesContext context = new CompaniesContext())
            {
                model = context.Companies.FirstOrDefault(c => c.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
            }

            return model == null ? NotFound() : View(model);
        }

        [HttpGet]
        public IActionResult GetColumns(TableType tableType)
        {
            return Json(TableTypes.Columns[tableType]);
        }

        [HttpGet]
        public IActionResult GetCompanies()
        {
            List<Company> companies;
            using (CompaniesContext context = new CompaniesContext())
            {
                companies = context.Companies.ToList();
            }

            return Json(companies?.Count > 0 ? companies : false);
        }

        [HttpGet]
        public IActionResult GetCompanyHistory(string id)
        {
            List<History>? model;
            using (CompaniesContext context = new CompaniesContext())
            {
                model = context.CompanyHistories.Where(ch => ch.CompanyId.Equals(id, StringComparison.OrdinalIgnoreCase))?.ToList();
            }

            return Json(model?.Count > 0 ? model : false);
        }

        [HttpGet]
        public IActionResult GetNotes(string id)
        {
            List<Note>? model;
            using (CompaniesContext context = new CompaniesContext())
            {
                model = context.Notes.Where(n => n.CompanyId.Equals(id, StringComparison.OrdinalIgnoreCase))?.ToList();
            }

            return Json(model?.Count > 0 ? model : false);
        }

        [HttpGet]
        public IActionResult GetEmployees(string id)
        {
            List<Employee>? model = null;

            try
            {
                using (CompaniesContext context = new CompaniesContext())
                {
                    model = context.Employees.Where(e => e.CompanyId.Equals(id, StringComparison.OrdinalIgnoreCase))?.ToList();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Json(model?.Count > 0 ? model : false);
        }


        [HttpGet]
        public IActionResult AddCompanies()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult AddCompanies(Company model)
        {
            if (!model.IsValid())
            {
                return Json(model.Errors);
            }

            using (CompaniesContext context = new CompaniesContext())
            {
                context.Companies.Add(model);
                context.SaveChanges();
            }

            return Json(true);
        }

        [HttpGet]
        public IActionResult AddNotes(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Json("Id is null");
            }

            Note model;

            using (CompaniesContext context = new CompaniesContext())
            {
                Company? company = context.Companies.FirstOrDefault(c => c.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

                if (company == null)
                {
                    return Json("Company not found");
                }

                company.Employees = context.Employees.Where(e => e.CompanyId.Equals(company.Id, StringComparison.OrdinalIgnoreCase))?.ToList() ?? new List<Employee>();
                company.Notes = context.Notes.Where(n => n.CompanyId.Equals(company.Id)).ToList();
                int invoiceNumber = company.Notes.Count() > 0 ? company.Notes.Max(n => n.InvoiceNumber) + 1 : 1;
                model = new Note(invoiceNumber, company);
            }

            if (model == null)
            {
                return Json("Failed to create new note");
            }

            return PartialView(model);
        }

        [HttpPost]
        public IActionResult AddNotes(Note model)
        {
            if (!model.IsValid())
            {
                return Json(model.Errors);
            }

            using (CompaniesContext context = new CompaniesContext())
            {
                Company? company = context.Companies.FirstOrDefault(c => c.Id.Equals(model.CompanyId, StringComparison.OrdinalIgnoreCase));

                if (company == null)
                {
                    model.Errors.Add(string.Empty, "Company not found");
                    return Json(model.Errors);
                }

                model.Employee = context.Employees.FirstOrDefault(e => e.Id.Equals(model.EmployeeId, StringComparison.OrdinalIgnoreCase));

                if (model.Employee == null)
                {
                    model.Errors.Add(string.Empty, "Employee not found");
                    return Json(model.Errors);
                }

                model.EmployeeName = model.Employee.FullName;
                company.Notes?.Add(model);
                context.Notes?.Add(model);
                context.SaveChanges();
            }

            return Json(true);
        }

        [HttpGet]
        public IActionResult AddEmployees(string id)
        {
            Employee model = new Employee();

            using (CompaniesContext context = new CompaniesContext())
            {
                Company? company = context.Companies.FirstOrDefault(c => c.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

                if (company == null)
                {
                    model.Errors.Add(string.Empty, "Company not found");
                    return Json(model.Errors);
                }

                model.CompanyId = company.Id;
            }

            ViewBag.IsEdit = false;
            ViewBag.IsAdd = true;
            return PartialView(model);
        }

        [HttpPost]
        public IActionResult AddEmployees(Employee model)
        {
            if (!model.IsValid())
            {
                return Json(model.Errors);
            }

            using (CompaniesContext context = new CompaniesContext())
            {
                Company? company = context.Companies.FirstOrDefault(c => c.Id.Equals(model.CompanyId, StringComparison.OrdinalIgnoreCase));

                if (company == null)
                {
                    model.Errors.Add(string.Empty, "Company not found");
                    return Json(model.Errors);
                }

                company.Employees.Add(model);
                context.Employees.Add(model);
                context.SaveChanges();
            }

            return Json(true);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult _EmployeeSummary(string id, bool isEdit = false)
        {
            Employee? model = null;

            using (CompaniesContext context = new CompaniesContext())
            {
                model = context.Employees.FirstOrDefault(e => e.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
            }

            ViewBag.IsEdit = isEdit;
            ViewBag.IsAdd = false;
            return PartialView("AddEmployees", model);
        }

        [HttpPost]
        public IActionResult _EmployeeSummary(Employee model)
        {
            ViewBag.IsEdit = true;

            if (!model.IsValid())
            {
                return Json(model.Errors);
            }

            using (CompaniesContext context = new CompaniesContext())
            {
                Employee? employee = context.Employees.FirstOrDefault(c => c.Id.Equals(model.Id, StringComparison.OrdinalIgnoreCase));

                if (employee == null)
                {
                    model.Errors.Add(string.Empty, "Employee not found");
                    return Json(model.Errors);
                }

                employee.CastPropertiesFrom(model);
                context.SaveChanges();
            }

            ViewBag.IsEdit = false;
            ViewBag.IsAdd = false;
            return PartialView("AddEmployees", model);
        }

        [HttpGet]
        public IActionResult Delete(string id, TableType tableType)
        {
            using (CompaniesContext context = new CompaniesContext())
            {
                switch (tableType)
                {
                    case TableType.Notes:
                        {
                            Note? note = context.Notes.FirstOrDefault(e => e.Id == id);

                            if(note == null)
                            {
                                return Json(false);
                            }

                            context.Notes.Remove(note);

                            break;
                        }
                    case TableType.Employees:
                        {
                            Employee? employee = context.Employees.FirstOrDefault(e => e.Id == id);

                            if (employee == null)
                            {
                                return Json(false);
                            }

                            context.Employees.Remove(employee);

                            break;
                        }
                    default:
                        return Json(false);

                }

                context.SaveChanges();
            }

            return Json(true);
        }
    }
}