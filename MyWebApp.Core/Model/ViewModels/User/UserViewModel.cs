using MyWebApp.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApp.Core.Model.ViewModels.User
{
    public class UserViewModel
    {
        public List<Role> UserRoleList { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? loginId { get; set; }
        public string? status { get; set; }
        public string? action { get; set; }
        public UserDTO? userDTO { get; set; }
        public UserRoleDTO? userRoleDTO { get; set; }
        public List<Role> roleSelect { get; set; }
        public bool permEdit { get; set; }
        public bool permView { get; set; }
        public bool permAdd { get; set; }
        public UserViewModel()
        {
            userDTO = new UserDTO();
            userRoleDTO = new UserRoleDTO();
            UserRoleList = new List<Role>();
            roleSelect = new List<Role>();
        }
        public class Role
        {
            public string? RoleCode { get; set; }
            public string? RoleName { get; set; }
            public string? UserLogin { get; set; }
            public bool RoleFlag { get; set; }
        }
    }
}
