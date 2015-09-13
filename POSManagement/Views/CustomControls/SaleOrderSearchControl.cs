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
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data.Entity;
using POSManagement.Repositories;
using POSManagement.Views.Controls;

namespace POSManagement.Views
{
    public partial class SaleOrderSearchControl : UserControl
    {
        public delegate void SetSaleOrderDelegate(SaleOrder saleOrder);

        public SetSaleOrderDelegate SetSaleOrderDelegateCallback;
        public SaleOrderSearchControl()
        {
            InitializeComponent();
            AdjustGridView();
            //BindData();
        }

        public void addCallbacksFn(SaleOrderControl editor)
        {
            this.SetSaleOrderDelegateCallback += new SetSaleOrderDelegate(editor.SetSaleOrderDelegateCallbackFn);
        }

        private void AdjustGridView()
        {
            // Show specific columns of Product
            dataGridView.AutoGenerateColumns = false;
            dataGridView.ColumnCount = 5;
            int index = 0;

            // Add column Customer Name
            dataGridView.Columns[index].Name = "CustomerName";
            dataGridView.Columns[index].DataPropertyName = "CustomerName";
            dataGridView.Columns[index].HeaderText = "Tên khách hàng";
            dataGridView.Columns[index].Width= 80;


            // Add column Saler Name
            //index++;
            //dataGridView.Columns[index].Name = "SalerName";
            //dataGridView.Columns[index].DataPropertyName = "SalerName";
            //dataGridView.Columns[index].HeaderText = "Saler";
            //dataGridView.Columns[index].Width = 80;

            // Add column Total Price
            index++;
            dataGridView.Columns[index].Name = "TotalPrice";
            dataGridView.Columns[index].DataPropertyName = "total_price";
            dataGridView.Columns[index].HeaderText = "Tổng giá bán";
            dataGridView.Columns[index].Width = 80;

            // Add column Taken Money
            index++;
            dataGridView.Columns[index].Name = "TakenMoney";
            dataGridView.Columns[index].DataPropertyName = "taken_money";
            dataGridView.Columns[index].HeaderText = "Tiền đã nhận";
            dataGridView.Columns[index].Width = 80;

            // Add column Status
            index++;
            dataGridView.Columns[index].Name = "Status";
            dataGridView.Columns[index].DataPropertyName = "order_status";
            dataGridView.Columns[index].HeaderText = "Tình trạng";
            dataGridView.Columns[index].Width = 80;

            // Add column Shipdate
            index++;
            dataGridView.Columns[index].Name = "Status";
            dataGridView.Columns[index].DataPropertyName = "date_ship";
            dataGridView.Columns[index].HeaderText = "Ngày giao hàng";
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void BindData()
        {
            using (var db = new SMEntities())
            {
                // Bind DataSource
                var so = db.SaleOrders.Where(o => true);
                if (txtCustomer.Text != string.Empty)
                    so = so.Where(p => p.Customer.cust_name.Contains(txtCustomer.Text));

                if (chbDate.Checked)
                {
                    so = so.Where(o => o.date_ship.Year >= dtpFrom.Value.Year &
                        o.date_ship.Month >= dtpFrom.Value.Month &
                        o.date_ship.Day >= dtpFrom.Value.Day &
                        o.date_ship.Year <= dtpTo.Value.Year &
                        o.date_ship.Month <= dtpTo.Value.Month &
                        o.date_ship.Day <= dtpTo.Value.Day);
                }

                if (cbbShipStatus.SelectedItem != null && !cbbShipStatus.SelectedItem.Equals("All"))
                {
                    so = so.Where(o => o.order_status.Equals(cbbShipStatus.SelectedItem.ToString()));
                }

                so = so.Include(o => o.Customer)
                    .Include(o => o.User);

                foreach (var s in so)
                {
                    s.CustomerName = s.Customer.cust_name;
                }
                dataGridView.DataSource = so.ToList();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow != null)
            {
                SaleOrder so = (SaleOrder)dataGridView.CurrentRow.DataBoundItem;
                if (so != null)
                    SetSaleOrderDelegateCallback(so);
            }
        }

        internal void addCallbacksFn(Views.Controls.ImportOrderControl soc)
        {
            //throw new NotImplementedException();
        }

        private void txtCustomer_Enter(object sender, EventArgs e)
        {
            CustomerSearchDialog dialog = new CustomerSearchDialog();
            dialog.ShowDialog(this);

            if (dialog.DialogResult == DialogResult.OK)
            {
                Customer c = dialog.ReturnCustomer();
                if (c != null)
                {
                    txtCustomer.Text = c.cust_name.Trim();
                }
            }
        }
    }
}
