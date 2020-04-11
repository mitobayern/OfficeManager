namespace OfficeManager.Services
{
    using OfficeManager.Areas.Administration.ViewModels.Landlords;
    using System.Threading.Tasks;

    public interface ILandlordsService
    {
        Task CreateLandlordAsync(CreateLandlordViewModel landlord);

        CreateLandlordViewModel GetLandlord();
        
        Task UpdateLandlordAsync(CreateLandlordViewModel input);
    }
}
