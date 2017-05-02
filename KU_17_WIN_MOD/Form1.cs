using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace KU_17_WIN_MOD
{
    public partial class Form1 : Form
    {
        private int maxChartMethods = 4;
        private int maxChartAverage = 10;
        private int maxChartexamples = 10;



        private bool allResults;
        private List<string> resList = new List<string>();
        readonly Controller _control = new Controller();
        private int[] numExampleSymbols;

        public Form1()
        {
            InitializeComponent();
            OnlyTime.Checked = false;
            InitProcessBrute();
            CleanScreen();
            
            progressBarOne.Maximum = maxChartAverage;
            progressBarMethod.Maximum = maxChartexamples;


            exampleData[0] = "b | a & b & -c";                                                                             //abc
            exampleData[1] = "b | a & b & -c | d & a";                                                                     //abcd
            exampleData[2] = "b | a & b & -c | d & a & e | f ";                                                            //abcdef
            exampleData[3] = "b | a & b & -c | d & a & e | f & g & h";                                                     //abcdefgh
            exampleData[4] = "b | a & b & -c & d & a & e | f & g & h | i | j";                                             //abcdefghij
            exampleData[5] = "b | a & b & -c & d & a & e | f & g & h | i | j & k | l";                                     //abcdefghijkl
            exampleData[6] = "b | a & b & -c & d & a & k | f & g & h | i | j & l | e | m & n";                             //abcdefghijklmn
            exampleData[7] = "b | a & b & -c | d & a & e | f & g & h | i | j & k | l | m & n & o | p";                     //abcdefghijklmnop
            exampleData[8] = "b | a & b & -c | d & a & e | f & g & h | i | j & k | l | m & n & o | p & q & r";             //abcdefghijklmnopqr
            exampleData[9] = "b | a & b & -c | d & a & e | f & g & h | i | j & k | l | m & n & o | p & q & r & s | t";     //abcdefghijklmnopqrst

            numExampleSymbols = new[] { 3, 4, 6, 8, 10, 12, 14, 16, 18, 20 };

            foreach (string s in exampleData)
            {
                comboBox1.Items.Add(s);
            }

            /*
             exampleData[0] = "a";                                                                                      //ab
             exampleData[1] = "b | a";
             exampleData[2] = "b | a & b & -c";                                                                     //abcd
             exampleData[3] = "b | a & b & -c | d & a";                                                                     //abcd
             exampleData[4] = "b | a & b & -c | d & a & e ";                                                            //abcdef
             exampleData[5] = "b | a & b & -c | d & a & e | f ";
             exampleData[6] = "b | a & b & -c | d & a & e | f & g";                                                     //abcdefgh
             exampleData[7] = "b | a & b & -c | d & a & e | f & g & h";
             exampleData[8] = "b | a & b & -c | d & a & e | f & g & h | i ";                                             //abcdefghij
             exampleData[9] = "b | a & b & -c | d & a & e | f & g & h | i | j";                                             //abcdefghij
              */

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
            _control._maxtime = int.Parse(maxTime.Value.ToString());
            resList = _control.InitBrute(textBoxFormula.Text, OnlyFirstData.Checked);

            PrintData(resList);
        }
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
            _control._maxtime = int.Parse(maxTime.Value.ToString());

            resList = _control.InitGreedy(textBoxFormula.Text, OnlyFirstData.Checked);

            PrintData(resList);
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
            _control._maxtime = int.Parse(maxTime.Value.ToString());
            _control.outTime = false;
            List<string> resList = _control.InitRandom(textBoxFormula.Text, OnlyFirstData.Checked);

            PrintData(resList);
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


        List<string> words = new List<string>();





        private void InitProcessDPLL()
        {
            richTextBoxRes.Text = string.Empty; //clean
            allResults = !OnlyTime.Checked;
            _control.Beta = checkBox1.Checked; //only for debug and beta versions//TODO
            _control._maxtime = int.Parse(maxTime.Value.ToString());

            List<string> resList = _control.InitDPLL(textBoxFormula.Text, OnlyFirstData.Checked);

            PrintData(resList);
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

        private void PrintData(List<string> resList)
        {
           

            if (!printChartProccess) //no print info form when create chart
            {
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

        }

        private bool chartOne = true;

        private void button1_Click(object sender, EventArgs e)
        {
            chartOne = true;
            button1.Enabled = !chartOne;
            chart1.Visible = chartOne;
            chart2.Visible = !chartOne;
            Createchart(true);
            button1.Enabled = true;
        }

        private void Createchart(bool firstData)
        {
            printChartProccess = true;

            chart1.Series.Clear();

            chart2.Series.Clear();



            richTextBoxRes.Text = "";

            //все дата
            OnlyTime.Checked = false;
            OnlyFirstData.Checked = firstData;
            allResults = !firstData;

            progressBarTotal.Value = 0;

            for (int m = 0; m < maxChartMethods; m++)
            {
                _actualMethod = m + 1;

                loadData4Charts();

                for (int index = 0; index < 10; index++)
                {
                    richTextBoxRes.Text += nameMethods[m] + "- NLetters:" +
                                           ((index + 1) * 2).ToString().PadLeft(2, '0') + " aveTime: " +
                                           TimeAllData[m, index].ToString("F8");
                    if (_control.outTime)
                    {
                        if (TimeAllData[m, index] == 15 || TimeAllData[m, index] == 0)
                        {
                            richTextBoxRes.Text += " outTime" + Environment.NewLine;    
                        }
                    }
                    else
                        richTextBoxRes.Text += " - " + Environment.NewLine;
                }
                _control.outTime = false;
                progressBarTotal.Value++;
            }




            PrintCharts();
            chart1.ChartAreas[0].AxisX.Minimum = 3;
            chart1.ChartAreas[0].AxisX.Maximum = (maxChartexamples * 2);
            chart2.ChartAreas[0].AxisX.Minimum = 3;
            chart2.ChartAreas[0].AxisX.Maximum = (maxChartexamples * 2);
            printChartProccess = false;
        }

        private bool printChartProccess = false;
        private int _actualMethod = 0;
        string[] exampleData = new string[10];
        string[] nameMethods = { "brute", "random", "greddy", "DPLL" };
        Double[,] TimeAllData = new double[4, 10];

        private void loadData4Charts()
        {
            progressBarMethod.Value = 0;
            for (int i = 0; i < maxChartexamples; i++)
            {
                
                progressBarOne.Value = 0;
                TimeSpan span = TimeSpan.Zero;
                for (int a = 0; a < maxChartAverage; a++)
                {
                    switch (_actualMethod)
                    {
                        case 1:
                            _control.InitBrute(exampleData[i], !allResults);
                            break;
                        case 2:
                            if (!_control.outTime)
                            {
                                _control.InitRandom(exampleData[i], !allResults);
                            }
                            break;
                        case 3:
                            if (!_control.outTime)
                            {
                                _control.InitGreedy(exampleData[i], !allResults);
                            }
                            break;
                        case 4:
                            _control.InitDPLL(exampleData[i], !allResults);
                            break;

                    }
                    
                    span += _control.Sw.Elapsed;
                    progressBarOne.Value++;
                }
                
                progressBarMethod.Value++;
                if (_control.outTime)
                {
                    TimeAllData[_actualMethod - 1, i] = 100;
                }
                else
                {
                    TimeAllData[_actualMethod - 1, i] = Math.Round((Double)span.TotalSeconds / (Double)maxChartAverage, 10);
                }
                
            }
        }


        private void PrintCharts()
        {
            for (int m = 0; m < 4; m++)
            {
                Series series = new Series(nameMethods[m])
                {
                    ChartType = SeriesChartType.Spline

                };

                series.BorderWidth = 5;
                switch (m)
                {
                    case 0:
                        series.Color = Color.Red;
                        break;
                    case 1:
                        series.Color = Color.Blue;
                        break;
                    case 2:
                        series.Color = Color.Green;
                        break;
                    case 3:
                        series.Color = Color.Coral;
                        break;
                }

                for (int i = 0; i < 10; i++)
                {
                    series.Points.AddXY(numExampleSymbols[i] , TimeAllData[m, i]);
                }
                if (chartOne)
                {
                    chart1.Series.Add(series);
                }
                else
                {
                    chart2.Series.Add(series);
                }
                    
                
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //CheckForIllegalCrossThreadCalls = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            chartOne = false;
            chart1.Visible = chartOne;
            chart2.Visible = !chartOne;
            button2.Enabled = false;
            Createchart(false);
            button2.Enabled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxFormula.Text = comboBox1.Text;
        }

    }
}
