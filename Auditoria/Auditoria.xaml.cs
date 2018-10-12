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
//using SiasoftAppExt.SeguimientoCliente;

namespace SiasoftAppExt
{

    public partial class Auditoria : Window
    {


        dynamic SiaWin;
        int idemp = 0;
        string vendedor = "";
        int codigoUsuario = 0;
        string cnEmp = "";
        public string Conexion;

        public Auditoria()
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;
            vendedor = SiaWin._UserAlias;
            codigoUsuario = SiaWin._UserId;

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
                //tabitem.Logo(idLogo, ".png");
                //tabitem.Title = "Seguimiento de cliente(" + aliasemp + ")";
                TextBx_fecha_nac.Text = DateTime.Now.ToString();
                TextBx_horaIni.Text = DateTime.Now.ToString("HH:mm:ss");
                TextBx_horaFin.Text = DateTime.Now.ToString("HH:mm:ss");
                //fecha de tab 2
                fecha_ini.Text = DateTime.Now.ToString();
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
                    if (tag == "CrMae_campa")
                    {
                        cmptabla = tag; cmpcodigo = "cod_camp"; cmpnombre = "UPPER(nom_camp)"; cmporden = "cod_camp"; cmpidrow = "cod_camp"; cmptitulo = "Maestra de Campaña"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "";
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
                        if (tag == "CrMae_campa")
                        {
                            LB_cam.Text = code; TextBx_cam.Text = nom;
                        }
                        if (tag == "CrMae_concepto1") {
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

        private void ValidacionNumeros(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.OemMinus || e.Key == Key.Subtract || e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right)
            {
                e.Handled = false;
            }
            else
            {
                MessageBox.Show("este campo solo admite valores numericos");
                e.Handled = true;
            }

            if (TextBx_horaFin.Text.Length > 8)
            {
                MessageBox.Show("ingreso mas valores de la cuenta");
                e.Handled = true;
            }
        }

        public string convertirEstado(string estado) {

            string valor = "";
            switch (estado)
            {
                case "Pendiente":
                    valor = "0";
                    break;
                case "Activo":
                    valor = "1";
                    break;
                case "Desactivo":
                    valor = "2";
                    break;
                default:
                    valor = "0";
                    break;
            }
            return valor;
        }

        public string convertirRecordar(string reco)
        {

            string valor = "";
            switch (reco)
            {
                case "No":
                    valor = "0";
                    break;
                case "Si":
                    valor = "1";
                    break;
                default:
                    valor = "0";
                    break;
            }
            return valor;
        }
    
        private void regitrarSeguimiento(object sender, RoutedEventArgs e)
        {
            if (LB_con.Text.Length > 0 &&  TextBx_CodVen.Text.Length > 0 &&   LB_cam.Text.Length > 0 && TextBx_fecha_nac.Text.Length > 0 &&  TextBx_horaIni.Text.Length > 0 && TextBx_horaFin.Text.Length > 0 && TextBx_obse.Text.Length > 0  &&  LB_ActSig.Text.Length > 0 && TextBx_contac.Text.Length > 0 && TextBxCB_est.Text.Length > 0 && TextBxCB_Rec.Text.Length > 0 )
            {
                try
                {
                    SiaWin.Func.SqlDT("insert into crseg_cli (fec_seg,cod_ter,cod_mer,cod_con,cod_camp,fec_comp,hora_ini,hora_fin,observ,cod_consig,contacto,estado,recordar) values ('" + DateTime.Now.ToString() + "', '" + TextBx_codigo.Text.Trim() + "', '" + TextBx_CodVen.Text.Trim() + "', '" + LB_con.Text.Trim() + "', '" + LB_cam.Text.Trim() + "','" + TextBx_fecha_nac.Text.Trim() + "', '" + TextBx_horaIni.Text.Trim() + "', '" + TextBx_horaFin.Text.Trim() + "', '" + TextBx_obse.Text.Trim() + "', '" + LB_ActSig.Text.Trim() + "', '" + TextBx_contac.Text.Trim() + "', '" + convertirEstado(TextBxCB_est.Text) + "', '" + convertirRecordar(TextBxCB_Rec.Text) + "' )", "Clientes", idemp);
                    MessageBox.Show("Seguimiento de Cliente Exitoso");
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
            //this.Close();
            //SeguimientoCliente cliente = new SeguimientoCliente();

        }
       

        //segundo tab *********************************************************************************

        private void cargarGrid(object sender, RoutedEventArgs e) {

            string fe_fin = fecha_fin.Text + " 23:59:59";

            try
            {
                string queryGrid = "select rtrim(Seguimi.fec_seg) as fec_seg,rtrim(Seguimi.cod_ter) as cod_ter,rtrim(clientes.nom_ter) as nom_ter, rtrim(Seguimi.cod_mer) as cod_mer,rtrim(Vendedores.nom_mer) as nom_mer,rtrim(Seguimi.cod_con) as cod_con,rtrim(Concepto.nom_con) as nom_con,rtrim(Seguimi.cod_camp) as cod_camp,rtrim(Campa.nom_camp) as nom_camp,rtrim(Seguimi.fec_comp) as fec_comp,rtrim(Seguimi.hora_ini) as hora_ini, rtrim(Seguimi.hora_fin) as hora_fin,rtrim(Seguimi.cod_consig) as cod_consig,rtrim(Seguimi.contacto) as contacto,rtrim(Seguimi.estado) as estado,rtrim(Seguimi.recordar) as recordar ";
                queryGrid = queryGrid + "from crseg_cli as Seguimi ";
                queryGrid = queryGrid + "full join COMAE_TER as Clientes on Clientes.cod_ter = Seguimi.cod_ter ";
                queryGrid = queryGrid + "full join InMae_mer as Vendedores on vendedores.cod_mer = Seguimi.cod_mer  ";
                queryGrid = queryGrid + "full join CrMae_concepto as Concepto on Concepto.cod_con = Seguimi.cod_con ";
                queryGrid = queryGrid + "full join CrMae_campa as Campa on Campa.cod_camp = Seguimi.cod_camp ";
                queryGrid = queryGrid + "where Clientes.cod_ter = Seguimi.cod_ter ";
                queryGrid = queryGrid + "and Seguimi.cod_con = Concepto.cod_con  ";
                queryGrid = queryGrid + "and fec_seg between '" + fecha_ini + "' and '" + fe_fin + "' ";
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

       
    }
}
