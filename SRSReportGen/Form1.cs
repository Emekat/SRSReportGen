using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            ls = IssuePager3.Search3(startdate, enddate);

        }

        private void EchannelReport(DateTime startdate, DateTime enddate)
        {
            ls = IssuePager3.Search3(startdate, enddate);

        }
    }
}
