using AspNetTestAssignment.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Numerics;
using System.Xml.Linq;

namespace AspNetTestAssignment.DataBase.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        public CompanyRepository()
        {
            using (CompaniesContext context = new CompaniesContext())
            {
                List<Company> companies = new List<Company>();

                context.Companies.AddRange(companies);
                context.SaveChanges();
            }
        }

        public List<Company> GetCompanies()
        {
            using (CompaniesContext context = new CompaniesContext())
            {
                List<Company> list = context.Companies
                                            .Include(a => a.Employees)
                                            .Include(a => a.Notes)
                                            .ToList();
                return list;
            }
        }
    }
}
