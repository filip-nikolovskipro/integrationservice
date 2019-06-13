using System;
using System.Collections.Generic;
using System.Text;

namespace BackgroundWorker
{
    public class DynamicQueryRequest
    {
        public int UserID { get; set; }
        public string ConditionsString { get; set; }
        public Period Period { get; set; }
        public List<string> RequestedGroups { get; set; }
        public string RequestedColumns { get; set; }
        public int CommandTimoutSeconds { get; set; }
     }

    public class Period
    {
        public Nullable<DateTime> BeginPeriod { get; set; }
        public Nullable<DateTime> EndPeriod { get; set; }
        public Nullable<TimeSpan> TimeOfDateBegin { get; set; }
        public Nullable<TimeSpan> TimeOfDateEnd { get; set; }
        public string Type { get; set; }
        public int Days { get; set; }
    }
}
