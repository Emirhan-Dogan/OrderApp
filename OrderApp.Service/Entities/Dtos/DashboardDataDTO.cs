using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class DashboardDataDTO
    {
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalStorages { get; set; }
        public int TotalEmployees { get; set; }
        public int TotalProducts { get; set; }

        public IEnumerable<DonutChart> DonutChartData { get; set; }
        public IEnumerable<LineChart> LineChartData { get; set; }
    }

    public class DonutChart
    {
        public string Category { get; set; }
        public int Value { get; set; }
    }

    public class LineChart
    {
        public DateTime Date { get; set; }
        public int OrderCount { get; set; }
        public float TotalPrice { get; set; }
    }
}
