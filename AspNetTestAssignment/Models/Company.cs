using AspNetTestAssignment.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AspNetTestAssignment.Models
{
    public class Company : IRemovable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public List<Note> Notes { get; set; }
        public List<Employee> Employees { get; set; }
        public List<History> CompanyHistory { get; set; }

        [NotMapped]
        public Dictionary<string,string> Errors { get; set; }

        public Company() 
        {
            Id = Guid.NewGuid().ToString();
            Name = string.Empty;
            Address = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Phone = string.Empty;
            Notes = new List<Note>();
            Employees = new List<Employee>();
            Errors = new Dictionary<string, string>();
        }

        public bool IsValid()
        {
            Errors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(Name) || !Regex.IsMatch(Name, @"^[A-Z][a-z-]"))
            {
                Errors.Add(nameof(Name), "Name is not valid");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Address))
            {
                Errors.Add(nameof(Address), "Address is not valid");
                return false;
            }

            if (string.IsNullOrWhiteSpace(City) || !Regex.IsMatch(City, @"^[A-Z][A-Za-z -]"))
            {
                Errors.Add(nameof(City), "City is not valid");
                return false;
            }

            if (string.IsNullOrWhiteSpace(State) || !Regex.IsMatch(State, @"^[A-Z][A-Za-z -]"))
            {
                Errors.Add(nameof(State), "State is not valid");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Phone) || !Regex.IsMatch(Phone, @"^[0-9\-\s]{6,12}$"))
            {
                Errors.Add(nameof(Phone), "Phone is not valid");
                return false;
            }

            return true;
        }
    }
}
