using System;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using White.Core.Factory;
using White.Core.InputDevices;
using White.Core.UIItems;
using White.Core.UIItems.Finders;
using White.Core.UIItems.WindowItems;

namespace POSTests {
    public class TestUtil {
        
        //public bool mapNetworkDrive(string driveLetter, string UNCPath) {
        //    //Process.Start("net.exe", "use " + driveLetter + ": " + UNCPath);
        //    //if(nd.MapNetworkDrive(@"\\servername\shardrive", "Z:", null, null) == 0) {
        //    if(NetworkDrive.MapNetworkDrive()) {
        //        Console.WriteLine("Mapped " + UNCPath + " to drive " + driveLetter);
        //        return true;
        //    } else {
        //        Console.WriteLine("Failed to map " + UNCPath + " to drive " + driveLetter + "!");
        //        return false;
        //    }
        //}

        public Win32Window getWindow(White.Core.Application app, string name) {
            return (Win32Window)app.GetWindow(name, InitializeOption.NoCache);
        }

        public AutomationElement getApplicationByMUIAFramework() {
            AutomationElement rootElement = AutomationElement.RootElement;
            System.Windows.Automation.Condition condition = new PropertyCondition(AutomationElement.NameProperty, "OperatorSelectDlg");
            return rootElement.FindFirst(TreeScope.Children, condition);
        }

        public AutomationElement getElementByMUIFramework(AutomationElement parentElement, string value) {
            AutomationElement element = null;
            int numWaits = 0;
            do {
                logMessage("Looking " + numWaits + " for element with Name " + value);
                element = parentElement.FindFirst(TreeScope.Subtree,
                                                 new PropertyCondition(AutomationElement.NameProperty,
                                                                       value));
                ++numWaits;
                sleep(200);
            }
            while(element == null && numWaits < 50);

            return element;
        }

        public IUIItem getItemByName(Win32Window window, string name) {
            IUIItem[] items = window.GetMultiple(SearchCriteria.All);
            foreach(IUIItem item in items) {
                logMessage("\nItem Name: " + item.Name);
                if(item.NameMatches(name)) {
                    AutomationElement elementList = item.AutomationElement;
                    logMessage("elementList Name: " + elementList.Current.Name);
                    if(elementList == null) {
                        logMessage("elementList is null");
                    }

                    //logMessage("Is Content Element: " + elementList.Current.IsContentElement);
                    //logMessage("Is Control Element: " + elementList.Current.IsControlElement);

                    Panel p = (Panel)item;
                    logMessage("Panel Items Count: " + p.Items.Count);
                    UIItemCollection panelItemCollection = p.Items;
                    foreach(UIItem ItemInPanel in panelItemCollection) {
                        logMessage("Name: " + ItemInPanel.Name);
                        logMessage("Bounds: " + ItemInPanel.Bounds.ToString());
                    }

                    //Panel itemKbd = (Panel)panelItemCollection[1];
                    //logMessage("Kbd Items Count: " + itemKbd.Items.Count);
                    //panelItemCollection = itemKbd.Items;
                    //foreach(UIItem ItemInKbd in panelItemCollection) {
                    //    logMessage("Name: " + ItemInKbd.Name);
                    //    logMessage("Bounds: " + ItemInKbd.Bounds.ToString());
                    //}

                    //logMessage("Bounds: " + item.Bounds.ToString());
                    //logMessage("toString: " + item.ToString());
                    logMessage("\nReturning " + item.Name);
                    return item;
                }
            }
            //No Items exist
            return null;
        }

        //public IUIItem getPaneElementByWhite(Win32Window window, string name) {
        //    IUIItem[] items = null;
        //    items = window.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));

        //    logMessage("Num Items: " + items.Length);

        //    foreach(IUIItem item in items) {
        //        logMessage("Name:" + item.Name);
        //        if(item.Name.Equals(name)) {
        //            return item;
        //        } else {
        //            return null;
        //        }
        //    }
        //    //No Panes exist
        //    return null;
        //}

        public void logMessage(string s) {
            Console.Out.WriteLine(s);
        }

        public void sleep(int millis) {
            Thread.Sleep(millis);
        }

        public void clickAndWait(AttachedMouse mouse, ClickLocation cl, int milli) {
            mouse.Click(cl.point);
            logMouseClick(cl.name, cl.point);
            sleep(milli);
        }

        private void logMouseClick(string s, Point location) {
            logMessage("Clicked " + s + " at location [x, y]: [" + location + "]");
        }
    }
}