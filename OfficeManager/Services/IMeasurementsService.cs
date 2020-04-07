namespace OfficeManager.Services
{
    using OfficeManager.Models;
    using OfficeManager.ViewModels.Measurements;

    using System;
    using System.Collections.Generic;

    public interface IMeasurementsService
    {
        bool IsFirstPeriod();

        string GetLastPeriodAsText();

        DateTime GetEndOfLastPeriod();

        DateTime GetStartOfNewPeroid();

        DateTime GetEndOfNewPeriod();

        void CreateAllMeasurements(CreateMeasurementsInputViewModel input);

        void CreateInitialMeasurements(CreateInitialMeasurementsInputViewModel input);
        
        List<OfficeMeasurementsInputViewModel> GetOfficesWithLastMeasurements();

        void CreateInitialElectricityMeasurement(DateTime periodEndTime, string elMeterName, decimal dayTimeMeasurement, decimal nightTimeMeasurement);

        void CreateInitialTemperatureMeasurement(DateTime periodEndTime, string tempMeterName, decimal heatingMeasurement, decimal coolingMeasurement);
        
        void CreateElectricityMeasurement(DateTime periodStartTime, DateTime periodEndTime, string elMeterName, decimal dayTimeMeasurement, decimal nightTimeMeasurement);

        void CreateTemperatureMeasurement(DateTime periodStartTime, DateTime periodEndTime, string tempMeterName, decimal heatingMeasurement, decimal coolingMeasurement);
    }
}
