using AsignacionClientes;
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

    public partial class AsignacionClientes : UserControl
    {

        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string vendedor = "";
        int codigoUsuario = 0;
        string cnEmp = "";
        DataTable dt = new DataTable();

        public AsignacionClientes(dynamic tabitem1)
        {

            InitializeComponent();

            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
            idemp = SiaWin._BusinessId;
            vendedor = SiaWin._UserAlias;
            codigoUsuario = SiaWin._UserId;


            LoadConfig();

            cargarVendedores();

            BTNCliAsi.IsEnabled = false;
            BTNCliReasig.IsEnabled = false;
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
                tabitem.Title = "Asignacion de Clientes(" + aliasemp + ")";

                //TxtUser.Text = SiaWin._UserAlias;                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void cargarVendedores() {

            string queryGrid = "select inmae_mer.cod_mer as cod_mer,inmae_mer.nom_mer as nom_mer from inmae_mer where inmae_mer.estado='1'  ";

            dt = SiaWin.Func.SqlDT(queryGrid, "Vendedores", idemp);
            dataGridCxC.ItemsSource = dt.DefaultView;
            TotalGrid.Text = dt.Rows.Count.ToString();
        }

        private void FirstDetailsViewGrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e) {
            cargarClientes();
            BTNCliReasig.IsEnabled = true;

        }

        public void cargarClientes() {
            try
            {
                DataRowView row = (DataRowView)dataGridCxC.SelectedItems[0];
                string cod_ven = row[0].ToString();

                string queryGrid = "select cod_ter,nom_ter from comae_ter where cod_ven='" + cod_ven + "' ";

                dt = SiaWin.Func.SqlDT(queryGrid, "Clientes", idemp);
                dataGridClientes.ItemsSource = dt.DefaultView;

                VendedorGrid.Text = row[1].ToString();
                ClientesTotal.Text = dt.Rows.Count.ToString();
            }
            catch (Exception)
            {

            }
        }


        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dataGridCxC.SelectedItems[0];
            string codVen = row[0].ToString();
            string nomVen = row[1].ToString();

            Clientes cliente = new Clientes();
            cliente.Nven = nomVen;
            cliente.Cven = codVen;

            cliente.ShowDialog();

            cargarClientes();
        }

        private void desbloqueBTN(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {

            BTNCliAsi.IsEnabled = true;
        }

        private void BTNCliAsi_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row1 = (DataRowView)dataGridCxC.SelectedItems[0];
            string nom_mer = row1[1].ToString();

            try
            {
                string cadena = "";
                Boolean not = false;
                var reflector = this.dataGridClientes.View.GetPropertyAccessProvider();
                foreach (var row in this.dataGridClientes.SelectedItems)
                {
                    foreach (var column in dataGridClientes.Columns)
                    {
                        var cellvalue = reflector.GetValue(row, column.MappingName);

                        cadena = cadena + " update comae_ter set cod_ven = '' where cod_ter = '" + cellvalue + "' ";
                        not = true;
                        break;
                    }
                }

                if (not == true)
                {
                    SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                }
                MessageBox.Show("Clientes eliminados del vendedor : " + nom_mer);
                cargarClientes();
            }
            catch (Exception w)
            {

                MessageBox.Show("error" + w);
            }

        }

        private void BTNCliReasig_Click(object sender, RoutedEventArgs e) {
            DataRowView row1 = (DataRowView)dataGridCxC.SelectedItems[0];
            string cod_mer = row1["cod_mer"].ToString();
            string nom_mer = row1["nom_mer"].ToString();

            
            Reasignar windows_reasignar = new Reasignar();
            windows_reasignar.CodVendedor = cod_mer;
            windows_reasignar.NomVendedor = nom_mer;
            windows_reasignar.ShowDialog();

            cargarClientes();

        }

    



    }
}
