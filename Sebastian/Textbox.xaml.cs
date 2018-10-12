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

namespace Sebastian
{
    /// <summary>
    /// Lógica de interacción para Controles.xaml
    /// </summary>
    public partial class Textbox : UserControl
    {
        public Textbox()
        {
            InitializeComponent();
        }

        private void Cambio_Click(object sender, RoutedEventArgs e)
        {
            double cambio = 0;
            try
            {
                if (Convert.ToDouble(ValueD.Text) != 0)
                {
                cambio = Convert.ToDouble(ValueD.Text);
                DoubleBox.Value = cambio;
                }
                else
                {
                    MessageBox.Show("Digite unicamente numeros con punto");
                }
        }
            catch (Exception ses)
            {
                MessageBox.Show("Error en : "+ses);
            }

        }
    

        private void Null_Click(object sender, RoutedEventArgs e)
        {
            Boolean bul = true;
            Boolean bel = false;
            DoubleTextBox textBox = new DoubleTextBox();
            if (DoubleBox.UseNullOption == false)
            {
                DoubleBox.UseNullOption = textBox.UseNullOption = bul;
                if (Convert.ToDouble(DoubleBox.Text) == 00.0)
                {
                    DoubleBox.Text = "";
                }
            }
            else
            {
                DoubleBox.UseNullOption = textBox.UseNullOption = bel;
            }
        }
    }
}
