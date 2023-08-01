using MyWebApp.Core.DTO;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.Master;

namespace MyWebApp.Core.Services.Contract
{
    public interface IMasterService
    {
        Task<Response<List<MasterDTO>>> GetAll();
        Task<Response<List<MasterDTO>>> Search(MasterViewModel model);
        Task<List<MasterDTO>> GetListByMasterType(string code);
        Task<List<MasterDTO>> GetListMasterActiveOnly();
        Task<MasterDTO> GetByCode(string code);
        Task<ResponseStatus> Add(MasterDTO model);
        Task<ResponseStatus> Update(MasterDTO model);
        Task<ResponseStatus> Delete(string code);
        Task<MasterViewModel> getIndex();
        Task<MasterViewModel> getDetail(string code, string action);
        Task<ResponseStatus> Save(MasterViewModel model);
    }
}
