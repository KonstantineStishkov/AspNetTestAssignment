﻿using AspNetTestAssignment.Constants;
using AspNetTestAssignment.DataBase;
using AspNetTestAssignment.Models;
using Microsoft.AspNetCore.Mvc;
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
                model = context.Companies.FirstOrDefault(c => c.Id.Equals(id, StringComparison.OrdinalIgnoreCase))?.CompanyHistory;
            }

            return Json(model?.Count > 0 ? model : false);
        }

        [HttpGet]
        public IActionResult GetNotes(string id)
        {
            List<Note>? model;
            using (CompaniesContext context = new CompaniesContext())
            {
                model = context.Companies.FirstOrDefault(c => c.Id.Equals(id, StringComparison.OrdinalIgnoreCase))?.Notes;
            }

            return Json(model?.Count > 0 ? model : false);
        }

        [HttpGet]
        public IActionResult GetEmployees(string id)
        {
            List<Employee>? model;
            using (CompaniesContext context = new CompaniesContext())
            {
                model = context.Companies.FirstOrDefault(c => c.Id.Equals(id, StringComparison.OrdinalIgnoreCase))?.Employees;
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

                int invoiceNumber = company.Notes?.Count > 0 ? company.Notes.Max(n => n.InvoiceNumber) : 1;
                model = new Note(invoiceNumber, company);
            }

            if(model == null)
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
                Company? company = context.Companies.FirstOrDefault(c => c.Id.Equals(model.Id, StringComparison.OrdinalIgnoreCase));

                if(company == null)
                {
                    model.Errors.Add(string.Empty, "Company not found");
                    return Json(model.Errors);
                }

                model.Employee = company.Employees.FirstOrDefault(e => e.Id.Equals(model.EmployeeId, StringComparison.OrdinalIgnoreCase));

                if(model.Employee == null)
                {
                    model.Errors.Add(string.Empty, "Employee not found");
                    return Json(model.Errors);
                }

                company.Notes?.Add(model);
                context.SaveChanges();
            }

            return Json(true);
        }

        [HttpGet]
        public IActionResult AddEmployees()
        {
            return PartialView();
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
                Company? company = context.Companies.FirstOrDefault(c => c.Id.Equals(model.Id, StringComparison.OrdinalIgnoreCase));

                if (company == null)
                {
                    model.Errors.Add(string.Empty, "Company not found");
                    return Json(model.Errors);
                }

                company.Employees.Add(model);
                context.SaveChanges();
            }

            return Json(true);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}