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
using Accounting.Utilities;
using Accounting.ViewModels;

namespace Accounting.App
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            
            InitializeComponent();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            frmCustomers frmCustomers = new frmCustomers();
            frmCustomers.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            frmAddNewAccounting frmAddNewAccounting = new frmAddNewAccounting(0);
            frmAddNewAccounting.ShowDialog();
        }

        private void btnPayTransactionReports_Click(object sender, EventArgs e)
        {
            frmTransactionReport frmTransactionReport = new frmTransactionReport(2);
            frmTransactionReport.ShowDialog();
        }

        private void btnReceiveTransactionReports_Click(object sender, EventArgs e)
        {
            frmTransactionReport frmTransactionReport = new frmTransactionReport(1);
            frmTransactionReport.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.ToPersianDate();
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss");
            Refresh();
            this.Hide();
            frmLogin frmLogin = new frmLogin(0);
            if(frmLogin.ShowDialog()==DialogResult.OK)
            {
                this.Show();
            }
            else
            {
                Application.Exit();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss");
        }

        private void تنظیماتورودToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin frmLogin = new frmLogin(1);
            frmLogin.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        void Refresh()
        {
            using(UnitOfWork _db=new UnitOfWork())
            {
                MonthlyReportViewModel report = _db.AccountingRepository.GetReport();
                lblBalance.Text = report.Balance.ToString("#,0");
                lblPay.Text = report.Pay.ToString("#,0");
                lblReceive.Text = report.Receive.ToString("#,0");
            }
        }
    }
}
