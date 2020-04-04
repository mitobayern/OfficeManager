using Microsoft.EntityFrameworkCore;
using OfficeManager.Areas.Administration.ViewModels.Tenants;
using OfficeManager.Data;
using OfficeManager.Models;
using OfficeManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace OfficeManager.Tests.TenantsTests
{
    public class TenantsServiceTests
    {
        [Fact]
        public void TestIfTemperatureMeterIsCreatedCorrectly()
        {
            int actualTenantsCount;

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITenantsService tenantsService = new TenantsService(dbContext);

                tenantsService.CreateTenant(new CreateTenantViewModel
                {
                    CompanyName = "FirstTestCompanyName",
                    CompanyOwner = "TestCompanyOwner",
                    Bulstat = "123456789",
                    Address = "TestAddress",
                    Email = "test@email.com",
                    Phone = "0888888888",
                    StartOfContract = DateTime.UtcNow,
                });

                for (int i = 0; i < 3; i++)
                {
                    tenantsService.CreateTenant(new CreateTenantViewModel
                    {
                        CompanyName = "SecondTestCompanyName",
                        CompanyOwner = "TestCompanyOwner",
                        Bulstat = "123456789",
                        Address = "TestAddress",
                        Email = "test@email.com",
                        Phone = "0888888888",
                        StartOfContract = DateTime.UtcNow,
                    });
                }

                actualTenantsCount = dbContext.Tenants.Count();
            }

            Assert.Equal(2, actualTenantsCount);
        }

        [Fact]
        public void TestIfAllTenantsAreReturnedCorrectrly()
        {
            string names = string.Empty;
            List<TenantOutputViewModel> allTenants = new List<TenantOutputViewModel>();

            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITenantsService tenantsService = new TenantsService(dbContext);

                for (int i = 1; i <= 3; i++)
                {
                    tenantsService.CreateTenant(new CreateTenantViewModel
                    {
                        CompanyName = i.ToString(),
                        CompanyOwner = "TestCompanyOwner",
                        Bulstat = "123456789",
                        Address = "TestAddress",
                        Email = "test@email.com",
                        Phone = "0888888888",
                        StartOfContract = DateTime.UtcNow,
                    });
                }
                allTenants = tenantsService.GetAllTenants().ToList();
            }

            foreach (var tenant in allTenants)
            {
                names += tenant.CompanyName;
            }

            Assert.Equal(3, allTenants.Count);
            Assert.Equal("123", names);
        }

        [Fact]
        public void TestIfGetTenantByIdReturnsCorrectrly()
        {
            string companyName;
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITenantsService tenantsService = new TenantsService(dbContext);

                for (int i = 1; i <= 3; i++)
                {
                    tenantsService.CreateTenant(new CreateTenantViewModel
                    {
                        CompanyName = i.ToString(),
                        CompanyOwner = "TestCompanyOwner",
                        Bulstat = "123456789",
                        Address = "TestAddress",
                        Email = "test@email.com",
                        Phone = "0888888888",
                        StartOfContract = DateTime.UtcNow,
                    });
                }
                companyName = tenantsService.GetTenantById(2).CompanyName;
            }

            Assert.Equal("2", companyName);
        }

        [Fact]
        public void TestIfGetTenantByCompanyNameReturnsCorrectrly()
        {
            string companyOwner;
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITenantsService tenantsService = new TenantsService(dbContext);
                {
                    tenantsService.CreateTenant(new CreateTenantViewModel
                    {
                        CompanyName = "TestCompanyName",
                        CompanyOwner = "TestCompanyOwner",
                        Bulstat = "123456789",
                        Address = "TestAddress",
                        Email = "test@email.com",
                        Phone = "0888888888",
                        StartOfContract = DateTime.UtcNow,
                    });
                }
                companyOwner = tenantsService.GetTenantByCompanyName("TestCompanyName").CompanyOwner;
            }

            Assert.Equal("TestCompanyOwner", companyOwner);
        }


        [Theory]
        [InlineData("123456789")]
        [InlineData("BG123456789")]
        public void TestIfGetTenantEIKReturnsCorrectrly(string bulstat)
        {
            string eik;
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITenantsService tenantsService = new TenantsService(dbContext);
                {
                    tenantsService.CreateTenant(new CreateTenantViewModel
                    {
                        CompanyName = "TestCompanyName",
                        CompanyOwner = "TestCompanyOwner",
                        Bulstat = bulstat,
                        Address = "TestAddress",
                        Email = "test@email.com",
                        Phone = "0888888888",
                        StartOfContract = DateTime.UtcNow,
                    });
                }
                eik = tenantsService.GetTenantEIK("TestCompanyName");
            }

            Assert.Equal("123456789", eik);
        }

        [Fact]
        public void TestIfTenantOfficesAreReturnedCorrectrly()
        {
            int numberOfOffices;
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITenantsService tenantsService = new TenantsService(dbContext);
                {
                    tenantsService.CreateTenant(new CreateTenantViewModel
                    {
                        CompanyName = "TestCompanyName",
                        CompanyOwner = "TestCompanyOwner",
                        Bulstat = "123456789",
                        Address = "TestAddress",
                        Email = "test@email.com",
                        Phone = "0888888888",
                        StartOfContract = DateTime.UtcNow,
                    });
                    var tenant = tenantsService.GetTenantByCompanyName("TestCompanyName");
                    for (int i = 1; i <= 3; i++)
                    {
                        Office office = new Office
                        {
                            Name = i.ToString(),
                            Area = 50M,
                            RentPerSqMeter = 7.2M,
                        };
                        tenant.Offices.Add(office);
                    }
                    dbContext.SaveChanges();

                    numberOfOffices = tenantsService.GetTenantOffices(new TenantIdViewModel { Id = 1 }).Count();
                }
            }

            Assert.Equal(3, numberOfOffices);
        }

        [Fact]
        public void TestIfTenantOfficesAsTextWithMoreThanOneOfficeAreReturnedCorrectrly()
        {
            string actualResultSingleOffice;
            string actualResultМanyOffices;
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITenantsService tenantsService = new TenantsService(dbContext);
                {
                    for (int i = 1; i <= 2; i++)
                    {
                        tenantsService.CreateTenant(new CreateTenantViewModel
                        {
                            CompanyName = i.ToString(),
                            CompanyOwner = "TestCompanyOwner",
                            Bulstat = "123456789",
                            Address = "TestAddress",
                            Email = "test@email.com",
                            Phone = "0888888888",
                            StartOfContract = DateTime.UtcNow,
                        });
                        var tenant = tenantsService.GetTenantByCompanyName(i.ToString());
                        if (i == 1)
                        {
                            for (int j = 1; j <= 3; j++)
                            {
                                Office office = new Office
                                {
                                    Name = j.ToString(),
                                    Area = 50M,
                                    RentPerSqMeter = 7.2M,
                                };
                                tenant.Offices.Add(office);
                            }
                        }
                        else
                        {
                            Office office = new Office
                            {
                                Name = "Единствен",
                                Area = 50M,
                                RentPerSqMeter = 7.2M,
                            };
                            tenant.Offices.Add(office);
                        }
                        dbContext.SaveChanges();
                    }
                    actualResultМanyOffices = tenantsService.GetTenantOfficesAsText("1");
                    actualResultSingleOffice = tenantsService.GetTenantOfficesAsText("2");
                }
            }
            Assert.Equal("офиси 1, 2 и 3", actualResultМanyOffices);
            Assert.Equal("офис Единствен", actualResultSingleOffice);
        }

        [Fact]
        public void TestIfTenantIsUpdatedCorrectrly()
        {
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITenantsService tenantsService = new TenantsService(dbContext);
                {
                    tenantsService.CreateTenant(new CreateTenantViewModel
                    {
                        CompanyName = "TestCompanyName",
                        CompanyOwner = "TestCompanyOwner",
                        Bulstat = "123456789",
                        Address = "TestAddress",
                        Email = "test@email.com",
                        Phone = "0888888888",
                        StartOfContract = DateTime.UtcNow,
                    });                    
                    dbContext.SaveChanges();

                    tenantsService.UpdateTenant(new TenantToEditViewModel
                    {
                        Id = 1,
                        CompanyName = "UpdatedCompanyName",
                        CompanyOwner = "UpdatedCompanyOwner",
                        Bulstat = "987654321",
                        Address = "UpdatedAddress",
                        Email = "Updated@email.com",
                        Phone = "0999999999",
                        StartOfContract = DateTime.UtcNow,
                    });

                    Assert.Equal("UpdatedCompanyName", tenantsService.GetTenantByCompanyName("UpdatedCompanyName").CompanyName);
                    Assert.Equal("UpdatedCompanyOwner", tenantsService.GetTenantByCompanyName("UpdatedCompanyName").CompanyOwner);
                    Assert.Equal("987654321", tenantsService.GetTenantByCompanyName("UpdatedCompanyName").Bulstat);
                    Assert.Equal("UpdatedAddress", tenantsService.GetTenantByCompanyName("UpdatedCompanyName").Address);
                    Assert.Equal("Updated@email.com", tenantsService.GetTenantByCompanyName("UpdatedCompanyName").Email);
                    Assert.Equal("0999999999", tenantsService.GetTenantByCompanyName("UpdatedCompanyName").Phone);
                }
            }
        }

        [Fact]
        public void TestIfTenantIReturnedForEditionCorrectrly()
        {
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITenantsService tenantsService = new TenantsService(dbContext);
                {
                    var tenantToEdit = tenantsService.EditTenant(new Tenant
                    {
                        CompanyName = "TestCompanyName",
                        CompanyOwner = "TestCompanyOwner",
                        Bulstat = "123456789",
                        Address = "TestAddress",
                        Email = "test@email.com",
                        Phone = "0888888888",
                        StartOfContract = DateTime.UtcNow,
                    });

                    Assert.Equal("TestCompanyName", tenantToEdit.CompanyName);
                    Assert.Equal("TestCompanyOwner", tenantToEdit.CompanyOwner);
                    Assert.Equal("123456789", tenantToEdit.Bulstat);
                    Assert.Equal("TestAddress", tenantToEdit.Address);
                    Assert.Equal("test@email.com", tenantToEdit.Email);
                    Assert.Equal("0888888888", tenantToEdit.Phone);
                }
            }
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
