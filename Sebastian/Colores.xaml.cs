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

namespace Sebastian
{

    public partial class Colores : UserControl
    {
        public string Name
        {
            get;
            set;
        }
        public int Age
        {
            get;
            set;
        }
        #region IDataErrorInfo Members
        public string Error
        {
            get { return this[String.Empty]; }
        }
        public string this[string columnName]
        {
            get
            {
                string result = String.Empty;
                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(this.Name))
                    {
                        result = "Name is mandatory";
                    }
                }
                if (columnName == "Age")
                {
                    if (this.Age < 1 || this.Age > 100)
                    {
                        result = "Invalid Age";
                    }
                }
                return result;
            }
        }
        #endregion
        public class ViewModel
        {
            public ObservableCollection<Colores> Coloress { get; set; }
            public ViewModel()
            {
                Coloress = new ObservableCollection<Colores>();
                PopulateData();
            }
            private void PopulateData()
            {
                Coloress.Add(new Colores() { Name = "John", Age = 26 });
                Coloress.Add(new Colores() { Name = "Mark", Age = 25 });
                Coloress.Add(new Colores() { Name = "Steven", Age = 26 });
            }
        }
        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string cnEmp = "";
        public Colores()
        {
            InitializeComponent();
        }
        private DataSet LoadData()
        {
            try
            {
                SqlConnection con = new SqlConnection(cnEmp);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                cmd = new SqlCommand("UxPaleta", con);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                foreach (DataTable table in ds.Tables)
                {
                    //            newColumn.DefaultValue = "Your DropDownList value";
                    System.Data.DataColumn newColumn = new System.Data.DataColumn("ven_net", typeof(System.Double));
                    System.Data.DataColumn newColumn1 = new System.Data.DataColumn("util", typeof(System.Double));
                    System.Data.DataColumn newColumn2 = new System.Data.DataColumn("por_util", typeof(System.Double));
                    System.Data.DataColumn newColumn3 = new System.Data.DataColumn("por_parti", typeof(System.Double));
                    System.Data.DataColumn newColumn4 = new System.Data.DataColumn("can_net", typeof(System.Double));
                    ds.Tables[table.TableName].Columns.Add(newColumn);
                    ds.Tables[table.TableName].Columns.Add(newColumn1);
                    ds.Tables[table.TableName].Columns.Add(newColumn2);
                    ds.Tables[table.TableName].Columns.Add(newColumn3);
                    ds.Tables[table.TableName].Columns.Add(newColumn4);
                }
                return ds;
                //VentasPorProducto.ItemsSource = ds.Tables[0];
                //VentaPorBodega.ItemsSource = ds.Tables[1];
                //VentasPorCliente.ItemsSource = ds.Tables[2];
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }


    }
}
