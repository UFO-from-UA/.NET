using System;
using System.Windows;
using System.Security;
using System.Runtime.InteropServices;
using Test.Models;

namespace Test.Views
{

    public partial class Auth : Window
    {
        private  System.Windows.Threading.DispatcherTimer _dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        private string _role;
        public Auth()
        {
            InitializeComponent();
            pb_pass.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                pb_pass.Focus();
            }));

        }
        public Auth(string s):this()
        {
            _role = s;
            this.Title += " " + s;
        }

        private void OK(object sender, RoutedEventArgs e)
        {
            var a = ConvertToUnsecureString(pb_pass.SecurePassword);

            if (_role=="admin")
            {
                if (a == "zzz")
                {
                    this.DialogResult = true;
                }
            }
            else
            {
                if (a == "1") // q1w2e3r4
                {
                    this.DialogResult = true;
                }
                else
                {
                    if (_dispatcherTimer.IsEnabled)
                    {
                        _dispatcherTimer.Stop();
                        _dispatcherTimer.Tick -= Timer_Tick;
                        borderMain.Opacity = 1;
                    }
                    borderMain.Background = System.Windows.Media.Brushes.Red;
                    _dispatcherTimer.Tick += new EventHandler(Timer_Tick);
                    _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
                    _dispatcherTimer.Start();
                }
            }
            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                borderMain.Opacity -= 0.01;
                if (this.Background.Opacity < 0)
                {
                    _dispatcherTimer.Stop();
                    _dispatcherTimer.Tick -= Timer_Tick;
                }
            }
            catch (Exception ex)
            {
                _dispatcherTimer.Stop();
                _dispatcherTimer.Tick -= Timer_Tick;
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 86, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
            {
                return string.Empty;
            }

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        private void OK_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                OK(this, new RoutedEventArgs());
            }
        }
    }
}
