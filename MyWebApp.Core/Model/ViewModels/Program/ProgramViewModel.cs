using MyWebApp.Core.Domain.Entities;
using MyWebApp.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApp.Core.Model.ViewModels.Program
{
    public class ProgramViewModel
    {
        public M_PROGRAM program { get; set; }
        public ProgramDTO ProgramDTO { get; set; }
        public string? action { get; set; }

        public class SP_GET_MENU_BY_ROLE_Result
        {
            public string? PROG_CODE { get; set; }
            public string? PROG_NAME { get; set; }
            public int? PROG_LEVEL { get; set; }
            public string? PROG_PARENT_CODE { get; set; }
            public string? PROG_TARGET_URL { get; set; }
            public int? PROG_ORDER { get; set; }
            public string? PROG_ICON { get; set; }
        }
        public ProgramViewModel()
        {
            program = new M_PROGRAM();
            ProgramDTO = new ProgramDTO();
        }
    }
}
