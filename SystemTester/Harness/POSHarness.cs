using System;
using com.bp.remoteservices.file;

namespace com.bp.test {
    public class POSHarness : FileProperties {
        public static void test() {
            Console.Out.WriteLine("Writing " + 
                                  "AssertTrans_TEMPLATE.sql" + Environment.NewLine +
                                  "AssertTrans.sql" + Environment.NewLine +
                                  "ActualsData.txt" + Environment.NewLine +
                                  "Chup Chups 13G,ActualsData.txt" + Environment.NewLine + 
                                  "1,.72,.07" +
                                  " to " + 
                                  WORKING_DIR + DIR_SEP + "Handball.txt");
            FileWriter.writeAllText(WORKING_DIR + DIR_SEP + "Handball.txt", true, "AssertTrans_TEMPLATE.sql" + Environment.NewLine +
                                                                                  "AssertTrans.sql" + Environment.NewLine +
                                                                                  "ActualsData.txt" + Environment.NewLine +
                                                                                  "Chup Chups 13G,ActualsData.txt" + Environment.NewLine + 
                                                                                  "1,.72,.07"); 
        }
    }
}