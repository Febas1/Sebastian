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

namespace SiasoftAppExt
{

    public partial class InformeEfectividad : UserControl
    {

        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string cnEmp = "";


        public InformeEfectividad(dynamic tabitem1)
        {
            InitializeComponent();

            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
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
                tabitem.Title = "Informe de Efectividad(" + aliasemp + ")";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /*
        select nom_mer as nom_mer, count(distinct nom_ter) total_cli_crm,count(distinct temporal.cod_ter) as clientes_campa,Count(case when seguimiento.cod_con= '02' then seguimiento.cod_con end) as llamas_campaña,Count(case when seguimiento.cod_con= '03' then seguimiento.cod_con end) as llamas_cumpleaños,Count(case when seguimiento.cod_con= '03' or seguimiento.cod_con= '02' then seguimiento.cod_con end) as total_llamadas
        from comae_ter as cliente
        inner join InMae_mer as vendedor on vendedor.cod_mer = cliente.cod_ven
        full join(select distinct cod_ter, cod_con from Crseg_cli) as seguimiento on seguimiento.cod_ter = cliente.cod_ter
        full join(select distinct cod_ter from CrTemCampa) as temporal on temporal.cod_ter = cliente.cod_ter
        where clasific=1
        and vendedor.cod_mer='02'
        group by vendedor.nom_mer
        */


    }
}
