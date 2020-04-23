namespace OfficeManager.Areas.Administration.ViewModels.Tenants
{
    using System;

    public class TenantOutputViewModel
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string CompanyOwner { get; set; }

        public string Bulstat { get; set; }

        public string Address { get; set; }

        public DateTime StartOfContract { get; set; }

        public bool HasContract { get; set; }
    }
}
