namespace OfficeManager.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using OfficeManager.Areas.Administration.ViewModels.Landlords;
    using OfficeManager.Data;
    using OfficeManager.Models;

    public class LandlordsService : ILandlordsService
    {
        private readonly ApplicationDbContext dbContext;

        public LandlordsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateLandlordAsync(CreateLandlordViewModel input)
        {
            Landlord landlord = new Landlord()
            {
                CompanyName = input.LandlordName,
                CompanyOwner = input.LandlordOwner,
                Bulstat = input.Bulstat,
                Address = input.Address,
                Email = input.Email,
                Phone = input.Phone,
            };

            await this.dbContext.Landlords.AddAsync(landlord);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateLandlordAsync(CreateLandlordViewModel input)
        {
            Landlord landlordToEdit = this.dbContext.Landlords.FirstOrDefault();

            landlordToEdit.CompanyName = input.LandlordName;
            landlordToEdit.CompanyOwner = input.LandlordOwner;
            landlordToEdit.Bulstat = input.Bulstat;
            landlordToEdit.Phone = input.Phone;
            landlordToEdit.Email = input.Email;
            landlordToEdit.Address = input.Address;

            await this.dbContext.SaveChangesAsync();
        }

        public CreateLandlordViewModel GetLandlord()
        {
            var landlord = this.dbContext.Landlords.FirstOrDefault();

            var outputLandlord = new CreateLandlordViewModel()
            {
                Id = landlord.Id,
                LandlordName = landlord.CompanyName,
                LandlordOwner = landlord.CompanyOwner,
                Bulstat = landlord.Bulstat,
                Address = landlord.Address,
                Email = landlord.Email,
                Phone = landlord.Phone,
            };

            return outputLandlord;
        }
    }
}
