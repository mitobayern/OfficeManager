namespace OfficeManager.Tests.MeasurementsTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using OfficeManager.Data;
    using OfficeManager.Services;
    using OfficeManager.ViewModels.Measurements;
    using Xunit;

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
        public async Task TestIfElectricityMeasurementsAreCreatedCorrectlyAsync()
        {
            using var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions());
            IElectricityMetersService electricityMeters = new ElectricityMetersService(dbContext);

            IMeasurementsService measurementsService =
                new MeasurementsService(dbContext, electricityMeters, this.temperatureMetersService.Object);

            await electricityMeters.CreateElectricityMeterAsync(this.electricityMeterName, 10M);
            await measurementsService.CreateElectricityMeasurementAsync(
                this.periodStartTime,
                this.periodEndTime,
                this.electricityMeterName,
                this.dayTimeMeasurement,
                this.nightTimeMeasurement);

            var countOfMeasurements = electricityMeters.GetElectricityMeterByName(this.electricityMeterName).ElectricityMeasurements.Count();
            string period = electricityMeters.GetElectricityMeterById(1).ElectricityMeasurements.FirstOrDefault().Period;
            Assert.Equal("1 януари - 31 януари 2020 г.", period);
            Assert.Equal(1, dbContext.ElectricityMeasurements.Count());
            Assert.Equal(1, countOfMeasurements);
        }

        [Fact]
        public async Task TestIfInitialElectricityMeasurementsAreCreatedCorrectlyAsync()
        {
            using var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions());
            IElectricityMetersService electricityMeters = new ElectricityMetersService(dbContext);

            IMeasurementsService measurementsService =
               new MeasurementsService(dbContext, electricityMeters, this.temperatureMetersService.Object);

            await electricityMeters.CreateElectricityMeterAsync(this.electricityMeterName, 10M);
            await measurementsService.CreateInitialElectricityMeasurementAsync(
                this.periodEndTime,
                this.electricityMeterName,
                this.dayTimeMeasurement,
                this.nightTimeMeasurement);

            var countOfMeasurements = electricityMeters.GetElectricityMeterByName(this.electricityMeterName).ElectricityMeasurements.Count();
            string period = electricityMeters.GetElectricityMeterById(1).ElectricityMeasurements.FirstOrDefault().Period;
            Assert.Equal("Starting period 31 януари 2020 г.", period);
            Assert.Equal(1, dbContext.ElectricityMeasurements.Count());
            Assert.Equal(1, countOfMeasurements);
        }

        [Fact]
        public async Task TestIfTemperatureMeasurementsAreCreatedCorrectlyAsync()
        {
            using var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions());
            ITemperatureMetersService temperatureMeters = new TemperatureMetersService(dbContext);

            IMeasurementsService measurementsService =
                new MeasurementsService(dbContext, this.electricityMetersService.Object, temperatureMeters);

            await temperatureMeters.CreateTemperatureMeterAsync(this.temperatureMeterName);
            await measurementsService.CreateTemperatureMeasurementAsync(
                this.periodStartTime,
                this.periodEndTime,
                this.temperatureMeterName,
                this.heatingMeasurement,
                this.coolingMeasurement);

            var countOfMeasurements = temperatureMeters.GetTemperatureMeterByName(this.temperatureMeterName).TemperatureMeasurements.Count();
            string period = temperatureMeters.GetTemperatureMeterById(1).TemperatureMeasurements.FirstOrDefault().Period;
            Assert.Equal("1 януари - 31 януари 2020 г.", period);
            Assert.Equal(1, dbContext.TemperatureMeasurements.Count());
            Assert.Equal(1, countOfMeasurements);
        }

        [Fact]
        public async Task TestIfInitialTemperatureMeasurementsAreCreatedCorrectlyAsync()
        {
            using var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions());
            ITemperatureMetersService temperatureMeters = new TemperatureMetersService(dbContext);

            IMeasurementsService measurementsService =
                new MeasurementsService(dbContext, this.electricityMetersService.Object, temperatureMeters);

            await temperatureMeters.CreateTemperatureMeterAsync(this.temperatureMeterName);
            await measurementsService.CreateInitialTemperatureMeasurementAsync(
                this.periodEndTime,
                this.temperatureMeterName,
                this.heatingMeasurement,
                this.coolingMeasurement);

            var countOfMeasurements = temperatureMeters.GetTemperatureMeterByName(this.temperatureMeterName).TemperatureMeasurements.Count();
            string period = temperatureMeters.GetTemperatureMeterById(1).TemperatureMeasurements.FirstOrDefault().Period;
            Assert.Equal("Starting period 31 януари 2020 г.", period);
            Assert.Equal(1, dbContext.TemperatureMeasurements.Count());
            Assert.Equal(1, countOfMeasurements);
        }

        [Fact]
        public async Task TestIfAllMeasurementsAreCreatedAndReturnedCorrectlyAsync()
        {
            using var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions());
            ITemperatureMetersService temperatureMeters = new TemperatureMetersService(dbContext);
            IElectricityMetersService electricityMeters = new ElectricityMetersService(dbContext);
            IOfficesService officesService = new OfficesService(dbContext, this.tenantsService.Object, electricityMeters, temperatureMeters);

            IMeasurementsService measurementsService =
                new MeasurementsService(dbContext, electricityMeters, temperatureMeters);

            await this.CreateOfficeWithMetersAsync(temperatureMeters, electricityMeters, officesService);

            CreateMeasurementsInputViewModel input = new CreateMeasurementsInputViewModel
            {
                StartOfPeriod = this.periodStartTime,
                EndOfPeriod = this.periodEndTime,
                EndOfLastPeriod = this.periodStartTime.AddDays(-1),
                LastPeriod = "1 декември - 31 декември 2019 г.",
                Offices = new List<OfficeMeasurementsInputViewModel> { this.GetOfficeInputViewModel() },
            };

            await measurementsService.CreateAllMeasurementsAsync(input);

            var result = measurementsService.GetOfficesWithLastMeasurements().FirstOrDefault();


            Assert.Equal("TestOfficeName", result.Name);
            Assert.Equal(this.electricityMeterName, result.ElectricityMeter.Name);
            Assert.Equal(this.dayTimeMeasurement, result.ElectricityMeter.DayTimeMinValue);
            Assert.Equal(this.nightTimeMeasurement, result.ElectricityMeter.NightTimeMinValue);
            Assert.Equal(this.temperatureMeterName, result.TemperatureMeters.FirstOrDefault().Name);
            Assert.Equal(this.heatingMeasurement, result.TemperatureMeters.FirstOrDefault().HeatingMinValue);
            Assert.Equal(this.coolingMeasurement, result.TemperatureMeters.FirstOrDefault().CoolingMinValue);
            Assert.Equal(1, dbContext.TemperatureMeasurements.Count());
            Assert.Equal(1, dbContext.ElectricityMeasurements.Count());
        }

        [Fact]
        public async Task TestIfAllPeriodsAreReturnedCorrectlyAsync()
        {
            using var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions());
            ITemperatureMetersService temperatureMeters = new TemperatureMetersService(dbContext);
            IElectricityMetersService electricityMeters = new ElectricityMetersService(dbContext);
            IOfficesService officesService = new OfficesService(dbContext, this.tenantsService.Object, electricityMeters, temperatureMeters);

            IMeasurementsService measurementsService =
                new MeasurementsService(dbContext, electricityMeters, temperatureMeters);

            await this.CreateOfficeWithMetersAsync(temperatureMeters, electricityMeters, officesService);

            CreateMeasurementsInputViewModel input = new CreateMeasurementsInputViewModel
            {
                StartOfPeriod = this.periodStartTime,
                EndOfPeriod = this.periodEndTime,
                EndOfLastPeriod = this.periodStartTime.AddDays(-1),
                LastPeriod = "1 декември - 31 декември 2019 г.",
                Offices = new List<OfficeMeasurementsInputViewModel> { this.GetOfficeInputViewModel() },
            };

            await measurementsService.CreateAllMeasurementsAsync(input);

            Assert.Single(measurementsService.GetAllMeasurements());
            var result = measurementsService.GetMeasurementsByStartingPeriod(this.periodStartTime);

            Assert.Equal("1 януари - 31 януари 2020 г.", measurementsService.GetLastPeriodAsText());
            Assert.Equal(new DateTime(2020, 2, 1), measurementsService.GetStartOfNewPeroid());
            Assert.Equal(new DateTime(2020, 2, 29), measurementsService.GetEndOfNewPeriod());
            Assert.Equal(new DateTime(2020, 1, 31), measurementsService.GetEndOfLastPeriod());
        }

        [Fact]
        public async Task TestIfAllInitialMeasurementsAreCreatedCorrectlyAsync()
        {
            using var dbContext = new ApplicationDbContext(this.GetInMemoryDadabaseOptions());
            ITemperatureMetersService temperatureMeters = new TemperatureMetersService(dbContext);
            IElectricityMetersService electricityMeters = new ElectricityMetersService(dbContext);
            IOfficesService officesService = new OfficesService(dbContext, this.tenantsService.Object, electricityMeters, temperatureMeters);

            IMeasurementsService measurementsService =
                new MeasurementsService(dbContext, electricityMeters, temperatureMeters);

            await this.CreateOfficeWithMetersAsync(temperatureMeters, electricityMeters, officesService);

            CreateInitialMeasurementsInputViewModel input = new CreateInitialMeasurementsInputViewModel
            {
                EndOfPeriod = this.periodEndTime,
                Offices = new List<OfficeMeasurementsInputViewModel> { this.GetOfficeInputViewModel() },
            };

            await measurementsService.CreateInitialMeasurementsAsync(input);

            Assert.Equal(1, dbContext.TemperatureMeasurements.Count());
            Assert.Equal(1, dbContext.ElectricityMeasurements.Count());
            Assert.True(measurementsService.IsFirstPeriod());
        }

        private OfficeMeasurementsInputViewModel GetOfficeInputViewModel()
        {
            ElectricityMeasurementInputViewModel electricityMeasurement = new ElectricityMeasurementInputViewModel()
            {
                Name = this.electricityMeterName,
                DayTimeMeasurement = this.dayTimeMeasurement,
                DayTimeMinValue = this.dayTimeMeasurement,
                NightTimeMeasurement = this.nightTimeMeasurement,
                NightTimeMinValue = this.nightTimeMeasurement,
            };
            TemperatureMeasurementInputViewModel temperatureMeasurement = new TemperatureMeasurementInputViewModel
            {
                Name = this.temperatureMeterName,
                HeatingMeasurement = this.heatingMeasurement,
                HeatingMinValue = this.heatingMeasurement,
                CoolingMeasurement = this.coolingMeasurement,
                CoolingMinValue = this.coolingMeasurement,
            };

            OfficeMeasurementsInputViewModel officeViewModel = new OfficeMeasurementsInputViewModel
            {
                Name = "TestOfficeName",
                ElectricityMeter = electricityMeasurement,
                TemperatureMeters = new List<TemperatureMeasurementInputViewModel> { temperatureMeasurement },
            };

            return officeViewModel;
        }

        private async Task CreateOfficeWithMetersAsync(ITemperatureMetersService temperatureMeters, IElectricityMetersService electricityMeters, IOfficesService officesService)
        {
            await officesService.CreateOfficeAsync("TestOfficeName", 50M, 7.2M);
            await electricityMeters.CreateElectricityMeterAsync(this.electricityMeterName, 10M);
            await temperatureMeters.CreateTemperatureMeterAsync(this.temperatureMeterName);
            await officesService.AddElectricityMeterToOfficeAsync(1, this.electricityMeterName);
            await officesService.AddTemperatureMetersToOfficeAsync(1, new List<string> { this.temperatureMeterName });
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
