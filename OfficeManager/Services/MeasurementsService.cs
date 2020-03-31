namespace OfficeManager.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OfficeManager.Data;
    using OfficeManager.Models;
    using OfficeManager.ViewModels.Measurements;

    public class MeasurementsService : IMeasurementsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IElectricityMetersService electricityMetersService;
        private readonly ITemperatureMetersService temperatureMetersService;
        private readonly ITenantsService tenantsService;

        public MeasurementsService(ApplicationDbContext dbContext,
                                   IElectricityMetersService electricityMetersService,
                                   ITemperatureMetersService temperatureMetersService,
                                   ITenantsService tenantsService)
        {
            this.dbContext = dbContext;
            this.electricityMetersService = electricityMetersService;
            this.temperatureMetersService = temperatureMetersService;
            this.tenantsService = tenantsService;
        }
        public string GetLastPeriodAsText()
        {
            string periodAsText = this.dbContext.ElectricityMeasurements.OrderByDescending(x => x.EndOfPeriod).First().Period;
            return periodAsText;
        }

        public DateTime GetEndOfLastPeriod()
        {
            DateTime endOfLastPeriod = this.dbContext.ElectricityMeasurements
                .OrderByDescending(x => x.EndOfPeriod)
                .FirstOrDefault().EndOfPeriod;

            return endOfLastPeriod;
        }

        public DateTime GetStartOfNewPeroid()
        {
            DateTime startOfNewPeriod = this.GetEndOfLastPeriod().AddDays(1);
            return startOfNewPeriod;
        }

        public DateTime GetEndOfNewPeriod()
        {
            DateTime startOfNewPeriod = this.GetStartOfNewPeroid();
            DateTime endOfNewPeriod;

            int[] monthsWith31Days = new int[] { 1, 3, 5, 7, 8, 10, 12 };
            int[] monthsWith30Days = new int[] { 4, 6, 9, 11 };

            if (monthsWith31Days.Contains(startOfNewPeriod.Month))
            {
                endOfNewPeriod = startOfNewPeriod.AddDays(30);
            }
            else if (monthsWith30Days.Contains(startOfNewPeriod.Month))
            {
                endOfNewPeriod = startOfNewPeriod.AddDays(29);
            }
            else
            {
                if (DateTime.IsLeapYear(startOfNewPeriod.Year))
                {
                    endOfNewPeriod = startOfNewPeriod.AddDays(28);
                }
                else
                {
                    endOfNewPeriod = startOfNewPeriod.AddDays(27);
                }
            }

            return endOfNewPeriod;
        }

        public void CreateElectricityMeasurement(DateTime periodStartTime, DateTime periodEndTime, string elMeterName, decimal dayTimeMeasurement, decimal nightTimeMeasurement)
        {
            DateTime startOfPeriod = periodStartTime;
            DateTime endOfPeriod = periodEndTime;

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

            var currentElectricityMeter = this.electricityMetersService.GetElectricityMeterByName(elMeterName);

            ElectricityMeasurement currentElectricityMeasurement = new ElectricityMeasurement
            {
                StartOfPeriod = startOfPeriod.Date,
                EndOfPeriod = endOfPeriod.Date,
                DayTimeMeasurement = dayTimeMeasurement,
                NightTimeMeasurement = nightTimeMeasurement,
                CreatedOn = DateTime.UtcNow.Date,
                Period = period,
                ElectricityMeter = currentElectricityMeter,
                ElectricityMeterId = currentElectricityMeter.Id,
            };

            this.dbContext.ElectricityMeasurements.Add(currentElectricityMeasurement);
            currentElectricityMeter.ElectricityMeasurements.Add(currentElectricityMeasurement);

            this.dbContext.SaveChanges();
        }

        public void CreateTemperatureMeasurement(DateTime periodStartTime, DateTime periodEndTime, string tempMeterName, decimal heatingMeasurement, decimal coolingMeasurement)
        {
            DateTime startOfPeriod = periodStartTime;
            DateTime endOfPeriod = periodEndTime;

            if (this.dbContext.TemperatureMeasurements.Count() == 0)
            {
                startOfPeriod = endOfPeriod;
            }

            string period;

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


            var currentTemperatureMeter = this.temperatureMetersService.GetTemperatureMeterByName(tempMeterName);

            TemperatureMeasurement currentTemperatureMeasurement = new TemperatureMeasurement
            {
                StartOfPeriod = startOfPeriod.Date,
                EndOfPeriod = endOfPeriod.Date,
                HeatingMeasurement = heatingMeasurement,
                CoolingMeasurement = coolingMeasurement,
                Period = period,
                TemperatureMeter = currentTemperatureMeter,
                TemperatureMeterId = currentTemperatureMeter.Id,
            };

            this.dbContext.TemperatureMeasurements.Add(currentTemperatureMeasurement);
            currentTemperatureMeter.TemperatureMeasurements.Add(currentTemperatureMeasurement);

            this.dbContext.SaveChanges();
        }

        public List<OfficeMeasurementsInputViewModel> GetOfficesWithLastMeasurements()
        {
            var offices = this.dbContext.Offices.Select(x => new OfficeMeasurementsInputViewModel
            {
                Name = x.Name,
                ElectricityMeter = new ElectricityMeasurementInputViewModel
                {
                    Name = x.ElectricityMeter.Name,
                    DayTimeMinValue = x.ElectricityMeter
                                                   .ElectricityMeasurements
                                                   .OrderByDescending(x => x.EndOfPeriod)
                                                   .FirstOrDefault()
                                                   .DayTimeMeasurement,
                    NightTimeMinValue = x.ElectricityMeter
                                                   .ElectricityMeasurements
                                                   .OrderByDescending(x => x.EndOfPeriod)
                                                   .FirstOrDefault()
                                                   .NightTimeMeasurement,
                },
                TemperatureMeters = x.TemperatureMeters.Select(y => new TemperatureMeasurementInputViewModel
                {
                    Name = y.Name,
                    HeatingMinValue = y.TemperatureMeasurements
                                      .OrderByDescending(x => x.EndOfPeriod)
                                      .FirstOrDefault()
                                      .HeatingMeasurement,
                    CoolingMinValue = y.TemperatureMeasurements
                                      .OrderByDescending(x => x.EndOfPeriod)
                                      .FirstOrDefault()
                                      .CoolingMeasurement,
                })
                .OrderBy(x => x.Name)
                .ToList()
            })
            .OrderBy(x => x.Name)
            .ToList();

            return offices;
        }

        public void CreateAllMeasurements(CreateMeasurementsInputViewModel input)
        {
            foreach (var office in input.Offices)
            {
                CreateElectricityMeasurement(input.StartOfPeriod, input.EndOfPeriod, office.ElectricityMeter.Name,
                    office.ElectricityMeter.DayTimeMeasurement, office.ElectricityMeter.NightTimeMeasurement);

                foreach (var temperatureMeter in office.TemperatureMeters)
                {
                    CreateTemperatureMeasurement(input.StartOfPeriod, input.EndOfPeriod, temperatureMeter.Name,
                        temperatureMeter.HeatingMeasurement, temperatureMeter.CoolingMeasurement);
                }
            }
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

        public TenantTemperatureConsummationViewModel GetTenantTemperatureConsummationByPeriod(string tenantCompanyName, string period)
        {
            decimal tenantHeatingConsummation = 0;
            decimal tenantCoolingConsummation = 0;

            Tenant tenant = this.tenantsService.GetTenantByName(tenantCompanyName);

            foreach (var office in tenant.Offices)
            {
                var name = office.Name;

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
