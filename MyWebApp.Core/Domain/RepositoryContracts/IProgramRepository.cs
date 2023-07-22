using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.DTO;
using MyWebApp.Core.Model;

namespace MyWebApp.Core.Domain.RepositoryContracts
{
    public interface IProgramRepository 
    {
        string GetStringBySpAsync(string code);
        Task<List<ProgramDTO>> GetMenuByRole(string role);
        Task<string> GetMenuDefault(string code);
    }
}
