using System.ComponentModel.DataAnnotations;

namespace API.Controllers.Patients.Requests
{
    public class CreatePatient
    {
        [Required(ErrorMessage = "Patient ID is required.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("^[MF]$", ErrorMessage = "Gender must be 'M' or 'F'.")]
        public char Gender { get; set; }

        [Required(ErrorMessage = "Contact information is required.")]
        [Phone(ErrorMessage = "Invalid contact information format.")]
        [MaxLength(15, ErrorMessage = "Contact information cannot exceed 15 characters.")]
        public string ContactInfo { get; set; }
    }
}
