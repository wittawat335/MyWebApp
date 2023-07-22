using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApp.Core.Model.ViewModels.TodoList
{
    public class SearchViewModel
    {
        public string? refNo { get; set; }
        public string? contractNo { get; set; }
        public string? custCode { get; set; }
        public string? caseCode { get; set; }
        public string? caseStatus { get; set; }
        public string? trafficCode { get; set; }
        public string? adminCode { get; set; }
        public string? oaCode { get; set; }
        public string? adminDateStart { get; set; }
        public string? adminDateEnd { get; set; }
        public DateTime? oaDateStart { get; set; }
        public DateTime? oaDateEnd { get; set; }
        public string? succStatus { get; set; }
        public string? searchDefault { get; set; }
    }
}
