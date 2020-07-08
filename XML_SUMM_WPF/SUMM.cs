using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Xml.XPath;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace XML_SUMM_WPF
{
    class SUMM
    {
    public static async Task<DATA.SummData> CountSummAsync(string path,Boolean BarCodeCheck = false)
        {
            DATA.SummData Summ = await Task.Run(()=>CountSumm(path,BarCodeCheck));
            return Summ;
        }
    public static DATA.SummData CountSumm(string path, Boolean BarCodeCheck = false)
        {

            XElement Doc;
            FileInfo[] XmlFiles = new DirectoryInfo(path).GetFiles("*.xml");
            var Summ = new DATA.SummData();
            foreach (FileInfo XmlFile in XmlFiles)
            {
                if (XmlFile.Name.IndexOf("OUT") != -1)
                {
                    //NOT OUR TYPE HERE
                    continue;
                }
                
                Doc = XElement.Load(XmlFile.FullName);
                switch (Doc.Element("ПачкаВходящихДокументов").Attribute("ДоставочнаяОрганизация").Value)
                {
                    case "БАНК":
                        //BANK CODE
                        if (XmlFile.Name.IndexOf("-OPPF-") != -1) //Это опись
                            Summ.BankOpis += Convert.ToDecimal(Doc.Element("ПачкаВходящихДокументов").Element("ОПИСЬ_ПЕРЕДАВАЕМЫХ_ФАЙЛОВ_НА_ЗАЧИСЛЕНИЕ").Element("ПередаваемыйФайл").Element("СуммаПоЧастиМассива").Value.Replace(".", ","));
                        else
                        {
                            Summ.BankSpis += Convert.ToDecimal(Doc.Element("ПачкаВходящихДокументов").Element("СПИСОК_НА_ЗАЧИСЛЕНИЕ").Element("СуммаПоФилиалу").Value.Replace(".", ","));
                            Summ.BankCnt += Convert.ToInt32(Doc.Element("ПачкаВходящихДокументов").Element("СПИСОК_НА_ЗАЧИСЛЕНИЕ").Element("КоличествоПолучателей").Value);
                        }
                        break;
                    case "ПОЧТА":
                        //POST CODE
                        if (Doc.XPathSelectElement("/ПачкаВходящихДокументов/ВХОДЯЩАЯ_ОПИСЬ/СоставДокументов/НаличиеДокументов/ТипДокумента").Value == "ПОРУЧЕНИЕ_НА_ДОСТАВКУ_ПЕНСИЙ")
                        {
                            Summ.PostSpis += Convert.ToDecimal(Doc.XPathSelectElement("ПачкаВходящихДокументов/ВХОДЯЩАЯ_ОПИСЬ/СуммаПоМассиву").Value.Replace(".", ","));
                            Summ.PostCnt += Convert.ToInt32(Doc.XPathSelectElement("/ПачкаВходящихДокументов/ВХОДЯЩАЯ_ОПИСЬ/СоставДокументов/НаличиеДокументов/Количество").Value) ;
                        }
                        else if (Doc.XPathSelectElement("/ПачкаВходящихДокументов/ВХОДЯЩАЯ_ОПИСЬ/СоставДокументов/НаличиеДокументов/ТипДокумента").Value == "СОПРОВОДИТЕЛЬНАЯ_ОПИСЬ_ПОРУЧЕНИЙ")
                           
                        Summ.PostOpis += Convert.ToDecimal(Doc.XPathSelectElement("ПачкаВходящихДокументов/ВХОДЯЩАЯ_ОПИСЬ/СуммаПоМассиву").Value.Replace(".", ","));

                        if (BarCodeCheck == true)
                        {
                            if (CheckBarCode(Doc)==false)
                            {
                                System.Windows.Forms.MessageBox.Show(string.Format("Отсутствуют штрих-коды в файле {0}",XmlFile.Name));
                            }
                        }
                        break;
                    default:
                        break;
                }

            }
            
            return Summ;
        }
        private static Boolean CheckBarCode(XElement XmlDoc)
        {
            IEnumerable<XElement> TempElems = from el in XmlDoc.XPathSelectElements("/ПачкаВходящихДокументов/ПОРУЧЕНИЕ_НА_ДОСТАВКУ_ПЕНСИЙ") select el;
            foreach (XElement el in TempElems)
            {
                if (el.Element("ШтриховойКод").Value == "" && el.Element("ПризнакПечатиШтриховогоКода").Value == "ПЕЧАТАТЬ") return false;
                

            }
                return true;
        }
    }
}
