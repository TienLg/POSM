using System;
using System.Linq;
using System.Windows.Forms;
using POSManagement.Models;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data.Entity;
using System.Collections.Generic;

namespace POSManagement.Views
{
    public partial class ProductEditorControl : UserControl
    {
        public ProductEditorControl()
        {
            InitializeComponent();
            adjustGridView();
            registerHandlers();
            AutoSize = true;
        }

        private void registerHandlers()
        {
            txtId.KeyPress += this.inputValidator;
            txtQC.KeyPress += this.inputValidator;
            txtSalePriceStock.KeyPress += this.inputValidator;
        }

        public void setProductDelegateCallbackFn(Product product)
        {
            setProductInfo(product);
        }

        private void setProductInfo(Product product)
        {
            txtId.Text = product.prod_id.ToString();
            txtName.Text = product.prod_name.ToString();
            txtQC.Text = product.quantity_control.ToString();
            txtSalePriceStock.Text = product.sale_price_by_stock.ToString();
            txtSalePriceUnit.Text = product.sale_price_by_unit.ToString();

            using (var db = new SMEntities())
            {
                Product prod = db.Products.Where(p => p.prod_id.Equals(product.prod_id))
                    .Include(p => p.Stocks)
                    .First();
                prod.Stocks.OrderByDescending(s => s.quantity_by_stock);
                dataGridView.DataSource = prod.Stocks.ToList();

                decimal total = 0;
                foreach (var stock in prod.Stocks)
                {
                    total += stock.base_price_by_stock * stock.quantity_by_stock +
                        stock.base_price_by_unit * stock.quantity_by_unit;
                }
                lblTotal.Text = total.ToString("#,##0.000");
            }
        }

        private void adjustGridView()
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.ColumnCount = 5;

            int index = 0;
            // Add column Total Price
            dataGridView.Columns[index].Name = "quantity_control";
            dataGridView.Columns[index].DataPropertyName = "quantity_control";
            dataGridView.Columns[index].HeaderText = "Quy cách";
            dataGridView.Columns[index].Width = 40;


            // Add column Total Price
            index++;
            dataGridView.Columns[index].Name = "quantity_by_stock";
            dataGridView.Columns[index].DataPropertyName = "quantity_by_stock";
            dataGridView.Columns[index].HeaderText = "Số lượng theo quy cách";

            // Add column Total Price
            index++;
            dataGridView.Columns[index].Name = "quantity_by_unit";
            dataGridView.Columns[index].DataPropertyName = "quantity_by_unit";
            dataGridView.Columns[index].HeaderText = "Số lượng theo đơn vị";

            // Add column Status
            index++;
            dataGridView.Columns[index].Name = "base_price_by_stock";
            dataGridView.Columns[index].DataPropertyName = "base_price_by_stock";
            dataGridView.Columns[index].HeaderText = "Giá gốc";

            // Add column Status
            index++;
            dataGridView.Columns[index].Name = "base_price_by_stock";
            dataGridView.Columns[index].DataPropertyName = "date_expired";
            dataGridView.Columns[index].HeaderText = "Ngày hết hạn";
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if(txtId.Text == string.Empty || txtName.Text == string.Empty ||
                txtQC.Text == string.Empty || txtSalePriceStock.Text == string.Empty)
            {
                MessageBox.Show("Chưa nhập đủ dữ liệu sản phẩm");
                return;
            }

            using (var db = new SMEntities())
            {
                // Bind DataSource
                Product p = new Product();
                p.prod_id = txtId.Text.ToString();
                p.prod_name = txtName.Text.ToString();
                p.quantity_control = int.Parse(txtQC.Text.ToString());

                p.sale_price_by_stock = decimal.Parse(txtSalePriceStock.Text.ToString());
                p.sale_price_by_unit = decimal.Parse(txtSalePriceUnit.Text.ToString());

                // Get default values
                p.quantity_by_stock = 0;
                p.quantity_by_unit = 0;
                p.date_added = DateTime.Now;
                p.date_modified = p.date_added;

                // Check as if products has been existed
                var query = db.Products.Where(t => t.prod_id.Equals(p.prod_id));
                if (!query.Any())// Not exist
                {
                    db.Products.Add(p);
                    db.SaveChanges();
                    clearEntries();
                }
                else
                {
                    // Accept change Name and sale price
                    var result = MessageBox.Show(this, "Sản phẩm đã có trong dữ liệu.\n Bạn có muốn lưu tên và giá bán hiện tại?",
                         "Lưu sản phẩm hàng",
                         MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        Product updatePro = query.First();
                        updatePro.prod_name = p.prod_name;
                        updatePro.sale_price_by_stock = p.sale_price_by_stock;
                        updatePro.sale_price_by_unit = p.sale_price_by_unit;
                        db.SaveChanges();
                        clearEntries();
                    }
                }
            }
        }

