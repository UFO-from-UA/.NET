using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Test.Models;

namespace Test.Views
{
    public partial class QuestionSelection : Window
    {
        public int pos;
        public QuestionSelection()
        {
            InitializeComponent();
        }
        public QuestionSelection(List<Answer> Answers):this()
        {
            Fill(Answers);
        }

        private void Fill(List<Answer> answers)
        {
            try
            {

                for (int i = 1; i < 61; i++)
                {
                    Button newBtn = new Button();
                    newBtn.Content = i.ToString();
                    UNIFORM.Children.Add(newBtn);
                    if (answers.ElementAt(i - 1).AnswerNumber != 0)
                    {
                        newBtn.Background = (Brush)new BrushConverter().ConvertFrom("#4efc14");
                        newBtn.Click += b_click;
                        newBtn.MouseEnter += b_enter;
                        newBtn.MouseLeave += b_leave;
                        newBtn.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFromString("#ff8800ff"));
                    }
                    else
                    {
                        newBtn.Background = (Brush)new BrushConverter().ConvertFrom("#3c4047");
                        newBtn.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 53, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        private void b_leave(object sender, MouseEventArgs e)
        {
            (sender as Button).Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff4efc14"));
            (sender as Button).Foreground = Brushes.Black;
            (sender as Button).Cursor = Cursors.Arrow;
        }

        private void b_enter(object sender, MouseEventArgs e)
        {
            (sender as Button).Background = (SolidColorBrush)(new BrushConverter().ConvertFromString("#ff8800ff"));
            (sender as Button).Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff00"));
            (sender as Button).Cursor = Cursors.Hand;
        }

        private void b_click(object sender, RoutedEventArgs e)
        {
            try
            {
                pos = int.Parse((sender as Button).Content.ToString());
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 80, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        private void OK(object sender, RoutedEventArgs e)
        {
            pos = (int)Slider_pos.Value;
            this.DialogResult = true;
        }
    }
}
