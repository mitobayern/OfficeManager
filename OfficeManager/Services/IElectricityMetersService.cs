namespace OfficeManager.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using OfficeManager.Areas.Administration.ViewModels.ElectricityMeters;
    using OfficeManager.Models;

    public interface IElectricityMetersService
    {
        Task CreateElectricityMeterAsync(string name, decimal powerSupply);

        Task UpdateElectricityMeterAsync(int id, string name, decimal powerSupply);

        Task DeleteElectricityMeterAsync(int id);

        ElectricityMeter GetElectricityMeterById(int id);

        ElectricityMeter GetElectricityMeterByName(string name);

        IQueryable<ElectricityMeterOutputViewModel> GetAllElectricityMeters();
    }
}
