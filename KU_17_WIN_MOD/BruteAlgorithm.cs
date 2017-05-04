using System.Collections.Generic;
using System.Diagnostics;

namespace KU_17_WIN_MOD
{
    class BruteAlgorithm
    {
        Stopwatch sw = new Stopwatch();
        public int _numChars;
        public List<string> initAlgorithm(string[] local, bool onlyFirstData, string input, int maxtime)
        {
            sw.Reset();
            sw.Start();
            List<string> resultList = new List<string>();
            Dictionary<string, int> letternum = new Dictionary<string, int>();

            int count = 0;
            foreach (string t in local)
            {
                if (letternum.ContainsKey(t)) continue;
                letternum.Add(t, count);
                count++;
            }
            int numChars = letternum.Count;

            List<char[]> ses = new Combinations().Combine(numChars);

            string local2 = input.Replace(" ", "").Replace("^", ",").Replace("v", ".");

            foreach (char[] se in ses)
            {
                string local3 = local2;
                string tempstring = "";
                foreach (var i in letternum)
                {
                    int ubicacoin = letternum[i.Key];
                    string resulado = se[ubicacoin].ToString();
                    local3 = local3.Replace(i.Key, resulado);
                    tempstring += i.Key + "=" + resulado + ", ";
                }

                bool resultad = ReplaceOrAnd.WorkWithOrAndV2(local3);

                if (!resultad) continue;
                resultList.Add(tempstring + ": " + resultad);
                if (onlyFirstData) break;
                if (sw.Elapsed.TotalSeconds >= maxtime)
                {
                    break;
                }
            }
            _numChars = numChars;
            sw.Stop();
            return resultList;
        }
    }
}
