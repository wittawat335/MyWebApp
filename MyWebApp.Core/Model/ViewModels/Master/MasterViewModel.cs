﻿using MyWebApp.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApp.Core.Model.ViewModels.Master
{
    public class MasterViewModel
    {
        public string? masterType { get; set; }
        public string? masterCode { get; set; }
        public string? MasterNameTH { get; set; }
        public string? MasterNameEN { get; set; }
        public string? masterStatus { get; set; }
        public string? action { get; set; }
        public MasterDTO? masterDTO { get; set; }
        public List<MasterDTO>? listMaster { get; set; }
        public bool permEdit { get; set; }
        public bool permView { get; set; }
        public bool permAdd { get; set; }
        public MasterViewModel()
        {
            masterDTO = new MasterDTO();
            listMaster = new List<MasterDTO>();
        }
    }
}
