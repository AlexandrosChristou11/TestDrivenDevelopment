// Alexandros Christou - 19Feb21
// This class contains all units tests 

using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment_SoftwareDevelopment
{
    class BuildingControllerTests
    {
        #region PRIVATE MEthods
        private string ID = "newID";

        private const string STATE_CLOSED = "closed";
        private const string STATE_OUT_OF_HOURS = "out of hours";
        private const string STATE_OPEN = "open";
        private const string STATE_FIRE_DRILL = "fire drill";
        private const string STATE_FIRE_ALARM = "fire alarm";

        #endregion

        #region LEVEL 1 REQUIREMENTS

        // ** ----------------- LEVEL 1 REQUIRMENTS ----------------------------- **

        // --> ( L1 R1 ) <--

        // - (1) id is withing boundary
        [TestCase("ne")]           // middle length
        [TestCase("a")]            // min limit length
        [TestCase("alex1")]        // max limit length
        [TestCase("1")]            // stringed number
        
        public void is_IdValid_DoNotThrowException(string id)
        {

            // Arrange
            // Act
            TestDelegate t = () => new BuildingController(id);

            // Assert

            Assert.DoesNotThrow(t);

        }

        // (2) throws exception when id is out of boundary
        [TestCase("")]              // empty 
        [TestCase("alexandros")]    // exceed boundaries 
        [TestCase("  ")]            // whitespaces


        public void is_IdInvalid_ThrowException(string id)
        {

            // Arrange

            // Act

            // Assert
            Assert.Throws<Exception>(() => new BuildingController(id));

        }

        // (3) special charachters -> throw exception
        [TestCase("__")]
        [TestCase("#@*&")]
        [TestCase("asa&")]
        [TestCase("   !")]

        public void is_IdSpecialCharacter_ThrowException(string id)
        {

            // Arrange

            // Act

            // Assert
            Assert.Throws<Exception>(() => new BuildingController(id));
            

        }


        // --> ( L1 R2 ) <--
        // if buildingId is within the constraints -> get it..
        public void is_idValid_ReturnId()
        {
            // Arrange
            BuildingController b = new BuildingController(ID);

            // Act
            string id = b.GetBuildingID();

            // Assert
            Assert.AreEqual(ID, id);


        }


        // check if invalid id is accessed ..
        public void is_idInValid_ReturnFalse()
        {
            // Arrange
            BuildingController b = new BuildingController(ID);

            // Act
            
            // Assert
            Assert.AreNotEqual(b.GetBuildingID(), "John");


        }

        // --> ( L1 R5 ) <--
        // check if initial state is "out of hours"
        public void is_initialStateOutOfHours_ReturnSame()
        {

            // Arrange
            BuildingController b = new BuildingController(ID);

            // Act
            string expected = STATE_OUT_OF_HOURS;

            // Assert
            Assert.Equals(expected, b.GetCurrentState());
        }


        
        // --> ( L1 R7 ) <--

        // - valid test cases 
        // - test will be passed as long valid states are supplied..
        [TestCase(STATE_CLOSED)]
        [TestCase(STATE_OUT_OF_HOURS)]
        [TestCase(STATE_OPEN)]
        [TestCase(STATE_FIRE_DRILL)]
        [TestCase(STATE_FIRE_ALARM)]

        public void is_StateValid_ReturTrue(string state)
        {
            // Arrange
           
            // Act
            BuildingController bc = new BuildingController(ID);
            bool result = bc.SetCurrentState(state);
            
            // Assert
            Assert.IsTrue(result);



        }

        // -invalid test cases
        // - test will be passed as long invalid states will be supplied ..
        [TestCase("dummyString")]
        [TestCase("ANORTHOSI")]
        [TestCase("CLOSED")]
        [TestCase("CLOSEd")]
        [TestCase("Out of hours")]
        [TestCase("")]
        [TestCase("FIRE DRILL")]
        [TestCase("1")]
        [TestCase("   ")]

        public void is_StateInvalid_ReturnFalse(string state)
        {
            // Arrange
            string id = "newId";
            BuildingController bc = new BuildingController(id);

            // Act
            bool result = bc.SetCurrentState(state);

            // Assert 
            Assert.IsFalse(result);


        }

        
        #endregion

        #region LEVEL 2 REQUIREMENTS

        // ** ----------------- LEVEL 2 REQUIRMENTS ----------------------------- **

        // --> ( L2 R1 )  <--

        // (1) State Change sequence is according to the F.S.D ..

        // (a) Closed state..
        [TestCase(STATE_CLOSED, STATE_OUT_OF_HOURS)]
        [TestCase(STATE_CLOSED, STATE_FIRE_DRILL)]
        [TestCase(STATE_CLOSED, STATE_FIRE_ALARM)]

        // (b) Open state ..
        [TestCase(STATE_OPEN, STATE_OUT_OF_HOURS)]
        [TestCase(STATE_OPEN, STATE_FIRE_DRILL)]
        [TestCase(STATE_OPEN, STATE_FIRE_ALARM)]

        // (c) Fire Drill state ..
        [TestCase(STATE_FIRE_DRILL, STATE_OUT_OF_HOURS)]

        // (d) Fire alarm state .. 
        [TestCase(STATE_FIRE_ALARM, STATE_OUT_OF_HOURS)]

        // (e) Out Of Hour State ..
        [TestCase(STATE_OUT_OF_HOURS, STATE_OPEN)]
        [TestCase(STATE_OUT_OF_HOURS, STATE_CLOSED)]
        [TestCase(STATE_OUT_OF_HOURS, STATE_FIRE_ALARM)]
        [TestCase(STATE_OUT_OF_HOURS, STATE_FIRE_DRILL)]


        public void is_stateChangeSequencialValid_ReturnTrue(string state, string newState)
        {
            // Arrange

            BuildingController bc = new BuildingController(ID);
            
            // Act
            bool result = bc.SetCurrentState(state);
            bool fResult = bc.SetCurrentState(newState);

            // Assert
            Assert.IsTrue(fResult);


        }

        // (2) (a) Invalid sequence of state change ..

        // (a) Fire Drill State ..
        [TestCase(STATE_OPEN, STATE_FIRE_DRILL, STATE_OPEN)]
        [TestCase(STATE_CLOSED, STATE_FIRE_DRILL, STATE_CLOSED)]

        // (b) Fire Alarm State .
        [TestCase(STATE_OPEN, STATE_FIRE_ALARM, STATE_OPEN)]
        [TestCase(STATE_CLOSED, STATE_FIRE_ALARM, STATE_CLOSED)]

        public void is_stateChangeToFireSequencialValid_ReturnTrue(string oldState, string currentState, string newState)
        {
            // Arrange

            BuildingController bc = new BuildingController(ID);

            // Act
            bool state1 = bc.SetCurrentState(oldState);
            bool state2 = bc.SetCurrentState(currentState);
            bool fResult = bc.SetCurrentState(newState);

            // Assert
            Assert.IsTrue(fResult);

        }

        // - invalid states -> Return false

        // (a) Closed state ..
        [TestCase(STATE_CLOSED, STATE_OPEN)]



        // (b) Open state ..
        [TestCase(STATE_OPEN, STATE_CLOSED)]


        // (c) Fire Drill | HistoryState = out of hour
        [TestCase(STATE_FIRE_DRILL, STATE_OPEN)]
        [TestCase(STATE_FIRE_DRILL, STATE_CLOSED)]
        [TestCase(STATE_FIRE_DRILL, STATE_FIRE_ALARM)]


        // (d) Fire Alarm | HistoryState = out of hour
        [TestCase(STATE_FIRE_ALARM, STATE_OPEN)]
        [TestCase(STATE_FIRE_ALARM, STATE_CLOSED)]
        [TestCase(STATE_FIRE_ALARM, STATE_FIRE_DRILL)]



        public void is_stateChangeSequenceInvalid_ReturnTrue(string currentState, string newState)
        {
            // Arrange

            BuildingController bc = new BuildingController(ID);

            // Act
            bool cState = bc.SetCurrentState(currentState);
            bool nState = bc.SetCurrentState(newState);

            // Assert
            Assert.IsFalse(nState);

        }

        // - Fire Drill state to an invalid state (not HISTORY)
        [TestCase(STATE_OPEN, STATE_FIRE_DRILL, STATE_CLOSED)]
        [TestCase(STATE_OPEN, STATE_FIRE_DRILL, STATE_OUT_OF_HOURS)]
        [TestCase(STATE_OPEN, STATE_FIRE_DRILL, STATE_FIRE_ALARM)]


        [TestCase(STATE_CLOSED, STATE_FIRE_DRILL, STATE_OPEN)]
        [TestCase(STATE_CLOSED, STATE_FIRE_DRILL, STATE_OUT_OF_HOURS)]
        [TestCase(STATE_CLOSED, STATE_FIRE_DRILL, STATE_FIRE_ALARM)]

        // - Fire Alarm state to an invalid state (not HISTORY)
        [TestCase(STATE_OPEN, STATE_FIRE_ALARM, STATE_CLOSED)]
        [TestCase(STATE_OPEN, STATE_FIRE_ALARM, STATE_OUT_OF_HOURS)]
        [TestCase(STATE_OPEN, STATE_FIRE_ALARM, STATE_FIRE_DRILL)]

        [TestCase(STATE_CLOSED, STATE_FIRE_ALARM, STATE_OPEN)]
        [TestCase(STATE_CLOSED, STATE_FIRE_ALARM, STATE_OUT_OF_HOURS)]
        [TestCase(STATE_CLOSED, STATE_FIRE_ALARM, STATE_FIRE_DRILL)]

        public void is_stateChangeFireSequenceInvalid_ReturnFalse(string oldState, string currentState, string newState)
        {

            // Arrange
            BuildingController bc = new BuildingController(ID);

            // Act
            bool oState = bc.SetCurrentState(oldState);
            bool cState = bc.SetCurrentState(currentState);
            bool nState = bc.SetCurrentState(newState);

            // Assert
            Assert.IsFalse(nState);
        }


        // --> ( L2 R2 )  <--

        // - attempt to set the state the same one as the current state ..
        [TestCase(STATE_CLOSED, STATE_CLOSED)]
        [TestCase(STATE_OPEN, STATE_OPEN)]
        [TestCase(STATE_OPEN, STATE_OPEN)]
        [TestCase(STATE_FIRE_ALARM, STATE_FIRE_ALARM)]

        public void is_newStateSameAsCurrentState_ReturnTrue(string currentState, string newState)
        {

            // Arrange 
            BuildingController bc = new BuildingController(ID);

            // Act
            bool cState = bc.SetCurrentState(currentState);
            bool nState = bc.SetCurrentState(newState);

            // Assert
            Assert.IsTrue(nState);

        }

        [TestCase(STATE_CLOSED, STATE_FIRE_ALARM, STATE_FIRE_ALARM)]
        [TestCase(STATE_OPEN, STATE_FIRE_ALARM, STATE_FIRE_ALARM)]
        [TestCase(STATE_CLOSED, STATE_FIRE_DRILL, STATE_FIRE_DRILL)]
        [TestCase(STATE_OPEN, STATE_FIRE_DRILL, STATE_FIRE_DRILL)]

        public void is_newStateFireSameAsCurrentState_ReturnTrue(string oldState, string currentState, string newState)
        {
            // Arrange
            BuildingController bc = new BuildingController(ID);
            // Act
            bool oState = bc.SetCurrentState(oldState);
            bool cState = bc.SetCurrentState(currentState);
            bool nState = bc.SetCurrentState(newState);

            // Assert
            Assert.IsTrue(nState);

        }

        // --> ( L2 R3 )  <--

        [TestCase("cLoSed")]
        [TestCase("CLOSED")]
        [TestCase("opeN")]
        [TestCase("out of hours")]


        public void is_StateInLowerCaseValid_ReturnTrue(string state)
        {

            // Arrange
            string id = "newId";
            BuildingController bc = new BuildingController(id, state);

            // Act
            bool result = bc.AnalayzeStateLetterForm();

            // Assert
            Assert.IsTrue(result);

        }

        [TestCase("cLoSedd")]
        [TestCase("opeNn")]
        [TestCase("out of  hours")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("fire alarm")]
        [TestCase("fire drill")]
        [TestCase("1")]
        [TestCase("    ")]
        [TestCase("_#@4")]

        public void is_InitalStateInvalid_ThrowException(string state)
        {

            // Arrange
            string id = "newId";

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => new BuildingController(id, state));
        }


        [TestCase("open")]
        [TestCase("closed")]
        [TestCase("out of hours")]

        public void is_InitalStateValid_DoNotThrowException(string state)
        {
            // Arrange
            string id = "newId";

            // Act
            TestDelegate t = () => new BuildingController(id, state);

            // Assert
            Assert.DoesNotThrow(t);
        }

        #endregion

        #region LEVEL 3 REQUIREMENTS

        // ** ----------------- LEVEL 3 REQUIRMENTS ----------------------------- **

        // ( --> L3 R1 <-- )


        // (1) Light manager not passed -> throw exception
        [Test]
        public void is_LightManagerNotPassed_DoNotThrowException()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<NullReferenceException>(() => new BuildingController(ID, null, GetMockFireAlarmManager(), GetMockDoorManager(),
                                        GetMockIWebService(), GetMockEmailService()));
            ;

        }

        // (2) Fire alarm manager not passed -> throw exception
        [Test]
        public void is_FireAlarmManagerNotPassed_DoNotThrowException()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<NullReferenceException>(() => new BuildingController(ID, GetMockLightManager(), null, GetMockDoorManager(),
                                        GetMockIWebService(), GetMockEmailService()));
 

        }
        // (3) Door manager not passed -> throw exception
        [Test]
        public void is_DoorManagerNotPassed_DoNotThrowException()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<NullReferenceException>(() => new BuildingController(ID, GetMockLightManager(), GetMockFireAlarmManager(), null,
                                        GetMockIWebService(), GetMockEmailService()));
 

        } 
        
        // (4) web service not passed -> throw exception
        [Test]
        public void is_WebServiceNotPassed_DoNotThrowException()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<NullReferenceException>(() => new BuildingController(ID, GetMockLightManager(), GetMockFireAlarmManager(), GetMockDoorManager(),
                                        null, GetMockEmailService()));

        }
        // (5) email service not passed -> throw exception
        [Test]
        public void is_EmailSerViceNotPassed_DoNotThrowException()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<NullReferenceException>(() => new BuildingController(ID, GetMockLightManager(), GetMockFireAlarmManager(), GetMockDoorManager(),
                                        GetMockIWebService(), null));

        }

        // (6) All objects are passed -> do not throw exception ..
        [Test]
        public void are_AllObjectsPassedDoNotThrowException()
        {
            // Arrange

            // Act
            TestDelegate t = () => new BuildingController(ID, GetMockLightManager(), GetMockFireAlarmManager(), GetMockDoorManager(),
                                        GetMockIWebService(), GetMockEmailService());

            // Assert
            Assert.DoesNotThrow(t);
        }

        
        //( --> L3 R2 <-- )


        // - GetStatus() return a string containing comma-separated values
        // (1) ILightManager Stub

        [Test]
        public void is_LightManagerGetStatus_ReturnsMessage()
        {

            // Arrange
            string message = "Lights,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK";
            var stubLightManager = Substitute.For<ILightManager>();
            var stubFireManager = Substitute.For<IFireAlarmManager>();
            var stubDoorManager = Substitute.For<IDoorManager>();
            var stubWebService = Substitute.For<IWebService>();

            stubLightManager.GetStatus().Returns(message);
            stubFireManager.GetStatus().Returns("");
            stubDoorManager.GetStatus().Returns("");

            // Act
            BuildingController bc = new BuildingController(ID, stubLightManager, stubFireManager, stubDoorManager, stubWebService, GetMockEmailService());

            // Assert
            
            Assert.AreEqual(message, bc.GetStatusReport());

        }

        // - GetStatus() return a string containing comma-separated values
        // (2) IDoorManager Stub

        [Test]
        public void is_DoorManagerGetStatus_ReturnsMessage()
        {

            // Arrange
            string message = "Doors,OK,OK,OK,OK,OK,FAULT,OK,OK,OK,OK";
            var stubLightManager = Substitute.For<ILightManager>();
            var stubFireManager = Substitute.For<IFireAlarmManager>();
            var stubDoorManager = Substitute.For<IDoorManager>();
            var stubWebService = Substitute.For<IWebService>();
            stubDoorManager.GetStatus().Returns(message);
            stubFireManager.GetStatus().Returns("");
            stubLightManager.GetStatus().Returns("");

            // Act
            BuildingController bc = new BuildingController(ID, stubLightManager, stubFireManager, stubDoorManager, stubWebService, GetMockEmailService());


            // Assert

            Assert.AreEqual(message, bc.GetStatusReport());

        }

        // - GetStatus() return a string containing comma-separated values
        // (3) IDoorManager Stub

        [Test]
        public void is_FireAlarmManagerGetStatus_ReturnsMessage()
        {

            // Arrange
            string message = "FireAlarm,OK,OK,OK,FAULT,OK,FAULT,OK,OK,OK,OK";
            var stubLightManager = Substitute.For<ILightManager>();
            var stubFireManager = Substitute.For<IFireAlarmManager>();
            var stubDoorManager = Substitute.For<IDoorManager>();
            var stubWebService = Substitute.For<IWebService>();
            stubFireManager.GetStatus().Returns(message);
            stubDoorManager.GetStatus().Returns("");
            stubLightManager.GetStatus().Returns("");

            // Act
            BuildingController b = new BuildingController(ID, stubLightManager, stubFireManager, stubDoorManager, stubWebService, GetMockEmailService());

            // Assert

            Assert.AreEqual(message, b.GetStatusReport());

        }

        //( --> L3 R3 <-- )

        // - GetStatus() methods of all 3 manager classes (LightManager, DoorManager and FireAlarmManager)
        // and appends each string returned together into a single string 
        // (in the following order: lightStatus+doorStatus+fireAlarmStatus) before returning the result

        [Test]
        public void is_AllManagerStatusesAppended_ReturnMessage()
        {
            string id = "id";

            // Arrange
            string msgDoorManager = "Doors,OK,OK,OK,FAULT,OK,FAULT,OK,OK,OK,OK";
            string msgLightManager = "Lights,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK";
            string msgFireAlarmManager = "FireAlarm,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK";

            var stubFireAlarmManager = Substitute.For<IFireAlarmManager>();
            var stubDoorManager = Substitute.For<IDoorManager>();
            var stubLightManger = Substitute.For<ILightManager>();
            var stubWebService = Substitute.For<IWebService>();

            stubDoorManager.GetStatus().Returns(msgDoorManager);
            stubFireAlarmManager.GetStatus().Returns(msgFireAlarmManager);
            stubLightManger.GetStatus().Returns(msgLightManager);

            // Act

            BuildingController bc = new BuildingController(id, stubLightManger, stubFireAlarmManager, stubDoorManager
                                 , stubWebService, GetMockEmailService());

            string result = bc.GetStatusReport();

            // Assert
            Assert.AreEqual(stubLightManger.GetStatus() + stubDoorManager.GetStatus() + stubFireAlarmManager.GetStatus()
                            , result);


        }


        //( --> L3 R4 <-- )

        // - (a) All doors were succesfully open

        [Test]
        public void is_stateChangesToOpenUnlocksAllDoors_ReturnTrue()
        {
            // Arrange
            var stubDoorManager = Substitute.For<IDoorManager>();
            stubDoorManager.OpenAllDoors().Returns(true);

            // Act 
            BuildingController bc = new BuildingController(ID, GetMockLightManager(), GetMockFireAlarmManager(), stubDoorManager, GetMockIWebService(), GetMockEmailService());

            bool result = bc.SetCurrentState(STATE_OPEN);

            // Assert 
            Assert.AreEqual(stubDoorManager.OpenAllDoors(), result);


        }

        // -(b)  fail to open all doors ..

        [Test]
        public void is_stateChangesToOpenFailToUnlocksAllDoors_ReturnFalse()
        {
            // Arrange
            var stubDoorManager = Substitute.For<IDoorManager>();
            stubDoorManager.OpenAllDoors().Returns(false);

            // Act 
            BuildingController bc = new BuildingController(ID, GetMockLightManager(), GetMockFireAlarmManager(), stubDoorManager, GetMockIWebService(), GetMockEmailService());

            bool result = bc.SetCurrentState(STATE_OPEN);

            // Assert 
            Assert.AreEqual(stubDoorManager.OpenAllDoors(), result);


        }


        // the first change to Open will successfully open all doors ..
        // but the second time will fail to unlock all doors ..
        // [OPEN (unlock successfull)] -> [FIRE] -> [OPEN (fail to unlock)]

        [TestCase(STATE_OPEN, STATE_FIRE_DRILL, STATE_OPEN)]
        [TestCase(STATE_OPEN, STATE_FIRE_ALARM, STATE_OPEN)]

        public void is_stateChangesToOpenFromFireStatesFailsToUnlocksAllDoors_ReturnFalse(string Oldstate, string currentState, string newState)
        {
            // Arrange
            var stubDoorManager = Substitute.For<IDoorManager>();
            var stubFireAlarmManager = Substitute.For<IFireAlarmManager>();
            var stubLightManager = Substitute.For<ILightManager>();
            var stubWebService = Substitute.For<IWebService>();
            var stubEmailService = Substitute.For<IEmailService>();

            stubDoorManager.OpenAllDoors().Returns(true, false);

            // Act 
            BuildingController bc = new BuildingController(ID, stubLightManager, stubFireAlarmManager, stubDoorManager, stubWebService, stubEmailService);

            bool stateFirst = bc.SetCurrentState(Oldstate);
            bool stateSecond = bc.SetCurrentState(currentState);
            bool result = bc.SetCurrentState(newState);

            bool expected = false;

            // Assert 
            Assert.AreEqual(expected, result);

        }

        #endregion

        #region LEVEL 4 REQUIREMENTS

        // ** ----------------- LEVEL 4 REQUIRMENTS ----------------------------- **

        //( --> L4 R1 <-- )

        // - 'closed' state -> Lock all doors and turn off all lights ..
      
        [Test]
        public void is_StateChangesToClosedLockAllDoorsAndTurnOffLights_ReturnTrue()
        {

            // Arrange 
            var stubDoorManager = Substitute.For<IDoorManager>();
            var stubLightManager = Substitute.For<ILightManager>();
            
            stubDoorManager.LockAllDoors().Returns(true);
            /// stubLightManager.SetAllLights(false);

            // Act
            BuildingController bc = new BuildingController(ID, stubLightManager, GetMockFireAlarmManager(), stubDoorManager, GetMockIWebService(), GetMockEmailService());

            bool result = bc.SetCurrentState(STATE_CLOSED);
            bool expected = true;

            // Assert
            Assert.AreEqual(expected, result);
            

        }

        // 'closed' state -> fail lock doors ..

        [Test]
        public void is_StateChangeToClosedFailToLockAllDoorsAndFailTurnOffLights_ReturnTrue()
        {

            // Arrange 
            var stubDoorManager = Substitute.For<IDoorManager>();
            var stubLightManager = Substitute.For<ILightManager>();

            stubDoorManager.LockAllDoors().Returns(false);
            /// stubLightManager.SetAllLights(false);

            // Act
            BuildingController bc = new BuildingController(ID, stubLightManager, GetMockFireAlarmManager(), stubDoorManager, GetMockIWebService(), GetMockEmailService());

            bool result = bc.SetCurrentState(STATE_CLOSED);
            bool expected = false;

            // Assert

            Assert.AreEqual(expected, result);

        }

        //( --> L4 R2 <-- )

        // when state is setted to 'fire alarm' -> open all doors +

        [Test]

        public void is_state_FireAlarm_EnableAlarmUnlockAllDoorsTurnOnLights_ReturnTrue()
        {

            // Arrange
            var stubFireAlarmManager = Substitute.For<IFireAlarmManager>();
            var stubDoorManager = Substitute.For<IDoorManager>();
            var stubLightManager = Substitute.For<ILightManager>();
            var stubWebService = Substitute.For<IWebService>();
            var stubIEmailService = Substitute.For<IEmailService>();



            // Act
            BuildingController bc = new BuildingController(ID, stubLightManager, stubFireAlarmManager, stubDoorManager, stubWebService, stubIEmailService);
            bool result = bc.SetCurrentState(STATE_FIRE_ALARM);


            // Assert
            stubWebService.Received().LogFireAlarm("fire alarm");
            stubFireAlarmManager.Received().SetAlarm(true);
            stubLightManager.Received().SetAllLights(true);
            stubDoorManager.Received().OpenAllDoors();

        }


        //( --> L4 R3 <-- )

        // - if a faulty report exist -> webService log which section is faulty ... 

        // (1) LIGHTS  |  DOORS  |  FIREALARM 
        //       OK    |   OK    |     OK          

        [Test]
        public void are_AllStatusReportsOK_ReturnNoFaults()
        {

            // Arrange

            string msgDoorManager = "Doors,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK";
            string msgLightManager = "Lights,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK";
            string msgFireAlarmManager = "FireAlarm,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK";

            var stubFireAlarmManager = Substitute.For<IFireAlarmManager>();
            var stubDoorManager = Substitute.For<IDoorManager>();
            var stubLightManger = Substitute.For<ILightManager>();
            var stubWebService = Substitute.For<IWebService>();
            var stubIEmailService = Substitute.For<IEmailService>();


            stubDoorManager.GetStatus().Returns(msgDoorManager);
            stubFireAlarmManager.GetStatus().Returns(msgFireAlarmManager);
            stubLightManger.GetStatus().Returns(msgLightManager);


            // Act
            BuildingController bc = new BuildingController(ID, stubLightManger, stubFireAlarmManager, stubDoorManager, stubWebService, stubIEmailService);
            string b = bc.GetStatusReport();


            // Assert
            stubWebService.Received().LogEngineerRerquired("NO FAULTS");
        }



        // - if a faulty report exist -> webService log which section is faulty ... 

        // (2) LIGHTS  |  DOORS  |  FIREALARM 
        //      FAULT  |   OK    |     OK       

        [Test]

        public void is_LightsReportFaulty_LogMessageLights()
        {

            // Arrange

            string msgLightManager = "Lights,FAULT,OK,OK,FAULT,OK,OK,OK,OK,OK,OK";
            string msgDoorManager = "Doors,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK";
            string msgFireAlarmManager = "FireAlarm,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK";

            var stubFireAlarmManager = Substitute.For<IFireAlarmManager>();
            var stubDoorManager = Substitute.For<IDoorManager>();
            var stubLightManger = Substitute.For<ILightManager>();
            var stubWebService = Substitute.For<IWebService>();
            var stubIEmailService = Substitute.For<IEmailService>();

            stubDoorManager.GetStatus().Returns(msgDoorManager);
            stubFireAlarmManager.GetStatus().Returns(msgFireAlarmManager);
            stubLightManger.GetStatus().Returns(msgLightManager);


            // Act
            BuildingController bc = new BuildingController(ID, stubLightManger, stubFireAlarmManager, stubDoorManager, stubWebService, stubIEmailService);
            bc.GetStatusReport();


            // Assert
            stubWebService.Received().LogEngineerRerquired("Lights");

        }

        // - if a faulty report exist -> webService log which section is faulty ... 

        // (3) LIGHTS  |  DOORS  |  FIREALARM 
        //      OK     |  FAULT  |     OK       

        [Test]

        public void is_DoorsReportFaulty_LogMessageDoors()
        {

            // Arrange

            string msgLightManager = "Lights,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK";
            string msgDoorManager = "Doors,OK,OK,FAULT,OK,OK,FAULT,OK,OK,OK,OK";
            string msgFireAlarmManager = "FireAlarm,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK";

            var stubFireAlarmManager = Substitute.For<IFireAlarmManager>();
            var stubDoorManager = Substitute.For<IDoorManager>();
            var stubLightManger = Substitute.For<ILightManager>();
            var stubWebService = Substitute.For<IWebService>();
            var stubIEmailService = Substitute.For<IEmailService>();


            stubDoorManager.GetStatus().Returns(msgDoorManager);
            stubFireAlarmManager.GetStatus().Returns(msgFireAlarmManager);
            stubLightManger.GetStatus().Returns(msgLightManager);

            // Act
            BuildingController bc = new BuildingController(ID, stubLightManger, stubFireAlarmManager, stubDoorManager, stubWebService, stubIEmailService);
            bc.GetStatusReport();


            // Assert
            stubWebService.Received().LogEngineerRerquired("Doors");

        }

        // - if a faulty report exist -> webService log which section is faulty ... 

        // (4) LIGHTS  |  DOORS  |  FIREALARM 
        //      OK     |    OK   |     FAULT       

        [Test]

        public void is_FireAlarmReportFaulty_LogMessageFireAlarm()
        {

            // Arrange

            string msgLightManager = "Lights,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK";
            string msgDoorManager = "Doors,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK";
            string msgFireAlarmManager = "FireAlarm,OK,FAULT,OK,OK,OK,FAULT,OK,OK,OK,OK";

            var stubFireAlarmManager = Substitute.For<IFireAlarmManager>();
            var stubDoorManager = Substitute.For<IDoorManager>();
            var stubLightManger = Substitute.For<ILightManager>();
            var stubWebService = Substitute.For<IWebService>();
            var stubIEmailService = Substitute.For<IEmailService>();


            stubDoorManager.GetStatus().Returns(msgDoorManager);
            stubFireAlarmManager.GetStatus().Returns(msgFireAlarmManager);
            stubLightManger.GetStatus().Returns(msgLightManager);

            // Act
            BuildingController bc = new BuildingController(ID, stubLightManger, stubFireAlarmManager, stubDoorManager, stubWebService, stubIEmailService);
            bc.GetStatusReport();


            // Assert
            stubWebService.Received().LogEngineerRerquired("FireAlarm");

        }

        // - if a faulty report exist -> webService log which section is faulty ... 

        // (5) LIGHTS  |  DOORS     |  FIREALARM 
        //      FAULT  |  FAULT     |     OK       

        [Test]

        public void are_LightsAndDoorsReportFaulty_LogMessageLightsDoors()
        {

            // Arrange

            string msgLightManager = "Lights,FAULT,OK,OK,OK,OK,OK,OK,OK,OK,OK";
            string msgDoorManager = "Doors,OK,OK,FAULT,OK,OK,OK,OK,OK,OK,OK";
            string msgFireAlarmManager = "FireAlarm,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK";

            var stubFireAlarmManager = Substitute.For<IFireAlarmManager>();
            var stubDoorManager = Substitute.For<IDoorManager>();
            var stubLightManger = Substitute.For<ILightManager>();
            var stubWebService = Substitute.For<IWebService>();
            var stubIEmailService = Substitute.For<IEmailService>();


            stubDoorManager.GetStatus().Returns(msgDoorManager);
            stubFireAlarmManager.GetStatus().Returns(msgFireAlarmManager);
            stubLightManger.GetStatus().Returns(msgLightManager);

            // Act
            BuildingController bc = new BuildingController(ID, stubLightManger, stubFireAlarmManager, stubDoorManager, stubWebService, stubIEmailService);
            bc.GetStatusReport();


            // Assert
            stubWebService.Received().LogEngineerRerquired("Lights, Doors");

        }

        // - if a faulty report exist -> webService log which section is faulty ... 

        // (6) LIGHTS  |  DOORS     |  FIREALARM 
        //      FAULT  |  OK        |     FAULT       

        [Test]

        public void are_LightsAndFireAlartmFaulty_LogMessageLightsFireAlarm()
        {

            // Arrange

            string msgLightManager = "Lights,FAULT,OK,OK,OK,OK,OK,OK,OK,OK,OK";
            string msgDoorManager = "Doors,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK";
            string msgFireAlarmManager = "FireAlarm,OK,OK,OK,OK,FAULT,OK,OK,OK,OK,OK";

            var stubFireAlarmManager = Substitute.For<IFireAlarmManager>();
            var stubDoorManager = Substitute.For<IDoorManager>();
            var stubLightManger = Substitute.For<ILightManager>();
            var stubWebService = Substitute.For<IWebService>();
            var stubIEmailService = Substitute.For<IEmailService>();


            stubDoorManager.GetStatus().Returns(msgDoorManager);
            stubFireAlarmManager.GetStatus().Returns(msgFireAlarmManager);
            stubLightManger.GetStatus().Returns(msgLightManager);

            // Act
            BuildingController bc = new BuildingController(ID, stubLightManger, stubFireAlarmManager, stubDoorManager, stubWebService, stubIEmailService);
            bc.GetStatusReport();


            // Assert
            stubWebService.Received().LogEngineerRerquired("Lights, FireAlarm");

        }

        // - if a faulty report exist -> webService log which section is faulty ... 

        // (7) LIGHTS  |  DOORS     |  FIREALARM 
        //      OK     |  FAULT     |     FAULT       

        [Test]

        public void are_DoorAndFireAlarmFaulty_LogMessageDoorsFireAlarm()
        {

            // Arrange

            string msgLightManager = "Lights,OK,OK,OK,OK,OK,OK,OK,OK,OK,OK";
            string msgDoorManager = "Doors,OK,FAULT,OK,OK,OK,OK,OK,OK,OK,OK";
            string msgFireAlarmManager = "FireAlarm,OK,OK,OK,OK,FAULT,OK,OK,OK,OK,OK";

            var stubFireAlarmManager = Substitute.For<IFireAlarmManager>();
            var stubDoorManager = Substitute.For<IDoorManager>();
            var stubLightManger = Substitute.For<ILightManager>();
            var stubWebService = Substitute.For<IWebService>();


            stubDoorManager.GetStatus().Returns(msgDoorManager);
            stubFireAlarmManager.GetStatus().Returns(msgFireAlarmManager);
            stubLightManger.GetStatus().Returns(msgLightManager);
            var stubIEmailService = Substitute.For<IEmailService>();

            // Act
            BuildingController bc = new BuildingController(ID, stubLightManger, stubFireAlarmManager, stubDoorManager, stubWebService, stubIEmailService);
            bc.GetStatusReport();


            // Assert
            stubWebService.Received().LogEngineerRerquired("Doors, FireAlarm");

        }

        // - if a faulty report exist -> webService log which section is faulty ... 

        // (8) LIGHTS  |  DOORS     |  FIREALARM 
        //      FAULT  |  FAULT     |     FAULT       

        [Test]

        public void are_AllReportsFaulty_LogMessageLightsDoors_FireAlarm()
        {

            // Arrange

            string msgLightManager = "Lights,FAULT,OK,OK,OK,OK,OK,OK,OK,OK,OK";
            string msgDoorManager = "Doors,OK,FAULT,OK,OK,OK,OK,OK,OK,OK,OK";
            string msgFireAlarmManager = "FireAlarm,OK,OK,OK,OK,FAULT,OK,OK,OK,OK,OK";

            var stubFireAlarmManager = Substitute.For<IFireAlarmManager>();
            var stubDoorManager = Substitute.For<IDoorManager>();
            var stubLightManger = Substitute.For<ILightManager>();
            var stubWebService = Substitute.For<IWebService>();
            var stubIEmailService = Substitute.For<IEmailService>();

            stubDoorManager.GetStatus().Returns(msgDoorManager);
            stubFireAlarmManager.GetStatus().Returns(msgFireAlarmManager);
            stubLightManger.GetStatus().Returns(msgLightManager);

            // Act
            BuildingController bc = new BuildingController(ID, stubLightManger, stubFireAlarmManager, stubDoorManager, stubWebService, stubIEmailService );
            bc.GetStatusReport();


            // Assert
            stubWebService.Received().LogEngineerRerquired("Lights, Doors, FireAlarm");

        }

        //( --> L4 R4 <-- )

        // - When LogFireAlarm throws an expcetion trigger email service to send an email .. 

        [Test]

        public void if_LogFireAlarmThrowsException_SendEmail()
        {

            // Arrange

            var stubFireAlarmManager = Substitute.For<IFireAlarmManager>();
            var stubDoorManager = Substitute.For<IDoorManager>();
            var stubLightManager = Substitute.For<ILightManager>();
            var mockWebService = Substitute.For<IWebService>();
            var stubEmailService = Substitute.For<IEmailService>();

            mockWebService.When(
                service => service.LogFireAlarm(Arg.Any<string>())).
                Do(info => { throw new Exception(); });

            // Act
            BuildingController bc = new BuildingController(ID, stubLightManager, stubFireAlarmManager, stubDoorManager, mockWebService, stubEmailService);
            bc.SetCurrentState(STATE_FIRE_ALARM);


            // Assert 

            stubEmailService.Received().SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());


        }
        #endregion

        #region DUMMY OBJECTS

        ILightManager GetMockLightManager()
        {
            ILightManager stubLightManager = Substitute.For<ILightManager>();
            return stubLightManager;
        }

        IDoorManager GetMockDoorManager()
        {
            IDoorManager stubDoorManager = Substitute.For<IDoorManager>();
            return stubDoorManager;
        }

        IFireAlarmManager GetMockFireAlarmManager()
        {
            IFireAlarmManager stubFireAlarmManager = Substitute.For<IFireAlarmManager>();
            return stubFireAlarmManager;

        }

        IWebService GetMockIWebService()
        {
            IWebService mockWebService = Substitute.For<IWebService>();
            return mockWebService;
        }


        IEmailService GetMockEmailService()
        {
            IEmailService stubEmailService = Substitute.For<IEmailService>();
            return stubEmailService;
        }

        #endregion

    }

}

