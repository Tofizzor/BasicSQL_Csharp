using System;
using System.Collections.Generic;
using System.Text;

namespace MakeSQL
{
    class Row
    {
        List<String> rows = new List<string>();

        public Row()
        {
        }

        public void PutRowInfo(string a)
        {
            if (a == null || a.TrimStart().Equals(""))
                throw new NullReferenceException("Row must have a value");
            rows.Add(a);
        }

        public string GetRow()
        {
            StringBuilder data = new StringBuilder();
            foreach(var row in rows)
            {
                data.Append(row.ToString() + ",");
            }
            data.Remove(data.Length - 1, 1);
            return data.ToString();
        }

        //Prints out the whole list of values
        public void GetRows()
        {
            for (int i = 0; i < rows.Count; i++)
            {
                Console.Write(rows[i] + "|");
            }
            Console.WriteLine();
        }

        public bool ContainsValue(string a)
        {
            if (rows.Contains(a))
            {
                return true;
            }
            else
                return false;
        }

        public void ChangeData(string oldValue, string newValue)
        {
            int index = rows.IndexOf(oldValue);
            if (index != -1)
                rows[index] = newValue;
        }

    }
}
