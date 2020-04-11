namespace OfficeManager.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;
    using OfficeManager.Models;

    public interface ITemperatureMetersService
    {
        Task CreateTemperatureMeterAsync(string name);

        Task UpdateTemperatureMeterAsync(int id, string name);

        EditTemperatreMeterViewModel EditTemperatureMeter(int id);

        TemperatureMeter GetTemperatureMeterById(int id);

        TemperatureMeter GetTemperatureMeterByName(string name);

        IQueryable<TemperatureMeterOutputViewModel> GetAllTemperatureMeters();
    }
}
