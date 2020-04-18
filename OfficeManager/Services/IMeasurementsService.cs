namespace OfficeManager.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using OfficeManager.ViewModels.Measurements;

    public interface IMeasurementsService
    {
        Task CreateInitialElectricityMeasurementAsync(DateTime periodEndTime, string elMeterName, decimal dayTimeMeasurement, decimal nightTimeMeasurement);

        Task CreateElectricityMeasurementAsync(DateTime periodStartTime, DateTime periodEndTime, string elMeterName, decimal dayTimeMeasurement, decimal nightTimeMeasurement);

        Task CreateInitialTemperatureMeasurementAsync(DateTime periodEndTime, string tempMeterName, decimal heatingMeasurement, decimal coolingMeasurement);

        Task CreateTemperatureMeasurementAsync(DateTime periodStartTime, DateTime periodEndTime, string tempMeterName, decimal heatingMeasurement, decimal coolingMeasurement);

        Task CreateInitialMeasurementsAsync(CreateInitialMeasurementsInputViewModel input);

        Task CreateAllMeasurementsAsync(CreateMeasurementsInputViewModel input);

        Task EditElectricityMeasurementAsync(DateTime periodStartTime, DateTime periodEndTime, string elMeterName, decimal dayTimeMeasurement, decimal nightTimeMeasurement);

        Task EditTemperatureMeasurementAsync(DateTime periodStartTime, DateTime periodEndTime, string tempMeterName, decimal heatingMeasurement, decimal coolingMeasurement);

        Task EditAllMeasurementsAsync(CreateMeasurementsInputViewModel input);

        IQueryable<AllMeasurementsOutputViewModel> GetAllMeasurements();

        CreateMeasurementsInputViewModel GetMeasurementsByStartingPeriod(DateTime startOfPeriod);

        bool IsFirstPeriod();

        string GetLastPeriodAsText();

        DateTime GetEndOfLastPeriod();

        DateTime GetStartOfNewPeroid();

        DateTime GetEndOfNewPeriod();

        List<OfficeMeasurementsInputViewModel> GetOfficesWithLastMeasurements();
    }
}
