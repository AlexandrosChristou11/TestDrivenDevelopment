using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment_SoftwareDevelopment
{
    interface IDoorManager
    {
        bool SetEngineerRequired(bool needsEngineer);
        string GetStatus();
        bool OpenAllDoors();
        bool LockAllDoors();
        bool OpenDoor(int doorID);
        bool LockDoor(int doorID);
    }
}
