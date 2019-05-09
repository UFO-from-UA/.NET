using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Test.Models;

namespace Test.Views
{
    public partial class YesNo : Window
    {
        public YesNo()
        {
            InitializeComponent();
        }
        public YesNo(Style backgr = null, Style buttons=null, Style text = null,string t = null, string button_OK = null, string title = null) :this()
        {
            try
            {
                DEFAULT(backgr, buttons, text, t);
                if (title!=null)
                {
                    this.Title = title;
                }
                if (t.Length>60)
                {
                    Height += 200;
                }
                if (button_OK!=null)
                {
                    bNo.Visibility = Visibility.Collapsed;
                    bYes.Content = button_OK;
                }
            }
            catch (Exception ex)
            {
                ExceptionForMail.ExceptionList.Add(new myException(this.GetType().Name, 46, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name));
            }
        }

        private void DEFAULT(Style backgr, Style buttons, Style text, string t)
        {
            if (buttons == null)
            {
                defaultStyle();
            }
            if (backgr == null)
            {
                defaultStyleBackgroundProperty();
            }
            if (text == null)
            {
                defaultStyleText();
            }
            if (t==null)
            {
                Text.Text = "Вы действительно хотите выйти ?";
            }
            if (t != null)
            {
                Text.Text = t;
            }
        }

        private void defaultStyleText()
        {
            Style b = new Style();
            b.Setters.Add(new Setter { Property = Control.FontSizeProperty, Value = 22.0 });
            //b.Setters.Add(new Setter { Property = Control.FontFamilyProperty, Value = new FontFamily("Verdana") });
            b.Setters.Add(new Setter { Property = Control.MarginProperty, Value = new Thickness(10) });
            b.Setters.Add(new Setter { Property = Control.ForegroundProperty, Value = new SolidColorBrush(Colors.White) });
            b.Setters.Add(new Setter { Property = Control.VerticalAlignmentProperty, Value = VerticalAlignment.Center});
            b.Setters.Add(new Setter { Property = Control.HorizontalAlignmentProperty, Value = HorizontalAlignment.Center });
            Text.Style = b;
        }

        private void defaultStyleBackgroundProperty()
        {
            // Create a linear gradient brush with five stops 
            LinearGradientBrush fiveColorLGB = new LinearGradientBrush();
            fiveColorLGB.StartPoint = new Point(0, 0);
            fiveColorLGB.EndPoint = new Point(1, 1);

            // Create and add Gradient stops
            GradientStop blueGS = new GradientStop();
            blueGS.Color = Colors.Blue;
            blueGS.Offset = 0.0;
            fiveColorLGB.GradientStops.Add(blueGS);

            GradientStop orangeGS = new GradientStop();
            orangeGS.Color = Colors.Purple;
            orangeGS.Offset = 0.25;
            fiveColorLGB.GradientStops.Add(orangeGS);

            GradientStop yellowGS = new GradientStop();
            yellowGS.Color = Colors.Black;
            yellowGS.Offset = 0.50;
            fiveColorLGB.GradientStops.Add(yellowGS);

            GradientStop greenGS = new GradientStop();
            greenGS.Color = Colors.Green;
            greenGS.Offset = 0.75;
            fiveColorLGB.GradientStops.Add(greenGS);

            GradientStop redGS = new GradientStop();
            redGS.Color = Colors.Red;
            redGS.Offset = 1.0;
            fiveColorLGB.GradientStops.Add(redGS);

            this.Background = fiveColorLGB;
    }

        private void defaultStyle()
        {
            Style b = new Style();
            b.Setters.Add(new Setter { Property = Control.FontSizeProperty, Value = 16.0 });
            //b.Setters.Add(new Setter { Property = Control.FontFamilyProperty, Value = new FontFamily("Verdana") });
            b.Setters.Add(new Setter { Property = Control.MarginProperty, Value = new Thickness(10) });
            b.Setters.Add(new Setter { Property = Control.BackgroundProperty, Value = new SolidColorBrush(Colors.Black) });
            b.Setters.Add(new Setter { Property = Control.ForegroundProperty, Value = new SolidColorBrush(Colors.White) });
            b.Setters.Add(new Setter { Property = Control.MarginProperty, Value = new Thickness(5) });
            b.Setters.Add(new Setter { Property = Control.WidthProperty, Value = 150.0 });
            b.Setters.Add(new Setter { Property = Control.HeightProperty, Value = 50.0 });

            bYes.Style = b;
            bNo.Style = b;
        }

        private void bYes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
