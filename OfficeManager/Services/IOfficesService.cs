namespace OfficeManager.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using OfficeManager.Areas.Administration.ViewModels.Offices;
    using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;
    using OfficeManager.Models;

    public interface IOfficesService
    {
        Task CreateOfficeAsync(string name, decimal area, decimal rentPerSqMeter);

        EditOfficeViewModel EditOffice(int id);

        Task UpdateOfficeAsync(int id, string name, decimal area, decimal rentPerSqMeter);

        Task AddOfficesToTenantAsync(int id, List<string> offices);

        Task RemoveOfficesFromTenantAsync(int id, List<string> offices);

        Task AddTemperatureMetersToOfficeAsync(int id, List<string> temperatureMeters);

        Task RemoveTemperatureMetersFromOfficeAsync(int id, List<string> temperatureMeters);

        Task AddElectricityMeterToOfficeAsync(int id, string electricityMeterName);

        Task RemoveElectricityMeterFromOfficeAsync(int id);

        Task DeleteOfficeAsync(int id);

        Office GetOfficeById(int id);

        Office GetOfficeByName(string name);

        IQueryable<EditOfficeViewModel> GetAllAvailableOffices();

        IQueryable<OfficeOutputViewModel> GetAllOffices();

        IEnumerable<TemperatureMeterOutputViewModel> GetOfficeTemperatureMeters(int id);
    }
}
