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
            {
                EchannelReport(startdate, enddate);
                MessageBox.Show("File generated successfully, check application bin\\Debug folder");
            }
                
            else if (checkBox2.Checked == true)
            {
                CardReport(startdate, enddate);
              
            }
              
            else
            {
                BothReport(startdate, enddate);
                MessageBox.Show("Files generated successfully");

            }
        }

        private void BothReport(DateTime startdate, DateTime enddate)
        {
            throw new NotImplementedException();
        }

        private void CardReport(DateTime startdate, DateTime enddate)
        {
            
            DataTable dt = IssuePager3.SearchCard(startdate, enddate);


            string fle = "CardIssue";
            DataExport g = new DataExport();
            byte[] bt = g.convertDataSetToCSV(dt, "Records");
            
       

            File.WriteAllBytes(fle + ".csv", bt);
         


        }

        private void EchannelReport(DateTime startdate, DateTime enddate)
        {
            ls = IssuePager3.Search3(startdate, enddate);

        }
    }
}
