using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KU_17_WIN_MOD
{
    /// <summary>
    /// DPLL алгоритм
    /// </summary>
    class DPLLAlgorithm
    {
        string[] clozes;
        public bool DPLLMethod(string formule)
        {
            ParseForDPLL parseDPLL = new ParseForDPLL(formule);
            clozes = parseDPLL.Clozes;
            return false;
        }
    }
}
