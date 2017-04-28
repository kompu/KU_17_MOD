using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace KU_17_WIN_MOD
{
    public partial class Form1 
        : Form
    {
        readonly Controller _control = new Controller();

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        #region Felippe. Алгоритм полного перебора
        /// <summary>
        /// Решить SAT алгоритмом полного перебора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBrute_Click(object sender, EventArgs e)
        {
            string textverify = textBoxFormula.Text
                .Replace(" ", "").Replace("&", "")
                .Replace("|", "").Replace("-", "");

            Regex or = new Regex(@"\A\w+\Z");
            Match match = or.Match(textverify);
            if (match.Success)
                InitProcessBrute();
            else
                richTextBoxRes.Text =
                    "Введена неверная формула: \t\n " +
                    "Пример формулы: " +
                    "b | a & b & -c | d & a";
        }

        /// <summary>
        /// Инициализация решения случайным перебором
        /// </summary>
        private void InitProcessBrute()
        {
            bool trueEntries = comboBox1.SelectedIndex == 2;

            richTextBoxRes.Text = string.Empty; //clean

            _control.Beta = checkBox1.Checked; //only for debug and beta versions//TODO

            List<string> resList = _control.InitBrute(textBoxFormula.Text, trueEntries);

            if (comboBox1.SelectedIndex != 0)
            {
                foreach (string resText in resList)
                {
                    richTextBoxRes.Text += resText + Environment.NewLine;
                }
            }

            labelTime.Text = _control.Sw.Elapsed.ToString();
            int numChar = _control.NumChars;
            LabelNumChars.Text = numChar.ToString();
            labelNumComb.Text = Math.Pow(2, numChar).ToString();
            labelNumTrue.Text = _control.Numtrue.ToString();
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
            string textverify = textBoxFormula.Text
                .Replace(" ", "").Replace("&", "")
                .Replace("|", "").Replace("-", "");

            Regex or = new Regex(@"\A\w+\Z");
            Match match = or.Match(textverify);
            if (match.Success)
                InitProcessGreedy();
            else
                richTextBoxRes.Text =
                    "Введена неверная формула: \t\n " +
                    "Пример формулы: " +
                    "b | a & b & -c | d & a";
        }

        private void InitProcessGreedy()
        {
            bool trueEntries = comboBox1.SelectedIndex == 2;

            richTextBoxRes.Text = string.Empty; //clean

            _control.Beta = checkBox1.Checked; //only for debug and beta versions//TODO

            List<string> resList = _control.InitGreedy(textBoxFormula.Text, trueEntries);

            if (comboBox1.SelectedIndex != 0)
            {
                foreach (string resText in resList)
                {
                    richTextBoxRes.Text += resText + Environment.NewLine;
                }
            }

            labelTime.Text = _control.Sw.Elapsed.ToString();
            int numChar = _control.NumChars;
            LabelNumChars.Text = numChar.ToString();
            labelNumComb.Text = Math.Pow(2, numChar).ToString();
            labelNumTrue.Text = _control.Numtrue.ToString();
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
            string textverify = textBoxFormula.Text
                .Replace(" ", "").Replace("&", "")
                .Replace("|", "").Replace("-", "");

            Regex or = new Regex(@"\A\w+\Z");
            Match match = or.Match(textverify);
            if (match.Success)
                InitProcessRandom();
            else
                richTextBoxRes.Text =
                    "Введена неверная формула: \t\n " +
                    "Пример формулы: " +
                    "b | a & b & -c | d & a";
        }

        private void InitProcessRandom()
        {
            bool trueEntries = comboBox1.SelectedIndex == 2;

            richTextBoxRes.Text = string.Empty; //clean

            _control.Beta = checkBox1.Checked; //only for debug and beta versions//TODO

            List<string> resList = _control.InitRandom(textBoxFormula.Text, trueEntries);

            if (comboBox1.SelectedIndex != 0)
            {
                foreach (string resText in resList)
                {
                    richTextBoxRes.Text += resText + Environment.NewLine;
                }
            }

            labelTime.Text = _control.Sw.Elapsed.ToString();
            int numChar = _control.NumChars;
            LabelNumChars.Text = numChar.ToString();
            labelNumComb.Text = Math.Pow(2, numChar).ToString();
            labelNumTrue.Text = _control.Numtrue.ToString();
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
            string textverify = textBoxFormula.Text
                .Replace(" ", "").Replace("&", "")
                .Replace("|", "").Replace("-", "");

            Regex or = new Regex(@"\A\w+\Z");
            Match match = or.Match(textverify);
            if (match.Success)
                InitProcessDPLL();
            else
                richTextBoxRes.Text =
                    "Введена неверная формула: \t\n " +
                    "Пример формулы: " +
                    "b | a & b & -c | d & a";
        }

        private void InitProcessDPLL()
        {
            bool trueEntries = comboBox1.SelectedIndex == 2;

            richTextBoxRes.Text = string.Empty; //clean

            _control.Beta = checkBox1.Checked; //only for debug and beta versions//TODO

            List<string> resList = _control.InitDPLL(textBoxFormula.Text, trueEntries);

            if (comboBox1.SelectedIndex != 0)
            {
                foreach (string resText in resList)
                {
                    richTextBoxRes.Text += resText + Environment.NewLine;
                }
            }

            labelTime.Text = _control.Sw.Elapsed.ToString();
            int numChar = _control.NumChars;
            LabelNumChars.Text = numChar.ToString();
            labelNumComb.Text = Math.Pow(2, numChar).ToString();
            labelNumTrue.Text = _control.Numtrue.ToString();
        }
        #endregion
    }
}
