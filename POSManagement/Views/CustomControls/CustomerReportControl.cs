using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POSManagement.Models;

namespace POSManagement.Views.Controls
{
    public partial class CustomerReportControl : UserControl
    {
        private SMEntities db;
        private Customer cust;
        public CustomerReportControl()
        {
            InitializeComponent();
            db = new SMEntities();
            adjustGridView();
        }

        private void adjustGridView()
        {
            //dataGridView.DataError += new DataGridViewDataErrorEventHandler(this.dataGridView_DataError);
            dataGridView.AutoGenerateColumns = false;
            dataGridView.ColumnCount = 4;

            // Add column Product ID
            int index = 0;
            dataGridView.Columns[index].Name = "ProductID";
            dataGridView.Columns[index].DataPropertyName = "CustomerName";
            dataGridView.Columns[index].HeaderText = "Tên khách hàng";
            dataGridView.Columns[index].ReadOnly = true;
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // Add column Total Stock
            index++;
            dataGridView.Columns[index].Name = "QuantityByStock";
            dataGridView.Columns[index].DataPropertyName = "TotalMoney";
            dataGridView.Columns[index].HeaderText = "Tổng tiền";

            // Add column Total Unit
            index++;
            dataGridView.Columns[index].Name = "QuantityByUnit";
            dataGridView.Columns[index].DataPropertyName = "TotalTaken";
            dataGridView.Columns[index].HeaderText = "Tổng tiền đã thu";

            // Add column Total item Price
            index++;
            dataGridView.Columns[index].Name = "TotalItemPrice";
            dataGridView.Columns[index].DataPropertyName = "TotalDept";
            dataGridView.Columns[index].HeaderText = "Tổng tiền chưa thu";
            dataGridView.Columns[index].ReadOnly = true;
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void BindData()
        {
            // First get all import order
            var orders = db.SaleOrders.Where(o => true);
            // Filter by customer
            if (cust != null)
            {
                orders = orders.Where(o => o.cust_id.Equals(cust.cust_id));
            }
            // If search by Date
            if (chbDate.Checked)
            {
                orders = orders.Where(o => o.date_created.Year >= dtpFrom.Value.Year &
                    o.date_created.Month >= dtpFrom.Value.Month &
                    o.date_created.Day >= dtpFrom.Value.Day &
                    o.date_created.Year <= dtpTo.Value.Year &
                    o.date_created.Month <= dtpTo.Value.Month &
                    o.date_created.Day <= dtpTo.Value.Day);
            }

            //List<SaleOrderItem> itemsList = new List<SaleOrderItem>();
            //foreach (var order in orders)
            //{
            //    itemsList.AddRange(order.SaleOrderItems);
            //}
            var result = from item in orders
                        group item by item.cust_id
                        into g
                        select new
                        {
                            CustomerID = g.Key,
                            CustomerName = (from c in db.Customers
                                        where c.cust_id.Equals(g.Key)
                                        select c.cust_name)
                                        .FirstOrDefault(),
                            TotalMoney = g.Sum(i => i.total_price),
                            TotalTaken = g.Sum(i => i.taken_money),
                            TotalDept = g.Sum(i => i.total_price) - g.Sum(i => i.taken_money)
                        };

            // Count total
            decimal totalMoney = 0;
            decimal totalTaken = 0;
            foreach (var order in result)
            {
                totalMoney += order.TotalMoney;
                totalTaken += order.TotalTaken;
            }

            // Update label
            lblMoneyTotal.Text = totalMoney.ToString("#,##0.000");
            lblTakenMoney.Text = totalTaken.ToString("#,##0.000");
            lblDeptTotal.Text = (totalMoney - totalTaken).ToString("#,##0.000");
            lblOrderNum.Text = orders.Count().ToString();

            // Update DS
            dataGridView.DataSource = result.ToList();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null)
                return;
            if (e.ColumnIndex == 0)
            {
                e.Value = e.Value.ToString().Trim();
            }
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 3) // Format Product Name
            {
                e.Value = Convert.ToDecimal(e.Value).ToString("#,##0.000");
            }
        }

        private void btnCustSearch_Click(object sender, EventArgs e)
        {
            CustomerSearchDialog dialog = new CustomerSearchDialog();
            dialog.ShowDialog(this);

            if (dialog.DialogResult == DialogResult.OK)
            {
                cust = dialog.ReturnCustomer();
                if (cust != null)
                {
                    txtCustName.Text = cust.cust_name.Trim();
                    txtAddress.Text = cust.cust_address.Trim();
                    txtPhone.Text = cust.cust_phone.Trim();
                }
            }
        }

        private void btnClearCust_Click(object sender, EventArgs e)
        {
            cust = null;
            txtCustName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtPhone.Text = string.Empty;
        }
    }
}
