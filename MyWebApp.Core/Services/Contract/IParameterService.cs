using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.Parameter;

namespace MyWebApp.Core.Services.Contract
{
    public interface IParameterService
    {
        Task<ParameterViewModel> GetAll(string action);
        Task<ParameterDTO> GetByCode(string code);
        Task<bool> Save(List<M_PARAMETER> model);
        Task<ResponseStatus> postSave(List<M_PARAMETER> Para);
    }
}
