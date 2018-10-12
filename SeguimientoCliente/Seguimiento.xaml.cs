using Microsoft.Win32;
using Syncfusion.UI.Xaml.Grid.Converter;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using SeguimientoCliente;
using System.Data.SqlClient;

namespace SeguimientoCliente
{

    public partial class Seguimiento : Window
    {
        dynamic SiaWin;
        int idemp = 0;
        string vendedor = "";
        int codigoUsuario = 0;
        string cnEmp = "";
        string codigoVendedor;
        string tipoUsuario;
        public string Conexion;

        public string cod_ter = "";
        public string nom_comple = "";
        public string tel1 = "";
        public string tel2 = "";
        public string cel = "";
        public string email = "";
        public string dir = "";
        public string cod_mer = "";

        public string ct_email = "";
        public string ct_correspondencia = "";
        public string ct_whats = "";
        public string ct_sms = "";
        public string ct_celular = "";




        public Seguimiento()
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;
            vendedor = SiaWin._UserAlias;
            codigoUsuario = SiaWin._UserId;

            codigoVendedor = SiaWin._UserTag1;
            tipoUsuario = SiaWin._UserTag2;

            this.MinWidth = 1200;
            this.MinHeight = 550;
            this.MaxWidth = 1200;
            this.MaxHeight = 550;

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


                TextBx_obse.Text = "NINGUNA";
                fecha_ini.Text = DateTime.Now.AddYears(-1).ToString();
                fecha_fin.Text = DateTime.Now.ToString();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }



        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //validacion para que se ingrese fijo el campo de una maestra
                string idTab = ((TextBox)sender).Tag.ToString();
                if (idTab.Length > 0)
                {
                    string tag = ((TextBox)sender).Tag.ToString();
                    string cmptabla = ""; string cmpcodigo = ""; string cmpnombre = ""; string cmporden = ""; string cmpidrow = ""; string cmptitulo = ""; string cmpconexion = ""; bool mostrartodo = true; string cmpwhere = "";
                    if (string.IsNullOrEmpty(tag)) return;

                    if (tag == "CrMae_concepto")
                    {
                        cmptabla = tag; cmpcodigo = "cod_con"; cmpnombre = "UPPER(nom_con)"; cmporden = "cod_con"; cmpidrow = "cod_con"; cmptitulo = "Maestra de Conceptos"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }
                    if (tag == "CrMae_concepto1")
                    {
                        cmptabla = "CrMae_concepto"; cmpcodigo = "cod_con"; cmpnombre = "UPPER(nom_con)"; cmporden = "cod_con"; cmpidrow = "cod_con"; cmptitulo = "Maestra de Conceptos"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
                    }

                    int idr = 0; string code = ""; string nom = "";
                    dynamic winb = SiaWin.WindowBuscar(cmptabla, cmpcodigo, cmpnombre, cmporden, cmpidrow, cmptitulo, cnEmp, mostrartodo, cmpwhere);

                    winb.ShowInTaskbar = false;
                    winb.Owner = Application.Current.MainWindow;
                    winb.ShowDialog();
                    idr = winb.IdRowReturn;
                    code = winb.Codigo;
                    nom = winb.Nombre;
                    winb = null;
                    if (idr > 0)
                    {
                        if (tag == "CrMae_concepto")
                        {
                            LB_con.Text = code; TextBx_con.Text = nom;
                        }
                        if (tag == "CrMae_concepto1")
                        {
                            LB_ActSig.Text = code; TextBx_ActSig.Text = nom;
                        }

                        var uiElement = e.OriginalSource as UIElement;
                        uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                    }
                    e.Handled = true;
                }
                if (e.Key == Key.Enter)
                {
                    var uiElement = e.OriginalSource as UIElement;
                    uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }


