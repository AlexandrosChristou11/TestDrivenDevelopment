// Alexandros Christou - 19Feb21
//The BuildingController class is responsible for
//managing the various smart systems in the
//building by communicating with three different
//dependencies(LightManager, DoorManager and
//FireAlarmManager).

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Assignment_SoftwareDevelopment
{
    class BuildingController
    {

        // (a) Member Variables:

        private const int MIN_LIMIT_ID = 1;
        private const int MAX_LIMIT_ID = 5;

        private const string STATE_CLOSED = "closed";
        private const string STATE_OUT_OF_HOURS = "out of hours";
        private const string STATE_OPEN = "open";
        private const string STATE_FIRE_DRILL = "fire drill";
        private const string STATE_FIRE_ALARM = "fire alarm";

        private string buildingID;
        private string currentState;
        private string historyState;

        private ILightManager _lightManager;
        private IFireAlarmManager _fireAlarmManager;
        private IDoorManager _doorManager;
        private IWebService _webService;
        private IEmailService _emailService;




        // (b) Member Methods:
        // (i) PUBLIC:

        // Constructors ..
        public BuildingController(string id)
        {
            // validates that id is within the limits
            checkIdIsWithinLimits(id);      // (L1R1)

            this.buildingID = id.ToLower();     // converts Uppercase letters -> Lowercase ..
            this.currentState = "out of hours";      //(L1R5)
            this.historyState = "";

            ThrowExceptionIfIdIsNull(id); // (L1R1)

        }

        // throws exception if id is null 
        private void ThrowExceptionIfIdIsNull(string id)
        {
            if (buildingID == null)
            {
                throw new NullReferenceException("NullRefence Exception: Object cannot be null!");
            }

        }

        public BuildingController(string id, string startState)
        {
            checkIdIsWithinLimits(id);

            this.buildingID = id.ToLower();
            this.currentState = startState.ToLower();
            ThrowExceptionStateInvalid();           //(L2R3)

            ThrowExceptionIfIdIsNull(id); // (L1R1)


        }

        //(L3R1)
        public BuildingController(string id, ILightManager iLightManager,
                                            IFireAlarmManager iFireAlarmManager,
                                             IDoorManager iDoorManager, IWebService iWebService,
                                             IEmailService iEmailService)
        {
            checkIdIsWithinLimits(id);

            this.buildingID = id.ToLower();
            this._lightManager = iLightManager;
            this._fireAlarmManager = iFireAlarmManager;
            this._doorManager = iDoorManager;
            this._webService = iWebService;
            this._emailService = iEmailService;

            this.currentState = STATE_OUT_OF_HOURS; //(L1R2)

            ThrowExcpetionIfAnObjectIsNull(); //(L3R1)

            ThrowExceptionStateInvalid();           //(L2R3)

            ThrowExceptionIfIdIsNull(id); // (L1R1)

        }


        // throws an exception if an object is null / invalid ..
        private void ThrowExcpetionIfAnObjectIsNull()
        {
           if (_lightManager == null ||
                _fireAlarmManager == null ||
                _emailService == null ||
                _doorManager == null ||
                _webService == null)
            {
               throw new NullReferenceException("NullRefence Exception: Object cannot be null!");
                
            }
        }

        //(L3R2)
        public string GetStatusReport()
        {
            checkForFaultyReports();        //(L4R3)
            return _lightManager.GetStatus() + _doorManager.GetStatus() + _fireAlarmManager.GetStatus(); //(L3R3)
        }


        // return true if state name is lower case .
        public bool AnalayzeStateLetterForm()
        {
            if (validInitalStates())
            { // (a) valid state name ..
                int count = 0;
                char[] a = currentState.ToCharArray();
                for (int i = 0; i < a.Length; i++)
                {
                    if (char.IsLower(a[i]) || a[i] == ' ')
                    {
                        count++;
                    }
                }
                return (a.Length == count) ? true : false;
            }
            else
            {
                // (b) invalid state name ..
                return false;
            }

        }

        // returns true if state is within valid states .. (closed, open,out of hours)
        public void ThrowExceptionStateInvalid()
        {
            if (!validInitalStates())
            {
                throw new ArgumentException("Argument Exception: BuildingController can only be" +
                                 " initialised to the following states 'open', 'closed', 'out of hours'");
            }
        }

        // Getters, Setters ..

        public string GetBuildingID() { return this.buildingID; } //(L1R2)
        public void SetBuildingID(string id) { this.buildingID = id.ToLower(); } // converts Uppercase letters -> Lowercase ..
        public string GetCurrentState() { return this.currentState; }       // (L1R6)

        // - check that the string supplied is a valid state
        //      (a) valid state -> assign it..
        //      (b) not valid state -> return false ..
        public bool SetCurrentState(string newState)
        {
            //(L1R7)
            if (!checkValidState(newState))
            {
                // (i) not valid state 
                return false;
            }
            else
            {
                // (ii) valid state ..

                //(L2R2)
                if (stateIsSame(newState))
                {   // (1) newState  is the same as the current state ..
                    return true;
                }
                //(L2R1)
                // states are accordingly to the S.T.D ..
                else if (checkSequencialStates(newState))
                {   // (2)  Correct sequence ..

                    return true;
                }
                else
                {
                    // (3) Invalid sequence ..
                    return false;
                }
            }
        }


        // (ii) PRIVATE:

        private void checkIdIsWithinLimits(string id)
        {
            // (a) id less than the limit
            if (id.Length < MIN_LIMIT_ID)
            {
                throw new Exception("Exception: Bigger id required!");
            }
            // (b) id greater than the limit
            else if (id.Length > MAX_LIMIT_ID)
            {
                throw new Exception("Exception: Smaller id required!");
            }
            // (c) id has empty spaces
            else if (String.IsNullOrWhiteSpace(id) || id.Trim().Length == 0)
            {
                throw new Exception("Exception: No empty ids!");
            }
            // (d) id does not contains special characters
            else if (!id.All(c => char.IsLetterOrDigit(c)))
            {
                throw new Exception("Exception: No special characters allowed!");
            }
            // (e) null value
            else if (id == null)
            {
                throw new Exception();
            }


        }

        // appends log meesage 
        private void appendLogMessage(ref string msg, string dept)
        {
            if (msg == " ")
            {   // (a) msg is empty - > initialize it !
                msg = dept;
            }
            else
            {   // (b) msg in not empty - > append it !
                msg = msg + ", " + dept;
            }
        }

        // checks for faulty reports and log messages..
        // L4R3
        private void checkForFaultyReports()
        {
            string msg = " ";

            // (i) Lights are faulty -> append them to msg ..
            if (_lightManager.GetStatus().Contains("FAULT"))
            {

                appendLogMessage(ref msg, "Lights");
            }

            // (ii) Doors are faulty - > appends them to msg ..
            if (_doorManager.GetStatus().Contains("FAULT"))
            {
                appendLogMessage(ref msg, "Doors");
            }

            // (iii) Doors are faulty - > appends them to msg ..
            if (_fireAlarmManager.GetStatus().Contains("FAULT"))
            {
                appendLogMessage(ref msg, "FireAlarm");
            }

            if (msg == " ")
            {
                // (a) empty msg -> NO FAULTY REPORTS
                msg = "NO FAULTS";
            }

            _webService.LogEngineerRerquired(msg);


        }

        // check if state is in 'Fire' states and sets 'History' state's value ..
        private void checkStateIsFireSetHistory(string newState)
        {
            if (stateIsFire(newState))
            {
                this.historyState = currentState;
            }
        }


        // opens all doors ..
        private bool openStateSetAllDoors()
        {
            // no door manager object exist ..
            if (_doorManager == null)
                return true;
            else
            {
                return _doorManager.OpenAllDoors(); //L3R4
            }
        }

        // valid states that can be initialize from the beginning ..
        private bool validInitalStates()
        {
            return (currentState == STATE_CLOSED
                || currentState == STATE_OUT_OF_HOURS
                || currentState == STATE_OPEN);
        }

        // check if newState name is the same as the current state
        private bool stateIsSame(string newState)
        {
            return (newState == this.currentState);
        }

        // check if state name is valid ..
        private bool checkValidState(string state)
        {
            return (state == STATE_CLOSED || state == STATE_OUT_OF_HOURS || state == STATE_OPEN
                || state == STATE_FIRE_DRILL || state == STATE_FIRE_ALARM);
        }


        //(L2R1)
        // check the sequence' state change ..
        private bool checkSequencialStates(string newState)
        {
            switch (this.currentState)
            {
                case STATE_OUT_OF_HOURS:
                    return (whichStateCanGoFromOOH(newState));
                case STATE_OPEN:
                    return (whichStateCanGoFromOpenOrClose(newState));
                case STATE_CLOSED:
                    return (whichStateCanGoFromOpenOrClose(newState));
                case STATE_FIRE_ALARM:
                    return (whichStateCanGoFromFire(newState));
                case STATE_FIRE_DRILL:
                    return (whichStateCanGoFromFire(newState));
            }
            return false;
        }

        // check if historyState is equal with the new state..
        private bool whichStateCanGoFromFire(string newState)
        {
            // - new state is same as the History state ..
            if (newState == this.historyState)
            {
                // (i) if state is open -> set all doors to open ..
                if (newState == STATE_OPEN)
                {
                    return stateIsOpenUnlockAllDoors(newState);

                }// (ii) 'closed' state -> close all doors & turn off all lights ..
                else if (newState == STATE_CLOSED)
                {
                    return closeStateTurnOffLightsAndLockAllDoors(newState);
                }

                currentState = newState;
                return true;
            }

            return false;
        }

        // valid states that can move from out of hours state ...
        private bool whichStateCanGoFromOOH(string newState)
        {
            // - valid states ..
            if (newState == STATE_CLOSED
                || newState == STATE_OPEN
                || newState == STATE_FIRE_ALARM
                || newState == STATE_FIRE_DRILL)
            {
                // (i)'open' state -> set all doors to OPEN ..
                //(L3R4)
                if (newState == STATE_OPEN)
                {
                    return stateIsOpenUnlockAllDoors(newState);
                }
                // (ii) 'closed' state -> close all doors & turn off all lights ..
                // L4R1
                else if (newState == STATE_CLOSED)
                {
                    return closeStateTurnOffLightsAndLockAllDoors(newState);
                }  
                // (iii) 'fire  alarm' state -> enable alarm + open all doors + turn on lights + online log msg ..
                else if (newState == STATE_FIRE_ALARM)
                {
                    // L4R2
                    try
                    {
                        _fireAlarmManager.SetAlarm(true);
                        _doorManager.OpenAllDoors();
                        _lightManager.SetAllLights(true);

                       
                    }
                    catch (NullReferenceException) {; }
                    LogFireAlarm(); // L4R4

                }

                checkStateIsFireSetHistory(newState);
                this.currentState = newState.ToLower();

                return true;

            }
            return false;

        }

        // turns off lights if doors are locked ..
        private bool closeStateTurnOffLightsAndLockAllDoors(string newState)
        {
            if (doorsAreLocked())
            {   // - doors are locked ..
                checkStateIsFireSetHistory(newState);
                this.currentState = newState.ToLower();

                // L4R1
                try { _lightManager.SetAllLights(false); }
                catch (NullReferenceException) {; }

                return true;
            }
            // - doors are not locked ..
            return false;
        }

        // checks if doors are locked
        private bool doorsAreLocked()
        {
            // door manager object is empty ..
            if (_doorManager == null)
                return true;

            return _doorManager.LockAllDoors();
        }

        // checks if doors are open ? true -> set up state : false -> dont change state ..
        private bool stateIsOpenUnlockAllDoors(string newState)
        {
          

            if (openStateSetAllDoors())
            {
                checkStateIsFireSetHistory(newState); // new state is FIRE -> save current state as 'History' for the return ..
                this.currentState = newState.ToLower(); // L3R5 -> doors successfullu open, change state ..
                return true;
            }

            else { return false; }
            
        }


        // check which states can be navigated from Open/Close state ...
        private bool whichStateCanGoFromOpenOrClose(string newState)
        {
            if (newState == STATE_OUT_OF_HOURS
                || newState == STATE_FIRE_DRILL
                || newState == STATE_FIRE_ALARM)
            {
                // new state is FIRE -> save current state for the return ..
                checkStateIsFireSetHistory(newState);

                // (i) 'fire  alarm' state -> enable alarm + open all doors + turn on lights + online log msg ..
                if (newState == STATE_FIRE_ALARM)
                {
                    try
                    {
                        _fireAlarmManager.SetAlarm(true);
                        _doorManager.OpenAllDoors();
                        _lightManager.SetAllLights(true);
                    }
                    catch (NullReferenceException) {; }

                    LogFireAlarm(); // L4R4
                }



                this.currentState = newState.ToLower();
                return true;
            }
            return false;
        }

        // L4R4
        public void LogFireAlarm()
        {
            try
            {

                try
                {
                    _webService.LogFireAlarm("fire alarm");
                }
                catch (Exception e)
                {
                    _emailService.SendEmail("smartbuilding@uclan.ac.uk", "failed to log alarm”", e.ToString());
                }
            }
            catch (NullReferenceException) {; }
        }


        private bool stateIsFire(string newState)
        {
            return (newState == STATE_FIRE_ALARM || newState == STATE_FIRE_DRILL);
        }
    }

}

