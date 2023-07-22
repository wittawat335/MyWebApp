using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyWebApp.Core.Utility.Common;

namespace MyWebApp.Core.Model.ViewModels.Role
{
    public class RoleViewModel
    {
        public RoleViewModel()
        {
            role = new M_ROLE(); ;
            listRole = new List<M_ROLE>();
            listStatus = new List<ddlValuse>();
        }
        public string action { get; set; }
        public bool permEdit { get; set; }
        public bool permView { get; set; }
        public bool permAdd { get; set; }
        public M_ROLE? role { get; set; }
        public List<M_ROLE>? listRole { get; set; }
        public List<ddlValuse>? listStatus { get; set; }
    }

    public class SP_SEARCH_PERMISSION_BY_ROLE_Result
    {
        public string PERM_ID { get; set; }
        public string PERM_PARENT { get; set; }
        public string PERM_TEXT { get; set; }
        public string PERM_SELECT { get; set; }
    }
}
