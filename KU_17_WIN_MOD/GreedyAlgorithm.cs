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

        private void Print()
        {
            string result = null;
            for (int i = 0;i<valuesList.Count;i++)
            {
                result += valuesList[i] + "=" + boolListValues[i] + ", ";
            }
            result += ": " + isSolved;
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
            boolListValues = RandomSetBoolValuesInList();

            ClearFirstAndSecondStringClozes();
            FillFirstAndSecondStringClozes();
            firstGood = new int[firstStringClosez.Count];
            secondGood = new int[secondStringClozes.Count];

            while (true)
            {
                ClearFirstAndSecondStringClozes();
                FillFirstAndSecondStringClozes();

                if (LiteralsToDigits(firstStringClosez, false) &&
                LiteralsToDigits(secondStringClozes, true))
                {
                    isSolved = true;
                    Print();
                    break;
                }
                else
                {
                    SetNewBoolValuesInList();
                }
            }

            while (!isSolved)
            {
                //SATSolverIteration(firstStringClosez);
                //SATSolverIteration(secondStringClozes);
                break;
            }

            return false;
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
                            if (valuesList[x].Contains("-"))
                            {
                                if (charCloze[j].Equals("1")) charCloze[j] = "0";
                                else charCloze[j] = "1";
                            }
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
                        break;
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
            Random rnd = new Random();
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

            for (i = 0;i<firstGood.Length;)
            {
                if (firstGood[i] != 1)
                {
                    cloze = firstStringClosez[i];
                    fromCloze = i * 2;
                    break;
                }
                i++;
            }
            if (cloze == null)
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
                            //boolListValues[x] = 0;
                            //break;
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
                            //boolListValues[x] = 0;
                            //break;
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
            char[] opT = new char[13];
            opT[0] = 'a';
            opT[1] = 'b';
            opT[2] = 'c';
            opT[3] = 'd';
            opT[4] = 'e';
            opT[5] = 'f';
            opT[6] = 'g';
            opT[7] = 'h';
            opT[8] = 'k';
            opT[9] = 'l';
            opT[10] = 'm';
            opT[11] = 'n';
            opT[12] = '-';

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
                for (int j = 0;j<this.valuesList.Count;j++)
                {
                    if (this.valuesList[j].Equals(valuesList[i])) flag = true;
                }
                if (!flag) this.valuesList.Add(valuesList[i]);
            }
        }
    }
}
