namespace OfficeManager.Services
{
    using System;
    using System.Linq;
    using OfficeManager.Data;
    using OfficeManager.Models;
    using OfficeManager.ViewModels.Measurements;

    public class MeasurementsService : IMeasurementsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IElectricityMetersService electricityMetersService;
        private readonly ITemperatureMetersService temperatureMetersService;

        public MeasurementsService(ApplicationDbContext dbContext,
                                   IElectricityMetersService electricityMetersService,
                                   ITemperatureMetersService temperatureMetersService)
        {
            this.dbContext = dbContext;
            this.electricityMetersService = electricityMetersService;
            this.temperatureMetersService = temperatureMetersService;
        }
        public void CreateElectricityMeasurements(CreateElectricityMeasurementsInputViewModel input)
        {
            DateTime startOfPeriod = input.StarOfPeriod.Date;
            DateTime endOfPeriod = input.EndOfPeriod.Date;

            if (this.dbContext.ElectricityMeasurements.Count() == 0)
            {
                startOfPeriod = endOfPeriod;
            }

            string period = string.Empty;

            if (startOfPeriod.Year == endOfPeriod.Year)
            {
                period = startOfPeriod.ToString("d MMMM", new System.Globalization.CultureInfo("bg-BG"))
                    + " - "
                    + endOfPeriod.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("bg-BG"));
            }
            else
            {
                period = startOfPeriod.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("bg-BG"))
                    + " - "
                    + endOfPeriod.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("bg-BG"));
            }



            foreach (var electricityMeter in input.ElectricityMeters)
            {
                var currentElectricityMeter = this.electricityMetersService.GetElectricityMeterByName(electricityMeter.Name);

                ElectricityMeasurement currentElectricityMeasurement = new ElectricityMeasurement
                {
                    StartOfPeriod = startOfPeriod.Date,
                    EndOfPeriod = endOfPeriod.Date,
                    DayTimeMeasurement = electricityMeter.DayTimeMeasurement,
                    NightTimeMeasurement = electricityMeter.NightTimeMeasurement,
                    CreatedOn = DateTime.UtcNow.Date,
                    Period = period,
                    ElectricityMeter = currentElectricityMeter,
                    ElectricityMeterId = currentElectricityMeter.Id,
                };

                this.dbContext.ElectricityMeasurements.Add(currentElectricityMeasurement);
                currentElectricityMeter.ElectricityMeasurements.Add(currentElectricityMeasurement);
            }

            this.dbContext.SaveChanges();
        }

        public void CreateTemperatureMeasurements(CreateTemperatureMeasurementsInputViewModel input)
        {
            DateTime startOfPeriod = input.StarOfPeriod.Date;
            DateTime endOfPeriod = input.EndOfPeriod.Date;

            if (this.dbContext.TemperatureMeasurements.Count() == 0)
            {
                startOfPeriod = endOfPeriod;
            }

            string period = string.Empty;

            if (startOfPeriod.Year == endOfPeriod.Year)
            {
                period = startOfPeriod.ToString("d MMMM", new System.Globalization.CultureInfo("bg-BG"))
                    + " - "
                    + endOfPeriod.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("bg-BG"));
            }
            else
            {
                period = startOfPeriod.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("bg-BG"))
                    + " - "
                    + endOfPeriod.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("bg-BG"));
            }

            foreach (var temperatureMeter in input.TemperatureMeters)
            {
                var currentTemperatureMeter = this.temperatureMetersService.GetTemperatureMeterByName(temperatureMeter.Name);

                TemperatureMeasurement currentTemperatureMeasurement = new TemperatureMeasurement
                {
                    StartOfPeriod = startOfPeriod.Date,
                    EndOfPeriod = endOfPeriod.Date,
                    HeatingMeasurement = temperatureMeter.HeatingMeasurement,
                    CoolingMeasurement = temperatureMeter.CoolingMeasurement,
                    Period = period,
                    TemperatureMeter = currentTemperatureMeter,
                    TemperatureMeterId = currentTemperatureMeter.Id,
                };

                this.dbContext.TemperatureMeasurements.Add(currentTemperatureMeasurement);
                currentTemperatureMeter.TemperatureMeasurements.Add(currentTemperatureMeasurement);
            }

            this.dbContext.SaveChanges();
        }

        public TenantElectricityConsummationViewModel GetTenantElectricityConsummationByPeriod(Tenant tenant, string period)
        {
            decimal tenantDayTimeElectricityConsummation = 0;
            decimal tenantNightTimeElectricityConsummation = 0;


            foreach (var office in tenant.Offices)
            {
                var endOfPeriodElectricityMeasurement = this.dbContext.ElectricityMeasurements
                    .FirstOrDefault(x => x.ElectricityMeter.Office == office && x.Period == period);

                var startOfPeriodElectricityMeasurement = this.dbContext.ElectricityMeasurements
                    .Where(x => x.ElectricityMeterId == endOfPeriodElectricityMeasurement.ElectricityMeterId &&
                    x.Id < endOfPeriodElectricityMeasurement.Id)
                    .OrderByDescending(x => x.Id)
                    .First();

                decimal dayTimeOfficeConsumation =
                    endOfPeriodElectricityMeasurement.DayTimeMeasurement - startOfPeriodElectricityMeasurement.DayTimeMeasurement;

                decimal nightTimeOfficeConsumation =
                    endOfPeriodElectricityMeasurement.NightTimeMeasurement - startOfPeriodElectricityMeasurement.NightTimeMeasurement;

                tenantDayTimeElectricityConsummation += dayTimeOfficeConsumation;
                tenantNightTimeElectricityConsummation += nightTimeOfficeConsumation;
            }

            var tenantElectricityConsumation = new TenantElectricityConsummationViewModel
            {
                DayTimeConsummation = tenantDayTimeElectricityConsummation,
                NightTimeConsummation = tenantNightTimeElectricityConsummation
            };

            return tenantElectricityConsumation;
        }

        public TenantTemperatureConsummationViewModel GetTenantTemperatureConsummationByPeriod(Tenant tenant, string period)
        {
            decimal tenantHeatingConsummation = 0;
            decimal tenantCoolingConsummation = 0;


            foreach (var office in tenant.Offices)
            {
                foreach (var temperatureMeter in office.TemperatureMeters)
                {
                    var endOfPeriodTemperatureMeasurement = this.dbContext.TemperatureMeasurements
                        .FirstOrDefault(x => x.TemperatureMeter == temperatureMeter && x.Period == period);

                    var startOfPeriodTemperatureMeasurement = this.dbContext.TemperatureMeasurements
                        .Where(x => x.TemperatureMeterId == endOfPeriodTemperatureMeasurement.TemperatureMeterId &&
                        x.Id < endOfPeriodTemperatureMeasurement.Id)
                        .OrderByDescending(x => x.Id)
                        .First();

                    decimal heatingConsumation =
                        endOfPeriodTemperatureMeasurement.HeatingMeasurement - startOfPeriodTemperatureMeasurement.HeatingMeasurement;

                    decimal coolingConsumation =
                        endOfPeriodTemperatureMeasurement.CoolingMeasurement - startOfPeriodTemperatureMeasurement.CoolingMeasurement;

                    tenantHeatingConsummation += heatingConsumation;
                    tenantCoolingConsummation += coolingConsumation;
                }


            }

            var tenantTemperatureConsumation = new TenantTemperatureConsummationViewModel
            {
                HeatingConsummation = tenantHeatingConsummation,
                CoolingConsummation = tenantCoolingConsummation
            };

            return tenantTemperatureConsumation;
        }
    }
}
