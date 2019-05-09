using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Net.Mail;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Test.Views;
using Test.Models;

namespace Test
{
    public static class ExtensionMethods
    {
        private static Action EmptyDelegate = delegate () { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }

    class NaturalStringComparer : IComparer<string>
    {
        [System.Runtime.InteropServices.DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        static extern int StrCmpLogicalW(string s1, string s2);

        public int Compare(string x, string y)
        {
            return StrCmpLogicalW(x, y);
        }
    }

    public partial class MainWindow : Window
    {
        private FullTest _TEST_;
        private Student _Student = new Student();
        private Border beforeSelection = new Border();
        private bool FLAG = false;
        private bool _Shuffle_FLAG = false;
        private bool _state = false;
        #region const

        private const string VERSION = "1.00.00";
        private const string CREATOR = "UFO";
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Pupil LOGIN = new Pupil();
                this.Visibility = Visibility.Hidden;
                if (bool.Parse(LOGIN.ShowDialog().ToString()))
                {
                    _Student = LOGIN._Student;
                    FIO.Text = _Student.ToString();
                    _Shuffle_FLAG = bool.Parse(ConfigurationManager.AppSettings["CONFIG_SHUFFLE"]);
                    iClass.Text = "Класс : " + _Student.Group;
                    InfoShuffleText();
                    ValidEmail(_Student.Mail);
                    this.Visibility = Visibility.Visible;
                    _TEST_ = new FullTest(_Student);
                    BeginTest();
                }
                else
                {
                    MessageBox.Show("Работа приложения завершена", "EXIT", MessageBoxButton.OK, MessageBoxImage.Error);
                    KILL();
                }
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 87, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }
        #region Emai VALIDATION

        private void ValidEmail(string mail)
        {
            if (!IsValidEmail(mail))
            {
                b_changeMail.Visibility = Visibility.Visible;
                //MessageBox.Show("Введенны вами электронный адрес не верен!\nЕсли вы хоите получить результаты теста введите действительный адрес!\nВозможно отсутствует подключение к интернету","Не верная почта");
            }
            else
            {
                b_changeMail.Visibility = Visibility.Collapsed;
            }
        }

        bool IsValidEmail(string email)
        {
            return new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(email);
        } 
        #endregion

        private void InfoShuffleText()
        {
            var s = _Shuffle_FLAG ? "включено" : "выключено";
            iShuffle.Text = $"Перемешивание вопросов {s}";
        }

        private void KILL()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        #region FILE SORT

