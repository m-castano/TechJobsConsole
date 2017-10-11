using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TechJobsConsole
{
    class JobData
    {
        static List<Dictionary<string, string>> AllJobs = new List<Dictionary<string, string>>();
        static bool IsDataLoaded = false;

        public static List<Dictionary<string, string>> FindAll()
        {
            //method LoadData in line 67 returns all info in csv file
            LoadData();
            //AllJobs is a dict with kvp
            return AllJobs;
        }

        /*
         * Returns a list of all values contained in a given column,
         * without duplicates. 
         */
        //From Program.cs line 48, FindAll is called with (columnChoice)
        //core competency, employer, location, or position type
        public static List<string> FindAll(string column)
        {
            LoadData();

            List<string> values = new List<string>();

            //Alljobs is a list of dict items: e.g. AllJobs==[employer:Lockerdome, etc.]
            foreach (Dictionary<string, string> job in AllJobs)
            {
                //e.g. aValue==Lockerdome
                string aValue = job[column];

                if (!values.Contains(aValue))
                {
                    //values is a list (line 32) so it stores all the employer names
                    values.Add(aValue);
                }
            }
            return values;
        }
        //from Program.cs, line 82, calls FindByColumnAndValue(columnChoice e.g. employer, searchTerm e.g. lockerdome)
        public static List<Dictionary<string, string>> FindByColumnAndValue(string column, string value)
        {
            // load data, if not already loaded
            LoadData();

            //"jobs" is populated in line 66
            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>();

            //AllJobs is a List.Dict with all data. e.g. 1 iteration == row[employer,lockerdome] 
            foreach (Dictionary<string, string> row in AllJobs)
            {
                //1 iteration e.g. aValue==lockerdome column==employer-->row==lockerdome
                string aValue = row[column];
                aValue = aValue.ToLower();

                //e.g. if aValue==lockerdome
                if (aValue.Contains(value))
                {
                    jobs.Add(row);//jobs[employer:lockerdome]
                }
            }

            return jobs;
        }

        /*
         * Load and parse data from job_data.csv
         */
        private static void LoadData()
        {

            if (IsDataLoaded)
            {
                return;
            }

            //List of arrays?
            List<string[]> rows = new List<string[]>();

            using (StreamReader reader = File.OpenText("job_data.csv"))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    string[] rowArrray = CSVRowToStringArray(line);
                    if (rowArrray.Length > 0)
                    {
                        rows.Add(rowArrray);
                    }
                }
            }

            string[] headers = rows[0];
            rows.Remove(headers);

            // Parse each row array into a more friendly Dictionary
            foreach (string[] row in rows)
            {
                Dictionary<string, string> rowDict = new Dictionary<string, string>();

                for (int i = 0; i < headers.Length; i++)
                {
                    rowDict.Add(headers[i], row[i]);
                }
                //AllJobs is a List.Dict declared in line 11
                AllJobs.Add(rowDict);
            }

            IsDataLoaded = true;
        }

        /*
         * Parse a single line of a CSV file into a string array
         */
        private static string[] CSVRowToStringArray(string row, char fieldSeparator = ',',
            char stringSeparator = '\"')
        {
            bool isBetweenQuotes = false;
            StringBuilder valueBuilder = new StringBuilder();
            List<string> rowValues = new List<string>();

            // Loop through the row string one char at a time
            foreach (char c in row.ToCharArray())
            {
                if ((c == fieldSeparator && !isBetweenQuotes))
                {
                    rowValues.Add(valueBuilder.ToString());
                    valueBuilder.Clear();
                }
                else
                {
                    if (c == stringSeparator)
                    {
                        isBetweenQuotes = !isBetweenQuotes;
                    }
                    else
                    {
                        valueBuilder.Append(c);
                    }
                }
            }

            // Add the final value
            rowValues.Add(valueBuilder.ToString());
            valueBuilder.Clear();

            return rowValues.ToArray();
        }

        public static List<Dictionary<string, string>> FindByValue(string term)
        {
            LoadData();

            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>();

            foreach (Dictionary<string, string> row in AllJobs)
            {
                foreach (KeyValuePair<string, string> kvp in row)
                {
                    string value = kvp.Value;
                    value = value.ToLower();
                    if (value.Contains(term))
                    {
                        jobs.Add(row);
                        break;
                    }
                }
            }
            return jobs;
        }

    }
}