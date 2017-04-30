using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace KU_17_WIN_MOD
{
    /// <summary>
    /// Парсит формулу на клозы и операнды
    /// </summary>
    class ParseFormule
    {
        private List<char> alphavite;
        private List<char> allOperands;
        private List<string> firstClozes;
        private List<string> secondClozes;
        private List<string> allClozes;
        private GreedyAlgorithm greedyAlgorithm;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="formule"></param>
        /// <param name="greedyOptimizeAlgorithm"></param>
        public ParseFormule(string formule, GreedyAlgorithm greedyAlgorithm)
        {
            this.greedyAlgorithm = greedyAlgorithm;
            allClozes = GenerateFormulasListWithClozes(formule); // генерация операндов и клозов
            SeparateClozes(); // разбить клозы на 2 группы
            ReturnValues();
        }

        /// <summary>
        /// Вернуть значения
        /// </summary>
        private void ReturnValues()
        {
            greedyAlgorithm.allOperands = allOperands;
            greedyAlgorithm.alphavite = alphavite;
            greedyAlgorithm.firstClozes = firstClozes;
            greedyAlgorithm.secondClozes = secondClozes;
            greedyAlgorithm.allClozes = allClozes;
        }

        #region Получить клозы и операнды
        /// <summary>
        /// Разбить строку на строку клозов
        /// </summary>
        /// <param name="formule"></param>
        /// <returns></returns>
        private List<string> GenerateFormulasListWithClozes(string formule)
        {
            alphavite = new List<char>(); // алфавит
            alphavite = GenerateAlphavite(alphavite); // генерация алфавита

            List<char> sequenceOperators = new List<char>(); // все операнды из формулы
            sequenceOperators = FindSequenceOperatorsInFormule(formule); // получаем все операнды из формулы

            List<string> sequenceOperands = new List<string>(); // последовательность операндов из формулы
            sequenceOperands = FindSequenceOperandsInFormule(formule, alphavite); // получаем последовательность операндов из формулы

            this.allOperands = new List<char>(); // все операнды из формулы
            allOperands = FindAllOperandsInFormule(formule, alphavite); // получаем все операнды из формулы

            return GeneratedListClozes(sequenceOperands, sequenceOperators);
        }

        /// <summary>
        /// Разбить клозы на 2 группы
        /// </summary>
        private void SeparateClozes()
        {
            firstClozes = new List<string>();
            secondClozes = new List<string>();
            for (int i = 0; i < allClozes.Count; i++)
            {
                if ((i + 2) % 2 == 0) firstClozes.Add(allClozes[i]);
                else secondClozes.Add(allClozes[i]);
            }
        }

        /// <summary>
        /// Найти последовательсть всех операторов в формуле
        /// </summary>
        /// <param name="formule"></param>
        /// <returns></returns>
        private List<char> FindSequenceOperatorsInFormule(string formule)
        {
            List<char> allOperators = new List<char>();
            char[] chars = formule.ToCharArray();

            foreach (char ch in chars)
                if (ch.Equals('|') || ch.Equals('&')) allOperators.Add(ch);

            return allOperators;
        }

        /// <summary>
        /// Сгенерировать лист с клозами
        /// </summary>
        /// <param name="operands"></param>
        /// <param name="operators"></param>
        /// <returns></returns>
        private List<string> GeneratedListClozes(List<string> operands, List<char> operators)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < operands.Count - 1; i++)
            {
                list.Add(operands[i] + operators[i] + operands[i + 1]);
            }

            return list;
        }

        /// <summary>
        /// Получить последовательность всех операндов из формулы
        /// </summary>
        /// <param name="formule"></param>
        /// <param name="alphavite"></param>
        /// <returns></returns>
        private List<string> FindSequenceOperandsInFormule(string formule, List<char> alphavite)
        {
            List<string> sequence = new List<string>();
            char[] formuleChars = formule.ToCharArray();

            for (int j = 0; j < formuleChars.Length; j++)
            {
                for (int i = 0; i < alphavite.Count; i++)
                {
                    if (formuleChars[j].Equals('|') || formuleChars[j].Equals('&') || formuleChars[j].Equals('-')) break;
                    if (formuleChars[j].Equals(alphavite[i]))
                    {
                        if (j >= 1 && formuleChars[j - 1].Equals('-'))
                            sequence.Add("-" + alphavite[i].ToString());
                        else sequence.Add(alphavite[i].ToString());
                        break;
                    }
                }
            }
            return sequence;
        }

        /// <summary>
        /// Получить операнды из формулы
        /// </summary>
        /// <param name="formule"></param>
        /// <param name="alphavite"></param>
        /// <returns></returns>
        private List<char> FindAllOperandsInFormule(string formule, List<char> alphavite)
        {
            List<char> allOperands = new List<char>();
            char[] formuleChars = formule.ToCharArray();

            foreach (char ch in formuleChars)
            {
                foreach (char operand in alphavite)
                    if (ch.Equals(operand)) allOperands = AddTolistOfOperands(allOperands, operand);
            }

            return allOperands;
        }

        /// <summary>
        /// Добавить в лист операндов, проверив, нет ли совпадений
        /// </summary>
        private List<char> AddTolistOfOperands(List<char> allOperands, char operand)
        {
            bool isNotSame = true;
            foreach (char operandFromList in allOperands)
                if (operandFromList.Equals(operand)) { isNotSame = false; break; }
            if (isNotSame) allOperands.Add(operand);

            return allOperands;
        }

        /// <summary>
        /// Генерация алфавита
        /// </summary>
        /// <param name="operators"></param>
        /// <returns></returns>
        private List<char> GenerateAlphavite(List<char> operands)
        {
            operands.Add('a');
            operands.Add('b');
            operands.Add('c');
            operands.Add('d');
            operands.Add('e');
            operands.Add('f');
            operands.Add('g');
            operands.Add('h');
            operands.Add('i');
            operands.Add('j');
            operands.Add('k');
            operands.Add('l');
            operands.Add('m');
            operands.Add('n');
            operands.Add('o');
            operands.Add('p');
            operands.Add('q');
            operands.Add('r');
            operands.Add('s');
            operands.Add('t');
            operands.Add('u');
            operands.Add('v');
            operands.Add('w');
            operands.Add('x');
            operands.Add('y');
            operands.Add('z');

            return operands;
        }
        #endregion
    }

    /// <summary>
    /// Жадный алгоритм
    /// </summary>
    class GreedyAlgorithm
    {
        private string formule; // наша формула
        public List<string> resultList; // лист с правильными ответами
        public List<string> allClozes; // все клозы
        public List<string> firstClozes; // лист с первыми клозами
        public List<string> secondClozes; // лист со вторыми клозами
        public int sumTrueFirstClozes; // сумма всех правильных клозов из первого листа
        public int sumTrueSecondClozes; // сумма всех правильных клозов из второго листа
        public List<char> alphavite;
        public List<char> allOperands;
        private List<int> steps;
        private List<int> tempRandomClozeChange;
        private int[] valuesOperands;

        public long maxIterations;
        public int maxFlips;
        private Random rnd;
        private ParseFormule parseFormule;

        int tempSumTrueFirstClozes;
        int tempSumTrueSecondClozes;
        string firstString;
        string secondString;
        private bool _onlyFirstData;

        /// <summary>
        /// Жадный метод решения задачи SAT
        /// </summary>
        /// <param name="initFormula"></param>
        /// <param name="onlyFirstData"></param>
        /// <param name="countOfIterations"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        public List<string> GreedyMethod(string initFormula, bool onlyFirstData, int countOfIterations = 1000)
        {
            _onlyFirstData = onlyFirstData;
            Initialisation(initFormula, countOfIterations);
            Algorithm();
            return Finalization();
        }

        #region Алгоритм
        /// <summary>
        /// Запуск алгоритма
        /// </summary>
        /// <returns></returns>
        private bool Algorithm()
        {
            RandomValuesOperands(); // задаем начальные значения
            maxFlips = valuesOperands.Length - 1;
            long iterations = 0;

            while (resultList.Count < maxIterations)
            {
                FlipAndCalculate(); // изменяем значение переменной, чтобы изменилось максимальное количество клозов
                if (CheckCorrectly(this.formule)) // 
                {
                    Print();
                    RandomValuesOperands();
                }
                iterations++;
            }

            return true;
        }

        /// <summary>
        /// Проверить строку клозов на правильность
        /// </summary>
        /// <param name="listOfClozes"></param>
        /// <returns></returns>
        private bool CheckCorrectly(string formule)
        {
            string fornuleTemp = formule;
            for (int i = 0; i < valuesOperands.Length; i++)
                fornuleTemp = fornuleTemp.Replace(allOperands[i].ToString(), valuesOperands[i].ToString());

            return ReplaceOrAnd.WorkWithOrAndV2(fornuleTemp);
        }

        /// <summary>
        /// Случайная генерация значений операндов
        /// </summary>
        private void RandomValuesOperands()
        {
            for (int i = 0; i < valuesOperands.Length; i++)
                valuesOperands[i] = rnd.Next(0, 2);
        }

        /// <summary>
        /// Посчитать количество верных клозов
        /// </summary>
        private void CalculateTrueClozes()
        {
            sumTrueFirstClozes = 0;
            sumTrueSecondClozes = 0;

            for (int i = 0; i < firstClozes.Count; i++)
            {
                string firstString = firstClozes[i];
                for (int j = 0; j < valuesOperands.Length; j++)
                    firstString = firstString.Replace(allOperands[j], char.Parse(valuesOperands[j].ToString()));

                if (ReplaceOrAnd.WorkWithOrAndV2(firstString))
                {
                    sumTrueFirstClozes++;
                }
            }
            for (int i = 0; i < secondClozes.Count; i++)
            {
                string secondString = secondClozes[i];
                for (int j = 0; j < valuesOperands.Length; j++)
                    secondString = secondString.Replace(allOperands[j], char.Parse(valuesOperands[j].ToString()));

                if (ReplaceOrAnd.WorkWithOrAndV2(secondString))
                {
                    sumTrueSecondClozes++;
                }
            }
        }

        /// <summary>
        /// Посчитать количество верных клозов
        /// </summary>
        private bool EqualsTrueClozes(int[] valuesOperands)
        {
            tempSumTrueFirstClozes = 0;
            tempSumTrueSecondClozes = 0;

            for (int i = 0; i < firstClozes.Count; i++)
            {
                firstString = firstClozes[i];
                if (i < secondClozes.Count) secondString = secondClozes[i];

                for (int j = 0; j < valuesOperands.Length; j++)
                {
                    firstString = firstString.Replace(allOperands[j], char.Parse(valuesOperands[j].ToString()));
                    secondString = secondString.Replace(allOperands[j], char.Parse(valuesOperands[j].ToString()));
                }

                if (ReplaceOrAnd.WorkWithOrAndV2(firstString)) tempSumTrueFirstClozes++;
                if (ReplaceOrAnd.WorkWithOrAndV2(secondString)) tempSumTrueSecondClozes++;
            }

            return tempSumTrueFirstClozes + tempSumTrueSecondClozes >= this.sumTrueFirstClozes + this.sumTrueSecondClozes ? true : false;
        }

        /// <summary>
        /// Изменить значение переменной и чекнуть
        /// </summary>
        private void FlipAndCalculate()
        {
            CalculateTrueClozes();
            steps.Clear();
            int[] tempValueOperands = NewTempArray();

            int step;
            for (int i = 0; i < maxFlips + 1; i++)
            {
                step = NextStep();
                tempValueOperands[step] = ChangeValue(tempValueOperands[step]);
                if (EqualsTrueClozes(tempValueOperands))
                {
                    valuesOperands = tempValueOperands;
                    break;
                }
                else
                {
                    tempValueOperands[step] = ChangeValue(tempValueOperands[step]);
                }
            }
        }

        /// <summary>
        /// Следующий шаг для итерационного процесса
        /// </summary>
        /// <returns></returns>
        private int NextStep()
        {
            int newStep = rnd.Next(0, valuesOperands.Length);
            for (int i = 0; i < steps.Count; )
            {
                if (newStep == steps[i])
                {
                    newStep = rnd.Next(0, valuesOperands.Length);
                    i = 0;
                }
                else
                {
                    i++;
                }
            }
            steps.Add(newStep);
            return newStep;
        }

        /// <summary>
        /// Новая ссылка на массив
        /// </summary>
        /// <returns></returns>
        private int[] NewTempArray()
        {
            int[] tempValueOperands = new int[valuesOperands.Length];
            for (int i = 0; i < tempValueOperands.Length; i++)
                tempValueOperands[i] = valuesOperands[i];

            return tempValueOperands;
        }

        /// <summary>
        /// Изменить значение переменной
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int ChangeValue(int value)
        {
            return value == 0 ? 1 : 0;
        }
        #endregion

        #region Вывод и хранение данных
        /// <summary>
        /// Инициализация переменных при запуске метода
        /// </summary>
        /// <param name="formule"></param>
        private void Initialisation(string formule, int countOfIterations)
        {
            this.steps = new List<int>();
            parseFormule = new ParseFormule(formule, this);
            this.tempRandomClozeChange = new List<int>();
            this.formule = formule;
            this.resultList = new List<string>();
            this.valuesOperands = new int[allOperands.Count];
            this.rnd = new Random();
            this.maxIterations = _onlyFirstData ? 1 : countOfIterations;

        }

        /// <summary>
        /// Окончание работы метода
        /// </summary>
        private List<string> Finalization()
        {
            string emptyString = "Решения не было найдено!";
            if (resultList.Count == 0) resultList.Add(emptyString);

            return resultList;
        }

        /// <summary>
        /// Добавить в лист
        /// </summary>
        /// <param name="newResult"></param>
        private bool AddToList(string newResult)
        {
            foreach (string result in resultList)
                if (result.Equals(newResult)) return false;
            resultList.Add(newResult);
            return true;
        }

        /// <summary>
        /// Печать
        /// </summary>
        /// <param name="flag"></param>
        private void Print()
        {
            string result = null;
            for (int i = 0; i < valuesOperands.Length; i++)
                result += allOperands[i] + "=" + valuesOperands[i] + ", ";
            result += ": " + true;

            AddToList(result);
        }
        #endregion
    }
}