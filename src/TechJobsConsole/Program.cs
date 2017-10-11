using System;
using System.Collections.Generic;

namespace TechJobsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create two Dictionary vars to hold info for menu and data

            // Top-level menu options
            Dictionary<string, string> actionChoices = new Dictionary<string, string>();
            actionChoices.Add("search", "Search");
            actionChoices.Add("list", "List");

            // Column options
            Dictionary<string, string> columnChoices = new Dictionary<string, string>();
            columnChoices.Add("core competency", "Skill");
            columnChoices.Add("employer", "Employer");
            columnChoices.Add("location", "Location");
            columnChoices.Add("position type", "Position Type");
            columnChoices.Add("all", "All");

            Console.WriteLine("Welcome to LaunchCode's TechJobs App!");

            // Allow user to search/list until they manually quit with ctrl+c
            while (true)
            {
                //calls GetUserSelection returns "View Jobs by" and "search" or "list"
                string actionChoice = GetUserSelection("View Jobs", actionChoices);
                
                if (actionChoice.Equals("list"))
                {
                    //calls GetUserSelection returns "List by" and key: core competency, employer,
                    //location, position type, or all
                    string columnChoice = GetUserSelection("List", columnChoices);

                    if (columnChoice.Equals("all"))
                    {
                        //Method PrintJobs line 126 is called and returns ALL jobs and details in kvp form
                        PrintJobs(JobData.FindAll());
                    }
                    else //columnChoice == keys: core competency, employer, location, or position type
                    {
                        //The string "results" stores all jobs according to choice of column selcted by user
                        //without duplicates
                        List<string> results = JobData.FindAll(columnChoice);

                        Console.WriteLine("\n*** All " + columnChoices[columnChoice] + " Values ***");
                        foreach (string item in results)
                        {
                            //prints all results of the columnChoice without duplicates
                            Console.WriteLine(item);
                        }
                    }
                }
                else // choice is "search"
                {
                    // How does the user want to search (i.e. core competency, employer, location,
                    //or position type)
                    //calls GetUserSelection returns "Search by" and key: core competency, employer,
                    //location, position type, or all
                    string columnChoice = GetUserSelection("Search", columnChoices);

                    // What is their search term?
                    Console.WriteLine("\nSearch term: ");
                    string searchTerm = Console.ReadLine();

                    //Is searchTerm result missing here?

                    //declaration of list of dicts searchResults populated in line 75
                    List<Dictionary<string, string>> searchResults;

                    // Fetch results
                    if (columnChoice.Equals("all"))
                    {
                        searchResults = JobData.FindByValue(searchTerm.ToLower());
                        if (searchResults.Count == 0)
                        {
                            Console.WriteLine("No results");
                        }
                        else
                        {
                            PrintJobs(searchResults);
                        }
                    }
                    else//columnChoice==core competency, employer, location, or position type e.g. searchTerm==lockerdome
                    {
                        searchResults = JobData.FindByColumnAndValue(columnChoice, searchTerm.ToLower());
                        if (searchResults.Count == 0)
                        {
                            Console.WriteLine("No results");
                        }
                        else
                        {
                            PrintJobs(searchResults);
                        }
                    }
                }
            }
        }

        /*
         * Returns the key of the selected item from the choices Dictionary
         */
        private static string GetUserSelection(string choiceHeader, Dictionary<string, string> choices)
            //for actionChoice: choiceHeader=="View Jobs" choices==search/S and list/L
        {
            int choiceIdx;
            bool isValidChoice = false;
            string[] choiceKeys = new string[choices.Count];//array choiceKeys, choices.Count dec the # of elems (4 in actionChoices)

            int i = 0;
            foreach (KeyValuePair<string, string> choice in choices)//from line 30, choices is search&S and list&L
            {
                choiceKeys[i] = choice.Key; //After iterating, the array choiceKeys contains keys search and list
                i++;
            }

            do
            {
                Console.WriteLine("\n" + choiceHeader + " by:");//Line 30 "View jobs", prints "View jobs by"

                for (int j = 0; j < choiceKeys.Length; j++)//length==2 elements, line 88 obtains 2 keys s&l
                {
                    Console.WriteLine(j + " - " + choices[choiceKeys[j]]);//j==0,1; Prints 0 - Search, 1 - List
                }                                                          //Requests user's input

                string input = Console.ReadLine();//input stores user's input either 0 or 1 in string type
                choiceIdx = int.Parse(input);//turns user's choice into an integer

                if (choiceIdx < 0 || choiceIdx >= choiceKeys.Length)
                {
                    Console.WriteLine("Invalid choices. Try again.");
                }
                else
                {
                    isValidChoice = true;
                }

            } while (!isValidChoice);

            return choiceKeys[choiceIdx];//choiceKeys==keys search or list, choiceIdx==0 or 1
            //0 index will return search==S or 1 list==List
        }
        //from line 42, the parameters for PrintJobs are JobData.FindAll()
        //JobData is the class, FindAll the method which is of the same datatype
        //as the param of PrintJobs (someJobs) FindAll returns AllJobs which is a ListDict of all data
        private static void PrintJobs(List<Dictionary<string, string>> someJobs)
        {
            Console.WriteLine("*****");
            foreach (Dictionary<string, string> listItem in someJobs)
            {
                int jobsCounter = 0;
                foreach (KeyValuePair<string, string> kvp in listItem)
                {
                    string key = kvp.Key;
                    string value = kvp.Value;
                    Console.WriteLine(key+": "+ value);
                    jobsCounter++;
                }

                if (jobsCounter % 5 == 0) ;
                {
                    Console.WriteLine("*****");
                }
            }
        }
    }
}
