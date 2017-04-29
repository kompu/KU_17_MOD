using System;
using System.Collections.Generic;

namespace KU_17_WIN_MOD
{
    /// <summary>
    /// Жадный алгоритм решения SAT
    /// </summary>
    public class GreedyAlgorithm
    {
        private List<string> clozes = new List<string>();
        private List<string> valuesList = new List<string>();
        private int[] boolListValues;
        char[] operators = new char[2];
        List<string> firstStringClosez = new List<string>();
        List<string> secondStringClozes = new List<string>();
        int[] firstGood;
        int[] secondGood;
        List<string> resultList;
        bool isSolved = false;
        Random rnd = new Random();
        long iterations;

        /// <summary>
        /// Жадный алгоритм решения SAT 
        /// </summary>
        /// <param name="formule"></param>
        /// <returns></returns>
        public List<string> GreedyMethod(string formule)
        {
            ClearResult();
            ClearClozesAndValuesList();
            ClearFirstAndSecondStringClozes();

            clozes = GenerateFormulasListWithClozes(formule);
            ToSolveClozes();
            List<string> resultList = this.resultList;
            return resultList;
        }

        private void ClearResult()
        {
            this.resultList = new List<string>();
            isSolved = false;
        }

        private void Print(bool flag)
        {
            string result = null;
            if (flag)
            {
                for (int i = 0; i < valuesList.Count; i++)
                {
                    result += valuesList[i] + "=" + boolListValues[i] + ", ";
                }
                result += ": " + isSolved;
            }
            else
            {
                result = "Задачу невозможно решить!";
            }
            resultList.Add(result);
        }

        private void ClearClozesAndValuesList()
        {
            clozes = new List<string>();
            valuesList = new List<string>();
        }

        private void ClearFirstAndSecondStringClozes()
        {
            firstStringClosez = new List<string>();
            secondStringClozes = new List<string>();
        }

        private void FillFirstAndSecondStringClozes()
        {
            for (int i = 0; i < clozes.Count; i++)
            {
                if ((i + 2) % 2 == 0) firstStringClosez.Add(clozes[i]);
                else secondStringClozes.Add(clozes[i]);
            }
        }

        private bool ToSolveClozes()
        {
            long value = (long)Math.Pow(valuesList.Count, 2) * (long)Math.Pow(2,valuesList.Count)/2;
            iterations = 0;
            boolListValues = RandomSetBoolValuesInList();
            ClearFirstAndSecondStringClozes();
            FillFirstAndSecondStringClozes();
            firstGood = new int[firstStringClosez.Count];
            secondGood = new int[secondStringClozes.Count];

            while (iterations <= value)
            {
                ClearFirstAndSecondStringClozes();
                FillFirstAndSecondStringClozes();

                bool flag1 = LiteralsToDigits(firstStringClosez, false);
                bool flag2 = LiteralsToDigits(secondStringClozes, true);
                if (flag1 && flag2)
                {
                    isSolved = true;
                    Print(true);
                    break;
                }
                else
                {
                    SetNewBoolValuesInList();
                }
                iterations++;
            }

            if (isSolved)
            {
                return true;
            }
            else
            {
                Print(false);
                return false;
            }
        }


        /// <summary>
        /// Переводим из строки клозов с буквами в строку клозов с цифрами
        /// </summary>
        /// <param name="clozeString"></param>
        private bool LiteralsToDigits(List<string> clozeString,bool binaryGood)
        {
            bool flag = false;
            for (int i = 0;i<clozeString.Count;i++)
            {
                string[] charCloze = clozeString[i].Split(this.operators);
                char charOperator = CheckOperator(clozeString[i]);
                for (int j = 0;j<2;j++)
                {
                    for (int x = 0;x < valuesList.Count;x++)
                    {
                        if (charCloze[j].Equals(valuesList[x]))
                        {
                            charCloze[j] = boolListValues[x].ToString();
                            break;
                        }
                        else if (charCloze[j].Equals("-"+valuesList[x]))
                        {
                            charCloze[j] = boolListValues[x].ToString();
                            if (charCloze[j].Equals("1")) charCloze[j] = "0";
                            else charCloze[j] = "1";
                            break;
                        }
                    }
                   
                }
                clozeString[i] = charCloze[0] + charOperator + charCloze[1];
                if (binaryGood)
                {
                    secondGood[i] = CheckClozeString(charCloze[0] + charCloze[1], charOperator);
                    flag = CheckCorrectGood(true);
                }
                else
                {
                    firstGood[i] = CheckClozeString(charCloze[0] + charCloze[1], charOperator);
                    flag = CheckCorrectGood(false);
                }
               
            }
            return flag;
        }

