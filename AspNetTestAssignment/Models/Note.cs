using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetTestAssignment.Models
{
    public class Note
    {
        public string Id { get; set; }
        public string CompanyId { get; set; }
        public int InvoiceNumber { get; set; }
        public Employee? Employee { get; set; }

        [NotMapped]
        public SelectList AvailableEmployees { get; set; }

        [NotMapped]
        public string EmployeeId { get; set; }

        [NotMapped]
        public Dictionary<string, string> Errors { get; set; }

        public Note() 
        {
            Id = Guid.NewGuid().ToString();
            CompanyId = string.Empty;
            EmployeeId = string.Empty;
            Errors = new Dictionary<string, string>();
        }

        public Note(int invoiceNumber, Company company)
        {
            Id = Guid.NewGuid().ToString();
            CompanyId = company.Id;
            EmployeeId = string.Empty;
            InvoiceNumber = invoiceNumber;
            AvailableEmployees = new SelectList(company.Employees, "Id", "FullName");
            Errors = new Dictionary<string, string>();
        }

        public Note(string id, int invoiceNumber, Employee employee, Company company)
        {
            Id = id;
            CompanyId = company.Id;
            InvoiceNumber = invoiceNumber;
            Employee = employee;
            EmployeeId = employee.Id;
            Errors = new Dictionary<string, string>();
        }

        public Note(int invoiceNumber, Employee employee, Company company)
        {
            Id = Guid.NewGuid().ToString();
            CompanyId = company.Id;
            InvoiceNumber = invoiceNumber;
            Employee = employee;
            EmployeeId = employee.Id;
            Errors = new Dictionary<string, string>();
        }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(CompanyId))
            {
                Errors.Add(string.Empty, "Company Id should be provided");
                return false;
            }

            if (string.IsNullOrWhiteSpace(EmployeeId))
            {
                Errors.Add(string.Empty, "Employee Id should be provided");
                return false;
            }

            return true;
        }
    }
}
