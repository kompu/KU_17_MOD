using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace KU_17_WIN_MOD
{
    public partial class Form1 : Form
    {
        readonly Controller _control = new Controller();
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        
        private void buttonStart_Click(object sender, EventArgs e)
        {
            string textverify = textBoxFormula.Text
                .Replace(" ", "").Replace("&", "")
                .Replace("|", "").Replace("-","");

            Regex or = new Regex(@"\A\w+\Z");
            Match match = or.Match(textverify);
            if (match.Success)
                InitProcess();
            else
                richTextBoxRes.Text =
                    "неверную формулу: \t\n " +
                    "Пример формулы: " +
                    "b | a & b & -c | d & a";
        }

        private void InitProcess()
        {
            bool trueEntries = comboBox1.SelectedIndex == 2;

            richTextBoxRes.Text = string.Empty; //clean

            _control.Beta = checkBox1.Checked; //only for debug and beta versions//TODO

            List<string> resList = _control.Init(textBoxFormula.Text, trueEntries);

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
    }
}
