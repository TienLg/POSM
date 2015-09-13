using POSManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSManagement.Views.Controls
{
    public partial class CustomerSearchDialog : Form
    {
        public CustomerSearchDialog()
        {
            InitializeComponent();
            adjustGridView();
            BindData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void adjustGridView()
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.ColumnCount = 3;
            dataGridView.RowCount = 50;

            int index = 0;

            // Add column Customer Name
            dataGridView.Columns[index].Name = "CustomerName";
            dataGridView.Columns[index].DataPropertyName = "cust_name";
            dataGridView.Columns[index].HeaderText = "Customer Name";

            // Add column Saler Name
            index++;
            dataGridView.Columns[index].Name = "CustomerAddress";
            dataGridView.Columns[index].DataPropertyName = "cust_address";
            dataGridView.Columns[index].HeaderText = "Address";

            // Add column Total Price
            index++;
            dataGridView.Columns[index].Name = "CustomerPhone";
            dataGridView.Columns[index].DataPropertyName = "cust_phone";
            dataGridView.Columns[index].HeaderText = "Phone";
        }

        private void BindData()
        {
            using (var db = new SMEntities()){
                var cust = db.Customers.Where(c => true);
                if (txtCustName.Text != String.Empty)
                    cust = cust.Where(c => c.cust_name.Contains(txtCustName.Text));
                dataGridView.DataSource = cust.ToList();
            }
        }

        public Customer ReturnCustomer()
        {
            if (dataGridView.CurrentRow != null)
                return (Customer)dataGridView.CurrentRow.DataBoundItem;
            else
                return null;
        }

        private void btnCreateNew_Click(object sender, EventArgs e)
        {
            using(var db = new SMEntities()){
                DialogResult result = MessageBox.Show("Bạn muốn tạo mới khách hàng với thông tin như trên?",
                    "Confirmation", 
                    MessageBoxButtons.YesNoCancel);
                if(result == DialogResult.Yes)
                {
                    if (txtCustName.Text == String.Empty || txtAddress.Text == String.Empty)
                    {
                        MessageBox.Show("Nhập thiếu thông tin khách hàng");
                        return;
                    }
                    Customer c = db.Customers.Create();
                    c.cust_name = txtCustName.Text;
                    c.cust_phone = txtPhone.Text;
                    c.cust_address = txtAddress.Text;

                    db.Customers.Add(c);
                    db.SaveChanges();
                }
            }
        }
    }
}
