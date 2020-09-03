using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinForms = System.Windows.Forms;
namespace XML_SUMM_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private enum Tabs : int
        {
            Summ,
            Obr

        }

        private void BTN_BROWSE_Click(object sender, RoutedEventArgs e)
        {
            var FBD = new WinForms.FolderBrowserDialog() { ShowNewFolderButton = false, RootFolder = System.Environment.SpecialFolder.Desktop };
            FBD.ShowDialog();
            LBL_SELECTED_FOLDER.Content = FBD.SelectedPath;
            Properties.Settings.Default.XML_FOLDER = FBD.SelectedPath;
            Properties.Settings.Default.Save();
        }

        private void XML_SUMM_MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //DO CLEANUP OF LBLS FROM SYSTEM TEXT
            LBL_RESULT_SPIS.Content = "";
            LBL_RESULT_POST_SPIS.Content = "";
            LBL_RESULT_POST_OPIS.Content = "";
            LBL_RESULT_OPIS.Content = "";
            LBL_SELECTED_FOLDER.Content = "";
            //load PATH
            LBL_SELECTED_FOLDER.Content = Properties.Settings.Default.XML_FOLDER;

            foreach  (string ra in Configs.Raions.LoadRA())
            {
                LB_RA_LIST.Items.Add(ra);
            }
           
        }

        private async void BTN_START_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.XML_FOLDER == "" || Directory.Exists(Properties.Settings.Default.XML_FOLDER) == false)
            {
                WinForms.MessageBox.Show("Папка не указана, или не найдена!");
                return;
            }

            switch (TC_TABS.SelectedIndex)
            {
                case (int)Tabs.Summ:
                    DATA.SummData ready = await SUMM.CountSummAsync(Properties.Settings.Default.XML_FOLDER, CB_BARCODE.IsChecked.GetValueOrDefault());
                    LBL_RESULT_SPIS.Content = String.Format("{0:#,##0.00}", ready.BankSpis);
                    LBL_RESULT_POST_SPIS.Content = String.Format("{0:#,##0.00}", ready.PostOpis);
                    LBL_RESULT_POST_OPIS.Content = String.Format("{0:#,##0.00}", ready.PostOpis);
                    LBL_RESULT_OPIS.Content = String.Format("{0:#,##0.00}", ready.BankOpis);
                    LBL_CNT_BANK.Content = string.Format("{0} Чел.", ready.BankCnt);
                    LBL_CNT_POST.Content = string.Format("{0} Чел.", ready.PostCnt);
                    break;
                case (int)Tabs.Obr:

                    break;
                default:
                    break;
            }

          
          

        }

        private void BTN_RA_ADD_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TB_RA.Text))
                return;
            LB_RA_LIST.Items.Add(TB_RA.Text);
            Configs.Raions.SaveRa(LB_RA_LIST.Items);
        }

        private void BTN_RA_DELETE_Click(object sender, RoutedEventArgs e)
        {
            if (LB_RA_LIST.SelectedIndex < 0 || LB_RA_LIST.SelectedIndex > LB_RA_LIST.Items.Count)
                return;

            
            LB_RA_LIST.Items.RemoveAt(LB_RA_LIST.SelectedIndex);
            Configs.Raions.SaveRa(LB_RA_LIST.Items);
        }
    }
}
