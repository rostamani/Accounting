using Accounting.DataLayer;
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
using System.IO;

namespace Accounting.App
{
    public partial class frmAddOrEditCustomer : Form
    {
        private int CustomerId;

        public frmAddOrEditCustomer(int customerId)
        {
            this.CustomerId = customerId;
            InitializeComponent();
        }

        private void btnSelectPhoto_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                pbCustomerImage.ImageLocation = openFileDialog1.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (BaseValidator.IsFormValid(this.components))
            {
                string imageUniqueName = Guid.NewGuid().ToString() + Path.GetExtension(pbCustomerImage.ImageLocation);
                string imagePath = Application.StartupPath + "/Images/";
                if (!Directory.Exists(imagePath))
                {
                    Directory.CreateDirectory(imagePath);
                }
                pbCustomerImage.Image.Save(imagePath + imageUniqueName);
                Customers customer = new Customers()
                {
                    Name = txtName.Text,
                    Email = txtEmail.Text,
                    Address = txtAddress.Text,
                    Mobile = txtMobile.Text,
                    CustomerImage = imageUniqueName
                };
                using (UnitOfWork _db = new UnitOfWork())
                {
                    if (this.CustomerId == 0)
                        _db.CustomerRepository.InsertCustomer(customer);
                    else
                    {
                        customer.CustomerId = CustomerId;
                        _db.CustomerRepository.UpdatetCustomer(customer);
                    }

                    _db.Save();
                }

                DialogResult = DialogResult.OK;
            }
        }

        private void frmAddOrEditCustomer_Load(object sender, EventArgs e)
        {
            if (CustomerId != 0)
            {
                this.Text = "ویرایش شخص";
                btnSave.Text = "ویرایش";
                using (UnitOfWork _db = new UnitOfWork())
                {
                    var customer = _db.CustomerRepository.GetCustomerById(CustomerId);
                    txtAddress.Text = customer.Address;
                    txtName.Text = customer.Name;
                    txtMobile.Text = customer.Mobile;
                    txtEmail.Text = customer.Email;
                    pbCustomerImage.ImageLocation = Application.StartupPath + "/Images/" + customer.CustomerImage;
                }
                
            }
        }
    }
}
