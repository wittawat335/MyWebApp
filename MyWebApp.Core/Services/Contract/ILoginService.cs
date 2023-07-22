using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.DTO;

namespace MyWebApp.Core.Services.Contract
{
    public interface ILoginService
    {
        Task<UserDTO> findUser(string userLogin);
        Task<List<M_USER_ROLE>> getListUserRole(string userLogin = "");
        Task<UserDTO> getUser(string userLogin, string password);
        Task<List<T_CURRENT_LOGIN>> getListCurrentRole(string userLogin, string ip = "");
        Task<UserRoleDTO> getCurrentRoleList2(string userLogin, string roleCode);
        Task<M_ROLE> getRoleBycurrentRole(string roleCode);
        Task<bool> CheckUserPassOrAD(string userLogin, string password);
        Task<bool> SignIn(string userLogin, string role);
        Task<long> InsertCurrentLogin();
       
    }
}
