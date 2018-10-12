using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;
using System.Threading;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.XlsIO;
using Syncfusion.UI.Xaml.Grid.Converter;
using Microsoft.Win32;
using System.IO;
using System.Data;

namespace SiasoftAppExt
{
    public partial class AnalisisPV : Window
    {
        dynamic SiaWin;
        int idemp = 0;
        string cnEmp = "";
        public string cod_clie;
        public string nom_clie;

        public AnalisisPV()
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            idemp = SiaWin._BusinessId;

            this.MaxHeight = 600;
            this.MinHeight = 600;
            this.MinWidth = 1200;
            this.MaxWidth = 1200;


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

                FecIni.Text = DateTime.Today.AddYears(-1).ToString();
                FecFin.Text = DateTime.Now.ToShortDateString();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxTerI.Text = nom_clie;
            TextBoxTerCod.Text = cod_clie;
        }


        private void ButtonRefresh_Click(object sender, RoutedEventArgs e) {

            try
            {
                SqlConnection con = new SqlConnection(cnEmp);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                cmd = new SqlCommand("SpConsultaInAnalisisDeVentasPvCliente", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaIni", FecIni.Text);
                cmd.Parameters.AddWithValue("@FechaFin", FecFin.Text);
                cmd.Parameters.AddWithValue("@codter", cod_clie);
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                VentasPorProducto.ItemsSource = ds.Tables[0];

                TextSubtotal.Text = "0";
                TextDescuento.Text = "0";
                TextIva.Text = "0";
                TextTotal.Text = "0";

                double sub = Convert.ToDouble(ds.Tables[0].Compute("Sum(subtotal)", "").ToString());
                double descto = Convert.ToDouble(ds.Tables[0].Compute("Sum(val_des)", "").ToString());
                double iva = Convert.ToDouble(ds.Tables[0].Compute("Sum(val_iva)", "").ToString());
                double total = Convert.ToDouble(ds.Tables[0].Compute("Sum(tot_tot)", "").ToString());

                TextSubtotal.Text = sub.ToString("C");
                TextDescuento.Text = descto.ToString("C");
                TextIva.Text = iva.ToString("C");
                TextTotal.Text = total.ToString("C");


            }
            catch (Exception ex)
            {
                MessageBox.Show("error3:" + ex.Message,"ButtonRefresh");
                
            }
        }

       

        private void ExportaXLS_Click(object sender, RoutedEventArgs e)
        {
            
            var options = new Syncfusion.UI.Xaml.Grid.Converter.ExcelExportingOptions();
            options.ExcelVersion = ExcelVersion.Excel2013;
            var excelEngine = VentasPorProducto.ExportToExcel(VentasPorProducto.View, options);
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
