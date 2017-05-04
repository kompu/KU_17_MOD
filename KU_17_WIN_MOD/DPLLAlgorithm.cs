using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace KU_17_WIN_MOD
{
    /// <summary>
    /// DPLL алгоритм
    /// </summary>
    class DPLLAlgorithm
    {
        List<string> resultList = new List<string>();
        Dictionary<string, int> letternum = new Dictionary<string, int>();
        Dictionary<int, string> letter1 = new Dictionary<int, string>();
        private bool issolved;
        private bool allBad = false;
        public int _numChars;
        private byte[] letterState;
        Stopwatch sw = new Stopwatch();
        public List<string> DPLLMethodAllData(string[] local, string input, int maxtime)
        {
            sw.Reset();
            sw.Start();
            Clean();
            _maxtime = maxtime;


            string local2 = input.Replace(" ", "").Replace("^", ",").Replace("v", ".");
            int count = 0;
            foreach (string t in local)
            {
                if (!letternum.ContainsKey(t))
                {
                    letternum.Add(t, count);
                    letter1.Add(count, t);
                    count++;
                }
            }
            List<char[]> ses = new Combinations().Combine(letternum.Count);

            letterState = new byte[letternum.Count];

            for (int i = 0; i < letternum.Count; i++)
            {
                letterState[i] = 1;
            }

            bool isLeft = false;

            AlgorithmAllData(local2, ses);




            sw.Stop();
            _numChars = letternum.Count;
            return resultList;
        }
        private int _maxtime;

        private void AlgorithmAllData(string local2, List<char[]> ses)
        {
            byte value = 0;

            string[] clauses = divideClause(local2);
            Dictionary<string, char> fixedletters = new Dictionary<string, char>();
            for (int i = 0; i < clauses.Length; i++)
            {
                if (!clauses[i].Contains("&") && !clauses[i].Contains("|"))
                {

                    if (clauses[i].Contains("-"))
                    {
                        if (!fixedletters.ContainsKey(clauses[i].Substring(1, 1)))
                        {
                            fixedletters.Add(clauses[i].Substring(1, 1), '0');
                        }

                    }
                    else
                    {
                        if (!fixedletters.ContainsKey(clauses[i]))
                        {
                            fixedletters.Add(clauses[i], '1');
                        }
                    }
                }
            }


            Regex andInTwoOrs = new Regex(@"(\|[\-|\w]+\&[\-]?)(\w)(\&[\-|\w]+\|)");
            Regex and3 = new Regex(@"([\-|\w]+\&){3}([\-|\w])");
            Regex and2 = new Regex(@"([\||\&][\-|\w]+){2}\Z");
            Regex and1 = new Regex(@"(([\|][\-\w]+){2})[\&]([\-\w]+)\&");
            Regex andR = new Regex(@"\.*(\&)([\-\w]+)(\&)([\-|\w]+\|){2}[\-|\w]+\.*");

            Match matchOr = andInTwoOrs.Match(local2);
            string r2 = "";
            if (matchOr.Success)
            {
                r2 = matchOr.Groups[2].Value;
            }
            if (fixedletters.ContainsKey(r2))
            {
                fixedletters.Remove(r2);
            }

            Match matchand3 = and3.Match(local2);
            r2 = "";
            if (matchand3.Success)
            {
                r2 = matchand3.Groups[0].Value.Replace("&",",").Replace("|",",").Replace("-",",");
            }
            string[] Arrayr2 = r2.Split(new []{","}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string r2a in Arrayr2)
            {
                if (fixedletters.ContainsKey(r2a))
                {
                    fixedletters.Remove(r2a);
                }    
            }
            Match matchand2 = and2.Match(local2);
            r2 = "";
            if (matchand2.Success)
            {
                r2 = matchand2.Groups[0].Value.Replace("&", ",").Replace("|", ",").Replace("-", ",");
            }
            Arrayr2 = r2.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string r2a in Arrayr2)
            {
                if (fixedletters.ContainsKey(r2a))
                {
                    fixedletters.Remove(r2a);
                }
            }
            Match matchand1 = and1.Match(local2);
            r2 = "";
            if (matchand1.Success)
            {
                r2 = matchand1.Groups[3].Value.Replace("&", ",").Replace("|", ",").Replace("-", ",");
            }
            Arrayr2 = r2.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string r2a in Arrayr2)
            {
                if (fixedletters.ContainsKey(r2a))
                {
                    fixedletters.Remove(r2a);
                }
            }
            Match matchandR = andR.Match(local2);
            r2 = "";
            if (matchandR.Success)
            {
                r2 = matchandR.Groups[2].Value.Replace("&", ",").Replace("|", ",").Replace("-", ",");
            }
            Arrayr2 = r2.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string r2a in Arrayr2)
            {
                if (fixedletters.ContainsKey(r2a))
                {
                    fixedletters.Remove(r2a);
                }
            }

            Dictionary<string, char> tempfixedletters = new Dictionary<string, char>(fixedletters);
            
            foreach (KeyValuePair<string, char> pair in tempfixedletters)
            {
                int count = 0;
                foreach (string clause in clauses)
                {
                    if (clause.Contains(pair.Key))
                    {
                        count++;
                    }
                }
                if (count >= 2 )
                {
                    fixedletters.Remove(pair.Key);
                }

            }


            foreach (KeyValuePair<string, char> pair in fixedletters)
            {
                ses.RemoveAll(l => l[letternum[pair.Key]] != pair.Value);

            }

            List<char[]> finalList = new List<char[]>();
            finalList = ses.Clone();

            foreach (char[] c in finalList)
            {
                if (sw.Elapsed.TotalSeconds >= _maxtime)
                {
                    break;
                }
                if (CheckCorrectly(local2, c))
                {
                    Print(c);
                }
            }

            _numChars = letternum.Count;
        }


        public List<string> DPLLMethodFirstData(string[] local, string input, int maxtime)
        {
            _maxtime = maxtime;
            Clean();
            sw.Reset();
            sw.Start();

            string local2 = input.Replace(" ", "").Replace("^", ",").Replace("v", ".");
            int count = 0;
            foreach (string t in local)
            {
                if (!letternum.ContainsKey(t))
                {
                    letternum.Add(t, count);
                    letter1.Add(count, t);
                    count++;
                }
            }

            letterState = new byte[letternum.Count];

            for (int i = 0; i < letternum.Count; i++)
            {
                letterState[i] = 1;
            }

            bool isLeft = false;
            
            Algorithm(local2, isLeft);

            if (resultList.Count <= 0)
            {
                allBad = false;
                issolved = false;
                isLeft = true;


                for (int i = 0; i < letternum.Count; i++)
                {
                    letterState[i] = 0;
                }

                Algorithm(local2, isLeft);
            }



            sw.Stop();
            return resultList;
        }

        private void Algorithm(string local2, bool isLeft)
        {
            byte value;
            if (isLeft) value = 1;
            else value = 0;

            string[] clauses = divideClause(local2);

            for (int i = 0; i < letternum.Count; i++)
            {
                if (sw.Elapsed.TotalSeconds >= _maxtime)
                {
                    break;
                }
                if (allBad)
                {
                    i--;
                    allBad = false;
                }
                else
                {
                    letterState[i] = value;
                }

                bool result = true;
                foreach (string clause in clauses)
                {
                    result = checkClause(clause.Replace(letter1[i], letterState[i].ToString()));
                    if (!result) break;
                }
                if (result)
                {
                    if (CheckCorrectly(local2))
                    {
                        Print();
                        issolved = true;
                        break;
                    }

                }
                else
                {
                    if (allBad)
                    {
                        if (value == 0) letterState[i] = 1;
                        else letterState[i] = 0;
                    }

                }
            }
        }

        private bool CheckCorrectly(string formule, char[] values)
        {
            string fornuleTemp = formule;
            for (int i = 0; i < values.Length; i++)
                fornuleTemp = fornuleTemp.Replace(letter1[i], values[i].ToString());

            return ReplaceOrAnd.WorkWithOrAndV2(fornuleTemp);
        }

        private bool CheckCorrectly(string formule)
        {
            string fornuleTemp = formule;
            for (int i = 0; i < letter1.Count; i++)
                fornuleTemp = fornuleTemp.Replace(letter1[i], letterState[i].ToString());

            return ReplaceOrAnd.WorkWithOrAndV2(fornuleTemp);
        }


        private void Clean()
        {
            resultList.Clear();
            letternum.Clear();
            letter1.Clear();
            allBad = false;
            issolved = false;
        }

        private void Print(char[] values)
        {
            string result = null;
            for (int i = 0; i < values.Length; i++)
                result += letter1[i] + "=" + values[i] + ", ";
            result += ": " + true;

            AddToList(result);
        }

        /// <summary>
        /// Печать
        /// </summary>
        /// <param name="letterState"></param>
        /// <param name="flag"></param>
        private void Print()
        {
            string result = null;
            for (int i = 0; i < letterState.Length; i++)
                result += letter1[i] + "=" + letterState[i] + ", ";
            result += ": " + true;

            AddToList(result);
        }

        private bool AddToList(string newResult)
        {
            foreach (string result in resultList)
                if (result.Equals(newResult)) return false;
            resultList.Add(newResult);
            return true;
        }

        private bool checkClause(string clause)
        {
            if (clause.Equals("0"))
            {
                allBad = true;
                return false;
            }
            if (clause.Equals("-1"))
            {
                allBad = true;
                return false;
            }
            return
                !clause.Contains("&0") ||
                !clause.Contains("0&") ||
                clause.Contains("&-0") ||
                clause.Contains("-0&");
        }

        private string[] divideClause(string local2)
        {
            return !local2.Contains("&") ? new[] { local2 } : local2.Split('&');
        }


        private byte ChangeValue(byte value)
        {
            if (value.Equals(0)) return 1;
            return 0;
        }


    }

    static class Extensions
    {
        public static List<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}