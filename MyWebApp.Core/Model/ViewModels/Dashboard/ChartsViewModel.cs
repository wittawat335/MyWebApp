using MyWebApp.Core.Domain.Entities;

namespace MyWebApp.Core.Model.ViewModels.Dashboard
{
    public class ChartsViewModel
    {

    }
    public class ChartsSP
    {
        public ChartsSP()
        {
            repo = new List<T_JOB_REPO>();
        }

        public string? ID { get; set; }
        public string? TEXT { get; set; }
        public int? VALUE { get; set; }
        public string? COLOR { get; set; }
        List<T_JOB_REPO> repo { get; set; }
    }
    public class Charts
    {
        public string? id { get; set; }
        public string? text { get; set; }
        public int value { get; set; }
        public string? color { get; set; }
    }
}
