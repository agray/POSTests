using System;
using com.bp.remoteservices.file;

namespace com.bp.test {
    public class FileTester {
        public static void test() {

            Console.WriteLine("Contents of testFile.txt = " + FileReader.readAllText("testFile.txt", false));

            string[] lines = FileReader.readAllLines("testFile.txt", false);

            Console.WriteLine("Contents of testfile as lines =:");
            foreach(string line in lines) {
                Console.WriteLine("\t" + line);
            }

            // Keep the console window open in debug mode.
            //Console.WriteLine("Press any key to exit.");
            //Console.ReadKey();
        }
    }
}