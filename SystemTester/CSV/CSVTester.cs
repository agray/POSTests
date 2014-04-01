using System;
using com.bp.remoteservices;


namespace com.bp.test {
    public class CSVTester {
        public static void test() {
            Console.WriteLine(CSV.cleanseCSV("1.000000000000,  0.720000000000000, 1.8400000000000001"));
        }
    }
}
