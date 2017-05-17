using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRSReportGen.utility
{
    class DataExport
    {
        public string GenerateRndNumber(int cnt)
        {
            string[] key2 = new string[]
            {
                "0",
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9"
            };
            Random rand = new Random();
            string txt = "";
            for (int i = 0; i < cnt; i++)
            {
                txt += key2[rand.Next(0, 9)];
            }
            return txt;
        }

        public byte[] convertDataSetToCSV(DataSet ds, string sheetname)
        {
            string pth = ConfigurationManager.AppSettings["fileupload"];
            string fle = DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";



            string txt = "";
            for (int t = 0; t < ds.Tables.Count; t++)
            {
                DataTable dt = ds.Tables[t];
                int cnt_cols = dt.Columns.Count;
                int cnt_cols1 = cnt_cols - 1;
                for (int i = 0; i < cnt_cols; i++)
                {

                    txt += "\"" + dt.Columns[i].ColumnName + "\"";

                    if (i == cnt_cols1)
                    {
                        txt += Environment.NewLine;
                    }
                    else
                    {
                        txt += ",";
                    }
                }

                int cols = dt.Columns.Count;
                int rows = dt.Rows.Count;
                int cols1 = cols - 1;
                int k = 0;
                for (int i = 0; i < rows; i++)
                {
                    k += 1;
                    for (int j = 0; j < cols; j++)
                    {

                        string kb = dt.Columns[j].DataType.ToString();
                        string vl = "";
                        try
                        {
                            switch (kb)
                            {
                                case "System.Decimal":
                                    vl = Convert.ToDecimal(dt.Rows[i][j]).ToString("0.00");
                                    break;
                                case "System.Double":
                                    vl = Convert.ToDouble(dt.Rows[i][j]).ToString("0.00");
                                    break;
                                case "System.Int16":
                                    vl = Convert.ToInt16(dt.Rows[i][j]).ToString("0");
                                    break;
                                case "System.Int32":
                                    vl = Convert.ToInt32(dt.Rows[i][j]).ToString("0");
                                    break;
                                case "System.Int64":
                                    vl = Convert.ToInt64(dt.Rows[i][j]).ToString("0");
                                    break;
                                case "System.DateTime":
                                    vl = "" + Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy-MM-dd HH:mm:ss");
                                    break;
                                default:
                                    vl = Convert.ToString(dt.Rows[i][j]);
                                    break;
                            }
                        }
                        catch
                        {
                            vl = Convert.ToString(dt.Rows[i][j]);
                        }



                        txt += "\"" + vl + "\"";
                        if (j == cols1)
                        {
                            txt += Environment.NewLine;
                        }
                        else
                        {
                            txt += ",";
                        }
                    }

                    if (k % 100 == 0)
                    {
                        File.AppendAllText(pth + fle, txt);
                        txt = "";
                    }
                }

            }




            File.AppendAllText(pth + fle, txt);
            //return pth + fle;



            byte[] bytes = new byte[1];
            try
            {
                using (FileStream fsSource = new FileStream(pth + fle, FileMode.Open, FileAccess.Read))
                {
                    bytes = new byte[fsSource.Length];
                    int numBytesToRead = (int)fsSource.Length;
                    int numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        int n2 = fsSource.Read(bytes, numBytesRead, numBytesToRead);
                        if (n2 == 0)
                        {
                            fsSource.Close();
                            fsSource.Dispose();
                            break;
                        }
                        numBytesRead += n2;
                        numBytesToRead -= n2;
                    }
                }
                FileInfo fi = new FileInfo(pth + fle);
                //fi.Delete();
            }
            catch
            {
            }
            return bytes;

        }

        public byte[] convertDataSetToCSV(DataTable dt, string sheetname)
        {
            string pth = ConfigurationManager.AppSettings["fileupload"];
            string fle = DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";



            string txt = "";

            int cnt_cols = dt.Columns.Count;
            int cnt_cols1 = cnt_cols - 1;
            for (int i = 0; i < cnt_cols; i++)
            {

                txt += "\"" + dt.Columns[i].ColumnName + "\"";

                if (i == cnt_cols1)
                {
                    txt += Environment.NewLine;
                }
                else
                {
                    txt += ",";
                }
            }

            int cols = dt.Columns.Count;
            int rows = dt.Rows.Count;
            int cols1 = cols - 1;
            int k = 0;
            for (int i = 0; i < rows; i++)
            {
                k += 1;
                for (int j = 0; j < cols; j++)
                {

                    string kb = dt.Columns[j].DataType.ToString();
                    string vl = "";
                    try
                    {
                        switch (kb)
                        {
                            case "System.Decimal":
                                vl = Convert.ToDecimal(dt.Rows[i][j]).ToString("0.00");
                                break;
                            case "System.Double":
                                vl = Convert.ToDouble(dt.Rows[i][j]).ToString("0.00");
                                break;
                            case "System.Int16":
                                vl = Convert.ToInt16(dt.Rows[i][j]).ToString("0");
                                break;
                            case "System.Int32":
                                vl = Convert.ToInt32(dt.Rows[i][j]).ToString("0");
                                break;
                            case "System.Int64":
                                vl = Convert.ToInt64(dt.Rows[i][j]).ToString("0");
                                break;
                            case "System.DateTime":
                                vl = "" + Convert.ToDateTime(dt.Rows[i][j]).ToString("yyyy-MM-dd HH:mm:ss");
                                break;
                            default:
                                vl = Convert.ToString(dt.Rows[i][j]);
                                break;
                        }
                    }
                    catch
                    {
                        vl = Convert.ToString(dt.Rows[i][j]);
                    }



                    txt += "\"" + vl + "\"";
                    if (j == cols1)
                    {
                        txt += Environment.NewLine;
                    }
                    else
                    {
                        txt += ",";
                    }
                }

                if (k % 100 == 0)
                {
                    File.AppendAllText(pth + fle, txt);
                    txt = "";
                }
            }





            File.AppendAllText(pth + fle, txt);



            byte[] bytes = new byte[1];
            try
            {
                using (FileStream fsSource = new FileStream(pth + fle, FileMode.Open, FileAccess.Read))
                {
                    bytes = new byte[fsSource.Length];
                    int numBytesToRead = (int)fsSource.Length;
                    int numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        int n2 = fsSource.Read(bytes, numBytesRead, numBytesToRead);
                        if (n2 == 0)
                        {
                            fsSource.Close();
                            fsSource.Dispose();
                            break;
                        }
                        numBytesRead += n2;
                        numBytesToRead -= n2;
                    }
                }
                FileInfo fi = new FileInfo(pth + fle);
                //fi.Delete();
            }
            catch
            {
            }
            return bytes;

        }

        public string generateFileName()
        {
            string[] key = new string[]
            {
                "b",
                "c",
                "d",
                "f",
                "g",
                "h",
                "j",
                "k",
                "m",
                "n",
                "p",
                "q",
                "r",
                "t",
                "v",
                "w",
                "x",
                "y",
                "z"
            };
            Random rand = new Random();
            string nme = "";
            nme += DateTime.Now.ToString("yyMMddHHmmss");
            nme += key[rand.Next(0, 18)];
            nme += key[rand.Next(0, 18)];
            return nme + key[rand.Next(0, 18)];
        }
    }
}
