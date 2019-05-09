using System;
using System.Windows;
using System.Windows.Forms;
using Test.Models;

namespace Test.Views
{
    public partial class Settings : Window
    {
        public bool _PDF_out_save,_EXCEL_out_save;
        public bool _shuffle, _first59, _faker, _savePDF, _saveEXCEL;
        public string _folder_PDF, _folder_EXCEL;
        public Settings(bool shuffles = false, bool savePDF = false, bool saveEXCEL = false, bool first59 = false)
        {
            InitializeComponent();
            _shuffle = shuffles;
            _savePDF = savePDF;
            _saveEXCEL = saveEXCEL;
            _first59 = first59;
            _faker = false;
            _EXCEL_out_save = false;
            _PDF_out_save = false;

            CH_shuffle.IsChecked = _shuffle;
            CH_first59.IsChecked = _first59;
            CH_save_PDF.IsChecked = _savePDF;
            CH_save_EXCELL.IsChecked = _saveEXCEL;
        }
        #region OK
        private void OK(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void OK(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //this.OK(sender, e);
            this.DialogResult = true;
        }
        #endregion
        #region shuffle

        private void CH_shuffle_Checked(object sender, RoutedEventArgs e)
        {
            _shuffle = true;
        }

        private void CH_shuffle_Unchecked(object sender, RoutedEventArgs e)
        {
            _shuffle = false;
        }
        #endregion
        #region first59

        private void CH_first59_Checked(object sender, RoutedEventArgs e)
        {
            _first59 = true;
        }

        private void CH_first59_Unchecked(object sender, RoutedEventArgs e)
        {
            _first59 = false;
        }

        #endregion
        #region PDF
        private void CH_save_PDF_Checked(object sender, RoutedEventArgs e)
        {
            _savePDF = true;
        }
        private void CH_save_PDF_Unchecked(object sender, RoutedEventArgs e)
        {
            _savePDF = false;
        }
        private void b_PDF_PATH_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FolderBrowserDialog FD = new FolderBrowserDialog();

                DialogResult result = FD.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.Yes || result == System.Windows.Forms.DialogResult.OK)
                {
                    _folder_PDF = FD.SelectedPath;
                    _PDF_out_save = true;
                    CH_PDF_PATH.IsEnabled = true;
                    CH_PDF_PATH.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 98, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }
        private void CH_PDF_PATH_Unchecked(object sender, RoutedEventArgs e)
        {
            CH_PDF_PATH.IsEnabled = false;
            CH_PDF_PATH.IsChecked = false;
            _EXCEL_out_save = false;
            _folder_PDF = null;
        }
        #endregion

        #region EXCEL
        private void CH_save_EXCELL_Checked(object sender, RoutedEventArgs e)
        {
            _saveEXCEL = true;
        }
        private void CH_EX_PATH_Unchecked(object sender, RoutedEventArgs e)
        {
            CH_EX_PATH.IsEnabled = false;
            CH_EX_PATH.IsChecked = false;
                _EXCEL_out_save = false;
            _folder_EXCEL = null;
        }
        private void CH_save_EXCELL_Unchecked(object sender, RoutedEventArgs e)
        {
            _saveEXCEL = false;
        }
        private void b_EXCEL_PATH_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FolderBrowserDialog FD = new FolderBrowserDialog();

                DialogResult result = FD.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.Yes || result == System.Windows.Forms.DialogResult.OK)
                {
                    _folder_EXCEL = FD.SelectedPath;
                    _EXCEL_out_save = true;
                    CH_EX_PATH.IsEnabled = true;
                    CH_EX_PATH.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 144, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
            //SaveFileDialog SD = new SaveFileDialog();
            //SD.Filter = "excel (*.xlsx)|*.xlsx|All files(*.*)|*.*";
            //if (SD.ShowDialog() == System.Windows.Forms.DialogResult.No )
            //    return;
            //string filename = SD.FileName;
        }

        #endregion

        #region Creator
        private void CH_Creator_Checked(object sender, RoutedEventArgs e)
        {
            Auth auth_view = new Auth("admin");
            if (bool.Parse(auth_view.ShowDialog().ToString()))
            {
                CH_faker.Visibility = Visibility.Visible;
                CH_first59.Visibility = Visibility.Visible;
            }
            else
            {
                CH_Creator.IsChecked = false;
            }
        }

        private void CH_Creator_Unchecked(object sender, RoutedEventArgs e)
        {
            CH_faker.Visibility = Visibility.Collapsed;
            CH_first59.Visibility = Visibility.Collapsed;
        }

      
        #endregion

        #region Faker
        private void CH_faker_Checked(object sender, RoutedEventArgs e)
        {
            _faker = true;
        }

        private void CH_faker_Unchecked(object sender, RoutedEventArgs e)
        {
            _faker = false;
        }
        #endregion

    }
}
