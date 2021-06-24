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
using Accounting.Utilities;
using Accounting.ViewModels;

namespace Accounting.App
{
    public partial class frmTransactionReport : Form
    {
        private int TransactionType;

        public frmTransactionReport(int transactionType)
        {
            this.TransactionType = transactionType;
            InitializeComponent();
        }

        private void frmTransactionReport_Load(object sender, EventArgs e)
        {
            if (TransactionType == 1)
            {
                this.Text = "گزارش دریافتی ها";
            }
            else
            {
                this.Text = "گزارش پرداختی ها";
            }
            using(UnitOfWork _db=new UnitOfWork())
            {
                List<CustomersNamesViewModel> customers = new List<CustomersNamesViewModel>();
                customers.Add(new CustomersNamesViewModel()
                {
                    CustomerId = 0,
                    Name = "لطفا انتخاب کنید"
                });
                customers.AddRange(_db.CustomerRepository.GetCustomersNames());
                cbCustomer.DataSource =customers;
                cbCustomer.DisplayMember = "Name";
                cbCustomer.ValueMember = "CustomerId";
            }
            
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            dgvReport.AutoGenerateColumns = false;
            dgvReport.Rows.Clear();
            using (UnitOfWork _db = new UnitOfWork())
            {
                Nullable<DateTime> fromDate=null;
                Nullable<DateTime> toDate=null;
                //DateTime? fromDate;
                //DateTime? toDate;
                if(txtFromDate.Text!= "    /  /")
                {
                    fromDate=Convert.ToDateTime(txtFromDate.Text);
                    fromDate = DateConvertors.ToMiladi(fromDate.Value);
                }
                
                if(txtToDate.Text!= "    /  /")
                {
                    toDate = Convert.ToDateTime(txtToDate.Text);
                    toDate = DateConvertors.ToMiladi(toDate.Value);
                }
                var reports = _db.AccountingRepository.GetAllTransactions(TransactionType,(int)cbCustomer.SelectedValue,fromDate,toDate);               
                foreach (var transaction in reports)
                {
                    var customerName = _db.CustomerRepository.GetCustomerById(transaction.CustomerId).Name;
                    dgvReport.Rows.Add(transaction.AccountingId, customerName, transaction.Price, transaction.Date.ToPersianDate(), transaction.Description);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvReport.CurrentRow == null)
                MessageBox.Show("لطفا ابتدا یک تراکنش را انتخاب کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                DialogResult dr = MessageBox.Show("آیا از حذف این تراکنش اطمینان دارید؟", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (dr == DialogResult.Yes)
                {
                    using (UnitOfWork _db = new UnitOfWork())
                    {
                        _db.AccountingRepository.DeleteTransaction(int.Parse(dgvReport.CurrentRow.Cells["AccountingId"].Value.ToString()));
                        _db.Save();                        
                        BindGrid();
                    }
                }

            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(dgvReport.CurrentRow==null)
                MessageBox.Show("لطفا ابتدا یک تراکنش را انتخاب کنید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                int transactionId = int.Parse(dgvReport.CurrentRow.Cells["AccountingId"].Value.ToString());
                frmAddNewAccounting frmAddNewAccounting = new frmAddNewAccounting(transactionId);
                if(frmAddNewAccounting.ShowDialog()==DialogResult.OK)
                    BindGrid();
            }
        }
    }
}