        private void Qsort()
        {
            string[] subdirectoryEntries = Directory.GetDirectories(_TEST_.DirPath);
            string[] fileEntries = Directory.GetFiles(_TEST_.DirPath);

            Array.Sort(fileEntries, new NaturalStringComparer());

            for (int i = 0; i < subdirectoryEntries.Length; i++)
            {
                var zz = subdirectoryEntries[i].LastIndexOf('\\');
                subdirectoryEntries[i] = subdirectoryEntries[i].Substring(zz + 1);
            }
            for (int i = 1; i < 61; i++)
            {
                if (!subdirectoryEntries.Contains(i.ToString()))
                {
                    Directory.CreateDirectory($"{_TEST_.DirPath}\\{i}");
                    var part = i / 12;
                    if (part == 1)
                    {
                        var fileIMG = fileEntries.Where(x => x.Contains((i % 12).ToString())).Where(x => x.Substring(x.Length - 10).Contains("B")).First();
                        EXIST(fileIMG, $"{_TEST_.DirPath }\\{i}\\Q.png");
                    }
                    else if (part == 2)
                    {
                        if (i % 12 == 0)
                        {
                            var F = fileEntries.Where(x => x.Contains((12).ToString())).Where(x => x.Substring(x.Length - 10).Contains("B")).First();
                            EXIST(F, $"{_TEST_.DirPath }\\{i}\\Q.png");
                            continue;
                        }
                        var fileIMG = fileEntries.Where(x => x.Contains((i % 12).ToString())).Where(x => x.Substring(x.Length - 10).Contains("C")).First();
                        EXIST(fileIMG, $"{_TEST_.DirPath }\\{i}\\Q.png");
                    }
                    else if (part == 3)
                    {
                        if (i % 12 == 0)
                        {
                            var F = fileEntries.Where(x => x.Contains((12).ToString())).Where(x => x.Substring(x.Length - 10).Contains("C")).First();
                            EXIST(F, $"{_TEST_.DirPath }\\{i}\\Q.png");
                            continue;
                        }
                        var fileIMG = fileEntries.Where(x => x.Contains((i % 12).ToString())).Where(x => x.Substring(x.Length - 10).Contains("D")).First();
                        EXIST(fileIMG, $"{_TEST_.DirPath }\\{i}\\Q.png");
                    }
                    else if (part == 4)
                    {
                        if (i % 12 == 0)
                        {
                            var F = fileEntries.Where(x => x.Contains((12).ToString())).Where(x => x.Substring(x.Length - 10).Contains("D")).First();
                            EXIST(F, $"{_TEST_.DirPath }\\{i}\\Q.png");
                            continue;
                        }
                        var fileIMG = fileEntries.Where(x => x.Contains((i % 12).ToString())).Where(x => x.Substring(x.Length - 10).Contains("E")).First();
                        EXIST(fileIMG, $"{_TEST_.DirPath }\\{i}\\Q.png");
                    }
                    else
                    {
                        if (i % 12 == 0)
                        {
                            var F = fileEntries.Where(x => x.Contains((12).ToString())).Where(x => x.Substring(x.Length - 10).Contains("E")).First();
                            EXIST(F, $"{_TEST_.DirPath }\\{i}\\Q.png");
                        }
                    }
                }

            }
        }

        private void EXIST(string fileIMG, string v)
        {
            if (File.Exists($"{fileIMG}"))
            {
                // File.Copy(fileIMG, v.Replace("Q.png", fileIMG.Substring(fileIMG.Length-6)));
                File.Move(fileIMG, v);
            }
        }

        #endregion

        private void BeginTest()
        {
            // Qsort();
            try
            {
                if (!FLAG)
                {
                    if (_Shuffle_FLAG)
                    {
                        Random r = new Random();
                        List<Answer> tmp = _TEST_.Answers.Where(x => x.AnswerNumber == 0).ToList();//Default Test NEVER
                        _TEST_.CurrentQuestionNum = tmp.ElementAt(r.Next(0, tmp.Count())).QuestionNumber;
                        qNum.Text = "Вопрос № " + _TEST_.CurrentQuestionNum;
                    }
                }

                if (_TEST_.CurrentQuestionNum < 25)
                {
                    DrawQuestion6();
                }
                else
                {
                    DrawQuestion8();
                }
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 231, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        #region Draw

        private void DrawQuestion8()
        {
            try
            {
                AnswerVariant_7.Visibility = AnswerVariant_8.Visibility = Visibility.Visible;
                AnswerMarginLeft.Width = 300;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri($"{_TEST_.DirPath}{_TEST_.CurrentQuestionNum}\\Q.png");
                bitmap.EndInit();
                CroppedBitmap cb = new CroppedBitmap(bitmap, new Int32Rect(0, 0, 296, 247));
                imgQuestion.Source = cb;
                string imgName = "AnswerVariant_";
                for (int i = 1, j = 4, f = 1, l = 2; i < 5; i++, j++, f += 2, l += 2)
                {
                    int t = 118;
                    if (i == 4) { t = 120; }
                   (FindName(imgName + f) as Image).Source = new CroppedBitmap(bitmap, new Int32Rect((i - 1) * 118, 270, t, 70));
                    (FindName(imgName + l) as Image).Source = new CroppedBitmap(bitmap, new Int32Rect((i - 1) * 118, 368, t, 70));
                }
                beforeSelection.BorderBrush = null;
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 276, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));

            }
        }

