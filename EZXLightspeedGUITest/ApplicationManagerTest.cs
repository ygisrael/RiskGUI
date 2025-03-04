using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EZXLightspeedGUI;
using EZX.LightspeedMockup.Mock;
using EZX.LightSpeedEngine;
using EZXLightspeedGUI.Config;
using EZXWPFLibrary.Helpers;

namespace EZXLightspeedGUITest
{
    [TestClass()]
    public class ApplicationManagerTest
    {
        ApplicationManager app;

        [TestMethod()]
        public void ApplicationManagerConstrator_ForMOCKMODE_Test()
        {
            ApplicationManager.MOCK_MODE = true;
            app = new ApplicationManager();
            bool expected = true;
            bool actual = true;
            if (app.GUILSEngine.LSComMgr is MockLSCommunicationManager)
            {
                actual = true;
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ApplicationManagerConstrator_ForNonMOCKMODE_Test()
        {
            ApplicationManager.MOCK_MODE = false;
            app = new ApplicationManager();
            bool expected = true;
            bool actual = true;
            if (app.GUILSEngine.LSComMgr is LSCommunicationManager)
            {
                actual = true;
            }
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        public void ApplicationManagerConstrator_CheckConfig_Test()
        {
            ApplicationManager.MOCK_MODE = false;
            app = new ApplicationManager();
            Assert.IsNotNull(app.Config);
        }

        [TestMethod()]
        public void InitializeLogCategoryTest()
        {
            app = new ApplicationManager();
            Assert.IsNotNull(app.LogCategoryList);
            Assert.AreEqual("DEBUG", app.LogCategoryList[0]);
            Assert.AreEqual("INFO", app.LogCategoryList[1]);
            Assert.AreEqual("WARN", app.LogCategoryList[2]);
            Assert.AreEqual("ERROR", app.LogCategoryList[3]);
            Assert.AreEqual("FATAL", app.LogCategoryList[4]);
            Assert.AreEqual("ALL", app.LogCategoryList[5]);
            Assert.AreEqual("OFF", app.LogCategoryList[6]);
        }

        [TestMethod()]
        public void SaveSettingsTest()
        {
            app = new ApplicationManager();
            string expected = app.Config.Username;

            app.SaveSettings();

            LSConfigInfo config = XmlHelper.ReadFromFile<LSConfigInfo>(LSConfigInfo.FileNameWithPath);
            string actual = config.Username;

            Assert.AreEqual(expected, actual, "Config is not saving correctly");
        }
    }
}
