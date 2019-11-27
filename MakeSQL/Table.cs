using System;
using System.Collections.Generic;
using System.Text;

namespace MakeSQL
{
    [Serializable]
    class Table
    {
        private String tablename;
        public String tableName { get { return tablename; }
            set
            {
                if (value == null || value.TrimStart().Equals(""))
                    throw new NullReferenceException("Table name must have a value");
                tablename = value.TrimStart();
            }
        }
        List<Row> table = new List<Row>();
        private int tableSize;

        public Table()
        {

        }
        public Table(string a)
        {
            this.tableName = a;
            this.tableSize = 0;
        }

        public void AddColumns(string a)
        {
            string input = a.Replace(" ", "");
            string[] words = input.Split(",");
            if (words.Length == 0)
            {
                Console.WriteLine("No value entered");
            }
            else
            {
                Row newRow = new Row();
                for (int i = 0; i < words.Length; i++)
                {
                    newRow.PutRowInfo(words[i]);
                    tableSize++;
                }
                table.Add(newRow);
            }

        }

        public void GetTable()
        {
            foreach (var item in table)
            {
                item.GetRows();
            }

        }

        public void GetColumns()
        {
            table[0].GetRows();
        }

        public void Insert(string a)
        {
            string input = a.Replace(" ", "");
            string[] words = input.Split(",");
            if(words.Length == 0)
            {
                Console.WriteLine("No value entered");
            }
            else if(words.Length <= tableSize)
            {
                Row newRow = new Row();
                for(int i = 0; i < words.Length; i++)
                {
                    newRow.PutRowInfo(words[i]);
                }
                table.Add(newRow);
            }
            else
            {
                Console.WriteLine("Error");
            }
        }


        public void Delete(string a)
        {
            int indx = 0;
            for (int i = table.Count - 1; i >= 1; i--)
            {
                if(table[i].ContainsValue(a))
                {
                    table.Remove(table[i]);
                    indx++;
                }
            }
            Console.WriteLine($"{indx} rows have been deleted.");
        }

        public void Update(string oldValue, string newValue, string uniqueValue)
        {
            int indx = 0;
            for(int i = 1; i < table.Count; i++)
            {
                if (!uniqueValue.Equals("") && table[i].ContainsValue(uniqueValue))
                {
                    table[i].ChangeData(oldValue, newValue);
                    Console.WriteLine("Row has been updated");
                    break;
                }
                else
                {
                    table[i].ChangeData(oldValue, newValue);
                    indx++;
                    Console.WriteLine($"{indx} rows have been updated");
                }
            }
        }

        public void TwoTableJoin(Table secondTable, string key)
        {
            bool firstFound = false;
            bool secondFound = false;
            foreach(var row in this.table)
            {
                if (row.ContainsValue(key))
                {
                    firstFound = true;
                }
            }
            foreach (var row in secondTable.table)
            {
                if (row.ContainsValue(key))
                {
                    secondFound = true;
                }
            }
            if(firstFound && secondFound)
            {
                Table joinedTable = new Table();
                string data = "";
                String[] rowsData = data.Trim().Split(",");
                foreach (var row in this.table)
                {
                    Console.WriteLine(row.GetRow());
                    data = row.GetRow();
                    
                    foreach (var item in rowsData)
                    {
                        Console.Write(item + "-");
                    }
                    Console.WriteLine();

                }
            }
            else
            {
                Console.WriteLine("One of the tables does not contain the key");
            }

        }

        public void Select(string a)
        {
            bool rowFound = false;
            table[0].GetRows();
            for (int i = 1; i < table.Count; i++)
            {
                if (table[i].ContainsValue(a))
                {
                    table[i].GetRows();
                    rowFound = true;
                }
            }
            if (!rowFound)
            {
                Console.WriteLine("There was no row with such data");
            }
        }

    }

}

