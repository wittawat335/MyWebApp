using MyWebApp.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApp.Core.Model.ViewModels.Parameter
{
    public class ParameterViewModel
    {
        public List<ParameterDTO> ParameterList { get; set; }
        public string action { get; set; }
        public ParameterViewModel()
        {
            ParameterList = new List<ParameterDTO>();
        }
    }
}
