using System.Windows;

namespace Test.Views
{

    public partial class Result : Window
    {
        public Result()
        {
            InitializeComponent();
        }
        public Result(string FIO, string IQ, string percrnt=""):this()
        {
            tb.Text = $"{FIO}\n Ваш IQ  = {IQ}";
        }
    }
}
