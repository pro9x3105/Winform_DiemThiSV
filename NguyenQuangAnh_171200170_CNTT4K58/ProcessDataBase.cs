using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing;

namespace NguyenQuangAnh_171200170_CNTT4K58
{
    class ProcessDataBase
    {
        //Nhớ add Excel trong Add/Reference
        public string sql = "Data Source=DESKTOP-CTJL81R;Initial Catalog=ThiCuoiKi;User ID=sa;Password=31051999";
        public SqlConnection conn = null;
        public void KetNoiCSDL()
        {
            conn = new SqlConnection(sql);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
        }   
        private void DongKetNoiCSDL()
        {
            if(conn.State!=ConnectionState.Closed)
            {
                conn.Close();

            }
        }
        public DataTable DocBang(string sql)
        {
            DataTable dt = new DataTable();
            KetNoiCSDL();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql,conn);
            sqlDataAdapter.Fill(dt);
            DongKetNoiCSDL();
            return dt;
            
        }
        public void CapNhapDuLieu(string sql)
        {
            KetNoiCSDL();
            
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = conn;
            sqlCommand.CommandText = sql;
            
            sqlCommand.ExecuteNonQuery();
            DongKetNoiCSDL();
        }
        public void CapNhapDuLieuAnh(byte []sql,string x)
        {
            KetNoiCSDL();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = conn;
            sqlCommand.CommandText = x;
            sqlCommand.Parameters.AddWithValue("[Anh]",sql);

            sqlCommand.ExecuteNonQuery();
            DongKetNoiCSDL();
        }

        public void XuatExcelsqlcode(string sql)
        {
            DataTable dtExcel = DocBang(sql);
            XuatExcelDataTable(dtExcel);
        }

        public void XuatExcelDataTable(DataTable dtExcel)
        {
            SaveFileDialog saveExcel = new SaveFileDialog();
            if (dtExcel.Rows.Count > 0) //TH có dữ liệu được ghi
            {
                Excel.Application exApp = new Excel.Application();
                Excel.Workbook exBook = exApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                Excel.Worksheet exSheet = (Excel.Worksheet)exBook.Worksheets[1];
                string[] columnNames = dtExcel.Columns.Cast<DataColumn>()   //Lấy Tên Các Cột
                    .Select(x => x.ColumnName)
                    .ToArray();
                
                //In dữ liệu

                for (int i = 0; i < dtExcel.Rows.Count; i++)
                {
                    char ascii = (char)65;   //A=65
                    //Điền STT
                    exSheet.get_Range(ascii + (i + 2).ToString()).Font.Bold = false;
                    exSheet.get_Range(ascii + (i + 2).ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    exSheet.get_Range(ascii + (i + 2).ToString()).Value = (i + 1).ToString();
                    exSheet.get_Range(ascii + (i + 2).ToString()).BorderAround2();
                    exSheet.get_Range(ascii + (i + 2).ToString()).Font.Color = Color.Blue;
                    exSheet.get_Range(ascii + (i + 2).ToString()).Interior.Color = Color.Yellow;
                    for (int j = 0; j < dtExcel.Columns.Count; j++)
                    {
                        if (j == 0) //Tiêu đề STT
                        {
                            exSheet.get_Range(ascii + "1").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            exSheet.get_Range(ascii + "1").Value = "STT";
                            exSheet.get_Range(ascii + "1").ColumnWidth = 15;
                            exSheet.get_Range(ascii + "1").BorderAround2();
                            exSheet.get_Range(ascii + "1").Font.Bold = true;
                            exSheet.get_Range(ascii + "1").Font.Color = Color.Red;
                            exSheet.get_Range(ascii + "1").Interior.Color = Color.Black;
                        }
                        ascii++;
                        //Tiêu đề Cột
                            exSheet.get_Range(ascii + "1").HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            exSheet.get_Range(ascii + "1").Value = columnNames[j];
                            exSheet.get_Range(ascii + "1").ColumnWidth = 15;
                            exSheet.get_Range(ascii + "1").BorderAround2();
                            exSheet.get_Range(ascii + "1").Font.Bold = true;
                            exSheet.get_Range(ascii + "1").Font.Color = Color.Blue;
                            exSheet.get_Range(ascii + "1").Interior.Color = Color.LightPink;
                        //Dữ liệu từng Cột
                        exSheet.get_Range(ascii + (i + 2).ToString()).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        exSheet.get_Range(ascii + (i + 2).ToString()).Value = dtExcel.Rows[i][j].ToString();
                        exSheet.get_Range(ascii + (i + 2).ToString()).BorderAround2();
                        exSheet.get_Range(ascii + (i + 2).ToString()).Interior.Color = Color.AliceBlue;
                    }

                }
                exSheet.Name = dtExcel.Rows[0][0].ToString();
                exBook.Activate(); //Kích hoạt file Excel
                                   //Thiết lập các thuộc tính của SaveFileDialog
                saveExcel.Filter = "Excel Document(*.xls)|*.xls |Word Document(*.doc)| *.doc | All files(*.*) | *.* ";
                saveExcel.FilterIndex = 1;
                saveExcel.AddExtension = true;
                saveExcel.DefaultExt = ".xls";
                if (saveExcel.ShowDialog() == DialogResult.OK)
                {
                    exBook.SaveAs(saveExcel.FileName.ToString());//Luu file Excel
                    exApp.Visible = true;
                }
            }
            else
            {
                MessageBox.Show("Không có danh sách hàng để in");
            }
        }
    }
}
