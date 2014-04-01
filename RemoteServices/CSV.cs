using System;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;

namespace com.bp.remoteservices {
    public static class CSV {
        private static char[] DEFAULT_DELIMITER = {','};

        public static string toCSV(string[] inputArray) {
            if(inputArray.Length == 0) {
                return "";
            } else {
                return String.Join(", ", inputArray);
            }
        }

        public static ArrayList ArrayListFromCSV(string inputCSVString, char[] delimiter) {
            if(!inputCSVString.Contains(delimiter.ToString())) {
                //Not a CSV String that uses the delimiter
                return null;
            } else {
                ArrayList list = new ArrayList();
                string trimmedCSVString = removeSpacesAfterDelimiter(inputCSVString, delimiter.ToString());
                string[] intermediate = trimmedCSVString.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                foreach(string s in intermediate) {
                    list.Add(s);
                }
                return list;
            }
        }

        public static string[] StringArrayFromCSV(string inputCSVString, char[] delimiter) {
            if(inputCSVString.IndexOf(delimiter[0]) == -1) {
                //Not a CSV String that uses the delimiter
                return null;
            } else {
                return inputCSVString.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public static string[] StringArrayFromCSV(string inputCSVString) {
            return StringArrayFromCSV(inputCSVString, DEFAULT_DELIMITER);
        }

        public static string[] StringArrayFromCSV(string inputCSVString, bool removeAllSpaces) {
            if(removeAllSpaces) {
                return StringArrayFromCSV(Regex.Replace(inputCSVString, @"\s+", ""), DEFAULT_DELIMITER);
            } else {
                return StringArrayFromCSV(inputCSVString, DEFAULT_DELIMITER);
            }
        }

        public static ArrayList ArrayListFromCSV(string inputCSVString) {
            return ArrayListFromCSV(inputCSVString, DEFAULT_DELIMITER);
        }

        public static string cleanseCSV(string dirty) {
            string[] fields = StringArrayFromCSV(dirty, true);
            double result;

            ArrayList list = new ArrayList();
            foreach(string field in fields) {
                if(double.TryParse(field, out result)) {
                    list.Add(result.ToString("F2", CultureInfo.InvariantCulture));
                } else {
                    list.Add(field);
                }
            }

            return toCSV((string[])list.ToArray(typeof(string)));
        }

        private static string removeSpacesAfterDelimiter(string inputCSVString, string delimiter){
            return inputCSVString.Replace(delimiter + " ", delimiter);
        }
    }
}