        private void toolStripNew_Click(object sender, EventArgs e)
        {
            // Clear all text boxs
            clearEntries();
        }

        private void clearEntries()
        {
            txtId.Clear();
            txtName.Clear();
            txtSalePriceStock.Clear();
            txtSalePriceUnit.Clear();
            txtQC.Clear();
            lblTotal.Text = "0";
            dataGridView.DataSource = new List<Product>();
        }

        private void toolStripImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                object missing = Type.Missing;
                Excel.Range range;
                Excel.Worksheet sheet = null;
                Excel.Workbook wb = null;

                Excel.Application excelApp = new Excel.Application();
                wb = excelApp.Workbooks.Open(dlg.FileName,
                        missing, missing, missing,
                        missing, missing, missing, missing,
                        missing, missing, missing, missing,
                        missing, missing, missing);
                sheet = (Excel.Worksheet)wb.Worksheets[1];
                range = sheet.UsedRange;

                if (range != null)
                {
                    using (var db = new SMEntities())
                    {
                        // Bind DataSource
                        int nRows = range.Rows.Count;
                        int num;
                        decimal d;
                        bool parsed;
                        for (int iRow = 2; iRow <= nRows; iRow++)
                        {
                            Product p = new Product();
                            // Get ID
                            range = (Excel.Range)sheet.Cells[iRow, 2];
                            p.prod_id = range.Text;
                            // Get Name
                            range = (Excel.Range)sheet.Cells[iRow, 3];
                            p.prod_name = range.Text;

                            // Get Quality_control
                            range = (Excel.Range)sheet.Cells[iRow, 4];
                            parsed = int.TryParse(range.Text, out num);
                            if (parsed)
                            {
                                p.quantity_control = num;
                            }
                            else p.quantity_control = 1;

                            // Get Sale_price_by_stock
                            range = (Excel.Range)sheet.Cells[iRow, 5];
                            parsed = decimal.TryParse(range.Text, out d);
                            if (parsed)
                            {
                                p.sale_price_by_stock = d;
                            }
                            else p.sale_price_by_stock = 0;

                            // Set sale_price_by_unit
                            range = (Excel.Range)sheet.Cells[iRow, 6];
                            parsed = decimal.TryParse(range.Text, out d);
                            if (parsed)
                            {
                                p.sale_price_by_unit = d;
                            }
                            else p.sale_price_by_unit = 0;

                            // Check as if product has been existed
                            var prod = db.Products.Where(t => t.prod_id.Equals(p.prod_id));
                            if (!prod.Any())//Not exist
                            {
                                db.Products.Add(p);
                                db.SaveChanges();
                            }
                        }
                    } 
                }

                wb.Close();
                excelApp.Quit();

                Marshal.ReleaseComObject(wb);
                Marshal.ReleaseComObject(sheet);
            }
        }

        private void inputValidator(object sender, KeyPressEventArgs e)
        {
            TextBox t;
            if (sender is TextBox)
            {
                t = (TextBox)sender;
                if (t.Name == txtId.Name)
                {
                    // If not Letter or digit or delete then disallow the character
                    // 8 is Backspace character
                    if (!Char.IsLetterOrDigit(e.KeyChar) & e.KeyChar != (char)8)
                        e.Handled = true;
                }
                if (t.Name == txtQC.Name)
                {
                    // If new input is not integer then disallow the characte
                    int d;
                    string enterNumber = txtSalePriceStock.Text + e.KeyChar.ToString();
                    bool parsed = int.TryParse(enterNumber, out d);
                    if (!parsed)
                        e.Handled = true;
                    // But accept backspace
                    if( e.KeyChar == (char)8)
                        e.Handled = false;
                }
                if (t.Name == txtSalePriceStock.Name || t.Name == txtSalePriceUnit.Name)
                {
                    // If new input is not decimal then disallow the characte
                    decimal d;
                    string enterNumber = txtSalePriceStock.Text + e.KeyChar.ToString();
                    bool parsed = decimal.TryParse(enterNumber, out d);
                    if(!parsed)
                        e.Handled = true;
                    // But accept backspace
                    if (e.KeyChar == (char)8)
                        e.Handled = false;
                }
            }
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null)
                return;
            if (e.ColumnIndex == 3) // Format Product Name
            {
                e.Value = Convert.ToDecimal(e.Value).ToString("#,##0.000");
            }
            if (e.ColumnIndex == 4) // Format Product Name
            {
                e.Value = String.Format("{0:MM/dd/yyyy}", e.Value);
            }
        }
    }
}
