using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApp.Core.Model.ViewModels
{
    public class TreeViewInAspNetCor
    {
        public class City
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int StateId { get; set; }
            public virtual State State { get; set; }
        }

        public class State
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public virtual List<City> City { get; set; }
        }

        public class TreeViewNode
        {
            public string id { get; set; }
            public string parent { get; set; }
            public string text { get; set; }
            public string icon { get; set; }
            public OptState state { get; set; }
        }

        public class OptState
        {
            public bool opened { get; set; }
            public bool selected { get; set; }
        }
    }
}
