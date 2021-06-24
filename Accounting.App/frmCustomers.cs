using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting.DataLayer.Context;
namespace Accounting.App
{
    public partial class frmCustomers : Form
    {
        public frmCustomers()
        {
            InitializeComponent();
        }

        UnitOfWork _db = new UnitOfWork();

        private void frmCustomers_Load(object sender, EventArgs e)
        {
            BindGrid();
        }

        void BindGrid()
        {

            using(UnitOfWork _db=new UnitOfWork())
            {
                dgvCustomers.AutoGenerateColumns = false;
                dgvCustomers.DataSource = _db.CustomerRepository.GetAllCustomers();
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            BindGrid();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

            dgvCustomers.DataSource = _db.CustomerRepository.GetCustomersAfterFilter(txtSearch.Text);
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {

            if (dgvCustomers.CurrentRow == null)
                MessageBox.Show("لطفا ابتدا یک شخص را انتخاب کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                using (UnitOfWork _db = new UnitOfWork())
                {
                    string customerName = dgvCustomers.CurrentRow.Cells["FullName"].Value.ToString();
                    int customerId = int.Parse(dgvCustomers.CurrentRow.Cells["CustomerId"].Value.ToString());
                    DialogResult dr = MessageBox.Show($"آیا از حذف {customerName} اطمینان دارید ؟", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        bool deleteConfirm = _db.CustomerRepository.DeleteCustomer(customerId);
                        if (deleteConfirm)
                            MessageBox.Show(string.Format("{0} با موفقیت حذف شد", customerName), "حذف موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    _db.Save();
                    BindGrid();
                }
                
            }
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            frmAddOrEditCustomer frmAddOrEditCustomer = new frmAddOrEditCustomer(0);
            if (frmAddOrEditCustomer.ShowDialog() == DialogResult.OK)
            {
                BindGrid();
            }
        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow != null)
            {
                int customerId = int.Parse(dgvCustomers.CurrentRow.Cells["CustomerId"].Value.ToString());
                frmAddOrEditCustomer frmAddOrEditCustomer = new frmAddOrEditCustomer(customerId);
                if (frmAddOrEditCustomer.ShowDialog() == DialogResult.OK)
                {
                    BindGrid();
                }
            }
        }
    }
}
