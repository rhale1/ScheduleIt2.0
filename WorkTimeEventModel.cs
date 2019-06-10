using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleIt2._0.Models
{
    public class WorkTimeEventModel : EventModel
    {
        /// <summary>
        /// Model for when a user clocks in or out, stored in the Events Table
        /// </summary>
        /// 
        public WorkTimeEventModel()
        {
        }
        //Clocks the user in by creating a new worktimeevent
        public WorkTimeEventModel(ApplicationUser user, string message, DateTime? dt = null)
        {
            User = user;
            EventId = Guid.NewGuid();
            if (dt.HasValue) StartTime = dt.Value;
            else StartTime = DateTime.Now;
            Message = message;
        }
    }
}
