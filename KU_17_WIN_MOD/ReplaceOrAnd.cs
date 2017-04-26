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
        public bool WorkWithOrAndV1(string local3)
        {
            Regex or = new Regex(@"([\w])(\|)(\w)");
            Regex and = new Regex(@"([\w])(\&)(\w)");
            local3 = local3.Replace("-1", "0");
            local3 = local3.Replace("-0", "1");

            while (true)
            {
                Match matchOr = or.Match(local3);
                int r2;
                int start;
                int len;
                if (matchOr.Success)
                {
                    start = matchOr.Index;
                    len = matchOr.Length;
                    r2 = matchOr.Groups[1].Value + matchOr.Groups[3].Value == "00" ? 0 : 1;
                }
                else
                {
                    Match matchAnd = and.Match(local3);
                    if (matchAnd.Success)
                    {
                        start = matchAnd.Index;
                        len = matchAnd.Length;
                        r2 = matchAnd.Groups[1].Value + matchAnd.Groups[3].Value == "11" ? 1 : 0;
                    }
                    else
                    {
                        break;
                    }
                }
                string r1 = local3.Substring(0, start);
                string r3 = local3.Substring(start + len);
                local3 = r1 + r2 + r3;

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

        public bool WorkWithOrAndV2(string local3)
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
