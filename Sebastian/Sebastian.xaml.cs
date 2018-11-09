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
using Sebastian;



namespace SiasoftAppExt
{

    public partial class Sebastian : UserControl
    {

        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string cnEmp = "";

        public Sebastian(dynamic tabitem1)
        {
            InitializeComponent();
            tabitem = tabitem1;
            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;

            LoadConfig();
        }

        private void LoadConfig()
        {
            try
            {

                System.Data.DataRow foundRow = SiaWin.Empresas.Rows.Find(idemp);
                int idLogo = Convert.ToInt32(foundRow["BusinessLogo"].ToString().Trim());
                idemp = Convert.ToInt32(foundRow["BusinessId"].ToString().Trim());
                cnEmp = foundRow[SiaWin.CmpBusinessCn].ToString().Trim();
                string aliasemp = foundRow["BusinessAlias"].ToString().Trim();
                tabitem.Logo(idLogo, ".png");
                tabitem.Title = "UX Siasoft(" + aliasemp + ")";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void Efecto_Click(object sender, RoutedEventArgs e)
        {
            Panel.Children.Clear();
            Panel.Children.Add(new Grides());
        }
        private void Color_Click(object sender, RoutedEventArgs e)
        {
            Panel.Children.Clear();
            Panel.Children.Add(new Colores());
        }
        private void Ses_Click(object sender, RoutedEventArgs e)
        {
        
            if (accordion.Visibility == Visibility.Visible)
            {;
                accordion.Visibility = Visibility.Collapsed;
            }
            else
            {
                accordion.Visibility = Visibility.Visible;
            }
        }

        private void Texbotz_Click(object sender, RoutedEventArgs e)
        {
            Panel.Children.Clear();
            Panel.Children.Add(new Textbox());
        }
        
    }
}
