namespace OfficeManager.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using OfficeManager.Data;
    using OfficeManager.Models;
    using OfficeManager.ViewModels.Measurements;

    public class MeasurementsService : IMeasurementsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IElectricityMetersService electricityMetersService;
        private readonly ITemperatureMetersService temperatureMetersService;

        public MeasurementsService(
            ApplicationDbContext dbContext,
            IElectricityMetersService electricityMetersService,
            ITemperatureMetersService temperatureMetersService)
        {
            this.dbContext = dbContext;
            this.electricityMetersService = electricityMetersService;
            this.temperatureMetersService = temperatureMetersService;
        }

        public async Task CreateInitialElectricityMeasurementAsync(DateTime periodEndTime, string elMeterName, decimal dayTimeMeasurement, decimal nightTimeMeasurement)
        {
            DateTime startOfPeriod = periodEndTime;
            DateTime endOfPeriod = periodEndTime;

            string period = "Starting period "
                            + endOfPeriod.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("bg-BG"))
                            + " г.";

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

            await this.dbContext.ElectricityMeasurements.AddAsync(currentElectricityMeasurement);
            currentElectricityMeter.ElectricityMeasurements.Add(currentElectricityMeasurement);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task CreateElectricityMeasurementAsync(DateTime periodStartTime, DateTime periodEndTime, string elMeterName, decimal dayTimeMeasurement, decimal nightTimeMeasurement)
        {
            DateTime startOfPeriod = periodStartTime;
            DateTime endOfPeriod = periodEndTime;

            string period;

            if (startOfPeriod.Year == endOfPeriod.Year)
            {
                period = startOfPeriod.ToString("d MMMM", new System.Globalization.CultureInfo("bg-BG"))
                    + " - "
                    + endOfPeriod.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("bg-BG"))
                    + " г.";
            }
            else
            {
                period = startOfPeriod.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("bg-BG"))
                    + " г. - "
                    + endOfPeriod.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("bg-BG"))
                    + " г.";
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

            await this.dbContext.ElectricityMeasurements.AddAsync(currentElectricityMeasurement);
            currentElectricityMeter.ElectricityMeasurements.Add(currentElectricityMeasurement);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task CreateInitialTemperatureMeasurementAsync(DateTime periodEndTime, string tempMeterName, decimal heatingMeasurement, decimal coolingMeasurement)
        {
            DateTime endOfPeriod = periodEndTime;
            DateTime startOfPeriod = periodEndTime;

            string period = "Starting period "
                            + endOfPeriod.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("bg-BG"))
                            + " г.";

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

            await this.dbContext.TemperatureMeasurements.AddAsync(currentTemperatureMeasurement);
            currentTemperatureMeter.TemperatureMeasurements.Add(currentTemperatureMeasurement);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task CreateTemperatureMeasurementAsync(DateTime periodStartTime, DateTime periodEndTime, string tempMeterName, decimal heatingMeasurement, decimal coolingMeasurement)
        {
            DateTime startOfPeriod = periodStartTime;
            DateTime endOfPeriod = periodEndTime;

            string period;

            if (startOfPeriod.Year == endOfPeriod.Year)
            {
                period = startOfPeriod.ToString("d MMMM", new System.Globalization.CultureInfo("bg-BG"))
                    + " - "
                    + endOfPeriod.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("bg-BG"))
                    + " г.";
            }
            else
            {
                period = startOfPeriod.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("bg-BG"))
                    + " г. - "
                    + endOfPeriod.ToString("d MMMM yyyy", new System.Globalization.CultureInfo("bg-BG"))
                    + " г.";
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

            await this.dbContext.TemperatureMeasurements.AddAsync(currentTemperatureMeasurement);
            currentTemperatureMeter.TemperatureMeasurements.Add(currentTemperatureMeasurement);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task CreateInitialMeasurementsAsync(CreateInitialMeasurementsInputViewModel input)
        {
            foreach (var office in input.Offices)
            {
                await this.CreateInitialElectricityMeasurementAsync(
                    input.EndOfPeriod,
                    office.ElectricityMeter.Name,
                    office.ElectricityMeter.DayTimeMeasurement,
                    office.ElectricityMeter.NightTimeMeasurement);

                foreach (var temperatureMeter in office.TemperatureMeters)
                {
                    await this.CreateInitialTemperatureMeasurementAsync(
                        input.EndOfPeriod,
                        temperatureMeter.Name,
                        temperatureMeter.HeatingMeasurement,
                        temperatureMeter.CoolingMeasurement);
                }
            }
        }

        public async Task CreateAllMeasurementsAsync(CreateMeasurementsInputViewModel input)
        {
            foreach (var office in input.Offices)
            {
                await this.CreateElectricityMeasurementAsync(
                    input.StartOfPeriod,
                    input.EndOfPeriod,
                    office.ElectricityMeter.Name,
                    office.ElectricityMeter.DayTimeMeasurement,
                    office.ElectricityMeter.NightTimeMeasurement);

                foreach (var temperatureMeter in office.TemperatureMeters)
                {
                    await this.CreateTemperatureMeasurementAsync(
                        input.StartOfPeriod,
                        input.EndOfPeriod,
                        temperatureMeter.Name,
                        temperatureMeter.HeatingMeasurement,
                        temperatureMeter.CoolingMeasurement);
                }
            }
        }

        public bool IsFirstPeriod()
        {
            if (this.GetLastPeriodAsText().StartsWith("Starting period"))
            {
                return true;
            }

            return false;
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
            DateTime startOfNewPeriod = this.GetEndOfLastPeriod();

            if (!this.GetLastPeriodAsText().StartsWith("Starting period"))
            {
                startOfNewPeriod = startOfNewPeriod.AddDays(1);
            }

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
                .ToList(),
            })
            .OrderBy(x => x.Name)
            .ToList();

            return offices;
        }
    }
}
