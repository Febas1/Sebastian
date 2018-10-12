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

namespace SiasoftAppExt
{

    public partial class InformeCampa : UserControl
    {
        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string vendedor = "";
        int codigoUsuario = 0;
        string cnEmp = "";
        public string Conexion;
        string codigoVendedor;
        string tipoUsuario;
        


        public InformeCampa(dynamic tabitem1)
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
            idemp = SiaWin._BusinessId;
            vendedor = SiaWin._UserAlias;
            codigoUsuario = SiaWin._UserId;
            codigoVendedor = SiaWin._UserTag1;
            tipoUsuario = SiaWin._UserTag2;

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
                tabitem.Title = "Informe Campaña (" + aliasemp + ")";

                //TextBx_fecha_ini.Text = DateTime.Today.AddYears(-1).ToString();
                //TextBx_fecha_fin.Text = DateTime.Now.ToShortDateString();
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

                    if (tag == "CrMae_campa")
                    {
                        cmptabla = tag; cmpcodigo = "cod_camp"; cmpnombre = "UPPER(nom_camp)"; cmporden = "cod_camp"; cmpidrow = "cod_camp"; cmptitulo = "Maestra de Campaña"; cmpconexion = cnEmp; mostrartodo = true; cmpwhere = "estado='1'";
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
                        if (tag == "CrMae_campa")
                        {
                            LB_cod_cam.Text = code; TBX_name_cam.Text = nom;
                            BTNejec.IsEnabled = true;                            
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


        private void CargarGrid(object sender, RoutedEventArgs e) {

            if (LB_cod_cam.Text.Length > 0)
            {
                try
                {

                    string cadena = "select count(distinct case when concepto.cod_con = '02' then concepto.nom_con end) as llamada_por,cliente.cod_ter,cliente.nom_ter as nom_ter,cliente.tel1,cliente.email,sum( iif(cabeza.cod_trn between '004' and '005',CAST(cuerpo.cantidad as int),CAST(-cuerpo.cantidad as int) ) ) as cantidad, sum((cantidad*val_uni)*iif(trn.tip_trn=1,-1,1)) as monto, vendedor.nom_mer as nom_mer,max(iif(cabeza.cod_trn='005',fec_trn,'')) as ultfecha, max(iif(cabeza.cod_trn='005',cuerpo.cod_bod,'')) as bodega,max(bod.nom_bod) as nom_bod ";
                    cadena = cadena + "from InCab_doc as cabeza ";
                    cadena = cadena + "full join InCue_doc as cuerpo on cuerpo.idregcab = cabeza.idreg ";
                    cadena = cadena + "full join inmae_bod as bod on bod.cod_bod=cuerpo.cod_bod ";
                    cadena = cadena + "full join InMae_ref as referencia on referencia.cod_ref = cuerpo.cod_ref ";
                    cadena = cadena + "full join InMae_tip as linea on linea.cod_tip = referencia.cod_tip ";
                    cadena = cadena + "full join comae_ter as cliente on cliente.cod_ter = cabeza.cod_cli ";
                    cadena = cadena + "full join InMae_mer as vendedor on vendedor.cod_mer = cliente.cod_ven ";
                    cadena = cadena + "full join CrMae_cli as cliCamp on cliCamp.cod_ter = cliente.cod_ter  ";
                    cadena = cadena + "full join crTemCampa as temporal on temporal.cod_ter = cliente.cod_ter  ";
                    cadena = cadena + "full join crmae_campa as campa on campa.cod_camp = temporal.cod_camp  ";
                    cadena = cadena + "full join crseg_cli as seguimineto on seguimineto.cod_ter = cliente.cod_ter  ";
                    cadena = cadena + "full join crmae_concepto as concepto on concepto.cod_con = seguimineto.cod_con  and concepto.cod_con = '02' ";
                    cadena = cadena + "inner join InMae_trn trn on trn.cod_trn=cabeza.cod_trn and trn.ind_vtas=1 and trn.Tip_trn between 1 and 2 ";                    
                    cadena = cadena + "where cabeza.cod_trn between '004' and '008' and cliente.clasific='1' ";
                    cadena = cadena + "and campa.cod_camp='" + LB_cod_cam.Text + "' ";
                    cadena = cadena + "group by cliente.nom_ter,cliente.tel1,cliente.email,vendedor.nom_mer,cliente.cod_ter,campa.cod_camp,campa.nom_camp,campa.cod_camp ";
                    cadena = cadena + "order by cliente.nom_ter ";
                    
                    
                    DataTable dt = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                    dataGridCxC.ItemsSource = dt.DefaultView;

                    if (dt.Rows.Count <= 0) { MessageBox.Show("No existe clinetes registrados en esta campaña"); }
                    TotalReg.Text = dt.Rows.Count.ToString();

                    BTNexpor.IsEnabled = true;
                }
                catch (Exception w)
                {
                    MessageBox.Show("error:" + w);
                }
            }
            else {
                MessageBox.Show("llene los campos correspondientes para ejecutar el informe");
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
