using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.Program;

namespace MyWebApp.Core.Services.Contract
{
    public interface IProgramService 
    {
        List<ProgramDTO> GetByRole(string code);
        Task<List<ProgramDTO>> GetByRoleAsync(string code);
        Task<string> GetMeneDefaultAsync(string code);
        Task<List<M_PROGRAM>> GetAll();
        Task<M_PROGRAM> GetByCode(string code);
        Task<bool> Add(M_PROGRAM model);
        Task<bool> Update(ProgramDTO model);
        Task<bool> Delete(string code);
        Task<Response<List<M_PROGRAM>>> GetList();
        Task<ProgramViewModel> Detail(string code, string action);
        Task<ResponseStatus> Save(ProgramViewModel model);
        Task<ResponseStatus> sendDelete(string code);
    }
}
