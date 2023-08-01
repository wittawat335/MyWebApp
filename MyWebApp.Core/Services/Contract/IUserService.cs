using MyWebApp.Core.DTO;
using MyWebApp.Core.Model;
using MyWebApp.Core.Model.ViewModels.User;
using static MyWebApp.Core.Model.ViewModels.User.UserViewModel;

namespace MyWebApp.Core.Services.Contract
{
    public interface IUserService
    {
        Task<Response<List<UserDTO>>> GetAll();
        Task<Response<List<UserDTO>>> Search(UserViewModel model);
        Task<UserDTO> GetByCode(string code);
        Task<ResponseStatus> Add(UserDTO model);
        Task<bool> AddRole(List<Role> model, string userLogin);
        Task<ResponseStatus> Update(UserDTO model);
        Task<ResponseStatus> Delete(string code);
        Task<bool> CheckDuplicate(string code);
        Task<List<Role>> GetListRoleActiveOnly(string userLogin);
        Task<UserViewModel> getIndex();
        Task<UserViewModel> getDetail(string code, string action);
        Task<ResponseStatus> Save(UserViewModel model);
    }
}
