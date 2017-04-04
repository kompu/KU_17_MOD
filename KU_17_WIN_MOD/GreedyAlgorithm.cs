using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KU_17_WIN_MOD
{
    /// <summary>
    /// Жадный алгоритм решения SAT
    /// </summary>
    public class GreedyAlgorithm
    {
        List<string> clozes = new List<string>();

        public bool GreedyMethod(string formule)
        {
            clozes = GenerateFormulasListWithClozes(formule);
            return false;
        }

        /// <summary>
        /// Создаем из строки лист с формулами, которые разбиты на клозы
        /// </summary>
        /// <param name="formule"></param>
        /// <returns></returns>
        public List<string> GenerateFormulasListWithClozes(string formule)
        {
            List<string> generatedListClozes = new List<string>(); // лист с выходными формулами

            for (int j = 0; j < 2; j++)
            {

                int x = j;
                List<char> elements = formule.ToList();
                /*
                for (int i = x; i < elements.Count; i++)
                {
                    if (i == j*2)
                    {
                        elements.Insert(i,'(');
                        x++;
                    }
                    else if (x == 4)
                    {
                        elements.Insert(i, ')');
                        x = 0;
                    }
                    else
                    {
                        x++;
                    }
                }*/
                // добавляем стринг-переменную в общий лист формул
                generatedListClozes.Insert(j, StringBuilderChars(elements)); 
            }

            return generatedListClozes;
        }

        static string StringBuilderChars(List<char> charSequence)
        {
            // В процессе разработки
            StringBuilder sb = new StringBuilder();
            Regex reg = new Regex(@"\w");
            foreach (char c in charSequence)
            {
                if (reg.IsMatch(c.ToString()))
                {
                    sb.Append('(');
                }
                sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