        public bool CheckCorrectGood(bool binaryGood)
        {
            int sum = 0;
            bool flag = false;

            if (!binaryGood)
            {
                foreach (int i in firstGood)
                {
                    sum += i;
                }
                flag = sum == firstGood.Length ? true : false;
            }
            else
            {
                foreach (int i in secondGood)
                {
                    sum += i;
                }
                flag = sum == secondGood.Length ? true : false;
            }

            return flag;
        }

        private int CheckTheBiggestStringOfClozes()
        {
            int flag = 0;
            int sum = 0;
            bool flagFirst;
            bool flagSecond;

            foreach (int i in firstGood)
            {
                sum += i;
            }
            flagFirst = sum == firstGood.Length ? true : false;
            sum = 0;
            foreach (int i in secondGood)
            {
                sum += i;
            }
            flagSecond = sum == secondGood.Length ? true : false;

            if (flagFirst && !flagSecond) flag = 1;
            else if (!flagFirst && flagSecond) flag = 2;
            else flag = 0;

            return flag;
        }

        public int CheckClozeString(string clozeString,char charOperator)
        {
            int flag = 0;
            if (charOperator.Equals('|'))
                flag = clozeString.Equals("00") ? 0 : 1;
            else
                flag = clozeString.Equals("11") ? 1 : 0;

            return flag;
        }
            

        /// <summary>
        /// Возвращаем оператор клоза
        /// </summary>
        /// <param name="cloze"></param>
        /// <returns></returns>
        private char CheckOperator(string cloze)
        {
            char[] charCloze = cloze.ToCharArray();
            char charOperator = '0';

            for (int i = 0; i < charCloze.Length; i++)
            {
                for (int j = 0;j<operators.Length;j++)
                {
                    if (charCloze[i].Equals(operators[j]))
                    {
                        charOperator = operators[j];
                        return charOperator;
                    }
                }

            }
            return charOperator;
        }

        /// <summary>
        /// Случайно задаем начальные значения переменных
        /// </summary>
        /// <param name="boolListValues"></param>
        /// <returns></returns>
        private int[] RandomSetBoolValuesInList()
        {

            int[] boolListValues = new int[this.valuesList.Count];
            for (int i = 0;i<boolListValues.Length;i++)
                boolListValues[i] = rnd.Next(0,2);

            return boolListValues;
        }

