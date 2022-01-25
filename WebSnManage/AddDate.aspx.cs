using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSnManage.Models;

namespace WebSnManage
{
    public partial class AddDate : System.Web.UI.Page
    {
        string conn;
        string upload_excel_Dir = $"{ConfigurationManager.AppSettings["TempFolder"]}\\";
        XSSFWorkbook workbook = null;
        XSSFSheet u_sheet = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            //自webconfig 讀取connection string資訊
            conn = WebConfigurationManager.ConnectionStrings["EvisionConnectionString"].ConnectionString.ToString();
            if (Session["LoginObject"] == null)
                Response.Redirect("Login.aspx");
        }

        protected void BtnImport_Click(object sender, EventArgs e)
        {   
            //if (FileUpload1.ToString == "")

            var filePath = SaveFileAndReturnPath();
            try
            {
                workbook = new XSSFWorkbook(FileUpload1.FileContent);
                u_sheet = (XSSFSheet)workbook.GetSheetAt(0);

                InsertSheet(u_sheet);

                ClientScript.RegisterClientScriptBlock(typeof(Page), "匯入完成", "alert('匯入完成');", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (workbook != null) this.workbook = null;
                if (this.u_sheet != null) this.u_sheet = null;

                File.Delete(filePath);
                GC.Collect();
            }
            //GridView1.DataBind();
        }
        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="u_sheet"></param>
        private SnInfo InsertSheet(XSSFSheet u_sheet)
        {
            SnInfo snInfo = new SnInfo();
            XSSFRow snRow = (XSSFRow)u_sheet.GetRow(1);//取得目前的資料列
            snInfo.Sn = snRow.GetCell(0).ToString();
            snInfo.StartDtm = snRow.GetCell(1).ToString();
            snInfo.EndDtm = snRow.GetCell(2).ToString();

            List<ItemCheck> ItemCheckList = new List<ItemCheck>();
            //因為要讀取的資料列不包含標頭，所以i從u_sheet.FirstRowNum + 1開始讀
            for (int i = u_sheet.FirstRowNum + 4; i <= u_sheet.LastRowNum; i++)/*一列一列地讀取資料*/
            {
                XSSFRow row = (XSSFRow)u_sheet.GetRow(i);//取得目前的資料列

                ItemCheck ItemLIst = new ItemCheck();
                ItemLIst.Item = row.GetCell(0).ToString();
                ItemLIst.Pass = row.GetCell(1).ToString();
                ItemCheckList.Add(ItemLIst);



                //目前資料列第1格的值
                string cell0 = row.GetCell(0).ToString();
                //目前資料列第2格的值
                string cell1 = row.GetCell(1).ToString();
                //目前資料列第3格的值
                string cell2 = row.GetCell(2).ToString();
                //目前資料列第4格的值
                string cell3 = row.GetCell(3).ToString();

                //string sql = @"
                //    INSERT INTO [dbo].[excel_demo]
                //           ([excel_a]
                //           ,[excel_b]
                //           ,[excel_c]
                //           ,[excel_d]
                //           ,[ins_dtm])
                //     VALUES
                //           (@excel_a
                //           ,@excel_b
                //           ,@excel_c
                //           ,@excel_d
                //           ,@ins_dtm)
                //";

                //SqlConnection dataConnection = new SqlConnection();
                //SqlCommand mySqlCmd = new SqlCommand(sql, dataConnection);

                //try
                //{
                //    //設置資料庫位置
                //    dataConnection.ConnectionString = conn;

                //    //連接資料庫
                //    dataConnection.Open();

                //    //塞C#參數到SQL語法中的參數
                //    mySqlCmd.Parameters.AddWithValue("@excel_a", cell0);
                //    mySqlCmd.Parameters.AddWithValue("@excel_b", cell1);
                //    mySqlCmd.Parameters.AddWithValue("@excel_c", cell2);
                //    mySqlCmd.Parameters.AddWithValue("@excel_d", cell3);
                //    mySqlCmd.Parameters.AddWithValue("@ins_dtm", DateTime.Now);

                //    //執行SQL INSERT 語法
                //    mySqlCmd.ExecuteNonQuery();

                //    //關閉資料庫
                //    dataConnection.Close();
                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}
                //finally
                //{
                //    //清除
                //    mySqlCmd.Cancel();
                //    dataConnection.Close();
                //    dataConnection.Dispose();
                //}
            }

            snInfo.ItemChecks = ItemCheckList;

            return snInfo;
        }

        /// <summary>
        /// 儲存EXCEL檔案給Server
        /// </summary>
        /// <returns></returns>
        private string SaveFileAndReturnPath()
        {
            string return_file_path = "";//上傳的Excel檔在Server上的位置
            if (FileUpload1.FileName != "")
            {
                return_file_path = Path.Combine(upload_excel_Dir, Guid.NewGuid().ToString() + ".xls");

                FileUpload1.SaveAs(return_file_path);
            }
            return return_file_path;
        }
    }
}