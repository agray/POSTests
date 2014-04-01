using com.bp.remoteservices;
using NUnit.Framework;
using POSTests.Properties;
using White.Core;
using White.Core.InputDevices;
using White.Core.UIItems;
using White.Core.UIItems.WindowItems;

namespace POSTests {
    [TestFixture]
    public class UnitTests {
        private TestUtil util = new TestUtil();
        private Settings SETTINGS = Settings.Default;

        private White.Core.Application application;
        private IUIItem rightPanelPane;
        private Win32Window window;
        private AttachedMouse mouse;
        private AttachedKeyboard keyboard;

        //private string passwordFirstChar = "2";
        //private string passwordSecondChar = "2";
        //private string passwordThirdChar = "1";
        //private string passwordFourthChar = "0";

        private string passwordFirstChar = "8";
        private string passwordSecondChar = "7";
        private string passwordThirdChar = "0";
        private string passwordFourthChar = "3";
        
        private ClickLocation TSOperator = new ClickLocation("EddieOperator", 600, 110);
        private ClickLocation OperatorStartOKButton = new ClickLocation("OperatorStartOKButton", 990, 700);
        private ClickLocation ChubbaChupButton = new ClickLocation("ChubbaChupButton", 570, 450);
        private ClickLocation CheckOutButton = new ClickLocation("CheckOutButton", 230, 710);
        private ClickLocation CounterLinesButton = new ClickLocation("CounterLinesButton", 930, 450);
        private ClickLocation NextPageButton = new ClickLocation("NextPageButton", 950, 590);
        private ClickLocation ExactCashButton = new ClickLocation("ExactCashButton", 180, 647);
        private ClickLocation CardPaymentButton = new ClickLocation("CardPaymentButton", 55, 647);
        private ClickLocation CloseButton = new ClickLocation("CloseButton", 740, 678);

        [SetUp]
        public void Setup() {
            // Start the app
            
            application = Application.Launch(SETTINGS.POSApplicationPath);
            util.sleep(SETTINGS.ApplicationLaunchWaitMS);
        }

        [TearDown]
        public void TearDown() {
            application.Kill();
        }

        //[Test]
        //public void CanLaunchApplicationTest() {
        //    Assert.IsNotNull(application);
        //}

        [Test]
        public void LoginTest() {
            Assert.IsNotNull(application);

            initialSetup();
            login(mouse, keyboard);
            //window.LogStructure();

            window = util.getWindow(application, "");

            Assert.IsNotNull(window);
            util.logMessage("Got POS window");

            rightPanelPane = util.getItemByName(window, "RightPanel");
            Assert.IsNotNull(rightPanelPane);
            util.logMessage("Got RightPanel");
        }

        [Test]
        public void Transaction_ChubbaChupExactCashTest() {
            Assert.IsNotNull(application);

            initialSetup();
            login(mouse, keyboard);

            DoExactCashTransaction(CounterLinesButton, ChubbaChupButton, 0, mouse, keyboard);

            Assert.IsTrue(Asserter.assert("Chup Chups 13G", "1.00, 0.72, 0.07"));
        }

        #region UtilMethods
        private void initialSetup() {
            // Find the main window
            window = util.getWindow(application, "OperatorSelectDlg");
            Assert.IsNotNull(window);
            util.logMessage("Got Application Window");
            util.sleep(10000);

            mouse = window.Mouse;
            keyboard = window.Keyboard;
        }

        private void login(AttachedMouse mouse, AttachedKeyboard keyboard) {
            util.logMessage("Performing Login Process");
            util.clickAndWait(mouse, TSOperator, 5000);

            keyboard.Enter(passwordFirstChar);
            keyboard.Enter(passwordSecondChar);
            keyboard.Enter(passwordThirdChar);
            keyboard.Enter(passwordFourthChar);
            util.logMessage("Entered password");
            util.sleep(2000);

            util.clickAndWait(mouse, OperatorStartOKButton, 5000);
            util.logMessage("Completed Login Process");
        }

        private void DoExactCashTransaction(ClickLocation itemGroup, ClickLocation ItemLocation, int page, AttachedMouse mouse, AttachedKeyboard keyboard) {
            util.sleep(10000);
            if(page != 0) {
                for(int i = 0; i < page; i++) {
                    util.clickAndWait(mouse, NextPageButton, 5000);
                }
            } else {
                util.clickAndWait(mouse, itemGroup, 5000);
            }

            util.clickAndWait(mouse, ItemLocation, 5000);
            util.clickAndWait(mouse, ExactCashButton, 5000);
        }
        #endregion
    }
}