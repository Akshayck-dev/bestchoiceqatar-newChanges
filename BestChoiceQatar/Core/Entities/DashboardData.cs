using System.Collections.Generic;
using BestChoiceQatar.Core.Entities.Charts;
using BestChoiceQatar.Models;

namespace BestChoiceQatar.Core.Entities
{
    public class DashboardData
    {
        
        public List<ChartItem> OrderSummary { get; set; }

        public List<DonutChartItem> TopHospitals { get; set; }
        public List<RecentAppointments> RecentAppointments { get; set; }          
        public string Lastmonth { get; set; }
       
        public int count { get; set; }
    }
}