using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment_SoftwareDevelopment
{
    interface IFireAlarmManager
    {
        bool SetEngineerRequired(bool needsEngineer);
        string GetStatus();
        void SetAlarm(bool isActive);
    }
}
