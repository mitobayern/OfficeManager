namespace OfficeManager.Services
{
    using OfficeManager.Areas.Administration.ViewModels.TemperatureMeters;
    using OfficeManager.Models;
    using System.Linq;

    public interface ITemperatureMetersService
    {
        void CreateTemperatureMeter(CreateTemperatureMeterViewModel inuput);

        IQueryable<TemperatureMeterOutputViewModel> GetAllTemperatureMeters();

        TemperatureMeter GetTemperatureMeterById(int id);

        EditTemperatreMeterViewModel EditTemperatureMeter(int id);

        void UpdateTemperatureMeter(EditTemperatreMeterViewModel input);

        TemperatureMeter GetTemperatureMeterByName(string name);

    }
}
