using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace XML_SUMM_WPF.EXPORT
{
    interface ISaveFile
    {
        void Save(string Content, string Path, bool AppendTimeStamp = true);
        

    }
}
