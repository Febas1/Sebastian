using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using SeguimientoCliente;
using Syncfusion.SfSkinManager;
using Syncfusion.XlsIO;
using Syncfusion.UI.Xaml.Grid.Converter;
using Microsoft.Win32;

namespace SiasoftAppExt
{

    public partial class SeguimientoCliente : UserControl
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
        DataTable dt = new DataTable();

        
        public byte[] _img_cli;


        public SeguimientoCliente(dynamic tabitem1)
        {
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
            idemp = SiaWin._BusinessId;
            vendedor = SiaWin._UserAlias;
            codigoUsuario = SiaWin._UserId;
            codigoVendedor = SiaWin._UserTag1;
            tipoUsuario = SiaWin._UserTag2;
            //SiaWin.Func.funcion();


            //carga los datos de los vendedores
            DatosUsuario();
            //carga datos de la maestra de terceros a la maestra de clientes
            cargarClientesMaestra();
            //cargar variales principales
            LoadConfig();
                       
            //tab 2
            llenarGridCamp();
            
            SfSkinManager.SetVisualStyle(dataGridCxC, VisualStyles.Metro);
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
                tabitem.Title = "Seguimiento de cliente(" + aliasemp + ")";
                //TxtUser.Text = SiaWin._UserAlias;                

                //trae los datos del cliente
                if (tipoUsuario == "3" || tipoUsuario == "4")
                {
                    RefeshDataGrid(1);
                }                
                

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        

        public void DatosUsuario() {
            TxtUser.Text = SiaWin._UserTag1;
            SqlDataReader drCli = SiaWin.Func.SqlDR("select * from InMae_mer where cod_mer='" + SiaWin._UserTag1 + "' ", idemp); ;

            while (drCli.Read()) {
                TxtUserName.Text = drCli["nom_mer"].ToString().Trim();
            }

            drCli.Close();
            if (tipoUsuario == "1" || tipoUsuario == "2")
            {
                TxtTipUser.Text = "Administrador";
                panelBuscar.Visibility = Visibility.Visible;
                
            }
            else {
                TxtTipUser.Text = "Vendedor";
                panelBuscar.Visibility = Visibility.Hidden;

            }

        }

        public void activarControles(){
            BtnEditar.IsEnabled = true;
            BtnSegCli.IsEnabled = true;
            BtnHisCom.IsEnabled = true;
            BtnSegComp.IsEnabled = true;
            BtnExpCli.IsEnabled = true;
        }

        //poner valores cada ves que se para sobre el usuario
        private void FirstDetailsViewGrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            activarControles();
            try
            {
                DataRowView row = (DataRowView)dataGridCxC.SelectedItems[0];

                TBcliente.Text = row["nom_ter"].ToString();
                TBcodcliente.Text = row["cod_ter"].ToString();

                TBtalla1.Text = row["nom_talla1"].ToString();
                TBtalla2.Text = row["nom_talla2"].ToString();
                TBtalla3.Text = row["nom_talla3"].ToString();
                TBtalla4.Text = row["nom_talla4"].ToString();
                TBtalla5.Text = row["nom_talla5"].ToString();

                image1.Visibility = Visibility.Visible;
                byte[] blob = (byte[])row["img_cli"];
                MemoryStream stream = new MemoryStream();
                stream.Write(blob, 0, blob.Length);
                stream.Position = 0;
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = ms;
                bi.EndInit();
                image1.Source = bi;


                
            }
            catch (Exception)
            {
                //MessageBox.Show("error imagen:" + w);
                image1.Visibility = Visibility.Hidden;
            }

        }

        // query ------------------------------------------------------------

