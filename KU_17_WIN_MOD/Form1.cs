using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace KU_17_WIN_MOD
{
    public partial class Form1 : Form
    {
        readonly Controller _control = new Controller();

        public Form1()
        {
            InitializeComponent();
            OnlyTime.Checked = false;
            InitProcessBrute();
            CleanScreen();
            

        }

        #region Felippe. Алгоритм полного перебора
        /// <summary>
        /// Решить SAT алгоритмом полного перебора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBrute_Click(object sender, EventArgs e)
        {
            if (textVerify(textBoxFormula.Text))
            {
                InitProcessBrute();
            }
        }

        /// <summary>
        /// Инициализация решения случайным перебором
        /// </summary>
        private void InitProcessBrute()
        {
            CleanScreen(); //clean
            allResults = !OnlyTime.Checked;
            _control.Beta = checkBox1.Checked; //only for debug and beta versions//TODO

            //Thread tread = new Thread(MethodParallelBrute);
            //tread.Start();
            resList = _control.InitBrute(textBoxFormula.Text, allResults);

            PrintData(resList, allResults);
        }

        private void MethodParallelBrute()
        {
            resList  = _control.InitBrute(textBoxFormula.Text, allResults);
        }

        private bool allResults;
        private List<string> resList = new List<string>();

        #endregion

        #region Oleg. Жадный алгоритм
        /// <summary>
        /// Решить SAT жадным алгоритмом
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGreedy_Click(object sender, EventArgs e)
        {
            if (textVerify(textBoxFormula.Text))
            {
                InitProcessGreedy();
            }
        }

        private void InitProcessGreedy()
        {
            CleanScreen(); //clean
            allResults = !OnlyTime.Checked;
            _control.Beta = checkBox1.Checked; //only for debug and beta versions//TODO

            List<string> resList = _control.InitGreedy(textBoxFormula.Text, allResults);

            PrintData(resList, allResults);
        }
        #endregion.

        #region Artem. Алгоритм случайного перебора
        /// <summary>
        /// Решить SAT алгоритмом случайного перебора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRandom_Click(object sender, EventArgs e)
        {
            if (textVerify(textBoxFormula.Text))
            {
                InitProcessRandom();
            }
        }

        private void InitProcessRandom()
        {
            CleanScreen(); //clean
            allResults = !OnlyTime.Checked;
            _control.Beta = checkBox1.Checked; //only for debug and beta versions//TODO

            List<string> resList = _control.InitRandom(textBoxFormula.Text, allResults);

            PrintData(resList, allResults);
        }


        #endregion

        #region Michail. DPLL
        /// <summary>
        /// Решить SAT алгоритмом DPLL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDPLL_Click(object sender, EventArgs e)
        {
            if (textVerify(textBoxFormula.Text))
            {
                InitProcessDPLL();
            }
        }

        private void InitProcessDPLL()
        {
            richTextBoxRes.Text = string.Empty; //clean
            allResults = !OnlyTime.Checked;
            _control.Beta = checkBox1.Checked; //only for debug and beta versions//TODO

            List<string> resList = _control.InitDPLL(textBoxFormula.Text, allResults);

            PrintData(resList, allResults);
        }
        #endregion


        ////////////////////////////////////общие методы////////////////////////////


        /// <summary>
        /// метод для проверки текста
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool textVerify(string text)
        {
            string textverify = text
                .Replace(" ", "").Replace("&", "")
                .Replace("|", "").Replace("-", "");

            Regex or = new Regex(@"\A\w+\Z");
            Match match = or.Match(textverify);
            if (match.Success) return true;             //return true and start method
            richTextBoxRes.Text =
                "Введена неверная формула: \t\n " +
                "Пример формулы: " +
                "b | a & b & -c | d & a";               //print error and return false
            return false;

        }

        private void CleanScreen()
        {
            richTextBoxRes.Text = string.Empty;
            labelNumTrue.Text = "0";
            labelNumComb.Text = "0";
            labelTime.Text = "0";
            LabelNumChars.Text = "0";
        }

        private void PrintData(List<string> resList, bool allResults)
        {
            if (printChartProccess) return;
            if (allResults)
                foreach (string resText in resList)
                {
                    richTextBoxRes.Text += resText + Environment.NewLine;
                }

            labelTime.Text = _control.Sw.Elapsed.ToString();
            int numChar = _control.NumChars;
            LabelNumChars.Text = numChar.ToString();
            labelNumComb.Text = Math.Pow(2, numChar).ToString();
            labelNumTrue.Text = _control.Numtrue.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printChartProccess = true;

            chart1.Titles.Add("Total Income");
            Thread tread = new Thread(loadData4Charts);
            tread.Start();
            tread.Join();

            foreach (string resultatChar in resultatChars)
            {
                richTextBoxRes.Text += resultatChar;
            }
            printChartProccess = false;
        }

        private bool printChartProccess = false;
        private int Progress1 = 0;
        private int Progress2 = 0;

        private void loadData4Charts()
        {
            string[] exampleData=new string[10];
            exampleData[0] = "b | a";                                                                                      //ab
            exampleData[1] = "b | a & b & -c | d & a";                                                                     //abcd
            exampleData[2] = "b | a & b & -c | d & a & e | f ";                                                            //abcdef
            exampleData[3] = "b | a & b & -c | d & a & e | f & g & h";                                                     //abcdefgh
            exampleData[4] = "b | a & b & -c | d & a & e | f & g & h | i | j";                                             //abcdefghij
            exampleData[5] = "b | a & b & -c | d & a & e | f & g & h | i | j & k | l";                                     //abcdefghijkl
            exampleData[6] = "b | a & b & -c | d & a & e | f & g & h | i | j & k | l | m & n";                             //abcdefghijklmn
            exampleData[7] = "b | a & b & -c | d & a & e | f & g & h | i | j & k | l | m & n & o | p";                     //abcdefghijklmnop
            exampleData[8] = "b | a & b & -c | d & a & e | f & g & h | i | j & k | l | m & n & o | p & q & r";             //abcdefghijklmnopqr
            exampleData[9] = "b | a & b & -c | d & a & e | f & g & h | i | j & k | l | m & n & o | p & q & r & s | t";     //abcdefghijklmnopqrst
            
            Double[] bruteAllData = new double[20];
            Double[] randomAllData = new double[20];
            Double[] GreedyAllData = new double[20];
            Double[] DPLLAllData = new double[20];
            
            //все дата
            OnlyFirstData.Checked = false;
            OnlyTime.Checked = false;
            
            ///////////////////////ready
            
            for (int i = 0; i < 7; i++)
            {
                TimeSpan span = TimeSpan.Zero;
                for (int a = 0; a < 5; a++)
                {
                    _control.InitBrute(exampleData[i], allResults);
                    span += _control.Sw.Elapsed;
                    progressBar2.Value++;
                }
                double AverageTime = span.TotalSeconds / (Double)5;
                bruteAllData[i] = Math.Round(AverageTime,10);
                progressBar1.Value++;
            }
            for (int index= 0; index < bruteAllData.Length; index++)
            {
                resultatChars[index] += "Brute- NLetters:" + index*2 +" aveTime: "+bruteAllData[index]+Environment.NewLine;
            }
        }

        string[] resultatChars = new string[20];

        private void printCharts( )
        {
            Series series = chart1.Series.Add("Total Income");
            series.ChartType = SeriesChartType.Spline;
            for (int i = 0; i < 10; i++)
            {
                series.Points.AddXY(i*2, 100);    
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

    }
}
