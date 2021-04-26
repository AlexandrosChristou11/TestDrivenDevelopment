using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment_SoftwareDevelopment
{
    interface ILightManager
    {
        bool SetEngineerRequired(bool needsEngineer);
        string GetStatus();
        void SetAllLights(bool isOn);
        void SetLight(bool isOn, int lightID);
    }
}
