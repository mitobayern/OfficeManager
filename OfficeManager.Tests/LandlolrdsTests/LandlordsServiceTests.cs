namespace OfficeManager.Tests.LandlolrdsTests
{
    using Microsoft.EntityFrameworkCore;
    using OfficeManager.Areas.Administration.ViewModels.Landlords;
    using OfficeManager.Data;
    using OfficeManager.Models;
    using OfficeManager.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class LandlordsServiceTests
    {
        private CreateLandlordViewModel landlord;

        public LandlordsServiceTests()
        {
            landlord = new CreateLandlordViewModel
            {
                LandlordName = "TestCompanyName",
                LandlordOwner = "TestCompanyOwner",
                Bulstat = "TestBulstat",
                Address = "TestAddress",
                Email = "Test@email.com",
                Phone = "0888888888"
            };
        }

        [Fact]
        public void TestIfLandlordCreatedCorrectly()
        {
            int actualLandlordsCount;

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ILandlordsService landlordsService = new LandlordsService(dbContext);

                landlordsService.CreateLandlord(this.landlord);
                landlordsService.CreateLandlord(this.landlord);

                actualLandlordsCount = dbContext.Landlords.Count();
            }

            Assert.Equal(1, actualLandlordsCount);
        }

        [Fact]
        public void TestIfLandlordIsReturnedCorrectrly()
        {
            string landlordName;
            string landlordOwner;
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ILandlordsService landlordsService = new LandlordsService(dbContext);
                landlordsService.CreateLandlord(this.landlord);
                landlordName = landlordsService.GetLandlord().LandlordName;
                landlordOwner = landlordsService.GetLandlord().LandlordOwner;
            }
            Assert.Equal("TestCompanyName", landlordName);
            Assert.Equal("TestCompanyOwner", landlordOwner);
        }

        [Fact]
        public void TestIfLandlordIsUpdatedCorrectrly()
        {
            var landlordToUpdate = new CreateLandlordViewModel
            {
                LandlordName = "UpdateCompanyName",
                LandlordOwner = "UpdateCompanyOwner",
                Bulstat = "UpdateBulstat",
                Address = "UpdateAddress",
                Email = "Update@email.com",
                Phone = "0888888888"
            };

            string landlordName;
            string landlordOwner;

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ILandlordsService landlordsService = new LandlordsService(dbContext);
                landlordsService.CreateLandlord(this.landlord);
                landlordsService.UpdateLandlord(landlordToUpdate);
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