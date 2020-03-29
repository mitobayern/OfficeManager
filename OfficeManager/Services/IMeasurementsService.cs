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
        void CreateElectricityMeasurements(CreateElectricityMeasurementsInputViewModel input);

        void CreateTemperatureMeasurements(CreateTemperatureMeasurementsInputViewModel input);

        TenantElectricityConsummationViewModel GetTenantElectricityConsummationByPeriod(Tenant tenant, string period);

        TenantTemperatureConsummationViewModel GetTenantTemperatureConsummationByPeriod(string tenantCompanyName, string period);
    }
}
