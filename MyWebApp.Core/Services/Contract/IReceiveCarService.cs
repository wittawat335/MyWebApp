using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.Model;
using static MyWebApp.Core.Model.ViewModels.ReceiveCar.ReceiveCarViewModel;

namespace MyWebApp.Core.Services.Contract
{
    public interface IReceiveCarService
    {
        Task<List<SP_SEARCH_RC_Result>> GetAllbySp(SearchModel model);
        Task<List<T_JOB_REPO>> GetAll();
        Task<bool> SaveFile(List<FileUploadModel> model);
        string GenerateAndDownLoadExcel();
        Task<Response<List<SP_SEARCH_RC_Result>>> GetList(SearchModel model);
        Task<Response<FileUploadModel>> Save(List<FileUploadModel> model);
    }
}