        public void cargarClientesMaestra()
        {

            try
            {
                SiaWin.Func.SqlDT("INSERT INTO CrMae_cli (cod_ter) SELECT cod_ter FROM COMAE_TER WHERE COMAE_TER.clasific = 1 and NOT EXISTS(Select cod_ter From CrMae_cli WHERE CrMae_cli.cod_ter = COMAE_TER.cod_ter)", "Clientes", idemp);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public void RefeshDataGrid(int tipo) {
            try
            {
                string queryGrid = "SELECT	rtrim(TER.cod_ter) as cod_ter, rtrim(TER.tdoc) as tdoc, rtrim(UPPER(IDENTIFICACION.nom_tdoc)) as nom_tdoc, rtrim(TER.nom_ter) as nom_ter,rtrim(UPPER(TER.nom1)) as nom1,rtrim(UPPER(TER.nom2)) as nom2,rtrim(UPPER(TER.apell1)) as apell1,rtrim(UPPER(TER.apell2)) as apell2,rtrim(TER.tel1) as tel1,rtrim(TER.tel2) as tel2,rtrim(TER.cel) as cel,rtrim(UPPER(TER.email)) as email,rtrim(UPPER(TER.dir1)) as dir1,rtrim(UPPER(TER.dir)) as dir,rtrim(UPPER(TER.dir2)) as dir2,rtrim(TER.cod_ciu) as cod_ciu, rtrim(UPPER(MUNICIPIO.nom_muni)) as nom_muni,rtrim(TER.cod_depa) as cod_depa, rtrim(UPPER(DEPARTAMENTO.nom_dep)) as nom_dep,CONVERT(varchar,fec_cump,103) as fec_cump, (cast(datediff(dd,TER.fec_cump,GETDATE()) / 365.25 as int)) as edad, ";
                queryGrid = queryGrid + "rtrim(CLIE.genero) as genero,rtrim(CLIE.est_civil) as est_civil,rtrim(UPPER(CLIE.nom_emp)) as nom_emp, rtrim(UPPER(CLIE.act_emp)) as act_emp,rtrim(UPPER(ACTIVIDAD.nom_actEmp)) as nom_actEmp, case when CLIE.ct_cel='1' then 'SI' else 'NO' end as ct_cel , case when CLIE.ct_email='1' then 'SI' else 'NO' end as ct_email , case when CLIE.ct_whats='1' then 'SI' else 'NO' end as ct_whats , case when CLIE.ct_sms='1' then 'SI' else 'NO' end as ct_sms , case when CLIE.ct_corres='1' then 'SI' else 'NO' end as ct_corres , rtrim(UPPER(CARGO.cod_cargo)) as cod_cargo,rtrim(UPPER(CARGO.nom_cargo)) as nom_cargo, rtrim(UPPER(OCUPACION.cod_ocup)) as cod_ocup,rtrim(UPPER(OCUPACION.nom_ocup)) as nom_ocup, rtrim(UPPER(PROFESION.cod_prof)) as  cod_prof, rtrim(UPPER(PROFESION.nom_prof)) as nom_prof, rtrim(CLIE.num_doc) as num_doc, rtrim(UPPER(TER.observ)) as observ, rtrim(UPPER(CLIE.hobbies)) as hobbies, rtrim(UPPER(CLIE.image_name)) as image_name, CLIE.img_cli as img_cli, rtrim(CLIE.ran_edad) as ran_edad,rtrim(CLIE.talla_zap_tenn) as talla_zap_tenn, rtrim(TALLA1.nom_talla) as nom_talla1,rtrim(CLIE.talla_pant_fald) as talla_pant_fald, rtrim(TALLA2.nom_talla) as nom_talla2,rtrim(CLIE.talla_vest_traj) as talla_vest_traj, rtrim(TALLA3.nom_talla) as nom_talla3,rtrim(CLIE.talla_camisa) as talla_camisa ,rtrim(TALLA4.nom_talla) as nom_talla4,rtrim(CLIE.talla_camisa_sport) as talla_camisa_sport,rtrim(TALLA5.nom_talla) as nom_talla5, rtrim(VENDEDOR.nom_mer) as nom_mer ";
                queryGrid = queryGrid + "FROM CrMae_cli as CLIE ";
                queryGrid = queryGrid + "full join CrMae_cargo as CARGO on CLIE.cod_cargo = CARGO.cod_cargo ";
                queryGrid = queryGrid + "full join CrMae_ocupacion as OCUPACION on CLIE.cod_ocup = OCUPACION.cod_ocup ";
                queryGrid = queryGrid + "full join CrMae_profesion as PROFESION on CLIE.cod_prof = PROFESION.cod_prof ";
                queryGrid = queryGrid + "full join CrMae_talla as TALLA1 on CLIE.talla_zap_tenn = TALLA1.cod_talla ";
                queryGrid = queryGrid + "full join CrMae_talla as TALLA2 on CLIE.talla_pant_fald = TALLA2.cod_talla ";
                queryGrid = queryGrid + "full join CrMae_talla as TALLA3 on CLIE.talla_vest_traj = TALLA3.cod_talla ";
                queryGrid = queryGrid + "full join CrMae_talla as TALLA4 on CLIE.talla_camisa = TALLA4.cod_talla ";
                queryGrid = queryGrid + "full join CrMae_talla as TALLA5 on CLIE.talla_camisa_sport = TALLA5.cod_talla ";
                queryGrid = queryGrid + "full join CrMae_ActEmp as ACTIVIDAD  on ACTIVIDAD.cod_actEmp = CLIE.act_emp, ";
                queryGrid = queryGrid + "COMAE_TER as TER ";
                queryGrid = queryGrid + "full join MmMae_muni as MUNICIPIO on TER.cod_ciu = MUNICIPIO.cod_muni ";
                queryGrid = queryGrid + "full join MmMae_depa as DEPARTAMENTO on TER.cod_depa = DEPARTAMENTO.cod_dep ";
                queryGrid = queryGrid + "full join MmMae_iden as IDENTIFICACION on TER.tdoc = IDENTIFICACION.cod_tdoc ";
                queryGrid = queryGrid + "full join InMae_mer as VENDEDOR on  VENDEDOR.cod_mer = TER.cod_ven ";
                
                if (tipo == 0)
                {
                    queryGrid = queryGrid + "where TER.clasific = 1 and CLIE.cod_ter = TER.cod_ter and CLIE.cod_ter='" + LB_cliente.Text + "' ORDER BY cod_ter  ";
                }
                else {
                    queryGrid = queryGrid + "where TER.clasific = 1 and CLIE.cod_ter = TER.cod_ter and TER.cod_ven='" + codigoVendedor + "'  ORDER BY cod_ter ";
                }

                dataGridCxC.ItemsSource = null;
                dt = SiaWin.Func.SqlDT(queryGrid, "Clientes", idemp);
                dataGridCxC.ItemsSource = dt.DefaultView;
                TxtCantiCli.Text = dt.Rows.Count.ToString();
                
                //dataGridCxC.UpdateLayout();
            }
            catch (Exception w)
            {
                MessageBox.Show("eror al cargar BD:" + w);
            }
        }

        
        //convertir datos
        public string convertirCT(string _ct)
        {
            string a = "";
            if (_ct == "0")
            {
                a = "NO";
            }
            else
            {
                a = "SI";
            }
            return a;
        }

        public string convertirCTCodi(string _ct)
        {
            string valor = "";
            switch (_ct)
            {
                case "SI":
                    valor = "1";
                    break;
                case "NO":
                    valor = "0";
                    break;
                default:
                    valor = "N";
                    break;
            }
            return valor;
        }

        // abrir ventana para editar
        private void BtnEdit_Click(object sender, RoutedEventArgs e) {

            try
            {
                editarCliente editar = new editarCliente(tabitem);
                DataRowView row = (DataRowView)dataGridCxC.SelectedItems[0];
                editar._cod_ter = row["cod_ter"].ToString();
                editar._documentoLB = row["tdoc"].ToString();
                editar._documento = row["nom_tdoc"].ToString();
                editar._nom_comple = row["nom_ter"].ToString();
                editar._nom1 = row["nom1"].ToString();
                editar._nom2 = row["nom2"].ToString();
                editar._appe1 = row["apell1"].ToString();
                editar._appe2 = row["apell2"].ToString();
                editar._tel1 = row["tel1"].ToString();
                editar._tel2 = row["tel2"].ToString();
                editar._cel = row["cel"].ToString();
                editar._email = row["email"].ToString();
                editar._dir = row["dir"].ToString();
                editar._dir1 = row["dir1"].ToString();
                editar._dir2 = row["dir2"].ToString();
                editar._muniLB = row["cod_ciu"].ToString();
                editar._muni = row["nom_muni"].ToString();
                editar._depaLB = row["cod_depa"].ToString();
                editar._depa = row["nom_dep"].ToString();
                editar._fecha_nac = row["fec_cump"].ToString();
                editar._genero = row["genero"].ToString();
                editar._est_civil = row["est_civil"].ToString();
                editar._nom_emp = row["nom_emp"].ToString();
                editar._ct_celLB = row["act_emp"].ToString();
                editar._act_emp = row["nom_actEmp"].ToString();                
                editar._ct_cel = convertirCTCodi(row["ct_cel"].ToString());
                editar._ct_email = convertirCTCodi(row["ct_email"].ToString());
                editar._ct_whats = convertirCTCodi(row["ct_whats"].ToString());
                editar._ct_sms = convertirCTCodi(row["ct_sms"].ToString());
                editar._ct_corres = convertirCTCodi(row["ct_corres"].ToString());
                editar._cod_cargoLB = row["cod_cargo"].ToString();
                editar._cod_cargo = row["nom_cargo"].ToString();
                editar._cod_ocupLB = row["cod_ocup"].ToString();
                editar._cod_ocup = row["nom_ocup"].ToString();
                editar._cod_profLB = row["cod_prof"].ToString();
                editar._cod_prof = row["nom_prof"].ToString();
                editar._num_doc = row["num_doc"].ToString();
                editar._obser = row["observ"].ToString();
                editar._hobbies = row["hobbies"].ToString();
                editar._image_name = row["image_name"].ToString();
                if (((byte[])row["img_cli"]).Length > 0) { editar._img_cli = (byte[])row["img_cli"]; } else { editar._img_cli = null; }
                editar._ran_edad = row["ran_edad"].ToString();

                editar._LB_talla_zap_tenn = row["talla_zap_tenn"].ToString();
                editar._talla_zap_tenn = row["nom_talla1"].ToString();
            
                editar._LB_talla_pant_fald = row["talla_pant_fald"].ToString();
                editar._talla_pant_fald = row["nom_talla2"].ToString();

                editar._LB_talla_vest_traj = row["talla_vest_traj"].ToString();
                editar._talla_vest_traj = row["nom_talla3"].ToString();

                editar._LB_talla_camisa = row["talla_camisa"].ToString();
                editar._talla_camisa = row["nom_talla4"].ToString();

                editar._LB_talla_camisa_sport = row["talla_camisa_sport"].ToString();
                editar._talla_camisa_sport = row["nom_talla5"].ToString();

                editar.ShowDialog();

                if (tipoUsuario == "1" || tipoUsuario == "2")
                {
                    RefeshDataGrid(0);
                }
                else {
                    RefeshDataGrid(1);
                }
                
            }
            catch (Exception)
            {
                MessageBox.Show("seleccione un cliente");                
            }


        }

        // abrir ventana para seguimineto
        private void BtnSeg_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Seguimiento seguir = new Seguimiento();
                DataRowView row = (DataRowView)dataGridCxC.SelectedItems[0];
                               
                seguir.cod_ter = row["cod_ter"].ToString(); 
                seguir.nom_comple = row["nom_ter"].ToString(); 
                seguir.tel1 = row["tel1"].ToString();
                seguir.tel2 = row["tel2"].ToString();
                seguir.cel = row["cel"].ToString();
                seguir.email = row["email"].ToString();
                seguir.dir = row["dir"].ToString();
                seguir.cod_mer = TxtUser.Text;

                seguir.ct_email = row["ct_email"].ToString(); 
                seguir.ct_correspondencia = row["ct_corres"].ToString();
                seguir.ct_whats = row["ct_whats"].ToString();
                seguir.ct_sms = row["ct_sms"].ToString();
                seguir.ct_celular = row["ct_cel"].ToString();

                seguir.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("seleccione un cliente");

            }

        }

