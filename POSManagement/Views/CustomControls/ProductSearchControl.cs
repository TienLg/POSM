using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POSManagement.Repositories;
using POSManagement.Models;

namespace POSManagement.Views
{
    public partial class ProductSearchControl : UserControl
    {
        public delegate void SetProductDelegate(Product product);

        public SetProductDelegate SetProductDelegateCallback;
        public ProductSearchControl()
        {
            InitializeComponent();
            AdjustGridView();
            dataGridView.CellMouseUp += DataGridView_CellMouseUp;
        }

        private void DataGridView_CellMouseUp(Object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip.Show(this.dataGridView, e.Location);
                contextMenuStrip.Show(Cursor.Position);
            }
        }

        public void addCallbacksFn(ProductEditorControl editor)
        {
            this.SetProductDelegateCallback += new SetProductDelegate(editor.setProductDelegateCallbackFn);
        }

        private void AdjustGridView()
        {
            // Show specific columns of Product
            dataGridView.AutoGenerateColumns = false;
            dataGridView.ColumnCount = 7;
            int index = 0;

            // Add column Id
            dataGridView.Columns[index].Name = "ProductId";
            dataGridView.Columns[index].DataPropertyName = "prod_id";
            dataGridView.Columns[index].HeaderText = "Mã";
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;


            // Add column Name
            index++;
            dataGridView.Columns[index].Name = "ProductName";
            dataGridView.Columns[index].DataPropertyName = "prod_name";
            dataGridView.Columns[index].HeaderText = "Tên";
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // Add column QuantityControl
            index++;
            dataGridView.Columns[index].Name = "QuantityControl";
            dataGridView.Columns[index].DataPropertyName = "quantity_control";
            dataGridView.Columns[index].HeaderText = "Quy cách";
            dataGridView.Columns[index].Width = 50;

            // Add column Quantity
            index++;
            dataGridView.Columns[index].Name = "Quantity";
            dataGridView.Columns[index].DataPropertyName = "quantity_by_stock";
            dataGridView.Columns[index].HeaderText = "Số lượng theo quy cách";
            dataGridView.Columns[index].Width = 50;

            // Add column BasePrice
            index++;
            dataGridView.Columns[index].Name = "BasePrice";
            dataGridView.Columns[index].DataPropertyName = "quantity_by_unit";
            dataGridView.Columns[index].HeaderText = "Số lượng theo đơn vị";
            dataGridView.Columns[index].Width = 50;

            // Add column SalePrice
            index++;
            dataGridView.Columns[index].Name = "SalePrice";
            dataGridView.Columns[index].DataPropertyName = "sale_price_by_stock";
            dataGridView.Columns[index].HeaderText = "Giá bán theo quy cách";

            // Add column SalePrice
            index++;
            dataGridView.Columns[index].Name = "SalePrice";
            dataGridView.Columns[index].DataPropertyName = "sale_price_by_unit";
            dataGridView.Columns[index].HeaderText = "Giá bán theo đơn vị";
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }

        private void BindData()
        {
            using (var db = new SMEntities())
            {
                // Bind DataSource
                var proc = db.Products.Where(p => true);
                if (txtId.Text != string.Empty)
                    proc = proc.Where(p => p.prod_id.Contains(txtId.Text));
                if (txtName.Text != string.Empty)
                    proc = proc.Where(p => p.prod_name.Contains(txtName.Text));
                dataGridView.DataSource = proc.ToList();
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow != null)
            {
                Product prod = (Product)dataGridView.CurrentRow.DataBoundItem;
                if(prod != null)
                    SetProductDelegateCallback(prod);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null)
                return;
            if (e.ColumnIndex == 0 || e.ColumnIndex == 1) // Format Product ID & Name
            {
                e.Value = e.Value.ToString().Trim();
            }
            if (e.ColumnIndex == 5 || e.ColumnIndex == 6) // Format Price
            {
                e.Value = Convert.ToDecimal(e.Value).ToString("#,##0");
            }
        }

    }
}
