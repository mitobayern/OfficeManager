namespace OfficeManager.Areas.Administration.ViewModels.Offices
{
    public class OfficeOutputViewModel
    {
        public int Id { get; set; }

        public string TenantName { get; set; }

        public string Name { get; set; }

        public decimal Area { get; set; }

        public decimal RentPerSqMeter { get; set; }
    }
}
