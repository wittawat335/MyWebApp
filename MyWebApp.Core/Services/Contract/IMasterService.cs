using MyWebApp.Core.DTO;
using MyWebApp.Core.Model.ViewModels.Master;

namespace MyWebApp.Core.Services.Contract
{
    public interface IMasterService
    {
        Task<List<MasterDTO>> GetAll();
        Task<List<MasterDTO>> Search(MasterViewModel model);
        Task<List<MasterDTO>> GetListByMasterType(string code);
        Task<List<MasterDTO>> GetListMasterActiveOnly();
        Task<MasterDTO> GetByCode(string code);
        Task<bool> Add(MasterDTO model);
        Task<bool> Update(MasterDTO model);
        Task<bool> Delete(string code);
    }
}
