using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApp.Core.Model.ViewModels.Role
{
    public class PermissionViewModel
    {
        public bool permEdit { get; set; }
        public bool permView { get; set; }
        public bool permAdd { get; set; }
        public bool Visibility { get; set; }
        public string RoleCode { get; set; }
        public M_PERMISSION permission { get; set; }
        public List<M_PERMISSION> listPermission { get; set; }
        public PermissionViewModel()
        {
            permission = new M_PERMISSION();
            listPermission = new List<M_PERMISSION>();
        }
    }
}
