using SRSReportGen.utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SRSReportGen
{
    public partial class Form1 : Form
    {
        List<CardIssue> ls;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime startdate = Convert.ToDateTime(dateTimePicker1.Text);
            DateTime enddate = Convert.ToDateTime(dateTimePicker2.Text);
            if (checkBox1.Checked == true)
                EchannelReport(startdate, enddate);
            else if (checkBox2.Checked == true)
                CardReport(startdate, enddate);
            else
            {
                BothReport(startdate, enddate);

            }
        }

        private void BothReport(DateTime startdate, DateTime enddate)
        {
            throw new NotImplementedException();
        }

        private void CardReport(DateTime startdate, DateTime enddate)
        {
         //   ls = IssuePager3.Search3(startdate, enddate);
            
            DataTable dt = IssuePager3.SearchCard(startdate, enddate);


            dt.Columns.Remove("xmlReceived");
            dt.Columns.Remove("RequestTypeId");
            //dt.Columns.Remove("NEResponsecode");
            //dt.Columns.Remove("Sortcode");
            //dt.Columns.Remove("Currencycode");
            dt.Columns.Remove("Sessionid");
            dt.Columns.Remove("Referenceid");
            //dt.Columns.Remove("BeneficiaryAccount");
            //dt.Columns.Remove("PrincipalAccount");
            dt.Columns.Remove("cusnum");
            dt.Columns.Remove("DestinationBankCode");
            //dt.Columns.Remove("IntermediaryBnk");
            //dt.Columns.Remove("IntermediaryBnkSortcode");
            //dt.Columns.Remove("Nuban");
            //dt.Columns.Remove("Carddeliverybranch");
            //dt.Columns.Remove("PINdeliverybranch");
            // dt.Columns.Remove("cuscity");
            //dt.Columns.Remove("cusregion");
            //dt.Columns.Remove("Cheqnum");
            //dt.Columns.Remove("valdate");
            // dt.Columns.Remove("BaseNum");

            dt.Columns.Remove("xmlResponse");
            //dt.Columns.Remove("PaymentReference");
            //dt.Columns.Remove("CardStatus");
            //dt.Columns.Remove("CardStatusMsg");
            //dt.Columns.Remove("ChequeStatus");
            //dt.Columns.Remove("ChequeStatusMsg");
            // dt.Columns.Remove("marktran");
            //dt.Columns.Remove("datemarked");
            // dt.Columns.Remove("PaymentRef");
            // dt.Columns.Remove("CreditAccount");
            // dt.Columns.Remove("dateFXCompleted");
            // dt.Columns.Remove("SFstatusflag");
            // dt.Columns.Remove("SFdateJobtreated");
            // dt.Columns.Remove("FeeAccount");
            //dt.Columns.Remove("RequestRefid");
            // dt.Columns.Remove("cur_code");
            dt.Columns.Remove("responsecategoryid");

            dt.AcceptChanges();
            string fle = "Transaction";
            DataExport g = new DataExport();
            byte[] bt = g.convertDataSetToCSV(dt, "Records");
            //File.WriteAllText(@"D:\devApps\NIBSSOUTPUTTest\" + filename + ".xml", globalxml);

            File.WriteAllBytes(@"D:\devApps\NIBSSOUTPUTTest\" + fle + ".csv", bt);
            MessageBox.Show("File generated successfully, check application bin\\Debug folder");


        }

        private void EchannelReport(DateTime startdate, DateTime enddate)
        {
            ls = IssuePager3.Search3(startdate, enddate);

        }
    }
}
