using System.ComponentModel.DataAnnotations;

namespace OfficeManager.Areas.Administration.ViewModels.Landlords
{
    public class CreateLandlordViewModel
    {
        public int Id { get; set; }

        [Required]
        public string LandlordName { get; set; }

        [Required]
        public string LandlordOwner { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [RegularExpression("^BG{0,1}[0-9]{9}$|^[0-9]{9}|$", ErrorMessage = "Невалиден Булстат")]
        public string Bulstat { get; set; }
    }
}