        private void regitrarSeguimiento(object sender, RoutedEventArgs e)
        {


            var selectedTag = ((ComboBoxItem)TextBxCB_camp.SelectedItem).Tag.ToString();

            if (LB_con.Text.Length > 0 && TextBx_CodVen.Text.Length > 0 && selectedTag.Length > 0 && LB_ActSig.Text.Length > 0 && TextBx_contac.Text.Length > 0 && TextBx_obse.Text.Length > 0 && TextBx_obse.Text.Length < 150)
            {
                try
                {
                    string cadena = "insert into crseg_cli (fec_seg, cod_ter, cod_mer, cod_con, cod_camp, cod_consig, contacto_cli, observ) values ( '" + DateTime.Now.ToString() + "', '" + TextBx_codigo.Text.Trim() + "', '" + TextBx_CodVen.Text.Trim() + "', '" + LB_con.Text.Trim() + "', '" + selectedTag.ToString() + "', '" + LB_ActSig.Text.Trim() + "', '" + TextBx_contac.Text + "', '" + TextBx_obse.Text + "') ";
                    //MessageBox.Show("XCAA:"+cadena);
                    SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                    MessageBox.Show("Seguimiento de Cliente Exitoso");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                MessageBox.Show("Faltan algunos campos por llenar");
            }

        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        //segundo tab *********************************************************************************

        private void cargarGrid(object sender, RoutedEventArgs e)
        {

            string fe_fin = fecha_fin.Text + " 23:59:59";

            try
            {
                string queryGrid = "select rtrim(Seguimi.fec_seg) as fec_seg,rtrim(Seguimi.cod_ter) as cod_ter,rtrim(clientes.nom_ter) as nom_ter, rtrim(Seguimi.cod_mer) as cod_mer,rtrim(Vendedores.nom_mer) as nom_mer,rtrim(Seguimi.cod_con) as cod_con,rtrim(Concepto.nom_con) as nom_con,rtrim(Seguimi.cod_camp) as cod_camp,rtrim(Campa.nom_camp) as nom_camp,rtrim(Seguimi.cod_consig) as cod_consig, rtrim(Concepto1.nom_con) as nom_con1,rtrim(Seguimi.contacto_cli) as contacto_cli ";
                queryGrid = queryGrid + "from crseg_cli as Seguimi ";
                queryGrid = queryGrid + "full join COMAE_TER as Clientes on Clientes.cod_ter = Seguimi.cod_ter ";
                queryGrid = queryGrid + "full join InMae_mer as Vendedores on vendedores.cod_mer = Seguimi.cod_mer  ";
                queryGrid = queryGrid + "full join CrMae_concepto as Concepto on Concepto.cod_con = Seguimi.cod_con ";
                queryGrid = queryGrid + "full join CrMae_concepto as Concepto1 on Concepto1.cod_con = Seguimi.cod_consig ";
                queryGrid = queryGrid + "full join CrMae_campa as Campa on Campa.cod_camp = Seguimi.cod_camp ";
                queryGrid = queryGrid + "where Clientes.cod_ter = Seguimi.cod_ter and Clientes.cod_ter ='" + TextBx_codigo.Text + "' ";
                queryGrid = queryGrid + "and Seguimi.cod_con = Concepto.cod_con  ";

                if (tipoUsuario == "3" || tipoUsuario == "4")
                {
                    queryGrid = queryGrid + "and Seguimi.cod_mer = '" + codigoVendedor + "' ";
                }

                queryGrid = queryGrid + "and fec_seg between '" + fecha_ini.Text + "' and '" + fe_fin + "' ";
                queryGrid = queryGrid + "order by convert(datetime, fec_seg, 103) desc ";

                DataTable dt = SiaWin.Func.SqlDT(queryGrid, "Clientes", idemp);
                dataGridCxC.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ExportaXLS_Click(object sender, RoutedEventArgs e)
        {

            var options = new Syncfusion.UI.Xaml.Grid.Converter.ExcelExportingOptions();
            options.ExcelVersion = ExcelVersion.Excel2013;
            var excelEngine = dataGridCxC.ExportToExcel(dataGridCxC.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];

            SaveFileDialog sfd = new SaveFileDialog
            {
                FilterIndex = 2,
                Filter = "Excel 97 to 2003 Files(*.xls)|*.xls|Excel 2007 to 2010 Files(*.xlsx)|*.xlsx|Excel 2013 File(*.xlsx)|*.xlsx"
            };

            if (sfd.ShowDialog() == true)
            {
                using (Stream stream = sfd.OpenFile())
                {
                    if (sfd.FilterIndex == 1)
                        workBook.Version = ExcelVersion.Excel97to2003;
                    else if (sfd.FilterIndex == 2)
                        workBook.Version = ExcelVersion.Excel2010;
                    else
                        workBook.Version = ExcelVersion.Excel2013;
                    workBook.SaveAs(stream);
                }

                //Message box confirmation to view the created workbook.
                if (MessageBox.Show("Usted quiere abrir el archivo en excel?", "Ver archvo",
                                    MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    //Launching the Excel file using the default Application.[MS Excel Or Free ExcelViewer]
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
            }


        }


        public void contactos_color() {
            if (TextBx_ct_email.Text == "SI") { TextBx_ct_email.Foreground = Brushes.Green; } else { TextBx_ct_email.Foreground = Brushes.Red; }
            if (TextBx_ct_corres.Text == "SI") { TextBx_ct_corres.Foreground = Brushes.Green; } else { TextBx_ct_corres.Foreground = Brushes.Red; }
            if (TextBx_ct_whats.Text == "SI") { TextBx_ct_whats.Foreground = Brushes.Green; } else { TextBx_ct_whats.Foreground = Brushes.Red; }
            if (TextBx_ct_sms.Text == "SI") { TextBx_ct_sms.Foreground = Brushes.Green; } else { TextBx_ct_sms.Foreground = Brushes.Red; }
            if (TextBx_ct_cel.Text == "SI") { TextBx_ct_cel.Foreground = Brushes.Green; } else { TextBx_ct_cel.Foreground = Brushes.Red; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBx_codigo.Text = cod_ter;
            TextBx_NomCom.Text = nom_comple;
            TextBx_Dir.Text = dir;
            TextBx_tel1.Text = tel1;
            TextBx_tel2.Text = tel2;
            TextBx_cel.Text = cel;
            TextBx_email.Text = email;
            TextBx_CodVen.Text = cod_mer;

            TextBx_ct_email.Text = ct_email;
            TextBx_ct_corres.Text = ct_correspondencia;
            TextBx_ct_whats.Text = ct_whats;
            TextBx_ct_sms.Text = ct_sms;
            TextBx_ct_cel.Text = ct_celular;

            contactos_color();
            // traer campañas del cliente
            string cadena = "select temporal.cod_camp as cod_camp,campa.nom_camp as nom_camp from CrTemCampa as temporal ";
            cadena = cadena + "inner join CrMae_campa as campa on campa.cod_camp = temporal.cod_camp ";
            cadena = cadena + "where temporal.cod_ter = '" + cod_ter + "' ";
            cadena = cadena + "group by temporal.cod_camp,campa.nom_camp ";
            
            SqlDataReader drCli = SiaWin.Func.SqlDR(cadena, idemp); ;
            while (drCli.Read())
            {
                var newItem = new ComboBoxItem();
                newItem.Content = drCli["nom_camp"].ToString().Trim();
                newItem.Tag = drCli["cod_camp"].ToString().Trim();
                TextBxCB_camp.Items.Add(newItem);                
            }
                      
        }

        



    }
}
