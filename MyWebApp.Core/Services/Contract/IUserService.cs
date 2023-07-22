using MyWebApp.Core.DTO;
using MyWebApp.Core.Model.ViewModels.User;
using static MyWebApp.Core.Model.ViewModels.User.UserViewModel;

namespace MyWebApp.Core.Services.Contract
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAll();
        Task<List<UserDTO>> Search(UserViewModel model);
        Task<UserDTO> GetByCode(string code);
        Task<bool> Add(UserDTO model);
        Task<bool> AddRole(List<Role> model, string userLogin);
        Task<bool> Update(UserDTO model);
        Task<bool> Delete(string code);
        Task<bool> CheckDuplicate(string code);
        Task<List<Role>> GetListRoleActiveOnly(string userLogin);
    }
}
