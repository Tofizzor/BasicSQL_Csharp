using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MakeSQL
{
    /* Author Justas Karalevicius
     * Created on 12/11/2019
     * Project: SQL in C#
     * Description: A program that acts as a database
     *              using similar to SQL commands
     */
    class Program
    {
        static void Main(string[] args)
        {
            var ui = new UserInterface();
            ui.GetChoice();

        }
    }
    [Serializable]
    class UserInterface
    {
        List<Table> tables = new List<Table>();
        //UI in progress

        public void CreateTable(string userInput)
        {
            bool exists = false;
            foreach (var table in tables)
            {
                if (userInput == table.tableName)
                {
                    exists = true;
                }
                else
                    exists = false;
            }
            if (!exists)
            {
                Table newTable = new Table(userInput);
                newTable.AddColumns(Util.Console.GetInfo($"Table with name {newTable.tableName} has been created, please enter the columns each seperated with ,"));
                tables.Add(newTable);
                Console.WriteLine("Columns have been created");
                
            }
            else
                Console.WriteLine($"Table with the name {userInput} has been already created.");
        }
        
        public void InsertRow(string a)
        {
            foreach (var table in tables)
            {
                if (table.tableName == a)
                {
                    bool adding = true;
                    Console.Write("Table has been found: ");
                    table.GetColumns();

                    table.Insert(Util.Console.GetInfo("Please enter the data into the table seperating fields with ,"));
                    while (adding)
                    {
                        if (Util.Console.GetInfo("Do you want to insert another row? Y/N").ToLower() == "y")
                        {
                            table.Insert(Util.Console.GetInfo("Enter data."));
                        }
                        else
                            adding = false;
                    }
                    
                }
            }
        }

        public void UpdateRow(string a)
        {
            foreach(var table in tables)
            {
                if(table.tableName == a)
                {
                    bool updating = true;
                    Console.Write("Table has been found: ");
                    table.GetTable();

                    string answer = Util.Console.GetInfo("Type Y to update all the rows or N to update specific row? Y/N").ToLower();
                    string uniqueValue = "";
                    if (answer.Equals("n"))
                    {
                        uniqueValue = Util.Console.GetInfo("Enter unique data from the row.");
                    }
                    else if (!answer.Equals("y"))
                    {
                        Console.WriteLine("Unrecognised command.");
                        break;
                    }

                    string oldValue = Util.Console.GetInfo("Enter old data.");
                    string newValue = Util.Console.GetInfo("Enter new data.");
                    table.Update(oldValue, newValue, uniqueValue);

                }
            }
        }

        public void PrintTables()
        {
            int indx = 1;
            foreach (var table in tables)
            {
                Console.WriteLine($"{indx}.{table.tableName}");
                indx++;
            }
        }

        public void SelectTable(string a)
        {
            bool tableFound = false;
            foreach (var table in tables)
            {
                if (table.tableName == a)
                {
                    table.GetTable();
                    tableFound = true;
                }

                //table.Select(a);
            }
            if (!tableFound)
            {
                Console.WriteLine("There is no such table created");
            }
        }

        public void RowSelect(string userInput)
        {
            string input = userInput.Replace(" ", "");
            string[] words = input.Split(",");
            if (words.Length == 1)
            {
                foreach (var table in tables)
                {
                    table.Select(userInput);
                }
            }
            else if (words.Length == 2)
            {
                foreach (var table in tables)
                {
                    if (table.tableName == words[0])
                    {
                        table.Select(words[1]);
                    }
                }
            }
            else
            {
                Console.WriteLine("Not implemented yet..");
            }
        }

        public void DeleteRow(string a)
        {
            bool tableFound = false;
            foreach (var table in tables)
            {
                if(table.tableName == a)
                {
                    table.Delete(Util.Console.GetInfo("When you will enter data, all the rows containing it will be deleted!\nEnter the data. "));
                    tableFound = true;
                }
            }
            if (!tableFound)
            {
                Console.WriteLine("There is no such table created");
            }
        }

        public void DropTable(string a)
        {
            bool tableFound = false;
            for (int i = 0; i < tables.Count; i++)
            {
                if (tables[i].tableName == a)
                {
                    tables.RemoveAt(i);
                    tableFound = true;
                }
            }
            if (!tableFound)
            {
                Console.WriteLine("There is no such table created");
            }
        }

        public delegate void GetFunction(string a);

        public void CheckForTables(int tablesCount, GetFunction function, string msg)
        {
            if(tablesCount == 0)
            {
                Console.WriteLine("No table has been created.");
            }
            else
            {
                function(Util.Console.GetInfo(msg));
            }
        }

        public void JoinTables()
        {
            if (tables.Count < 2)
            {
                Console.WriteLine("Not enough tables in the system.");
            }
            else
            {
                Console.WriteLine("Existing tables:");
                PrintTables();
                bool found = false;
                int tries = 0;
                while (!found)
                {
                    
                    string input = Util.Console.GetInfo("Enter table names and a key seperated by ,");
                    if (input.Trim().ToLower() == "stop")
                    {
                        Console.WriteLine("Back to main menu.");
                        break;
                    }
                    else
                    {
                        string[] words = input.Replace(" ", "").Split(",");
                        if (words.Length == 3)
                        {
                            
                            if (!FindTable(words[0]).Equals(null))
                            {
                                Table firstTable = FindTable(words[0]);
                                if (!FindTable(words[1]).Equals(null))
                                {
                                    Table secondTable = FindTable(words[1]);
                                    firstTable.TwoTableJoin(secondTable, words[2]);
                                }
                                
                            }
                            
                        }

                        else
                        {
                            Console.WriteLine("Enter 3 values please.");
                            tries++;
                            if (tries > 3)
                            {
                                Console.WriteLine("If you stuck, to go back to main menu write: stop");
                            }
                        }


                    }
                }

            }
        }

        private Table FindTable(string inputname)
        {
            foreach(var table in tables)
            {
                if(table.tableName == inputname)
                {
                    return table;
                }
            }
            throw new NullReferenceException("One of the tables you have entered does not exits.\n" +
                "Returned to main menu...");
            
        }


        public void GetChoice()
        {
            //string choice = Console.ReadLine();
            bool exit = false;
            Console.WriteLine("Welcome to simple SQL made by Justas Karalevicius \n" +
                "To get to know how to use the program please type: Help \n" +
                "Have Fun!");
            while (!exit)
            {
                try
                {
                    string choice = Console.ReadLine().ToLower().Trim();
                    switch (choice)
                    {
                        case "drop table":
                            DropTable(Util.Console.GetInfo("Enter the table name you want to delete"));
                            break;
                        case "view table":
                            SelectTable(Util.Console.GetInfo("Enter the table name you want to view"));
                            break;
                        case "view tables":
                            PrintTables();
                            break;
                        case "create table":
                            CreateTable(Util.Console.GetInfo("Enter table name"));
                            break;
                            //Call for delegate function. Checks for any existing tables, 
                            //select function to perform and write output message to the user
                        case "select":
                            CheckForTables(tables.Count, RowSelect, "Enter table and data.");
                            break;
                        case "insert":
                            CheckForTables(tables.Count, InsertRow, "Enter table name.");
                            break;
                        case "delete":
                            CheckForTables(tables.Count, DeleteRow, "Enter table name.");
                            break;
                        case "update":
                            CheckForTables(tables.Count, UpdateRow, "Enter table name.");
                            break;
                        case "join":
                            JoinTables();

                            break;
                        case "save":
                            Console.WriteLine("Not implemented");
                            break;
                        case "load":
                            Console.WriteLine("Not implemented");
                            break;
                        //Commands: Create Table, Insert, Delete, View Table/s, Drop Table, Exit
                        case "help":
                            Console.WriteLine("|To create table write: Create Table\n" +
                                            "Now enter the tables properties while seperating each " +
                                            "propery with comma - name, column1, column2 etc.\n" +
                                            "|To create new row in the table write: Insert\n" +
                                            "Now enter: table name, columnData etc.\n" +
                                            "|To update rows in table: Update\n" +
                                            "Select the table you want to update and enter old and new data\n"+
                                            "|To delete rows containing specified data type: Delete\n" +
                                            "Now enter: table name, dataToDelete\n" +
                                            "|To view the tables created write: View Tables\n" +
                                            "|To view a table with the rows inside write: View table\n" +
                                            "|To delete table write: Drop Table\n" +
                                            "Write the name of the table you want to delete\n" +
                                            "|Save all the tables: Save\n" +
                                            "|Load all the tables from a file: Load\n" +
                                            "|To exit from the program simply write: Exit");
                            break;
                        case "exit":
                            Console.WriteLine("Thank you for your time. Bye.");
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Unrecognised command, please try again or use help command");
                            break;


                    }
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine(ex);
                }
                catch (Exception)
                {
                    Console.WriteLine("Error!");
                }
            }
        }

    }

}

