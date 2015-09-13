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
using System.Data.Entity;

namespace POSManagement.Views.Controls
{
    public partial class ProductSearchDialog : Form
    {
        public ProductSearchDialog()
        {
            InitializeComponent();
            AdjustGridView();
            BindData();
            this.AutoSize = true;
        }

        private void AdjustGridView()
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.ColumnCount = 7;
            int index = 0;

            // Column 0 Add column Product ID
            dataGridView.Columns[index].Name = "ProductID";
            dataGridView.Columns[index].DataPropertyName = "prod_id";
            dataGridView.Columns[index].HeaderText = "Mã";
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // Column 1 Add column Product Name
            index++;
            dataGridView.Columns[index].Name = "ProductName";
            dataGridView.Columns[index].DataPropertyName = "prod_name";
            dataGridView.Columns[index].HeaderText = "Name";
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // Column 2 Add column Quantity Control
            index++;
            dataGridView.Columns[index].Name = "ProductQuantity";
            dataGridView.Columns[index].DataPropertyName = "quantity_control";
            dataGridView.Columns[index].HeaderText = "Quy cách";
            dataGridView.Columns[index].Width = 50;

            // Column 3 Add column Quantity By Stock
            index++;
            dataGridView.Columns[index].Name = "ProductQuantity";
            dataGridView.Columns[index].DataPropertyName = "quantity_by_stock";
            dataGridView.Columns[index].HeaderText = "Số lượng theo quy cách";
            dataGridView.Columns[index].Width = 50;

            // Column 4 Add column Quantity By Unit
            index++;
            dataGridView.Columns[index].Name = "ProductQuantity";
            dataGridView.Columns[index].DataPropertyName = "quantity_by_unit";
            dataGridView.Columns[index].HeaderText = "Số lượng theo đơn vị";
            dataGridView.Columns[index].Width = 50;

            // Column 5 Add column Sale Price By Stock
            index++;
            dataGridView.Columns[index].Name = "ProductPrice";
            dataGridView.Columns[index].DataPropertyName = "sale_price_by_stock";
            dataGridView.Columns[index].HeaderText = "Giá bán theo quy cách";

            // Column 6 Add column Sale Price By Unit
            index++;
            dataGridView.Columns[index].Name = "ProductPrice";
            dataGridView.Columns[index].DataPropertyName = "sale_price_by_unit";
            dataGridView.Columns[index].HeaderText = "Giá bán theo đơn vị";
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }

        private void BindData()
        {
            using (var db = new SMEntities())
            {
                var prods = db.Products.Where(c => true);
                if (txtProductID.Text != String.Empty)
                    prods = prods.Where(c => c.prod_id.Contains(txtProductID.Text));
                if (txtProductName.Text != String.Empty)
                    prods = prods.Where(c => c.prod_name.Contains(txtProductName.Text));
                dataGridView.DataSource = prods.ToList();
                dataGridView.ClearSelection();
            }
        }

        public Product ReturnProduct()
        {
            if (dataGridView.CurrentRow != null)
                return (Product)dataGridView.CurrentRow.DataBoundItem;
            else
                return null;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null)
                return;
            if (e.ColumnIndex == 0 || e.ColumnIndex == 1)
            {
                e.Value = e.Value.ToString().Trim();
            }
            if (e.ColumnIndex == 5 || e.ColumnIndex == 6)
            {
                e.Value = Convert.ToDecimal(e.Value).ToString("#,##0");
            }
        }
    }
}
