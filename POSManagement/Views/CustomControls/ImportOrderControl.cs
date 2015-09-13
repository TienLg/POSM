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
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace POSManagement.Views.Controls
{
    public partial class ImportOrderControl : UserControl
    {
        enum OrderState { New = 1, Done = 2, OnProgress = 3 };
        private OrderState orderState;
        private SMEntities db;

        ImportOrder importOrder;
        public ImportOrderControl()
        {
            InitializeComponent();
            AdjustGridView();
            db = new SMEntities();
            newOrder();
        }

        private void newOrder()
        {
            clearEntries();
            importOrder = db.ImportOrders.Create();
            importOrder.user_id = 1;
            importOrder.order_status = "Done";
            importOrder.total_price = 0;
            importOrder.date_import = DateTime.Now;
            dataGridView.DataSource = new List<ImportOrderItem>();
            orderState = OrderState.New;
        }

        private void clearEntries()
        {
            cbbStatus.SelectedItem = "Done";
            lblTotal.Text = "0";
            lblItemNum.Text = "0";
        }

        private void AdjustGridView()
        {
            dataGridView.DataError += new DataGridViewDataErrorEventHandler(this.dataGridView_DataError);

            dataGridView.AutoGenerateColumns = false;
            dataGridView.ColumnCount = 8;

            // Add column Customer Name
            int index = 0;
            dataGridView.Columns[index].Name = "ProductID";
            dataGridView.Columns[index].DataPropertyName = "prod_id";
            dataGridView.Columns[index].HeaderText = "Mã";
            dataGridView.Columns[index].ReadOnly = true;
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // Add column Saler Name
            index++;
            dataGridView.Columns[index].Name = "ProductName";
            dataGridView.Columns[index].DataPropertyName = "ProductName";
            dataGridView.Columns[index].HeaderText = "Tên";
            dataGridView.Columns[index].ReadOnly = true;
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;


            // Add column Total Price
            index++;
            dataGridView.Columns[index].Name = "BasePrice";
            dataGridView.Columns[index].DataPropertyName = "quantity_control";
            dataGridView.Columns[index].HeaderText = "Quy cách";
            dataGridView.Columns[index].ReadOnly = true;
            dataGridView.Columns[index].Width = 40;

            // Add column Total Price
            index++;
            dataGridView.Columns[index].Name = "BasePrice";
            dataGridView.Columns[index].DataPropertyName = "quantity_by_stock";
            dataGridView.Columns[index].HeaderText = "Số lượng theo quy cách";

            // Add column Total Price
            index++;
            dataGridView.Columns[index].Name = "BasePrice";
            dataGridView.Columns[index].DataPropertyName = "quantity_by_unit";
            dataGridView.Columns[index].HeaderText = "Số lượng theo đơn vị";

            // Add column Status
            index++;
            dataGridView.Columns[index].Name = "Quantity";
            dataGridView.Columns[index].DataPropertyName = "base_price_by_stock";
            dataGridView.Columns[index].HeaderText = "Giá gốc";

            // Add column Status
            index++;
            dataGridView.Columns[index].Name = "Quantity";
            dataGridView.Columns[index].DataPropertyName = "TotalItemPrice";
            dataGridView.Columns[index].HeaderText = "Tổng";
            dataGridView.Columns[index].ReadOnly = true;

            // Add column date_expired
            index++;
            dataGridView.Columns[index].Name = "Quantity";
            dataGridView.Columns[index].DataPropertyName = "date_expired";
            dataGridView.Columns[index].HeaderText = "Ngày hết hạn";
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (orderState == OrderState.Done)
            {
                MessageBox.Show("Không thể chỉnh sửa khi đã nhập hàng hoàn tất.");
                return;
            }
            addProduct();
        }

        private void addProduct()
        {
            ProductSearchDialog dialog = new ProductSearchDialog();
            dialog.ShowDialog();
            if (dialog.DialogResult == DialogResult.OK)
            {
                Product p = dialog.ReturnProduct();

                // Add new Item
                ImportOrderItem item = db.ImportOrderItems.Create();
                item.prod_id = p.prod_id;
                item.ProductName = p.prod_name;

                item.quantity_control = p.quantity_control;
                item.quantity_by_stock = 0;
                item.quantity_by_unit = 0;

                item.base_price_by_stock = 0;
                item.base_price_by_unit = 0;

                item.date_expired = DateTime.Now;

                importOrder.ImportOrderItems.Add(item);
                dataGridView.DataSource = importOrder.ImportOrderItems.ToList();
            }
        }

        private void deleteItem()
        {
            if(dataGridView.CurrentRow != null)
            {
                ImportOrderItem item = (ImportOrderItem)dataGridView.CurrentRow.DataBoundItem;
                importOrder.ImportOrderItems.Remove(item);
                dataGridView.DataSource = importOrder.ImportOrderItems.ToList();
            }
        }

        private void toolStripSavebtn_Click(object sender, EventArgs e)
        {
            saveOrder();
            clearEntries();
            newOrder();
        }

        private bool validateOrder(){
            if (importOrder.ImportOrderItems.Count <= 0)
            {
                 MessageBox.Show("Bạn chưa thêm mặt hàng.");
                 return false;
            }
            return true;
        }

        private void saveStock(SMEntities db){
            // Save to Stocks
                var stockList = db.Stocks;
                foreach (var item in importOrder.ImportOrderItems)
                {
                    // Get stock has same prod_id & base_price_by_unit to item
                    var stock = stockList.Where(s => s.prod_id.Equals(item.prod_id)
                        && s.base_price_by_unit.Equals(item.base_price_by_unit));
                    if (stock.Any())// Exist
                    {
                        Stock st = stock.First();
                        st.quantity_by_stock += item.quantity_by_stock;
                        st.quantity_by_unit += item.quantity_by_unit;
                    }
                    else// Create new stock for the product
                    {
                        Stock newStock = db.Stocks.Create();
                        newStock.prod_id = item.prod_id;
                        newStock.Product = item.Product;
                        newStock.quantity_by_stock = item.quantity_by_stock;
                        newStock.quantity_by_unit = item.quantity_by_unit;
                        newStock.base_price_by_stock = item.base_price_by_stock;
                        newStock.base_price_by_unit = item.base_price_by_unit;
                        newStock.quantity_control = item.quantity_control;
                        newStock.date_expired = item.date_expired;

                        // Save to Stocks
                        db.Stocks.Add(newStock);
                    }

                    // Update quantity on Product
                    var prod = db.Products.Find(item.prod_id);
                    prod.quantity_by_stock += item.quantity_by_stock;
                    prod.quantity_by_unit += item.quantity_by_unit;
                    prod.date_modified = DateTime.Now;

                    //db.Products.Attach(prod);
                    db.Entry(prod).State = EntityState.Modified;
                }
        }
        private void saveNewOrder()
        {
            foreach (var item in importOrder.ImportOrderItems)
            {
                item.base_price_by_unit = item.base_price_by_stock / item.quantity_control;
                //importOrder.total_price += item.base_price_by_stock * item.quantity_by_stock +
                //    item.base_price_by_unit * item.quantity_by_unit;
            }

            // Save to ImportOrder
            db.ImportOrders.Add(importOrder);

            // Save stock
            saveStock(db);

            // Save changes
            db.SaveChanges();
        }

        private void saveRepairOrder(){
            if (orderState.Equals((int) OrderState.OnProgress)) // Repair OnProgress-original order
            {
                if (importOrder.order_status.Equals("Done"))
                {
                    db.Entry(importOrder).State = EntityState.Modified;
                    foreach (var item in importOrder.ImportOrderItems)
                    {
                        db.Entry(item).State = EntityState.Modified;
                    }
                    // Save stock
                    saveStock(db);
                }
                else
                {
                    db.Entry(importOrder).State = EntityState.Modified;
                    foreach (var item in importOrder.ImportOrderItems)
                    {
                        db.Entry(item).State = EntityState.Modified;
                    }// Do not save stocks
                }
            }
            else// Edit done-original order
            {
                // Only save state of order. Not order items.
                db.Entry(importOrder).State = EntityState.Modified;
            }

            // Save changes
            db.SaveChanges();
        }

        private void btnDeleteProd_Click(object sender, EventArgs e)
        {
            if (orderState == OrderState.Done)
            {
                MessageBox.Show("Không thể chỉnh sửa khi đã nhập hàng hoàn tất.");
                return;
            }
            deleteItem();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (orderState == OrderState.Done)
            {
                MessageBox.Show("Không thể chỉnh sửa khi đã nhập hàng hoàn tất.");
                return;
            }
            addProduct();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (orderState == OrderState.Done)
            {
                MessageBox.Show("Không thể chỉnh sửa khi đã nhập hàng hoàn tất.");
                return;
            }

            deleteItem();
        }

        private void dataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip.Show(this.dataGridView, e.Location);
                contextMenuStrip.Show(Cursor.Position);
            }
        }

        internal void SetOrderDelegateCallbackFn(ImportOrder order)
        {
            if (importOrder.ImportOrderItems.Count > 0)
            {
                var result = MessageBox.Show(this, 
                    "Bạn có muốn lưu lại đơn đang làm hay không?",
                    "Lưu đơn hàng",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    saveOrder();
                }
            }

            importOrder = db.ImportOrders.Where(i => i.order_id.Equals(order.order_id))
                //.Include(i => i.ImportOrderItems)
                .First();
            foreach (var item in importOrder.ImportOrderItems)
            {
                item.ProductName = item.Product.prod_name;
                item.TotalItemPrice = item.quantity_by_stock * item.base_price_by_stock +
                    item.base_price_by_stock * item.quantity_by_unit / item.quantity_control;
            }
            dataGridView.DataSource = importOrder.ImportOrderItems.ToList();
            updateLblTotal();
            if (importOrder.order_status.Trim() == "Done")
            {
                orderState = OrderState.Done;
                cbbStatus.Text = "Done";
            }
            else
            {
                orderState = OrderState.OnProgress;
                cbbStatus.Text = "On progress";
            }
        }

        private void saveOrder()
        {
            bool validated = validateOrder();
            if (!validated)
                return;
            switch (orderState)
            {
                case OrderState.New:
                    saveNewOrder();
                    newOrder();
                    break;
                case OrderState.Done:
                    MessageBox.Show("Đơn hàng đã nhập hoàn tất. Không thể chỉnh sửa");
                    break;
                case OrderState.OnProgress:
                    saveRepairOrder();
                    newOrder();
                    break;
            }
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.ColumnIndex == 7)
                MessageBox.Show("Định dạng ngày không đúng. Mời nhập theo dạng mm/dd/yy.");
            else
                MessageBox.Show("Giá trị nhập không phù hợp. Mời nhập lại.");
        }

        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = dataGridView.CurrentCell;
            ImportOrderItem item = (ImportOrderItem)dataGridView.CurrentRow.DataBoundItem;

            if (cell.ColumnIndex == 3)// Edit quantity_by_stock
            {
                int num;
                bool parsed = int.TryParse(cell.Value.ToString(), out num);
                if (!parsed || num < 0)
                {
                    MessageBox.Show("Chỉ chấp nhận nhập số dương.");
                }
                // Update item price
                item.TotalItemPrice = item.base_price_by_stock * item.quantity_by_stock +
                    item.base_price_by_stock * item.quantity_by_unit / item.quantity_control;
                dataGridView.Rows[cell.RowIndex].Cells[6].Value = item.TotalItemPrice.ToString();

                // Update total price
                updateLblTotal();
            }

            if (cell.ColumnIndex == 4)// Edit quantity_by_unit
            {
                // If quantity is not integer then set cell text to 0
                int num;
                bool parsed = int.TryParse(cell.Value.ToString(), out num);
                if (!parsed || num < 0)
                {
                    cell.Value = 0;
                    MessageBox.Show("Chỉ chấp nhận nhập số dương.");
                }
                else
                {
                    if (num > item.quantity_control)
                    {
                        cell.Value = 0;
                        MessageBox.Show("Số hàng lẻ không thể vượt quá quy cách.");
                    }
                }

                // Update item price
                item.TotalItemPrice = item.base_price_by_stock * item.quantity_by_stock +
                    item.base_price_by_stock * item.quantity_by_unit / item.quantity_control;

                dataGridView.Rows[cell.RowIndex].Cells[6].Value = item.TotalItemPrice.ToString();

                updateLblTotal();
            }
            if (cell.ColumnIndex == 5)//Edit base_price_by_stock
            {
                // If quantity is not integer then set cell text to 0
                decimal num;
                bool parsed = decimal.TryParse(cell.Value.ToString(), out num);

                if (!parsed || num < 0)
                {
                    cell.Value = 0;
                    MessageBox.Show("Chỉ chấp nhận nhập số dương.");
                }
                else

                // Update item price
                item.TotalItemPrice = item.base_price_by_stock * item.quantity_by_stock +
                                item.base_price_by_stock * item.quantity_by_unit / item.quantity_control;
                dataGridView.Rows[cell.RowIndex].Cells[6].Value = item.TotalItemPrice.ToString();

                updateLblTotal();
            }

            if (cell.ColumnIndex == 7)//Edit expired_date
            {
                DateTime dt ;
                //bool parsed = DateTime.TryParseExact(cell.Value.ToString(),
                //    "{0:dd/mm/yyyy}",
                //    System.Globalization.CultureInfo.InvariantCulture, 
                //    System.Globalization.DateTimeStyles.None, out dt);
                bool parsed = DateTime.TryParse(cell.Value.ToString(), out dt);
                if (!parsed)
                {
                    MessageBox.Show("Định dạng ngày không đúng. Mời nhập theo dạng mm/dd/yy.");
                }
            }
        }

        private decimal updateLblTotal()
        {
            decimal total = 0;
            decimal itemPrice = 0;
            foreach (var item in importOrder.ImportOrderItems)
            {
                itemPrice = item.base_price_by_stock * item.quantity_by_stock +
                    item.base_price_by_stock * item.quantity_by_unit / item.quantity_control;
                total += itemPrice;
            }

            importOrder.total_price = total;
            lblTotal.Text = total.ToString("#,##0.000");
            lblItemNum.Text = importOrder.ImportOrderItems.Count.ToString();

            return total;
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0) // Format Product ID
            {
                e.Value = e.Value.ToString().Trim();
            }
            if (e.ColumnIndex == 1) // Format Product Name
            {
                e.Value = e.Value.ToString().Trim();
            }
            if (e.ColumnIndex == 5) // Format Sale Price
            {
                e.Value = Convert.ToDecimal(e.Value).ToString("#,##0.000");
            }
            if (e.ColumnIndex == 6) // Format Product Name
            {
                e.Value = Convert.ToDecimal(e.Value).ToString("#,##0.000");
            }
            if (e.ColumnIndex == 7) // Format Product Name
            {
                e.Value = String.Format("{0:MM/dd/yyyy}", e.Value);
            }
            
        }

        private void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (orderState == OrderState.Done)
            {
                MessageBox.Show("Không thể chỉnh sửa khi đã nhập hàng hoàn tất.");
                e.Cancel = true;
            }
        }

        private void cbbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (orderState == OrderState.Done)
            {
                if (cbbStatus.SelectedItem.Equals("On progress")) // Changed from 'Done' to 'On Progress'
                {
                    MessageBox.Show("Không thể thay đổi trạng thái của đơn hàng khi đã nhập hoàn tất.");
                    cbbStatus.SelectedItem = "Done";
                }
            }
            else
            {
                if (importOrder != null)
                  importOrder.order_status = cbbStatus.SelectedItem.ToString();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
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
                    // Bind DataSource
                    List<ImportOrderItem> itemsList = new List<ImportOrderItem>();

                    int nRows = range.Rows.Count;
                    int num;
                    decimal d;
                    bool parsed;
                    for (int iRow = 2; iRow <= nRows; iRow++)
                    {
                        ImportOrderItem item = db.ImportOrderItems.Create();
                        // Get ID
                        range = (Excel.Range)sheet.Cells[iRow, 2];
                        item.prod_id = range.Text;
                        // Get Name
                        range = (Excel.Range)sheet.Cells[iRow, 3];
                        item.ProductName = range.Text;

                        // Get Quality_control
                        range = (Excel.Range)sheet.Cells[iRow, 4];
                        parsed = int.TryParse(range.Text, out num);
                        if (parsed)
                        {
                            item.quantity_control = num;
                        }
                        else item.quantity_control = 1;

                        // Get Sale_price_by_stock
                        range = (Excel.Range)sheet.Cells[iRow, 5];
                        parsed = decimal.TryParse(range.Text, out d);
                        if (parsed)
                        {
                            item.base_price_by_stock = d;
                        }
                        else item.base_price_by_stock = 0;
                        // Set sale_price_by_unit
                        if (item.base_price_by_stock != 0)
                            item.base_price_by_unit = item.base_price_by_stock / item.quantity_control;

                        // Get quantity_by_unit
                        range = (Excel.Range)sheet.Cells[iRow, 6];
                        parsed = int.TryParse(range.Text, out num);
                        if (parsed)
                        {
                            item.quantity_by_unit = num;
                        }
                        else
                            item.quantity_by_unit = 0;
                        // Get quantity_by_stock
                        range = (Excel.Range)sheet.Cells[iRow, 7];
                        parsed = int.TryParse(range.Text, out num);
                        if (parsed)
                        {
                            item.quantity_by_stock = num;
                        }
                        else
                            item.quantity_by_stock = 0;
                        // Get TotalPrice
                        item.TotalItemPrice = item.quantity_by_stock * item.base_price_by_stock +
                            item.quantity_by_unit * item.base_price_by_stock / item.quantity_control;
                        // Get Date expired
                        range = (Excel.Range)sheet.Cells[iRow, 9];
                        DateTime dt;
                        parsed = DateTime.TryParse(range.Text, out dt);
                        if (parsed)
                        {
                            item.date_expired = dt;
                        }
                        //else
                        //    item.date_expired = DateTime.Now;


                        // Check as if product has been existed
                        var prod = db.Products.Where(t => t.prod_id.Equals(item.prod_id));
                        if (prod.Any())//Not exist
                        {
                            importOrder.ImportOrderItems.Add(item);
                        }
                    }

                    // Bind datasource
                    dataGridView.DataSource = importOrder.ImportOrderItems.ToList();
                    updateLblTotal();
                }

                wb.Close();
                excelApp.Quit();

                Marshal.ReleaseComObject(wb);
                Marshal.ReleaseComObject(sheet);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            clearEntries();
            newOrder();
        }
    }
}
