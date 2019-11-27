using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Util
{

    public static class ExtensionMethods
    {
        public static int toInt(this string value)
        {
            return int.Parse(value);
        }
    }

    class Console
    {
        static public string GetInfo(string info)
        {
            System.Console.WriteLine(info);
            return System.Console.ReadLine();
        }

        static public string GetInfo(int info)
        {
            System.Console.WriteLine(info);
            return System.Console.ReadLine();
        }

        static public int GetIntInfo(string info)
        {
            try
            {
                System.Console.WriteLine(info);
                return System.Console.ReadLine().toInt();
            }
            catch (Exception)
            {
                throw new FormatException("Input was not a number");
            }
        }

        


    }
}
