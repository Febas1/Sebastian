using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
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
using Syncfusion.Windows.Tools.Controls;
using Syncfusion.Windows.Tools;
using Syncfusion.Windows.Controls.Input;

namespace Sebastian
{
    public partial class Textbox : UserControl
    {
        public Textbox()
        {
            InitializeComponent();
        }
        private bool handle = true;
       
        private void Eleccion_DropDownClosed(object sender, EventArgs e)
        {
            if (handle) Handle();
            handle = true;
        }
        private void Eleccion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            handle = !cmb.IsDropDownOpen;
            Handle();
        }
        private void Handle()
        {
            switch (EleccionCMB.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last())
            {
                case "TextBoxExt":
                    TextBoxExt_func();
                    break;
                case "CurrencyTextBox":
                    break;
                case "DoubleTextBox":
                    break;
                case "IntegerTextBox":
                    break;
                case "PercentTextBox":
                    break;
                case "AutoComplete":
                    AutoComplete_func();
                    break;
            }
        }
        private void TextBoxExt_func()
        {
            TXTSpace.Children.Clear();
            SfTextBoxExt textBox = new SfTextBoxExt();
            textBox.VerticalAlignment = VerticalAlignment.Center;
            textBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            textBox.Margin = new Thickness(5);
            textBox.Watermark = "Esta es una prueba de la marca de agua de este control";
            TXTSpace.Children.Add(textBox);

        }
        private void CurrencyTextBox_func()
        {

        }
        private void DoubleTextBox_func()
        {

        }
        private void IntegerTextBox_func()
        {

        }
        private void PercentTextBox_func()
        {

        }
        private void AutoComplete_func()
        {
            TXTSpace.Children.Clear();
            AutoComplete AutoComplete1 = new AutoComplete();
            List<String> productSource = new List<String>();
            productSource.Add("WPF");
            productSource.Add("Chart");
            productSource.Add("GridView");
            productSource.Add("WF");
            productSource.Add("Xlsio");
            productSource.Add("Business Intelligence");
            productSource.Add("Tools");
            productSource.Add("Silverlight");
            productSource.Add("Schedule");
            productSource.Add("Mvc");
            productSource.Add("Pdf");


            AutoComplete1.CustomSource = productSource;
            AutoComplete1.SelectionMode = SelectionMode.Single;
            AutoComplete1.IsAutoAppend = true;
            AutoComplete1.IsFilter = true;
            AutoComplete1.StringMode = StringMode.AnyChar;
            AutoComplete1.Margin = new Thickness(5, 5, 5, 5);
            AutoComplete1.VerticalAlignment = VerticalAlignment.Center;
            AutoComplete1.HorizontalAlignment = HorizontalAlignment.Stretch;
                
            TXTSpace.Children.Add(AutoComplete1);
        }
    }
}
