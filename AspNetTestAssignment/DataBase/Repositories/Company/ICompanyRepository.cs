using AspNetTestAssignment.Models;

namespace AspNetTestAssignment.DataBase.Repositories
{
    public interface ICompanyRepository
    {
        public List<Company> GetCompanies();
    }
}
