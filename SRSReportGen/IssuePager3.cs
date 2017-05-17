using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Configuration;
using System.Threading.Tasks;

namespace SRSReportGen
{
    class IssuePager3
    {
      
        public static DataTable SearchCard(DateTime startDate, DateTime endDate)
        {

            int status = 0;

            //string sql = @"SELECT * from vew_cardissues where id > 0 ";

            string sql = @"
select i.id ,i.TransactionSourceId, i.CallerRef, i.AccountNumber, i.TransactionSourceDate, i.IssueStatus, i.IssueAction,
i.CustomerName, i.MobileNumber, i.Email, i.SiteUserId,i.NonIssueRef,i.DateAdded, i.LastUpdated, i.RecordStatus, 
pAmt.IssuePropertyText as Amount, 
pAcct.IssuePropertyText as CustAccount,   
pTrnx.IssuePropertyText as TrnxType, 
pPan.IssuePropertyText as PAN, 
pTrm.IssuePropertyText as Terminal,
ii.info
from dbo.tbl_issues i
left join tbl_issue_property_values pAmt on i.Id = pAmt.IssueId and pAmt.IssuePropertyId = 2
left join tbl_issue_property_values pAcct on i.Id = pAcct.IssueId and pAcct.IssuePropertyId = 3
left join tbl_issue_property_values pTrnx on i.Id = pTrnx.IssueId and pTrnx.IssuePropertyId = 9
left join tbl_issue_property_values pPan on i.Id = pPan.IssueId and pPan.IssuePropertyId = 10
left join tbl_issue_property_values pTrm on i.Id = pTrm.IssueId and pTrm.IssuePropertyId = 12
left join dbo.tbl_issue_info ii on i.id = ii.id
where TransactionSourceId = 3
and pTrnx.IssuePropertyText in 
('Cardholder accounts transfer','Payment from account','Cash withdrawal','Goods and services')
";

            //filter start date
            if (!(startDate == null))
            {
                DateTime sd = DateTime.Today;
                try { sd = Convert.ToDateTime(startDate); }
                catch { }
                sql += " and transactionSourceDate >= @startDate ";

            }

            //filter end date
            if (!(endDate == null))
            {
                DateTime ed = DateTime.Today.AddDays(1);
                try { ed = Convert.ToDateTime(endDate); ed = ed.AddDays(1); }
                catch { }
                sql += " and transactionSourceDate < @endDate ";

            }

            if (status >= 0)
            {
                sql += " and IssueStatus = @issuestatus ";

            }

            sql += @"  order by transactionSourceDate desc";
            SqlConnection cn = new SqlConnection();

            SqlCommand cmd = new SqlCommand();
            cn.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();

            DataTable dt = new DataTable();
            using (cn)
            {

                cmd.Connection = cn;

                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 20000000;

                cmd.CommandText = sql;

                cmd.Parameters.AddWithValue("@startDate ", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);
                cmd.Parameters.AddWithValue("@issuestatus ", status);

               
                try
                {
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    dt.Load(dr);

                    cn.Close();
                }
                catch (Exception ex)
                {

                    cn.Close();


                }
            }
            return dt;
        }
        public static List<CardIssue> Search3(DateTime startDate, DateTime endDate)
        {
            CardIssue cardIssue = new CardIssue();
            List<CardIssue> ls = new List<CardIssue>();
            int status = 0;

            //string sql = @"SELECT * from vew_cardissues where id > 0 ";

            string sql = @"
select i.id ,i.TransactionSourceId, i.CallerRef, i.AccountNumber, i.TransactionSourceDate, i.IssueStatus, i.IssueAction,
i.CustomerName, i.MobileNumber, i.Email, i.SiteUserId,i.NonIssueRef,i.DateAdded, i.LastUpdated, i.RecordStatus, 
pAmt.IssuePropertyText as Amount, 
pAcct.IssuePropertyText as CustAccount,   
pTrnx.IssuePropertyText as TrnxType, 
pPan.IssuePropertyText as PAN, 
pTrm.IssuePropertyText as Terminal,
ii.info
from dbo.tbl_issues i
left join tbl_issue_property_values pAmt on i.Id = pAmt.IssueId and pAmt.IssuePropertyId = 2
left join tbl_issue_property_values pAcct on i.Id = pAcct.IssueId and pAcct.IssuePropertyId = 3
left join tbl_issue_property_values pTrnx on i.Id = pTrnx.IssueId and pTrnx.IssuePropertyId = 9
left join tbl_issue_property_values pPan on i.Id = pPan.IssueId and pPan.IssuePropertyId = 10
left join tbl_issue_property_values pTrm on i.Id = pTrm.IssueId and pTrm.IssuePropertyId = 12
left join dbo.tbl_issue_info ii on i.id = ii.id
where TransactionSourceId = 3
and pTrnx.IssuePropertyText in 
('Cardholder accounts transfer','Payment from account','Cash withdrawal','Goods and services')
";

            //filter start date
            if (!(startDate == null))
            {
                DateTime sd = DateTime.Today;
                try { sd = Convert.ToDateTime(startDate); }
                catch { }
                sql += " and transactionSourceDate >= @startDate ";

            }

            //filter end date
            if (!(endDate == null))
            {
                DateTime ed = DateTime.Today.AddDays(1);
                try { ed = Convert.ToDateTime(endDate); ed = ed.AddDays(1); }
                catch { }
                sql += " and transactionSourceDate < @endDate ";

            }

            if (status >= 0)
            {
                sql += " and IssueStatus = @issuestatus ";

            }

            sql += @"  order by transactionSourceDate desc";
            SqlConnection cn = new SqlConnection();

            SqlCommand cmd = new SqlCommand();
            cn.ConnectionString = ConfigurationManager.AppSettings["DataConnection"].ToString();
            using (cn)
            {

                cmd.Connection = cn;

                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 20000000;

                cmd.CommandText = sql;

                cmd.Parameters.AddWithValue("@startDate ", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);
                cmd.Parameters.AddWithValue("@issuestatus ", status);

                DataTable dt = new DataTable();
                try
                {
                    cn.Open();
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                   
                    dt.Load(dr);

                    cn.Close();
                }
                catch (Exception ex)
                {

                    cn.Close();


                }

                ls = dt.ToList<CardIssue>();

                return ls;
            }
        }

    }
    
}
