using Accounting.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValidationComponents;
using Accounting.DataLayer;

namespace Accounting.App
{
    public partial class frmAddNewAccounting : Form
    {
        private int TransactionId;
        public frmAddNewAccounting(int transactionId)
        {
            TransactionId = transactionId;
            InitializeComponent();
        }

        private void frmAddNewAccounting_Load(object sender, EventArgs e)
        {
            if(TransactionId==0)
            {
                this.Text = "تراکنش جدید";
            }
            else
            {
                this.Text = "ویرایش تراکنش";
                btnAddAccounting.Text = "ویرایش";
            }
            BindGrid();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        void BindGrid()
        {

            using (UnitOfWork _db = new UnitOfWork())
            {
                dgvCustomers.AutoGenerateColumns = false;
                dgvCustomers.DataSource = _db.CustomerRepository.GetCustomersNames(txtSearch.Text);
                if(TransactionId!=0)
                {
                    var transaction = _db.AccountingRepository.GetTransactionById(TransactionId);
                    txtCustomerName.Text = _db.CustomerRepository.GetCustomerById(transaction.CustomerId).Name;
                    if(transaction.TypeId==1)
                    {
                        rbEarning.Checked = true;
                    }
                    else
                    {
                        rbPay.Checked = true;
                    }
                    txtDescription.Text = transaction.Description;
                    txtPrice.Value = transaction.Price;
                }
            }

        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCustomerName.Text = dgvCustomers.CurrentRow.Cells["FullName"].Value.ToString();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnAddAccounting_Click(object sender, EventArgs e)
        {
            if(BaseValidator.IsFormValid(this.components))
            {
                if(rbPay.Checked || rbEarning.Checked)
                {
                    DataLayer.Accounting transaction = new DataLayer.Accounting()
                    {
                        Price = int.Parse(txtPrice.Value.ToString()),
                        Description = txtDescription.Text,
                        Date = DateTime.Now,
                        CustomerId = int.Parse(dgvCustomers.CurrentRow.Cells["CustomerId"].Value.ToString()),
                        TypeId=(rbEarning.Checked==true) ? 1:2 ,
                    };
                    
                    using(UnitOfWork _db=new UnitOfWork())
                    {
                        if(TransactionId==0)
                        {
                            _db.AccountingRepository.InsertTransaction(transaction);
                        }
                        else
                        {
                            transaction.AccountingId = TransactionId;
                            _db.AccountingRepository.UpdatetTransaction(transaction);
                        }
                        _db.Save();
                    }

                    DialogResult = DialogResult.OK;
                }
            }

            else
            {
                MessageBox.Show("لطفا نوع تراکنش را انتخاب نمایید.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
