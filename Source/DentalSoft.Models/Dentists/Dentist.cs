namespace DentalSoft.Data.Models.Dentists
{
    public class Dentist : DeletableEntity
    {
      
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Initials { get; set; }

        public string ImageUrl { get; set; }

        //TODO: Check for SecondName
    }
}
