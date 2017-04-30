using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KU_17_WIN_MOD
{
    /// <summary>
    /// Парсим формулу на клозы для алгоритма DPLL
    /// </summary>
    class ParseForDPLL
    {
        private string[] clozes;
        private string formule;
        public string[] Clozes
        {
            get
            {
                return clozes;
            }

            set
            {
                clozes = value;
            }
        }

        /// <summary>
        /// Конструктор для парсинга
        /// </summary>
        /// <param name="formule"></param>
        public ParseForDPLL(string formule)
        {
            this.formule = formule;
            ParsingOnClozes();
        }

        /// <summary>
        /// Парсим на клозы
        /// </summary>
        private void ParsingOnClozes()
        {
            clozes = formule.Split('&');
        }

        private bool CheckClozes(byte[] valueOperands)
        {
            return true;
        }
    }
}
