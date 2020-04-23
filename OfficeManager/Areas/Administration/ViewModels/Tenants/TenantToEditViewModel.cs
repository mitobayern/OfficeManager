namespace OfficeManager.Areas.Administration.ViewModels.Tenants
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class TenantToEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string CompanyOwner { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool HasContract { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [RegularExpression("^BG{0,1}[0-9]{9}$|^[0-9]{9}|$", ErrorMessage = "Невалиден Булстат")]
        public string Bulstat { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartOfContract { get; set; }

        public IEnumerable<string> Offices { get; set; }

        public IEnumerable<string> AllOffices { get; set; }
    }
}
