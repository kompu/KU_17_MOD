﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace KU_17_WIN_MOD
{
    internal class Controller
    {
        public bool Beta = false; //only for debug and beta versions//TODO
        public Stopwatch Sw = new Stopwatch();
        public int NumChars;
        public int Numtrue;
        private readonly ReplaceOrAnd _replace = new ReplaceOrAnd();
        private readonly GreedyAlgorithm _greedyAlgorithm =  new GreedyAlgorithm();
        private readonly RandomAlgorithm _randomAlgorithm = new RandomAlgorithm();
        private readonly DPLLAlgorithm _DLLAlgorithm = new DPLLAlgorithm();

        internal List<string> InitBrute(string input, bool checkBoxAlllines)
        {
            Numtrue = 0;
            Sw.Reset();
            Sw.Start();
            string[] local = input.Split(new[] { ' ', '|', '&', '-' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> resultList = new List<string>();

            Dictionary<string, int> letternum = new Dictionary<string, int>();

            int count = 0;
            for (int i = 0; i < local.Length; i++)
            {
                if (!letternum.ContainsKey(local[i]))
                {
                    letternum.Add(local[i], count);
                    count++;
                }
            }

            NumChars = letternum.Count;

            List<char[]> ses = new Combinations().Combine(letternum.Count);

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
                    tempstring += i.Key + ": " + resulado + ", ";
                }

                bool resultad = Beta ? _replace.WorkWithOrAndV2(local3) : _replace.WorkWithOrAndV1(local3);

                if (resultad | checkBoxAlllines)
                {
                    resultList.Add(tempstring + " ||  " + local3 + " : " + resultad);
                    if (resultad)
                    {
                        Numtrue++;
                    }
                }

            }

            Sw.Stop();
            return resultList;
        }

        internal List<string> InitGreedy(string input, bool checkBoxAlllines)
        {
            Sw.Reset();
            Sw.Start();
            string[] local = input.Split(new[] { ' ', '|', '&', '-' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> resultList = new List<string>();

            string local2 = input.Replace(" ", "").Replace("^", ",").Replace("v", ".");

            resultList = _greedyAlgorithm.GreedyMethod(local2); // решение жадным методом

            Sw.Stop();
            return resultList;
        }

        internal List<string> InitRandom(string input, bool checkBoxAlllines)
        {
            Numtrue = 0;
            Sw.Reset();
            Sw.Start();
            string[] local = input.Split(new[] { ' ', '|', '&', '-' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> resultList = new List<string>();

            Dictionary<string, int> letternum = new Dictionary<string, int>();

            int count = 0;
            for (int i = 0; i < local.Length; i++)
            {
                if (!letternum.ContainsKey(local[i]))
                {
                    letternum.Add(local[i], count);
                    count++;
                }
            }

            NumChars = letternum.Count;

            List<char[]> ses = new Combinations().Combine(letternum.Count);

            string local2 = input.Replace(" ", "").Replace("^", ",").Replace("v", ".");

            _randomAlgorithm.RandomMethod(local2); // решение жадным методом

            Sw.Stop();
            return resultList;
        }

        internal List<string> InitDPLL(string input, bool checkBoxAlllines)
        {
            Numtrue = 0;
            Sw.Reset();
            Sw.Start();
            string[] local = input.Split(new[] { ' ', '|', '&', '-' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> resultList = new List<string>();

            Dictionary<string, int> letternum = new Dictionary<string, int>();

            int count = 0;
            for (int i = 0; i < local.Length; i++)
            {
                if (!letternum.ContainsKey(local[i]))
                {
                    letternum.Add(local[i], count);
                    count++;
                }
            }

            NumChars = letternum.Count;

            List<char[]> ses = new Combinations().Combine(letternum.Count);

            string local2 = input.Replace(" ", "").Replace("^", ",").Replace("v", ".");

            _DLLAlgorithm.DPLLMethod(local2); // решение жадным методом

            Sw.Stop();
            return resultList;
        }
    }
}
