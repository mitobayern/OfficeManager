using OfficeManager.Areas.Administration.ViewModels.ElectricityMeters;
using OfficeManager.Models;
using System.Linq;

namespace OfficeManager.Services
{
    public interface IElectricityMetersService
    {
        void CreateElectricityMeter(CreateElectricityMeterViewModel input);

        IQueryable<ElectricityMeterOutputViewModel> GetAllElectricityMeters();

        ElectricityMeter GetElectricityMeterById(int id);

        ElectricityMeter GetElectricityMeterByName(string name);

        void UpdateElectricityMeter(ElectricityMeterOutputViewModel input);
    }
}
