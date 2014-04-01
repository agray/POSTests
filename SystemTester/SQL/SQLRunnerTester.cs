using System;
using com.bp.remoteservices.sybase;

namespace com.bp.test {
    public class SQLRunnerTester {
        public static void test() {
            if(SQLRunner.run("getTransactionDetails.sql", "out.txt")) {
                Console.WriteLine("SQLRunnerTest succeeded");
            } else {
                Console.WriteLine("SQLRunnerTest failed");
            }
        }
    }
}