using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Model.Interface
{
    /// <summary>
    /// Interface for Station
    /// </summary>
    public interface IScheduleModel
    {
        string InsertSchedule(Schedule p_Schedule);
    }
}
