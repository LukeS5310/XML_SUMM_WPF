using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace XML_SUMM_WPF
{
    /// <summary>
    /// Generating list of people by specified creteria
    /// </summary>
    class OBR
    {
        private enum ValidationType : int
        {
            TYPE_BANK,
            TYPE_POST
        }

        private static Boolean CheckCodeValid(string code, ValidationType type)
        {
            switch (type)
            {
                case ValidationType.TYPE_BANK:
                    string[] bcodes = { "З1", "З2" };
                    foreach  (string cd in bcodes)
                    {
                        if (cd == code)
                        {
                            return true;
                        }
                    }
                    return false;
                case ValidationType.TYPE_POST:
                    string[] pcodes = { "2", "6", "8", "1", "7", "4", "5" };
                    foreach (string cd in pcodes)
                    {
                        if (cd == code)
                        {
                            return false;
                        }
                    }
                    return true;
                default:
                    break;
            }

            return false;
        }




    }
}
