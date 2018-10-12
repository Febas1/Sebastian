using System;
using System.Collections.Generic;
using System.Data;
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

namespace SiasoftAppExt
{
    
    public partial class ConfiguracionCRM : UserControl
    {

        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string cnEmp = "";


        public ConfiguracionCRM(dynamic tabitem1)
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
            idemp = SiaWin._BusinessId;

            LoadConfig();

            cargarGrid();
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
                tabitem.Title = "Configuracion del CRM (" + aliasemp + ")";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void cargarGrid() {
            string queryGrid = "select nom_configuracion,con_configuracion from CrMae_configuracion where idrow=1 ";            
            DataTable dt = SiaWin.Func.SqlDT(queryGrid, "Clientes", idemp);
            dataGridConfig.ItemsSource = dt.DefaultView;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BTNactualizar.IsEnabled = true;
                TXB_User_Correo.IsEnabled = true;
                TXB_Con_Correo.IsEnabled = true;
                DataRowView row = (DataRowView)dataGridConfig.SelectedItems[0];
                TXB_User_Correo.Text = row["nom_configuracion"].ToString();
                TXB_Con_Correo.Text = row["con_configuracion"].ToString();
            }
            catch (Exception)
            {
                
            }
            
        }

        private void dataGridConfig_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            BTNeditar.IsEnabled = true;
        }

        private void BTNactualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cadena = "update CrMae_configuracion set nom_configuracion= '" + TXB_User_Correo.Text + "', con_configuracion= '" + TXB_Con_Correo.Text + "' WHERE idrow=1";
                SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                MessageBox.Show("Actualziacion exitosa");
                bloaquear();
                cargarGrid();
                BTNeditar.IsEnabled = false;
            }
            catch (Exception w)
            {
                MessageBox.Show("error al actualizar: " +w);   
            }
            
        }

        public void bloaquear() {

            BTNeditar.IsEnabled = false;
            TXB_User_Correo.IsEnabled = false;
            TXB_User_Correo.Text = "";
            TXB_Con_Correo.IsEnabled = false;
            TXB_Con_Correo.Text = "";
            BTNactualizar.IsEnabled = false;       
        }






    }
}
