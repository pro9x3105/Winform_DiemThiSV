using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NguyenQuangAnh_171200170_CNTT4K58
{
    public partial class Form1 : Form
    {
        ProcessDataBase connv1 = new ProcessDataBase();
        DataTable dt;
        DataTable dt2;

        public Form1()
        {
            InitializeComponent();
            dt= connv1.DocBang("SELECT MaMon as [Mã Môn],TenMon as [Tên môn],SoTinChi as [Số Tín Chỉ] , DiemThi as [Điểm Thi] FROM tblDSDiem");
            dt2 = connv1.DocBang("SELECT * FROM tblDSDiem");
            dgvHienThi.DataSource = dt;
        }
        private void xoa()
        {
            txbMaSV.Clear();
            txbMaMon.Clear();
            txbTenMon.Clear();
            txbSoTinChi.Clear();
            txbDiemThi.Clear();
        }

        private void BtnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void DgvHienThi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txbMaSV.Text = dt2.Rows[0][0].ToString();
            txbMaMon.Text = dgvHienThi.CurrentRow.Cells[0].Value.ToString();
            txbTenMon.Text = dgvHienThi.CurrentRow.Cells[1].Value.ToString();
            txbSoTinChi.Text = dgvHienThi.CurrentRow.Cells[2].Value.ToString();
            txbDiemThi.Text = dgvHienThi.CurrentRow.Cells[3].Value.ToString();
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < dgvHienThi.Rows.Count - 1; i++)
            {
                if (txbMaMon.Text == dgvHienThi.Rows[i].Cells[0].Value.ToString())
                {
                    MessageBox.Show("Mã môn này bị trùng");
                    return;
                }
            }
            if (Convert.ToInt32(txbSoTinChi.Text) < 0)
            {
                MessageBox.Show("Số Tín Chỉ phải nhập số nguyên");
                return;
            }
            connv1.CapNhapDuLieu("INSERT into tblDSDiem(MaSV,MaMon,TenMon,SoTinChi,DiemThi) VALUES ('" + txbMaSV.Text + "',N'" + txbMaMon.Text + "','" + txbTenMon.Text + "','" + txbSoTinChi.Text + "','"+txbDiemThi.Text+"')");
            dt = connv1.DocBang("SELECT MaMon as [Mã Môn],TenMon as [Tên môn],SoTinChi as [Số Tín Chỉ] , DiemThi as [Điểm Thi] FROM tblDSDiem");
            dgvHienThi.DataSource = dt;
            xoa();
        }

        private void TxbSoTinChi_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxbSoTinChi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxbDiemThi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            try
            {
                connv1.CapNhapDuLieu("UPDATE tblDSDiem set TenMon='" + txbTenMon.Text + "',SoTinChi='" + txbSoTinChi.Text + "',DiemThi='" + txbDiemThi.Text + "' WHERE MaSV='" + txbMaSV.Text + "' and MaMon ='" + txbMaMon.Text + "'");
                dt = connv1.DocBang("SELECT MaMon as [Mã Môn],TenMon as [Tên môn],SoTinChi as [Số Tín Chỉ] , DiemThi as [Điểm Thi] FROM tblDSDiem");
                dgvHienThi.DataSource = dt;
                xoa();
            }
            catch
            {
                ;
            }
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            connv1.XuatExcelsqlcode("SELECT MaSV as [Mã Sinh Viên] ,MaMon as [Mã Môn],TenMon as [Tên môn],SoTinChi as [Số Tín Chỉ] , DiemThi as [Điểm Thi] FROM tblDSDiem");
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                connv1.CapNhapDuLieu("DELETE FROM tblDSDiem WHERE MaSV='" + txbMaSV.Text + "' and MaMon='"+txbMaMon.Text+"' and TenMon='" + txbTenMon.Text + "' and SoTinChi='" + txbSoTinChi.Text + "' and DiemThi='" + txbDiemThi.Text + "'");
                dt = connv1.DocBang("SELECT MaMon as [Mã Môn],TenMon as [Tên môn],SoTinChi as [Số Tín Chỉ] , DiemThi as [Điểm Thi] FROM tblDSDiem");
                dgvHienThi.DataSource = dt;
                xoa();
            }
        }

        private void BtnThongKe_Click(object sender, EventArgs e)
        {
            float tongtc = 0, tongdiem = 0;
            for(int i = 0; i < dgvHienThi.Rows.Count - 1; i++)
            {
                tongtc += Convert.ToInt32(dgvHienThi.Rows[i].Cells[2].Value.ToString());
                tongdiem += Convert.ToInt32(dgvHienThi.Rows[i].Cells[3].Value.ToString())* Convert.ToInt32(dgvHienThi.Rows[i].Cells[2].Value.ToString());
            }
            txbTongTC.Text = tongtc.ToString();
            txbTongDiem.Text = tongdiem.ToString();
            txbDiemTB.Text = (tongdiem / tongtc).ToString();

        }

        private void DgvHienThi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
