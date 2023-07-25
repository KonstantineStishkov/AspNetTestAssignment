using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AspNetTestAssignment.Models
{
    public class Employee
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public DateTime BirthDate { get; set; }
        public string Position { get; set; }
        public string CompanyId { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public Dictionary<string, string> Errors { get; set; }

        public Employee()
        {
            Id = Guid.NewGuid().ToString();
            CompanyId = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Title = string.Empty;
            BirthDate = DateTime.MinValue;
            Position = string.Empty;
            Errors = new Dictionary<string, string>();
        }
        public bool IsValid()
        {
            const int minYearsAgoForBirthDate = 100;

            Errors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(FirstName) || !Regex.IsMatch(FirstName, @"^[A-Z][a-z-]"))
            {
                Errors.Add(nameof(FirstName), "First Name is not valid");
                return false;
            }

            if (string.IsNullOrWhiteSpace(LastName) || !Regex.IsMatch(LastName, @"^[A-Z][a-z-]"))
            {
                Errors.Add(nameof(LastName), "Last Name is not valid");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Title) || !Regex.IsMatch(Title, @"^[A-Z][a-z-]"))
            {
                Errors.Add(nameof(Title), "Title is not valid");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Position) || !Regex.IsMatch(Position, @"^[A-Z][a-z-]"))
            {
                Errors.Add(nameof(Position), "Title is not valid");
                return false;
            }

            if(BirthDate < DateTime.Now.AddYears(-minYearsAgoForBirthDate))
            {
                Errors.Add(nameof(BirthDate), $"Birth Date should be less than {minYearsAgoForBirthDate} years ago");
                return false;
            }

            if (BirthDate >= DateTime.Now)
            {
                Errors.Add(nameof(BirthDate), $"Birth Date should be in the past");
                return false;
            }

            return true;
        }
    }
}
