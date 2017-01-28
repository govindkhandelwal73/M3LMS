using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using M3LMS.Models;

namespace WebApplication6
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        List<int> qlist;
        public static SqlConnection sqlconn;
        protected string PostBackStr;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlconn = new SqlConnection("data source=MC0ZHHJC\\GOVIND;initial catalog=DB_A14BC3_M3LMS;integrated security=true;");// ConfigurationManager.AppSettings["sqlconnstr"].ToString());
            PostBackStr = Page.ClientScript.GetPostBackEventReference(this, "time");

            if (IsPostBack)
            {
                string eventArg = Request["__EVENTARGUMENT"];
                if (eventArg == "time")
                {
                    getNextQuestion();
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DB_A14BC3_M3LMSEntities db = new DB_A14BC3_M3LMSEntities();
            var username = Session["UserName"].ToString();
            var ltm = db.ref_learnertestmapping.Where(a => a.EmailId == username & a.IsCompleted==false).FirstOrDefault() ;
            Label1.Visible = false;
            txtName.Visible = false;
            Button1.Visible = false;
            Panel1.Visible = true;
            //lblName.Text = "Name : " + txtName.Text;
            int score = Convert.ToInt32(txtScore.Text);
            lblScore.Text = "Score : " + Convert.ToString(score);
            Session["counter"] = "1";
            Random rnd = new Random();
           this.qlist = db.ref_TestQ.Where(a => a.TestId == ltm.TestId).Select(a => a.Qid).ToList();
            
            int i = rnd.Next(qlist.Min(), qlist.Max());//Here specify your starting slno of question table and ending no.
            getQuestion(i);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            getNextQuestion();
        }
        public void getQuestion(int no)
        {
            string str = "select * from ref_questions where slNo=" + no + "";
            SqlDataAdapter da2 = new SqlDataAdapter(str, sqlconn);
            DataSet ds2 = new DataSet();
            da2.Fill(ds2, "Question");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                DataRow dtr;
                int i = 0;
                while (i < ds2.Tables[0].Rows.Count)
                {
                    dtr = ds2.Tables[0].Rows[i];
                    Session["Answer"] = Convert.ToString(Convert.ToInt32(dtr["Answer"].ToString()) - 1);
                    lblQuestion.Text = "Q." + Session["counter"].ToString() + "  " + dtr["Question"].ToString();
                    RblOption.ClearSelection();
                    RblOption.Items.Clear();
                    RblOption.Items.Add(dtr["Option1"].ToString());
                    RblOption.Items.Add(dtr["Option2"].ToString());
                    RblOption.Items.Add(dtr["Option3"].ToString());
                    RblOption.Items.Add(dtr["Option4"].ToString());
                    i++;
                }
            }
        }
        public void getNextQuestion()
        {
            if (Convert.ToInt32(Session["counter"].ToString()) < 10)//10 is a counter which is used for 10 questions
            {
                if (RblOption.SelectedIndex >= 0)
                {
                    if (Session["Answer"].ToString() == RblOption.SelectedIndex.ToString())
                    {
                        int score = Convert.ToInt32(txtScore.Text) + 1;// 1 for mark for each question
                        txtScore.Text = score.ToString();
                        lblScore.Text = "Score : " + Convert.ToString(score);
                    }
                }
                Random rnd = new Random();
                int i = rnd.Next(this.qlist.Min(), this.qlist.Max());
                getQuestion(i);
                Session["counter"] = Convert.ToString(Convert.ToInt32(Session["counter"].ToString()) + 1);
            }
            else
            {
                Panel2.Visible = false;
                //code for displaying after completting the exam, if you want to show the result then you can code here.
            }
        }
        public void ConnectionOpen()
        {
            try
            {
                if (sqlconn.State == ConnectionState.Closed) { sqlconn.Open(); }
            }
            catch (SqlException ex)
            { }
            catch (SystemException sex)
            { }
        }
        public void ConnectionClose()
        {
            try
            {
                if (sqlconn.State != ConnectionState.Closed) { sqlconn.Close(); }
            }
            catch (Exception ex)
            {
            }
         
        }
    }
}