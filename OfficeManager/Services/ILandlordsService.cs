using OfficeManager.Areas.Administration.ViewModels.Landlords;
using OfficeManager.Models;

namespace OfficeManager.Services
{
    public interface ILandlordsService
    {
        void CreateLandlord(CreateLandlordViewModel landlord);

        LandlordOutputViewModel GetLandlord();

        CreateLandlordViewModel EditLandlord();

        void UpdateLandlord(CreateLandlordViewModel input);
    }
}
