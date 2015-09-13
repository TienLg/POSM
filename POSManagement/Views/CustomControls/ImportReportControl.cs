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
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Linq;

namespace POSManagement.Views.Controls
{
    public partial class ImportReportControl : UserControl
    {
        private SMEntities db;
        public ImportReportControl()
        {
            InitializeComponent();
            db = new SMEntities();
            adjustGridView();
        }

        private void adjustGridView()
        {
            //dataGridView.DataError += new DataGridViewDataErrorEventHandler(this.dataGridView_DataError);
            dataGridView.AutoGenerateColumns = false;
            dataGridView.ColumnCount = 5;

            // Add column Product ID
            int index = 0;
            dataGridView.Columns[index].Name = "ProductID";
            dataGridView.Columns[index].DataPropertyName = "ProductId";
            dataGridView.Columns[index].HeaderText = "Mã";
            dataGridView.Columns[index].ReadOnly = true;
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            //// Add column Product Name
            index++;
            dataGridView.Columns[index].Name = "ProductName";
            dataGridView.Columns[index].DataPropertyName = "ProductName";
            dataGridView.Columns[index].HeaderText = "Tên";
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView.Columns[index].ReadOnly = true;

            // Add column Total Stock
            index++;
            dataGridView.Columns[index].Name = "QuantityByStock";
            dataGridView.Columns[index].DataPropertyName = "TotalStock";
            dataGridView.Columns[index].HeaderText = "Tổng số lượng theo quy cách";

            // Add column Total Unit
            index++;
            dataGridView.Columns[index].Name = "QuantityByUnit";
            dataGridView.Columns[index].DataPropertyName = "TotalUnit";
            dataGridView.Columns[index].HeaderText = "Tổn số lượng theo đơn vị";

            // Add column Total item Price
            index++;
            dataGridView.Columns[index].Name = "TotalItemPrice";
            dataGridView.Columns[index].DataPropertyName = "TotalMoney";
            dataGridView.Columns[index].HeaderText = "Tổng tiền";
            dataGridView.Columns[index].ReadOnly = true;
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void BindData()
        {
            // First get all import order
            var orders = db.ImportOrders.Where(o => true);
            // If search by Date
            if (chbDate.Checked)
            {
                orders = orders.Where(o => o.date_import.Year >= dtpFrom.Value.Year &
                    o.date_import.Month >= dtpFrom.Value.Month &
                    o.date_import.Day >= dtpFrom.Value.Day &
                    o.date_import.Year <= dtpTo.Value.Year &
                    o.date_import.Month <= dtpTo.Value.Month &
                    o.date_import.Day <= dtpTo.Value.Day);
            }

            List<ImportOrderItem> itemsList = new List<ImportOrderItem>();
            foreach (var order in orders)
            {
                itemsList.AddRange(order.ImportOrderItems);
            }
            // Query result
            //var result = from item in db.ImportOrderItems
            //             join order in orders
            //             on item.order_id equals order.order_id
            var result = from item in itemsList
                         where item.prod_id.Contains(txtProductID.Text)
                         group item by item.prod_id
                             into g
                             select new
                             {
                                 ProductId = g.Key,
                                 ProductName = (from p in db.Products
                                                where p.prod_id.Equals(g.Key)
                                                select p.prod_name)
                                                        .FirstOrDefault(),
                                 TotalStock = g.Sum(i => i.quantity_by_stock),
                                 TotalUnit = g.Sum(i => i.quantity_by_unit),
                                 TotalMoney = g.Sum(i => i.base_price_by_stock * i.quantity_by_stock + i.quantity_by_unit * i.base_price_by_stock / i.quantity_control)
                             };

            // Count total
            int totalStock = 0;
            int totalUnit = 0;
            decimal totalMoney = 0;
            foreach (var prod in result)
            {
                totalStock += prod.TotalStock;
                totalUnit += prod.TotalUnit;
                totalMoney += prod.TotalMoney;
            }

            // Update label
            lblMoneyTotal.Text = totalMoney.ToString("#,##0.000");
            lblUnitTotal.Text = totalUnit.ToString();
            lblStockTotal.Text = totalStock.ToString();

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
            if (e.ColumnIndex == 0) // Format Product ID
            {
                e.Value = e.Value.ToString().Trim();
            }
            if (e.ColumnIndex == 1) // Format Product Name
            {
                e.Value = e.Value.ToString().Trim();
            }
            if (e.ColumnIndex == 4) // Format Product Name
            {
                e.Value = Convert.ToDecimal(e.Value).ToString("#,##0.000");
            }
        }
    }
}
