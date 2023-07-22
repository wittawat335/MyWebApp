using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.DTO;

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
        Task<bool> Update(M_PROGRAM model);
        Task<bool> Delete(string code);
    }
}