        private void DrawQuestion6()
        {
            try
            {
                AnswerVariant_7.Visibility = AnswerVariant_8.Visibility = Visibility.Hidden;
                AnswerMarginLeft.Width = 350;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri($"{_TEST_.DirPath}{_TEST_.CurrentQuestionNum}\\Q.png");
                bitmap.EndInit();
                CroppedBitmap cb = new CroppedBitmap(bitmap, new Int32Rect(0, 0, 340, 218));
                imgQuestion.Source = cb;
                string imgName = "AnswerVariant_";
                for (int i = 1, j = 4, f = 1, l = 2; i < 4; i++, j++, f += 2, l += 2)
                {
                    int t = 149;
                    if (bitmap.Width < 429) { t = 148; }
                   (FindName(imgName + f) as Image).Source = new CroppedBitmap(bitmap, new Int32Rect((i - 1) * 140, 245, t, 80));
                    (FindName(imgName + l) as Image).Source = new CroppedBitmap(bitmap, new Int32Rect((i - 1) * 140, 358, t, 80));
                }
                beforeSelection.BorderBrush = null;
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 305, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        #endregion

        private void B_Next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FLAG)
                {
                    _TEST_.Answers.Where(x => x.QuestionNumber == _TEST_.CurrentQuestionNum).First().AnswerNumber = RealAnswerNum(int.Parse(((beforeSelection.Child) as Image).Name.Last().ToString()));
                }

                if (_TEST_.Answers.ElementAt(_TEST_.CurrentQuestionNum - 1).AnswerNumber == 0 && FLAG == false)
                {
                    _TEST_.Answers.Where(x => x.QuestionNumber == _TEST_.CurrentQuestionNum).First().AnswerNumber = RealAnswerNum(int.Parse(((beforeSelection.Child) as Image).Name.Last().ToString()));
                    _TEST_.CurrentQuestionNum += 1;
                    this.Progress.Value += 1;
                }
                else
                {
                    var Q = _TEST_.Answers.Where(x => x.AnswerNumber == 0).FirstOrDefault();
                    if (Q == null) { ClearMainWindow(); return; }
                    _TEST_.CurrentQuestionNum = Q.QuestionNumber;
                    FLAG = false;
                }

                if (!_TEST_.Answers.Contains(_TEST_.Answers.Where(x => x.AnswerNumber == 0).FirstOrDefault())) { ClearMainWindow(); return; }

                b_Next.IsEnabled = false;
                qNum.Text = "Вопрос № " + _TEST_.CurrentQuestionNum;
                BeginTest();
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 328, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        private int RealAnswerNum(int imgName)
        {
            if (_TEST_.CurrentQuestionNum < 25)
            {
                switch (imgName)
                {
                    case 2: return 4;
                    case 3: return 2;
                    case 4: return 5;
                    case 5: return 3;
                    case 6: return 6;
                    default: return 1;
                }
            }
            else
                switch (imgName)
                {
                    case 2: return 5;
                    case 3: return 2;
                    case 4: return 6;
                    case 5: return 3;
                    case 6: return 7;
                    case 7: return 4;
                    case 8: return 8;
                    default: return 1;
                }
        }

        private void First59()
        {
            try
            {
                this.Progress.Value = 59;
                Random r = new Random();
                int max = 7;
                for (int i = True12(); i < 60; i++)
                {
                    if (i > 24) { max = 9; }
                    _TEST_.Answers.Where(x => x.QuestionNumber == i).First().AnswerNumber = r.Next(3, max);
                    _TEST_.CurrentQuestionNum = 60;
                    qNum.Text = "Вопрос № " + _TEST_.CurrentQuestionNum;
                    BeginTest();
                }
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 377, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        private void ClearMainWindow()
        {
            try
            {
                b_End.Visibility = Visibility.Visible;
                b_Next.Visibility = Visibility.Collapsed;
                b_Next.Width = 200;
                qNum.Text = "Tecт завершен!";
                imgQuestion.Source = null;
                for (int i = 1; i < 9; i++)
                {
                    (FindName("AnswerVariant_" + i) as Image).Source = null;
                }
                _TEST_.Student.End = DateTime.Now;
                b_change_info.Visibility = Visibility.Visible;
            }
            catch (Exception ex )
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 400, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        private string Q_categ(int n)
        {
            if (n < 13)
            {
                return "A";
            }
            else if (n > 12 && n < 25)
            {
                return "B";
            }
            else if (n > 24 && n < 37)
            {
                return "C";
            }
            else if (n > 36 && n < 49)
            {
                return "D";
            }
            else if (n > 48 && n < 61)
            {
                return "E";
            }
            return "ERROR";
        }

        private void B_End_Click(object sender, RoutedEventArgs e)
        {
            DateTime start = DateTime.Now;
            DisableALL();
            PlaseholderCreate();
            this.Refresh(); 

            Thread T1thread = new Thread(T1); //_TEST_.CheckResult();
            T1thread.Start();
            T1thread.Join();
            DateTime end = DateTime.Now;
            var DT = end - start;

            ExceptionForMail.ExceptionList.Add(new myException($" FAKE |startCheckResult duration|{DT } ", 441, $"Start {start}", $"end  {end}"));
            ThankYou();
            _state = true;
        }

        private void T1()
        {
            _TEST_.CheckResult();
        }

        private void PlaseholderCreate()
        {
            this.Dispatcher.Invoke(() =>
            {
                TextBlock newBtn = new TextBlock()
            {
                Text = "Пожалуйста подождите!",
                FontSize = 60,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid myGrid = (Grid)this.Content;// Children.OfType<ComboBox>(
            myGrid.Children.Add(newBtn);
            });

        }

        private void ThankYou()
        {
            try
            {
                DisableALL();
                this.Dispatcher.Invoke(() =>
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(Path.GetFullPath(@"Fonts/END.jpg"));
                    bitmap.EndInit();
                    grid.Background = new ImageBrush() { ImageSource = bitmap };
                    this.Icon = new BitmapImage(new Uri("pack://application:,,,/Icons/end.png"));
                    this.Title += " завершен";
                    Result r = new Result(_TEST_.Student.Lname + " " + _TEST_.Student.Fname + " " + _TEST_.Student.Sname, _TEST_.IQ, _TEST_.IQ_percent);
                    this.Visibility = Visibility.Hidden;
                    r.ShowDialog();
                    this.Visibility = Visibility.Visible;
                    Closing -= Window_Closing;
                });
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 517, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        private void DisableALL()
        {
            this.Dispatcher.Invoke(() =>
             {
                 try
                 {
                     grid.Children.RemoveRange(0, grid.Children.Capacity);
                 }
                 catch (Exception ex)
                 {
                     ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 554, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));

                 }
             });
        }

        private void DrawSelection(object sender, MouseButtonEventArgs e)
        {
            beforeSelection.BorderBrush = null;
            (sender as Border).BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#5ef740"));
            b_Next.IsEnabled = true;
            beforeSelection = (sender as Border);
        }

        private void MENU_changeAnswe(object sender, RoutedEventArgs e)
        {
            try
            {
                b_End.Visibility = Visibility.Hidden; b_Next.Visibility = Visibility.Visible;
                Auth auth = new Auth();
                if (bool.Parse(auth.ShowDialog().ToString()))
                {
                    QuestionSelection qs = new QuestionSelection(_TEST_.Answers);
                    if (bool.Parse(qs.ShowDialog().ToString()))
                    {
                        b_Next.IsEnabled = false;
                        _TEST_.CurrentQuestionNum = qs.pos;
                        qNum.Text = "Вопрос № " + _TEST_.CurrentQuestionNum;
                        FLAG = true;
                        BeginTest();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 540, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        private void B_EXIT(object sender, RoutedEventArgs e)
        {
            KILL();
        }

        private void MENU_changeInfo(object sender, RoutedEventArgs e)
        {
            try
            {
                Auth auth = new Auth();
                if (bool.Parse(auth.ShowDialog().ToString()))
                {
                    Pupil LOGIN = new Pupil(_Student);
                    this.Visibility = Visibility.Hidden;
                    if (bool.Parse(LOGIN.ShowDialog().ToString()))
                    {
                        _Student = LOGIN._Student;
                        FIO.Text = _Student.ToString();
                        iClass.Text = "Класс : " + _Student.Group;
                        this.Visibility = Visibility.Visible;
                        _TEST_.Student = _Student;
                    }
                    else
                    {
                        MessageBox.Show("Изменения не внесены", "Отмена", MessageBoxButton.OK, MessageBoxImage.Error);
                        this.Visibility = Visibility.Visible;
                    }
                    ValidEmail(_TEST_.Student.Mail);
                }
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 576, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        private void FakeGeneration(object sender, RoutedEventArgs e)
        {
            try
            {
                Random r = new Random();
                int max = 7;
                for (int i = True12(); i < 61; i++)
                {
                    if (i > 24) { max = 9; }
                    _TEST_.Answers.Where(x => x.QuestionNumber == i).First().AnswerNumber = r.Next(3, max);
                }
                b_End.Visibility = Visibility.Visible;
                this.Progress.Value = 60;
                ClearMainWindow();
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 600, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        private int True12()
        {
            int t = 1;

            _TEST_.Answers.Where(x => x.QuestionNumber == t).First().AnswerNumber = 4; t++;
            _TEST_.Answers.Where(x => x.QuestionNumber == t).First().AnswerNumber = 5; t++;
            _TEST_.Answers.Where(x => x.QuestionNumber == t).First().AnswerNumber = 1; t++;
            _TEST_.Answers.Where(x => x.QuestionNumber == t).First().AnswerNumber = 2; t++;
            _TEST_.Answers.Where(x => x.QuestionNumber == t).First().AnswerNumber = 6; t++;
            _TEST_.Answers.Where(x => x.QuestionNumber == t).First().AnswerNumber = 3; t++;
            _TEST_.Answers.Where(x => x.QuestionNumber == t).First().AnswerNumber = 6; t++;
            _TEST_.Answers.Where(x => x.QuestionNumber == t).First().AnswerNumber = 2; t++;
            _TEST_.Answers.Where(x => x.QuestionNumber == t).First().AnswerNumber = 1; t++;
            _TEST_.Answers.Where(x => x.QuestionNumber == t).First().AnswerNumber = 3; t++;
            _TEST_.Answers.Where(x => x.QuestionNumber == t).First().AnswerNumber = 4; t++;
            _TEST_.Answers.Where(x => x.QuestionNumber == t).First().AnswerNumber = 5; t++;
            return t;
        }

        private void MENU_info(object sender, RoutedEventArgs e)
        {
            try
            {
                Style z = new Style(); z = null;
                YesNo dlg = new YesNo(z, button_OK: "OK", title: "Информация", t: $"It's realy easy!");
                dlg.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 630, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        private void MENU_about(object sender, RoutedEventArgs e)
        {
            try
            {
                Style z = new Style(); z = null;
                YesNo dlg = new YesNo(z, button_OK: "OK", title: "Информация о программе", t: $"Version: {VERSION}\nCreator: {CREATOR}\nContact E-mail: z201293@mail.ru\nhttps://github.com/UFO-from-UA/.NET");
                dlg.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 760, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        private void MENU_Settings(object sender, RoutedEventArgs e)
        {
            try
            {
                Auth auth_view = new Auth();
                if (bool.Parse(auth_view.ShowDialog().ToString()))
                {
                    Settings settings_view = new Settings(_Shuffle_FLAG, _TEST_._PDF_save, _TEST_._EXCEL_save);
                    if (bool.Parse(settings_view.ShowDialog().ToString()))
                    {
                        bool flagBefore = _Shuffle_FLAG;
                        _Shuffle_FLAG = settings_view._shuffle;
                        InfoShuffleText();

                        _TEST_._PDF_save = settings_view._savePDF;
                        _TEST_._EXCEL_save = settings_view._saveEXCEL;
                        if (settings_view._EXCEL_out_save)
                        {
                            _TEST_._folder_EXCEL = settings_view._folder_EXCEL;
                        }
                        if (settings_view._PDF_out_save)
                        {
                            _TEST_._folder_PDF = settings_view._folder_PDF;
                        }
                        if (flagBefore != _Shuffle_FLAG)
                        {
                            SetSetting("CONFIG_SHUFFLE", _Shuffle_FLAG.ToString());
                        }
                        if (!_Shuffle_FLAG)
                        {
                            var Q = _TEST_.Answers.Where(x => x.AnswerNumber == 0).FirstOrDefault();
                            if (Q == null) { ClearMainWindow(); return; }
                            _TEST_.CurrentQuestionNum = Q.QuestionNumber;
                            qNum.Text = "Вопрос № " + _TEST_.CurrentQuestionNum;
                            BeginTest();
                        }

                        if (settings_view._first59)
                        {
                            First59();
                        }
                        if (settings_view._faker)
                        {
                            FakeGeneration(sender, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 680, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        #region app SAVE settings

        private void SetSetting(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (IsContains(key))
            {
                config.AppSettings.Settings[key].Value = value;
                config.Save(ConfigurationSaveMode.Full, true);
                ConfigurationManager.RefreshSection("appSettings");
            }
            else
            {
                config.AppSettings.Settings.Add(key, value);
                config.Save(ConfigurationSaveMode.Minimal);
            }

        }

        public bool IsContains(string find)
        {
            bool flag = false;
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                if (key == find)
                { flag = true; break; }
            }
            return flag;

        }
        #endregion

        private void B_changeMail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Pupil LOGIN = new Pupil(_Student, "CHANGE_MAIL");
                this.Visibility = Visibility.Hidden;
                if (bool.Parse(LOGIN.ShowDialog().ToString()))
                {
                    _TEST_.Student.Mail = LOGIN._Student.Mail;
                    this.Visibility = Visibility.Visible;
                    b_changeMail.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageBox.Show("Изменения не внесены", "Отмена", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Visibility = Visibility.Visible;
                }
                ValidEmail(_TEST_.Student.Mail);
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 737, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }       

        private void DOUBLE_CLICK(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.B_Next_Click(sender,e);
                e.Handled = true;// чтобы отключить ивент MouseDown="DrawSelection"
            }
        }

        private void Progress_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Style z = new Style();
                z = null;
                YesNo dlg = new YesNo(z, button_OK: "OK", title:"Прогресс", t: $"Всего пройдено {Progress.Value} из 60 вопросов!");
                dlg.ShowDialog();
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 760, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Style z = new Style();
                z = null;
                YesNo exit = new YesNo(z);
                if (!bool.Parse(exit.ShowDialog().ToString()))
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 780, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_state)
            {
                DateTime start = DateTime.Now;
                _TEST_.MailTo();
                DateTime end = DateTime.Now;
                var DT = end - start;
            }
            else
            {
                try
                {
                    MailAddress from = new MailAddress("anastasia.75.work@gmail.com", "Psyholog");
                    MailAddress to = new MailAddress("yuhim93@gmail.com");
                    MailMessage m = new MailMessage(from, to);
                    m.Subject = "Тест";
                    string z = "";
                    foreach (var i in ExceptionForMail.ExceptionList){ z += i + "\n<br>"; }
                    m.Body = $"<h1>Имя компа {System.Security.Principal.WindowsIdentity.GetCurrent().Name}</h1>" +
                        $"<br><h2>EXCEPTION ({ExceptionForMail.ExceptionList.Count}) </h2><br>" + z;
                    m.IsBodyHtml = true;
                    #region Credentials
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new NetworkCredential("anastasia.75.work@gmail.com", "Q1W2E3R4QWER"),
                        EnableSsl = true
                    }; 
                    #endregion
                    smtp.Send(m);
                    m.Dispose();
                }
                catch (Exception ex)
                {
                    ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 817, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
                }
            }
        }
    }
}
