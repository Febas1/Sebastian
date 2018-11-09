using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using Syncfusion.Windows.Tools.Controls;

namespace Sebastian
{
    public partial class Colores : UserControl
    {
        public Colores()
        {
            InitializeComponent();
        }
    //    private DataSet LoadData()
    //    {
    //        try
    //        {
    //            SqlConnection con = new SqlConnection(cnEmp);
    //            SqlCommand cmd = new SqlCommand();
    //            SqlDataAdapter da = new SqlDataAdapter();
    //            DataSet ds = new DataSet();
    //            cmd = new SqlCommand("UxPaleta", con);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            da = new SqlDataAdapter(cmd);
    //            da.Fill(ds);
    //            con.Close();
    //            foreach (DataTable table in ds.Tables)
    //            {
    //                //            newColumn.DefaultValue = "Your DropDownList value";
    //                System.Data.DataColumn newColumn = new System.Data.DataColumn("ven_net", typeof(System.Double));
    //                System.Data.DataColumn newColumn1 = new System.Data.DataColumn("util", typeof(System.Double));
    //                System.Data.DataColumn newColumn2 = new System.Data.DataColumn("por_util", typeof(System.Double));
    //                System.Data.DataColumn newColumn3 = new System.Data.DataColumn("por_parti", typeof(System.Double));
    //                System.Data.DataColumn newColumn4 = new System.Data.DataColumn("can_net", typeof(System.Double));
    //                ds.Tables[table.TableName].Columns.Add(newColumn);
    //                ds.Tables[table.TableName].Columns.Add(newColumn1);
    //                ds.Tables[table.TableName].Columns.Add(newColumn2);
    //                ds.Tables[table.TableName].Columns.Add(newColumn3);
    //                ds.Tables[table.TableName].Columns.Add(newColumn4);
    //            }
    //            return ds;
    //            //VentasPorProducto.ItemsSource = ds.Tables[0];
    //            //VentaPorBodega.ItemsSource = ds.Tables[1];
    //            //VentasPorCliente.ItemsSource = ds.Tables[2];
    //        }
    //        catch (Exception e)
    //        {
    //            MessageBox.Show(e.Message);
    //            return null;
    //        }
    //    }
    }

    public class Couler
    {
        public string Control
        {
            get;
            set;
        }
        public string Color
        {
            get;
            set;
        }
        public string Grupo
        {
            get;
            set;
        }
    }
    public class VistaModelo
    {
        public ObservableCollection<Couler> Coloress { get; set; }
        public VistaModelo()
        {
            Coloress = new ObservableCollection<Couler>();
            PopulateData();
        }
        private void PopulateData()
        {
            Coloress.Add(new Couler() { Control = "Button", Color = "#34495e", Grupo = "Flexo" });
            Coloress.Add(new Couler() { Control = "Button", Color = "#1abc9c", Grupo = "Leco" });
            Coloress.Add(new Couler() { Control = "Grid", Color = "#2980b9", Grupo = "Leco" });
            Coloress.Add(new Couler() { Control = "Grid", Color = "#00adb5", Grupo = "Pre" });
            Coloress.Add(new Couler() { Control = "Grid", Color = "#222831", Grupo = "Pre" });
            Coloress.Add(new Couler() { Control = "Grid", Color = "#393e46", Grupo = "Pre" });
            Coloress.Add(new Couler() { Control = "Grid", Color = "#eeeeee", Grupo = "Pre" });
        }
    }
}