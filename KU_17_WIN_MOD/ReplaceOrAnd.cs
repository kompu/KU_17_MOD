using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace KU_17_WIN_MOD
{
    /// <summary>
    /// Метод полного перебора. 
    /// </summary>
    class ReplaceOrAnd
    {
        

        public static bool WorkWithOrAndV2(string local3)
        {
            local3 = local3.Replace("-1", "0");
            local3 = local3.Replace("-0", "1");

            while (true)
            {
                local3 = local3.Replace("0|0", "0").Replace("1|1", "1").Replace("1|0", "1").Replace("0|1", "1");
                local3 = local3.Replace("0&0", "0").Replace("1&0", "0").Replace("0&1", "0").Replace("1&1", "1");
                if (local3.Length == 1)
                {
                    break;
                }
            }
            switch (local3)
            {
                case "1":
                    return true;
                case "0":
                    return false;
                default:
                    return false; //error
            }
        }
    }

    class Combinations
    {
        public List<char[]> Combine(int round)
        {
            List<char[]> bagcombwords = new List<char[]>();
            for (int i = 0; i < (Math.Pow(2, round)); i++)
            {
                string linea = Convert.ToString(i, 2);
                char[] newchar = linea.PadLeft(round, '0').ToCharArray();
                bagcombwords.Add(newchar);
            }
            return bagcombwords;
        }
    }
}
