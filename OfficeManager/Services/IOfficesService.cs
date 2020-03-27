using OfficeManager.Areas.Administration.ViewModels.Offices;
using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;
using OfficeManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManager.Services
{
    public interface IOfficesService
    {
        void CreateOffice(CreateOfficeViewModel input);

        IQueryable<OfficeOutputViewModel> GetAllOffices();

        IQueryable<EditOfficeViewModel> GetAllAvailableOffices();

        void AddOfficesToTenant(int id, List<string> offices);

        void RemoveOfficeFromTenant(int id, List<string> offices);

        void AddTemperatureMetersToOffice(int id, List<string> temperatureMeters);

        void RemoveTemperatureMetersFromOffice(int id, List<string> temperatureMeters);

        void AddElectricityMeterToOffice(int id, string electricityMeterName);

        Office GetOfficeByName(string name);

        Office GetOfficeById(int id);

        EditOfficeViewModel EditOffice(int id);

        void UpdateOffice(int id, string name, decimal area, decimal rentPerSqMeter);

        IEnumerable<TemperatureMeterOutputViewModel> GetOfficeTemperatureMeters(int id);
    }
}
