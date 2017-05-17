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
        static int startIndexFromDataSetMethod = 1, PageSizeFromDataSetMethod = 100000000;


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

        public static DataTable SearchEchannel(DateTime startDate, DateTime endDate)
        {

            int status = 0;

            string sql = @";WITH CTE_Orders AS
          ( select t.id as txid, convert(varchar(max),d.RequestData) as xmlReceived, '' as cusnum, l09.FieldValue as SessionID, t.ReferenceId,t.RequestTypeId,l05.FieldValue as FromAccount, l10.FieldValue as ToAccount, 
l02.FieldValue as Amount, l04.FieldValue as DestinationBankCode, l03.FieldValue as BeneficiaryName, p.bankname as DestinationBank, t.AppId, convert(varchar(max),d.ResponseData) as xmlResponse,t.TimeIn as dateadded, t.[timeout] as dateResponded , t.ResponseCode as FinalResponseCode, ROW_NUMBER() OVER (ORDER BY t.id) AS ResultSetRowNumber 
from tbl_trnx_batch t join tbl_trnx_data_batch d on t.RequestTypeId = 101 and  t.Id = d.TrnxId
left join tbl_trnx_details_batch l02 on t.Id = l02.TrnxId and l02.FieldHead = 'Amount' 
left join tbl_trnx_details_batch l03 on t.Id = l03.TrnxId and l03.FieldHead = 'BenefiName'
left join tbl_trnx_details_batch l04 on t.Id = l04.TrnxId and l04.FieldHead = 'DestinationBankCode' 
left join tbl_trnx_details_batch l05 on t.Id = l05.TrnxId and l05.FieldHead = 'FromAccount' 
left join tbl_trnx_details_batch l09 on t.Id = l09.TrnxId and l09.FieldHead = 'SessionID'  
left join tbl_trnx_details_batch l10 on t.Id = l10.TrnxId and l10.FieldHead = 'ToAccount' 
left join tbl_participatingBanks p on t.Id = l04.TrnxId and l04.FieldHead = 'DestinationBankCode' 
and l04.FieldValue = p.bankcode where t.TimeIn between '" + startDate + "' and '" + endDate + "' union ";

            sql += @" select t.id as txid, convert(varchar(max),d.RequestData) as xmlReceived, '' as cusnum, '' as SessionID,  t.ReferenceId,t.RequestTypeId,l05.FieldValue as FromAccount, l10.FieldValue as ToAccount, 
  l02.FieldValue as Amount, '000001' as DestinationBankCode,'' as BeneficiaryName, 'Sterling Bank' as DestinationBank, t.AppId, convert(varchar(max),d.ResponseData) as xmlResponse,
t.TimeIn as dateadded, t.[timeout] as dateResponded , t.ResponseCode as FinalResponseCode,ROW_NUMBER() OVER (ORDER BY t.id) AS ResultSetRowNumber 
from tbl_trnx_batch t join tbl_trnx_data_batch d on t.RequestTypeId = 102 and  t.Id = d.TrnxId 
left join tbl_trnx_details_batch l02 on t.Id = l02.TrnxId and l02.FieldHead = 'Amount' 
left join tbl_trnx_details_batch l05 on t.Id = l05.TrnxId and l05.FieldHead = 'FromAccount' 
left join tbl_trnx_details_batch l10 on t.Id = l10.TrnxId and l10.FieldHead = 'ToAccount' where t.TimeIn between '" + startDate + "' and '" + endDate + "')";

            sql += @" SELECT t.txid,t.xmlReceived,t.cusnum,t.Sessionid,t.Referenceid,  t.RequestTypeId,t.FromAccount,t.ToAccount,t.Amount,t.DestinationBankCode,t.BeneficiaryName, t.DestinationBank,t.Appid as AppId,
 t.xmlResponse,t.dateadded,t.dateResponded ,t.FinalResponseCode,  a.ApplicationName, r.RequestTypeName , n.respdesc as  FinalResponseDescription,
  isnull(f.issueStatus,0) as issueStatus,n.responsecategoryid, cat.responseCategoryname FROM CTE_Orders t inner join tbl_applicationKey a on t.Appid = a.Appid
             inner join tbl_requesttypes r on t.RequestTypeid = r.RequestTypeId
             inner join tbl_nipresponses n on t.FinalResponseCode = n.respcode
             inner join tbl_issue_sources i on i.recordstatus = 1 and t.appid = i.Id
             inner join tbl_responsecat cat on n.responsecategoryid = cat.responsecategoryid
               left join tbl_issue_info f on f.id = t.txid 
              where t.txid > 0  ";

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

            sql += @" and r.[CategoryType] = 1 and  ResultSetRowNumber between " + startIndexFromDataSetMethod + " AND " + PageSizeFromDataSetMethod + " order by txid desc " ;
            SqlConnection cn = new SqlConnection();

            SqlCommand cmd = new SqlCommand();
            cn.ConnectionString = ConfigurationManager.AppSettings["echannelwsConnection"].ToString();

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
