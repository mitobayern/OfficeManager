using Microsoft.EntityFrameworkCore;
using Moq;
using OfficeManager.Areas.Administration.ViewModels.ElectricityMeters;
using OfficeManager.Areas.Administration.ViewModels.Offices;
using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;
using OfficeManager.Data;
using OfficeManager.Services;
using OfficeManager.ViewModels.Measurements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace OfficeManager.Tests.MeasurementsTests
{
    public class MeasurementsServiceTests
    {
        private readonly DateTime periodStartTime;
        private readonly DateTime periodEndTime;
        private readonly string electricityMeterName;
        private readonly string temperatureMeterName;
        private readonly decimal dayTimeMeasurement;
        private readonly decimal nightTimeMeasurement;
        private readonly decimal heatingMeasurement;
        private readonly decimal coolingMeasurement;


        private Mock<ITenantsService> tenantsService;
        private Mock<IElectricityMetersService> electricityMetersService;
        private Mock<ITemperatureMetersService> temperatureMetersService;

        public MeasurementsServiceTests()
        {
            this.periodStartTime = new DateTime(2020, 1, 1);
            this.periodEndTime = new DateTime(2020, 1, 31);
            this.electricityMeterName = "TestElectricityMeter";
            this.temperatureMeterName = "TestTemperatureMeter";
            this.dayTimeMeasurement = 10M;
            this.nightTimeMeasurement = 20M;
            this.heatingMeasurement = 30M;
            this.coolingMeasurement = 40M;

            this.tenantsService = new Mock<ITenantsService>();
            this.electricityMetersService = new Mock<IElectricityMetersService>();
            this.temperatureMetersService = new Mock<ITemperatureMetersService>();
        }

        [Fact]
        public void TestIfElectricityMeasurementsAreCreatedCorrectly()
        {
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                IElectricityMetersService electricityMeters = new ElectricityMetersService(dbContext);

                IMeasurementsService measurementsService =
                    new MeasurementsService(dbContext, electricityMeters, temperatureMetersService.Object, tenantsService.Object);

                electricityMeters.CreateElectricityMeter(new CreateElectricityMeterViewModel
                {
                    Name = electricityMeterName,
                    PowerSupply = 10M,
                });

                measurementsService.CreateElectricityMeasurementAsync(periodStartTime, periodEndTime, electricityMeterName, dayTimeMeasurement, nightTimeMeasurement);
                var countOfMeasurements = electricityMeters.GetElectricityMeterByName(electricityMeterName).ElectricityMeasurements.Count();
                string period = electricityMeters.GetElectricityMeterById(1).ElectricityMeasurements.FirstOrDefault().Period;
                Assert.Equal("1 януари - 31 януари 2020 г.", period);
                Assert.Equal(1, dbContext.ElectricityMeasurements.Count());
                Assert.Equal(1, countOfMeasurements);
            }
        }

        [Fact]
        public void TestIfInitialElectricityMeasurementsAreCreatedCorrectly()
        {
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                IElectricityMetersService electricityMeters = new ElectricityMetersService(dbContext);

                IMeasurementsService measurementsService =
                    new MeasurementsService(dbContext, electricityMeters, temperatureMetersService.Object, tenantsService.Object);

                electricityMeters.CreateElectricityMeter(new CreateElectricityMeterViewModel
                {
                    Name = electricityMeterName,
                    PowerSupply = 10M,
                });

                measurementsService.CreateInitialElectricityMeasurementAsync(periodEndTime, electricityMeterName, dayTimeMeasurement, nightTimeMeasurement);
                var countOfMeasurements = electricityMeters.GetElectricityMeterByName(electricityMeterName).ElectricityMeasurements.Count();
                string period = electricityMeters.GetElectricityMeterById(1).ElectricityMeasurements.FirstOrDefault().Period;
                Assert.Equal("Starting period 31 януари 2020 г.", period);
                Assert.Equal(1, dbContext.ElectricityMeasurements.Count());
                Assert.Equal(1, countOfMeasurements);
            }
        }

        [Fact]
        public void TestIfTemperatureMeasurementsAreCreatedCorrectly()
        {
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITemperatureMetersService temperatureMeters = new TemperatureMetersService(dbContext);

                IMeasurementsService measurementsService =
                    new MeasurementsService(dbContext, electricityMetersService.Object, temperatureMeters, tenantsService.Object);

                temperatureMeters.CreateTemperatureMeter(new CreateTemperatureMeterViewModel
                {
                    Name = temperatureMeterName
                });

                measurementsService.CreateTemperatureMeasurementAsync(periodStartTime, periodEndTime, temperatureMeterName, heatingMeasurement, coolingMeasurement);

                var countOfMeasurements = temperatureMeters.GetTemperatureMeterByName(temperatureMeterName).TemperatureMeasurements.Count();
                string period = temperatureMeters.GetTemperatureMeterById(1).TemperatureMeasurements.FirstOrDefault().Period;
                Assert.Equal("1 януари - 31 януари 2020 г.", period);
                Assert.Equal(1, dbContext.TemperatureMeasurements.Count());
                Assert.Equal(1, countOfMeasurements);
            }
        }

        [Fact]
        public void TestIfInitialTemperatureMeasurementsAreCreatedCorrectly()
        {
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITemperatureMetersService temperatureMeters = new TemperatureMetersService(dbContext);

                IMeasurementsService measurementsService =
                    new MeasurementsService(dbContext, electricityMetersService.Object, temperatureMeters, tenantsService.Object);

                temperatureMeters.CreateTemperatureMeter(new CreateTemperatureMeterViewModel
                {
                    Name = temperatureMeterName
                });

                measurementsService.CreateInitialTemperatureMeasurementAsync(periodEndTime, temperatureMeterName, heatingMeasurement, coolingMeasurement);

                var countOfMeasurements = temperatureMeters.GetTemperatureMeterByName(temperatureMeterName).TemperatureMeasurements.Count();
                string period = temperatureMeters.GetTemperatureMeterById(1).TemperatureMeasurements.FirstOrDefault().Period;
                Assert.Equal("Starting period 31 януари 2020 г.", period);
                Assert.Equal(1, dbContext.TemperatureMeasurements.Count());
                Assert.Equal(1, countOfMeasurements);
            }
        }

        [Fact]
        public void TestIfAllMeasurementsAreCreatedAndReturnedCorrectly()
        {
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITemperatureMetersService temperatureMeters = new TemperatureMetersService(dbContext);
                IElectricityMetersService electricityMeters = new ElectricityMetersService(dbContext);
                IOfficesService officesService = new OfficesService(dbContext, tenantsService.Object, electricityMeters, temperatureMeters);

                IMeasurementsService measurementsService =
                    new MeasurementsService(dbContext, electricityMeters, temperatureMeters, tenantsService.Object);

                CreateOfficeWithMeters(temperatureMeters, electricityMeters, officesService);

                CreateMeasurementsInputViewModel input = new CreateMeasurementsInputViewModel
                {
                    StartOfPeriod = periodStartTime,
                    EndOfPeriod = periodEndTime,
                    EndOfLastPeriod = periodStartTime.AddDays(-1),
                    LastPeriod = "1 декември - 31 декември 2019 г.",
                    Offices = new List<OfficeMeasurementsInputViewModel> { GetOfficeInputViewModel() },
                };
                measurementsService.CreateAllMeasurementsAsync(input);

                var result = measurementsService.GetOfficesWithLastMeasurements().FirstOrDefault();
                
                Assert.Equal("TestOfficeName", result.Name);
                Assert.Equal(electricityMeterName, result.ElectricityMeter.Name);
                Assert.Equal(dayTimeMeasurement, result.ElectricityMeter.DayTimeMinValue);
                Assert.Equal(nightTimeMeasurement, result.ElectricityMeter.NightTimeMinValue);
                Assert.Equal(temperatureMeterName, result.TemperatureMeters.FirstOrDefault().Name);
                Assert.Equal(heatingMeasurement, result.TemperatureMeters.FirstOrDefault().HeatingMinValue);
                Assert.Equal(coolingMeasurement, result.TemperatureMeters.FirstOrDefault().CoolingMinValue);
                Assert.Equal(1, dbContext.TemperatureMeasurements.Count());
                Assert.Equal(1, dbContext.ElectricityMeasurements.Count());
            }
        }

        [Fact]
        public void TestIfAllPeriodsAreReturnedCorrectly()
        {
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITemperatureMetersService temperatureMeters = new TemperatureMetersService(dbContext);
                IElectricityMetersService electricityMeters = new ElectricityMetersService(dbContext);
                IOfficesService officesService = new OfficesService(dbContext, tenantsService.Object, electricityMeters, temperatureMeters);

                IMeasurementsService measurementsService =
                    new MeasurementsService(dbContext, electricityMeters, temperatureMeters, tenantsService.Object);

                CreateOfficeWithMeters(temperatureMeters, electricityMeters, officesService);

                CreateMeasurementsInputViewModel input = new CreateMeasurementsInputViewModel
                {
                    StartOfPeriod = periodStartTime,
                    EndOfPeriod = periodEndTime,
                    EndOfLastPeriod = periodStartTime.AddDays(-1),
                    LastPeriod = "1 декември - 31 декември 2019 г.",
                    Offices = new List<OfficeMeasurementsInputViewModel> { GetOfficeInputViewModel() },
                };
                measurementsService.CreateAllMeasurementsAsync(input);

                Assert.Equal("1 януари - 31 януари 2020 г.", measurementsService.GetLastPeriodAsText());
                Assert.Equal(new DateTime(2020, 2, 1), measurementsService.GetStartOfNewPeroid());
                Assert.Equal(new DateTime(2020, 2, 29), measurementsService.GetEndOfNewPeriod());
                Assert.Equal(new DateTime(2020, 1, 31), measurementsService.GetEndOfLastPeriod());
            }
        }

        [Fact]
        public void TestIfAllInitialMeasurementsAreCreatedCorrectly()
        {
            using (var dbContext = new ApplicationDbContext(GetInMemoryDadabaseOptions()))
            {
                ITemperatureMetersService temperatureMeters = new TemperatureMetersService(dbContext);
                IElectricityMetersService electricityMeters = new ElectricityMetersService(dbContext);
                IOfficesService officesService = new OfficesService(dbContext, tenantsService.Object, electricityMeters, temperatureMeters);

                IMeasurementsService measurementsService =
                    new MeasurementsService(dbContext, electricityMeters, temperatureMeters, tenantsService.Object);

                CreateOfficeWithMeters(temperatureMeters, electricityMeters, officesService);

                CreateInitialMeasurementsInputViewModel input = new CreateInitialMeasurementsInputViewModel
                {
                    EndOfPeriod = periodEndTime,
                    Offices = new List<OfficeMeasurementsInputViewModel> { GetOfficeInputViewModel() },
                };

                measurementsService.CreateInitialMeasurementsAsync(input);

                Assert.Equal(1, dbContext.TemperatureMeasurements.Count());
                Assert.Equal(1, dbContext.ElectricityMeasurements.Count());
                Assert.True(measurementsService.IsFirstPeriod());
            }
        }

        private OfficeMeasurementsInputViewModel GetOfficeInputViewModel()
        {
            ElectricityMeasurementInputViewModel electricityMeasurement = new ElectricityMeasurementInputViewModel()
            {
                Name = electricityMeterName,
                DayTimeMeasurement = dayTimeMeasurement,
                DayTimeMinValue = dayTimeMeasurement,
                NightTimeMeasurement = nightTimeMeasurement,
                NightTimeMinValue = nightTimeMeasurement,
            };
            TemperatureMeasurementInputViewModel temperatureMeasurement = new TemperatureMeasurementInputViewModel
            {
                Name = temperatureMeterName,
                HeatingMeasurement = heatingMeasurement,
                HeatingMinValue = heatingMeasurement,
                CoolingMeasurement = coolingMeasurement,
                CoolingMinValue = coolingMeasurement,
            };

            OfficeMeasurementsInputViewModel officeViewModel = new OfficeMeasurementsInputViewModel
            {
                Name = "TestOfficeName",
                ElectricityMeter = electricityMeasurement,
                TemperatureMeters = new List<TemperatureMeasurementInputViewModel> { temperatureMeasurement },
            };

            return officeViewModel;
        }

        private void CreateOfficeWithMeters(ITemperatureMetersService temperatureMeters, IElectricityMetersService electricityMeters, IOfficesService officesService)
        {
            officesService.CreateOfficeAsync(new CreateOfficeViewModel
            {
                Name = "TestOfficeName",
                Area = 50M,
                RentPerSqMeter = 7.2M
            });

            electricityMeters.CreateElectricityMeter(new CreateElectricityMeterViewModel
            {
                Name = electricityMeterName,
                PowerSupply = 10M,
            });

            temperatureMeters.CreateTemperatureMeter(new CreateTemperatureMeterViewModel
            {
                Name = temperatureMeterName
            });

            officesService.AddElectricityMeterToOfficeAsync(1, electricityMeterName);
            officesService.AddTemperatureMetersToOfficeAsync(1, new List<string> { temperatureMeterName });
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
