using OfficeManager.Areas.Administration.ViewModels.Landlords;
using OfficeManager.Data;
using OfficeManager.Models;
using System.Linq;

namespace OfficeManager.Services
{
    public class LandlordsService : ILandlordsService
    {
        private readonly ApplicationDbContext dbContext;

        public LandlordsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateLandlord(CreateLandlordViewModel input)
        {
            if (this.dbContext.Landlords.Count() == 1)
            {
                return;
            }

            Landlord landlord = new Landlord()
            {
                CompanyName = input.LandlordName,
                CompanyOwner = input.LandlordOwner,
                Bulstat = input.Bulstat,
                Address = input.Address,
                Email = input.Email,
                Phone = input.Phone
            };

            this.dbContext.Landlords.Add(landlord);
            this.dbContext.SaveChanges();
        }

        //public CreateLandlordViewModel EditLandlord()
        //{
        //    var landlord = this.dbContext.Landlords.FirstOrDefault();

        //    var outputLandlord = new CreateLandlordViewModel()
        //    {
        //        Id = landlord.Id,
        //        LandlordName = landlord.CompanyName,
        //        LandlordOwner = landlord.CompanyOwner,
        //        Bulstat = landlord.Bulstat,
        //        Address = landlord.Address,
        //        Email = landlord.Email,
        //        Phone = landlord.Phone
        //    };

        //    return outputLandlord;
        //}

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
                Phone = landlord.Phone
            };

            return outputLandlord;
        }

        public void UpdateLandlord(CreateLandlordViewModel input)
        {
            Landlord landlordToEdit = this.dbContext.Landlords.FirstOrDefault();

            landlordToEdit.CompanyName = input.LandlordName;
            landlordToEdit.CompanyOwner = input.LandlordOwner;
            landlordToEdit.Bulstat = input.Bulstat;
            landlordToEdit.Phone = input.Phone;
            landlordToEdit.Email = input.Email;
            landlordToEdit.Address = input.Address;

            this.dbContext.SaveChanges();
        }
    }
}
