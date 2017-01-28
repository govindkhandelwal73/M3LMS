using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.Configuration;

namespace M3LMS.Charts
{
    public partial class Dashboard_1 : System.Web.UI.Page
    {
        private SqlConnection con;
        private SqlCommand com;
        private string constr, query;
        private void connection()
        {
            constr = "data source=SQL5006.myASP.NET;initial catalog=DB_A1122C_govind12;user id=DB_A1122C_govind12_admin;password=govind12*;";
            con = new SqlConnection(constr);
            con.Open();

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bindchart();

            }
        }


        private void Bindchart()
        {
            connection();
            com = new SqlCommand("GetDepartmentWiseUsersCount", con);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTable ChartData = ds.Tables[0];

            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[ChartData.Rows.Count];
            int[] YPointMember = new int[ChartData.Rows.Count];

            for (int count = 0; count < ChartData.Rows.Count; count++)
            {
                //storing Values for X axis  
                XPointMember[count] = ChartData.Rows[count]["Department"].ToString();
                //storing values for Y Axis  
                YPointMember[count] = Convert.ToInt32(ChartData.Rows[count]["Total_Users"]);

            }
            //binding chart control  
            Chart1.Series[0].Points.DataBindXY(XPointMember, YPointMember);

            //Setting width of line  
            Chart1.Series[0].BorderWidth = 10;
            //setting Chart type   
            Chart1.Series[0].ChartType = SeriesChartType.Pie;


            foreach (Series charts in Chart1.Series)
            {
                foreach (DataPoint point in charts.Points)
                {
                    switch (point.AxisLabel)
                    {
                        case "Sales11": point.Color = Color.RoyalBlue; break;
                        case "All": point.Color = Color.SaddleBrown; break;
                        
                    }
                    point.Label = string.Format("{0:0} - {1}", point.YValues[0], point.AxisLabel);

                }
            }


            //Enabled 3D  
            //  Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;  
            con.Close();

        }
    }  
}