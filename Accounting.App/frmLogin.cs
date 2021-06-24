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

namespace Accounting.App
{
    public partial class frmLogin : Form
    {
        private int EditEnable;
        public frmLogin(int editEnable)
        {
            this.EditEnable = editEnable;
            InitializeComponent();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                string username = txtUserName.Text;
                string password = txtPassword.Text;
                using (UnitOfWork _db = new UnitOfWork())
                {
                    if (EditEnable == 0)
                    {
                        if (_db.UserRepository.DoesExist(username, password))
                            DialogResult = DialogResult.OK;
                        else
                            MessageBox.Show("نام کاربری یا رمزعبور صحیح نمیباشد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        var user = _db.UserRepository.GetAdminInformation();


                        _db.Save();
                        MessageBox.Show(string.Format("تنظیمات با موفقیت تغییر یافت.لطفا دوباره وارد برنامه شوید"), "توجه!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Restart();

                    }


                }
            }

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (EditEnable == 1)
            {
                this.Text = "ویرایش تنظیمات ورود";
                btnlogin.Text = "ویرایش";
                using (UnitOfWork _db = new UnitOfWork())
                {
                    var user = _db.UserRepository.GetAdminInformation();
                    txtUserName.Text = user.Username;
                    txtPassword.Text = user.Password;
                }

            }
        }
    }
}
