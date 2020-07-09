using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using XML_SUMM_WPF.Properties;

namespace XML_SUMM_WPF.Configs
{
    class Raions
    {
        private static readonly string RA_Default = "1,20,32,42,45,132,4,11,227,327,427,527,27,001,501,502,504,505,506,507,508,509,510,511,512,513,801,802,803,805,807,812,813,815,701,702,703,705,707,708,709,710,711,712,713,714,715,716,717,7,36,56,156,201,202,203,205,206,207,208,209,210,211,212,213,214,215,216,6,23,50,150";
        private static readonly int RA_Ver = 1;
        public static string[] LoadRA()
        {

            if (Settings.Default.Raions == "" || Settings.Default.Ra_Ver < RA_Ver )
            {
                //SET TO DEFAULT

                Settings.Default.Raions = RA_Default;
                Console.WriteLine("DEFAULT RA LOADED");
                Settings.Default.Ra_Ver = RA_Ver;
                Settings.Default.Save();


            }
            string[] stmp;
            stmp = Settings.Default.Raions.Split(Convert.ToChar(","));
            return stmp;
        }

        public static void SaveRa(ItemCollection Raions)
        {
            string stmp ="";

            foreach (string RA in Raions)
            {
                if (stmp == "")
                    stmp = RA;
                stmp = string.Join(",", stmp, RA);
            }
            Settings.Default.Raions = stmp;
            Settings.Default.Save();

        }


    }
}
