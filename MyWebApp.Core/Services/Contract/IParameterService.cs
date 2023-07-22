using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.DTO;

namespace MyWebApp.Core.Services.Contract
{
    public interface IParameterService
    {
        Task<List<ParameterDTO>> GetAll();
        Task<ParameterDTO> GetByCode(string code);
        Task<bool> Save(List<M_PARAMETER> model);
    }
}
