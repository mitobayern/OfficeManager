namespace OfficeManager.Services
{
    using System.Threading.Tasks;
    using OfficeManager.Areas.Administration.ViewModels.Landlords;

    public interface ILandlordsService
    {
        Task CreateLandlordAsync(CreateLandlordViewModel landlord);

        Task UpdateLandlordAsync(CreateLandlordViewModel input);

        CreateLandlordViewModel GetLandlord();
    }
}
