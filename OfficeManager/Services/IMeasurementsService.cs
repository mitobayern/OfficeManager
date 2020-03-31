using OfficeManager.Models;
using OfficeManager.ViewModels.Measurements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManager.Services
{
    public interface IMeasurementsService
    {
        string GetLastPeriodAsText();

        DateTime GetEndOfLastPeriod();

        DateTime GetStartOfNewPeroid();

        DateTime GetEndOfNewPeriod();

        void CreateAllMeasurements(CreateMeasurementsInputViewModel input);

        List<OfficeMeasurementsInputViewModel> GetOfficesWithLastMeasurements();

        void CreateElectricityMeasurement(DateTime periodStartTime, DateTime periodEndTime, string elMeterName, decimal dayTimeMeasurement, decimal nightTimeMeasurement);

        void CreateTemperatureMeasurement(DateTime periodStartTime, DateTime periodEndTime, string tempMeterName, decimal heatingMeasurement, decimal coolingMeasurement);

        TenantElectricityConsummationViewModel GetTenantElectricityConsummationByPeriod(Tenant tenant, string period);

        TenantTemperatureConsummationViewModel GetTenantTemperatureConsummationByPeriod(string tenantCompanyName, string period);
    }
}
