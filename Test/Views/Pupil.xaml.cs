using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Test
{
    public partial class Pupil : Window
    {
        private       System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public Student _Student = new Student();
        public Pupil()
        {
            InitializeComponent();
            LastNаme.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                LastNаme.Focus();
            }));
        }
        public Pupil(Student s):this()
        {
            Fill(s);
        }
        public Pupil(Student s,string param):this(s)
        {
            ParamChange(param);
        }

        private void ParamChange(string param)
        {
            if (param == "CHANGE_MAIL")
            {
                LastNаme.IsEnabled = false;
                FirstName.IsEnabled = false;
                SecondName.IsEnabled = false;
                Group.IsEnabled = false;
                Age.IsEnabled = false;
                Phone.IsEnabled = false;
            }
        }

        private void Fill(Student s)
        {
            LastNаme.Text = s.Lname;
            FirstName.Text = s.Fname;
            SecondName.Text = s.Sname;
            Group.Text = s.Group;
            Age.Text = s.Age.ToString();
            Phone.Text = s.Phone;
            Mail.Text = s.Mail;
        }

        private void CHANGE(object sender, TextChangedEventArgs e)
        {
            var tb = (sender as TextBox);
            if (tb.Name == "Age") { tb.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#5ef740")); return; }
            if (tb.Text.Length > 1)
            {
                tb.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#5ef740"));
            }
            else
            {
                tb.BorderBrush = Brushes.Red;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Mail.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#5ef740"));
            Phone.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#5ef740"));
        }

        private void e_KeyDown(object sender, KeyEventArgs e)
        {            
            if (e.Key.ToString().Length == 2)
            {
                if (Char.IsNumber(e.Key.ToString()[1]))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = true;
            }
            OK_KeyDown(sender,e);
        }
        
        private void OK(object sender, RoutedEventArgs e)
        {
            int t = 0;
            //Валидация ! //Проблема пробелы Char.IsWhiteSpace
            var regexItem = new Regex("^[а-яёА-ЯЁ]+$");
            var regex = new Regex("^[0-9]+$");
            var regexGroup = new Regex(@"^[0-9-]*[а-яёА-ЯЁ]*$");
            if (!regexItem.IsMatch(LastNаme.Text))
            {
                borderLastNаme.Background = (Brush)new BrushConverter().ConvertFrom("#ff2128");
                LastNаme.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff2128"));
                //MessageBox.Show("Заполните все красные поля\n Пробелы и спец-символы не допускаются!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                LastNаme.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#5ef740"));
                borderLastNаme.Background = (Brush)new BrushConverter().ConvertFrom("#5ef740");
                t++;
            }
            if (!regexItem.IsMatch(FirstName.Text))
            {
                borderFirstName.Background = (Brush)new BrushConverter().ConvertFrom("#ff2128");
                FirstName.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff2128"));
               // MessageBox.Show("Заполните все красные поля\n Пробелы и спец-символы не допускаются!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                FirstName.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#5ef740"));
                borderFirstName.Background = (Brush)new BrushConverter().ConvertFrom("#5ef740");
                t++;
            }
            if (!regexItem.IsMatch(SecondName.Text))
            {
                borderSecondName.Background = (Brush)new BrushConverter().ConvertFrom("#ff2128");
                SecondName.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff2128"));
               // MessageBox.Show("Заполните все красные поля\n Пробелы и спец-символы не допускаются!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                SecondName.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#5ef740"));
                borderSecondName.Background = (Brush)new BrushConverter().ConvertFrom("#5ef740");
                t++;
            }
            if (!regexGroup.IsMatch(Group.Text))
            {
                borderGroup.Background = (Brush)new BrushConverter().ConvertFrom("#ff2128");
                Group.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff2128"));
              //  MessageBox.Show("Заполните все красные поля\n Пробелы и спец-символы не допускаются!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                Group.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#5ef740"));
                borderGroup.Background = (Brush)new BrushConverter().ConvertFrom("#5ef740");
                t++;
            }
            if (!regex.IsMatch(Age.Text))
            {
                borderAge.Background = (Brush)new BrushConverter().ConvertFrom("#ff2128");
                Age.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff2128"));
                //MessageBox.Show("Заполните все красные поля\n Пробелы и спец-символы не допускаются!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                Age.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#5ef740"));
                borderAge.Background = (Brush)new BrushConverter().ConvertFrom("#5ef740");
                t++;
            }

            borderPhone.Background = (Brush)new BrushConverter().ConvertFrom("#5ef740");
            borderMail.Background = (Brush)new BrushConverter().ConvertFrom("#5ef740");
            if (t==5)
            {
                _Student.Lname = LastNаme.Text;
                _Student.Fname = FirstName.Text;
                _Student.Sname = SecondName.Text;
                _Student.Group = Group.Text;
                _Student.Age = int.Parse(Age.Text);
                _Student.Phone = Phone.Text;
                _Student.Mail = Mail.Text;
                this.DialogResult = true;
            }
            else
            {
                //createTimer
                dispatcherTimer.Tick += new EventHandler(Timer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0,0,30);
                dispatcherTimer.Start();
                MessageBox.Show("Заполните все поля подчеркнутые красным\nФИО не должно содержать цифр\nЛюбые спец-символы запещены", "Ошибка ввода",MessageBoxButton.OK,MessageBoxImage.Warning);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            borderLastNаme.Background.Opacity -= 0.01;
            borderFirstName.Background.Opacity -= 0.01;
            borderSecondName.Background.Opacity -= 0.01;
            borderGroup.Background.Opacity -= 0.01;
            borderAge.Background.Opacity -= 0.01;
            borderPhone.Background.Opacity -= 0.01;
            borderMail.Background.Opacity -= 0.01;
            //this.Title = borderLastNаme.Background.Opacity.ToString();
            if (borderLastNаme.Background.Opacity<0)
            {
                dispatcherTimer.Stop();
                dispatcherTimer.Tick -= Timer_Tick;
            }
        }

        private void OK_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OK(this, new RoutedEventArgs());
            }
        }
    }
}
