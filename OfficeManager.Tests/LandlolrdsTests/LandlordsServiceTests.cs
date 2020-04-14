namespace OfficeManager.Tests.LandlolrdsTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using OfficeManager.Areas.Administration.ViewModels.Landlords;
    using OfficeManager.Data;
    using OfficeManager.Services;
    using Xunit;

    public class LandlordsServiceTests
    {
        private readonly CreateLandlordViewModel landlord;

        public LandlordsServiceTests()
        {
            this.landlord = new CreateLandlordViewModel
            {
                LandlordName = "TestCompanyName",
                LandlordOwner = "TestCompanyOwner",
                Bulstat = "TestBulstat",
                Address = "TestAddress",
                Email = "Test@email.com",
                Phone = "0888888888",
            };
        }

        [Fact]
        public async Task TestIfLandlordCreatedCorrectlyAsync()
        {
            int actualLandlordsCount;

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                ILandlordsService landlordsService = new LandlordsService(dbContext);

                await landlordsService.CreateLandlordAsync(this.landlord);
                await landlordsService.CreateLandlordAsync(this.landlord);
                actualLandlordsCount = dbContext.Landlords.Count();
            }

            Assert.Equal(1, actualLandlordsCount);
        }

        [Fact]
        public async Task TestIfLandlordIsReturnedCorrectrlyAsync()
        {
            string landlordName;
            string landlordOwner;
            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                ILandlordsService landlordsService = new LandlordsService(dbContext);
                await landlordsService.CreateLandlordAsync(this.landlord);
                landlordName = landlordsService.GetLandlord().LandlordName;
                landlordOwner = landlordsService.GetLandlord().LandlordOwner;
            }

            Assert.Equal("TestCompanyName", landlordName);
            Assert.Equal("TestCompanyOwner", landlordOwner);
        }

        [Fact]
        public async Task TestIfLandlordIsUpdatedCorrectrlyAsync()
        {
            var landlordToUpdate = new CreateLandlordViewModel
            {
                LandlordName = "UpdateCompanyName",
                LandlordOwner = "UpdateCompanyOwner",
                Bulstat = "UpdateBulstat",
                Address = "UpdateAddress",
                Email = "Update@email.com",
                Phone = "0888888888",
            };

            string landlordName;
            string landlordOwner;

            using (var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions()))
            {
                ILandlordsService landlordsService = new LandlordsService(dbContext);
                await landlordsService.CreateLandlordAsync(this.landlord);
                await landlordsService.UpdateLandlordAsync(landlordToUpdate);
                landlordName = landlordsService.GetLandlord().LandlordName;
                landlordOwner = landlordsService.GetLandlord().LandlordOwner;
            }

            Assert.Equal("UpdateCompanyName", landlordName);
            Assert.Equal("UpdateCompanyOwner", landlordOwner);
        }

        private DbContextOptions<ApplicationDbContext> GetInMemoryDadabaseOptions()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return options;
        }
    }
}