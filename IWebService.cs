using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment_SoftwareDevelopment
{
    interface IWebService
    {
        void LogFireAlarm(string logDetails);
        void LogEngineerRerquired(string logDetails);
    }
}