        private void SetNewBoolValuesInList()
        {
            int i = 0;
            int j = 0;
            int fromCloze = 0;
            string cloze = null;
            int flag = CheckTheBiggestStringOfClozes();
            bool firstOrSecond = false;

            if (flag == 0)
            {
                firstOrSecond = rnd.NextDouble() > 0.5f ? true : false;
            }
            else if (flag == 2)
            {
                firstOrSecond = true;
            }
            else if (flag == 1)
            {
                firstOrSecond = false;
            }

            if (firstOrSecond)
            {
                for (i = 0; i < firstGood.Length;)
                {
                    if (firstGood[i] != 1)
                    {
                        cloze = firstStringClosez[i];
                        fromCloze = i * 2;
                        break;
                    }
                    i++;
                }
            }
            else if (!firstOrSecond || cloze == null)
            {
                for (j = 0; j < secondGood.Length;j++)
                {
                    if (secondGood[j] != 1)
                    {
                        cloze = secondStringClozes[j];
                        if (j >= 1) fromCloze = j * 2 - 1;
                        else fromCloze = 1;
                        break;
                    }
                }
            }
            // выдает ошибку отсутствия клоза, если идем не по той строке 
            // (если первая строка клозов 1/1 а вторая 0/1, 
            // но мы идем по первой. 
            // проверка на сумму нужна
            string[] charCloze = cloze.Split(operators);

            if (charCloze[0].Equals("0"))
            {
                string[] s = clozes[fromCloze].Split(operators);
                for (int x = 0;x<valuesList.Count;x++)
                {
                    if (s[0].Equals(valuesList[x]))
                    {
                        if (boolListValues[x] == 0)
                        {
                            boolListValues[x] = 1;
                            break;
                        }
                        else
                        {
                            boolListValues[x] = 0;
                            break;
                        }
                    }
                    else if (s[0].Equals("-"+valuesList[x]))
                    {
                        if (boolListValues[x] == 0)
                        {
                            boolListValues[x] = 1;
                            break;
                        }
                        else
                        {
                            boolListValues[x] = 0;
                            break;
                        }
                    }
                }
            }
            else if (charCloze[1].Equals("0"))
            {
                string[] s = clozes[fromCloze].Split(operators);
                for (int x = 0; x < valuesList.Count; x++)
                {
                    if (s[1].Equals(valuesList[x]))
                    {
                        if (boolListValues[x] == 0)
                        {
                            boolListValues[x] = 1;
                            break;
                        }
                        else
                        {
                            boolListValues[x] = 0;
                            break;
                        }
                        
                    }
                    else if ((s[1].Equals("-"+valuesList[x])))
                    {
                        if (boolListValues[x] == 0)
                        {
                            boolListValues[x] = 1;
                            break;
                        }
                        else
                        {
                            boolListValues[x] = 0;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Создаем из строки лист с формулами, которые разбиты на клозы
        /// </summary>
        /// <param name="formule"></param>
        /// <returns></returns>
        private List<string> GenerateFormulasListWithClozes(string formule)
        {
            List<string> generatedListClozes = new List<string>(); // лист с выходными формулами
            this.operators[0] = '&';
            this.operators[1] = '|';
            char[] opT = new char[27];
            opT[0] = 'a';
            opT[1] = 'b';
            opT[2] = 'c';
            opT[3] = 'd';
            opT[4] = 'e';
            opT[5] = 'f';
            opT[6] = 'g';
            opT[7] = 'h';
            opT[8] = 'i';
            opT[9] = 'g';
            opT[10] = 'k';
            opT[11] = 'l';
            opT[12] = 'm';
            opT[13] = 'n';
            opT[14] = 'o';
            opT[15] = 'p';
            opT[16] = 'q';
            opT[17] = 'r';
            opT[18] = 's';
            opT[19] = 't';
            opT[20] = 'u';
            opT[21] = 'v';
            opT[22] = 'w';
            opT[23] = 'x';
            opT[24] = 'y';
            opT[25] = 'z';
            opT[26] = '-';

            string[] operands = formule.Split(this.operators);
            string[] operators = formule.Split(opT);
            List<string> operandsFull = new List<string>();
            List<string> operatorsFull = new List<string>();

            for (int i = 0;i<operands.Length;i++)
            {
                if (operands[i] != "")
                {
                    operandsFull.Add(operands[i]);
                }
            }
            for (int i = 0; i < operators.Length; i++)
            {
                if (operators[i] != "")
                {
                    operatorsFull.Add(operators[i]);
                }
            }

            for (int j = 0; j < operands.Length-1; j++)
            {
                generatedListClozes.Add(operandsFull[j] + operatorsFull[j] + operandsFull[j + 1]);
            }

            SetListValues(operandsFull);
            return generatedListClozes;
        }

        private void SetListValues(List<string> valuesList)
        {
            bool flag;
            for (int i = 0;i< valuesList.Count;i++)
            {
                flag = false;
                char[] chars = valuesList[i].ToCharArray();

                for (int j = 0;j<this.valuesList.Count;j++)
                {
                    if (this.valuesList[j].Equals(chars[chars.Length-1].ToString()))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    this.valuesList.Add(chars[chars.Length - 1].ToString());
                }
            }
        }
    }
}