        // abrir ventana de historico
        private void BtnHis_Click(object sender, RoutedEventArgs e) {

            try
            {
                DataRowView row = (DataRowView)dataGridCxC.SelectedItems[0];
                HistoricoComercial historico = new HistoricoComercial();
                historico.cod_cliente = row["cod_ter"].ToString();
                historico.nom_cliente = row["nom_ter"].ToString();
                historico.ShowDialog();
            }
            catch (Exception )
            {
                MessageBox.Show("Selecciones un cliente");
            }

        }

        //abrir seguimiento de compra
        private void BtnSegCompra_Click(object sender, RoutedEventArgs e) {

            try
            {
                SeguimientoCompra seg_compra = new SeguimientoCompra();
                DataRowView row = (DataRowView)dataGridCxC.SelectedItems[0];

                seg_compra.nombre_cli = row["nom_ter"].ToString();
                seg_compra.cod_cli = row["cod_ter"].ToString();
                seg_compra.nombre_ven = TxtUserName.Text.Trim();
                seg_compra.cod_ven = TxtUser.Text.Trim();

                seg_compra.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Selecciones un cliente");

            }
            

        }


        //exportar a excel
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

        
        //****************************** 2 tab ************************************************************

        public void llenarGridCamp() {

            if (tipoUsuario == "3" || tipoUsuario == "4")
            {            
                try
                {
                    //clientes que estan en una campaña
                    string cadena = "select cliente.cod_ter,cliente.nom_ter,temporal.cod_camp as cod_camp, campa.nom_camp as nom_camp from comae_ter as cliente ";                
                    cadena = cadena + "full join CrTemCampa as temporal on temporal.cod_ter = cliente.cod_ter ";
                    cadena = cadena + "full join CrMae_campa as campa on campa.cod_camp  = temporal.cod_camp ";
                    cadena = cadena + "where cliente.clasific=1 and cliente.cod_ven='" + SiaWin._UserTag1 + "' ";
                    cadena = cadena + "and campa.estado=1 ";
                    cadena = cadena + "group by cliente.cod_ter,cliente.nom_ter,temporal.cod_camp,campa.nom_camp ";
                    DataTable dt3 = new DataTable();
                    dt3 = SiaWin.Func.SqlDT(cadena, "Clientes", idemp);
                    dataGridCliCamp.ItemsSource = dt3.DefaultView;
                    TBcampSi.Text = dt3.Rows.Count.ToString();


                    //clientes que no estan en ninguna campaña
                    string query = "select cliente.cod_ter,cliente.nom_ter from comae_ter as cliente  ";
                    query = query + "full join CrTemCampa as temporal on temporal.cod_ter = cliente.cod_ter ";
                    query = query + "full join CrMae_campa as campa on campa.cod_camp = temporal.cod_camp ";
                    query = query + "where cliente.clasific=1 and cliente.cod_ven='" + SiaWin._UserTag1 + "' ";
                    query = query + "and campa.cod_camp IS NULL ";
                    query = query + "group by cliente.cod_ter,cliente.nom_ter,temporal.cod_camp,campa.nom_camp,campa.cod_camp ";               
                    DataTable dt4 = new DataTable();
                    dt4 = SiaWin.Func.SqlDT(query, "Clientes", idemp);
                    dataGridClientSinCamp.ItemsSource = dt4.DefaultView;
                    TBcampNo.Text = dt4.Rows.Count.ToString();

                    //clientes que estan en campañas inexistentes
                    string campaInex = "select cliente.cod_ter,cliente.nom_ter from comae_ter as cliente  ";
                    campaInex = campaInex + "full join CrTemCampa as temporal on temporal.cod_ter = cliente.cod_ter ";
                    campaInex = campaInex + "full join CrMae_campa as campa on campa.cod_camp = temporal.cod_camp ";
                    campaInex = campaInex + "where cliente.clasific=1 and cliente.cod_ven='" + SiaWin._UserTag1 + "' ";
                    campaInex = campaInex + "and campa.estado=0";
                    campaInex = campaInex + "group by cliente.cod_ter,cliente.nom_ter,temporal.cod_camp,campa.nom_camp,campa.cod_camp ";
                    DataTable dt5 = new DataTable();
                    dt5 = SiaWin.Func.SqlDT(campaInex, "Clientes", idemp);
                    dataGridClientCampInexistente.ItemsSource = dt5.DefaultView;
                    TBcampInac.Text = dt5.Rows.Count.ToString();

                }
                catch (Exception w)
                {

                    MessageBox.Show("error:" + w);
                }
            }

        }
 
        //boton de admonistrador para buscar los clientes
        private void TXB_cliente_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int idr = 0; string code = ""; string nombre = "";
                dynamic xx = SiaWin.WindowBuscar("Comae_ter", "cod_ter", "nom_ter", "cod_ter", "idrow", "Maestra De Clientes", cnEmp, false, "clasific=1");
                xx.ShowInTaskbar = false;
                xx.Owner = Application.Current.MainWindow;
                xx.ShowDialog();
                idr = xx.IdRowReturn;
                code = xx.Codigo;
                nombre = xx.Nombre;
                xx = null;

                if (idr > 0)
                {
                    LB_cliente.Text = code;
                    TXB_cliente.Text = nombre.Trim();                    
                    BTNbuscar.IsEnabled = true;                 
                }
                if (string.IsNullOrEmpty(code)) e.Handled = false;
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void BTNbuscar_Click(object sender, RoutedEventArgs e)
        {
            if (tipoUsuario == "1" || tipoUsuario == "2")
            {
                RefeshDataGrid(0);
            }

        }




    }
}





