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
using POSManagement.Repositories;
using POSManagement.Views.Controls;

namespace POSManagement.Views
{
    public partial class SaleOrderControl : UserControl
    {
        enum OrderState { New = 1, Done = 2, OnProgress = 3};

        private SMEntities db;

        private int orderState;

        private SaleOrder saleOrder;

        private Customer cust;

        private ICollection<Product> prodsList;
        public SaleOrderControl()
        {
            InitializeComponent();
            adjustGridView();
            db = new SMEntities();
            newOrder();
            registerHandlers();
        }

        private void ClearEntries()
        {
            txtAddress.Text = string.Empty;
            txtCustName.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtTakenMoney.Text = string.Empty;
            lblTotal.Text = "0";
            lblDept.Text = "0";
            if (cbbStatus.Items.Contains("Done"))
                cbbStatus.SelectedItem = "Done";
        }

        private void newOrder()
        {
            ClearEntries();

            saleOrder = db.SaleOrders.Create();
            saleOrder.user_id = 3;
            saleOrder.taken_money = 0;
            saleOrder.total_price = 0;

            prodsList = new HashSet<Product>();
            dataGridView.DataSource = new List<SaleOrderItem>();
            orderState = (int) OrderState.New;
        }

        private void registerHandlers()
        {
            txtCustName.KeyPress += this.inputValidator;
            txtPhone.KeyPress += this.inputValidator;
            txtTakenMoney.KeyPress += this.inputValidator;
        }

        private void inputValidator(object sender, KeyPressEventArgs e)
        {
            TextBox t;
            if (sender is TextBox)
            {
                t = (TextBox)sender;
                if (t.Name == txtCustName.Name)
                {
                    e.Handled = true;
                    CustomerSearchDialog dialog = new CustomerSearchDialog();
                    dialog.ShowDialog(this);

                    if (dialog.DialogResult == DialogResult.OK)
                    {
                        Customer c = dialog.ReturnCustomer();
                        if (c != null)
                        {
                            txtCustName.Text = c.cust_name.Trim();
                            txtAddress.Text = c.cust_address.Trim();
                            txtPhone.Text = c.cust_phone.Trim();

                            // Update Sale order
                            saleOrder.cust_id = c.cust_id;
                        }
                    }
                }
                if (t.Name == txtPhone.Name)
                {
                    // If new input is not integer then disallow the character
                    if (!Char.IsDigit(e.KeyChar) & e.KeyChar != (char)8)
                        e.Handled = true;
                }
                if (t.Name == txtTakenMoney.Name)
                {
                    // If new input is not decimal then disallow the characte
                    decimal takenMoney;
                    string enterNumber = txtTakenMoney.Text + e.KeyChar.ToString();
                    bool parsed = decimal.TryParse(enterNumber, out takenMoney);
                    if (!parsed)
                        e.Handled = true;
                    else
                    {
                        decimal total = decimal.Parse(lblTotal.Text.ToString());
                        if (takenMoney > total)
                            e.Handled = true;
                        else
                            lblDept.Text = (total - takenMoney).ToString("#,##0.000"); 
                    }
                    // But accept backspace
                    if (e.KeyChar == (char)8)
                    {
                        e.Handled = false;
                    }
                }
            }
        }
        private void dataGridView_EditingControlShowing(Object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridView.CurrentCell.ColumnIndex == 1)
            {
                var source = new AutoCompleteStringCollection();
                String[] stringArray = { "a", "b", "c", "d" };
                source.AddRange(stringArray);

                TextBox prodCode = e.Control as TextBox;
                if (prodCode != null)
                {
                    prodCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    prodCode.AutoCompleteCustomSource = source;
                    prodCode.AutoCompleteSource = AutoCompleteSource.CustomSource;
                }
            }
        }
        private void adjustGridView()
        {
            dataGridView.EditingControlShowing += dataGridView_EditingControlShowing;
            dataGridView.DataError += new DataGridViewDataErrorEventHandler(this.dataGridView_DataError);
            dataGridView.AutoGenerateColumns = false;
            dataGridView.ColumnCount = 8;

            // Add column Product ID
            int index = 0;
            dataGridView.Columns[index].Name = "ProductID";
            dataGridView.Columns[index].DataPropertyName = "prod_id";
            dataGridView.Columns[index].HeaderText = "Mã";
            dataGridView.Columns[index].ReadOnly = true;
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // Add column Product Name
            index++;
            dataGridView.Columns[index].Name = "ProductName";
            dataGridView.Columns[index].DataPropertyName = "ProductName";
            dataGridView.Columns[index].HeaderText = "Tên";
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView.Columns[index].ReadOnly = true;

            // Add column Quantity Control
            index++;
            dataGridView.Columns[index].Name = "QuantityControl";
            dataGridView.Columns[index].DataPropertyName = "quantity_control";
            dataGridView.Columns[index].HeaderText = "Quy cách";
            dataGridView.Columns[index].Width = 40;
            dataGridView.Columns[index].ReadOnly = true;

            //Add column Sale Total Price
            index++;
            dataGridView.Columns[index].Name = "SalePrice";
            dataGridView.Columns[index].DataPropertyName = "sale_price_by_stock";
            dataGridView.Columns[index].HeaderText = "Giá bán theo quy cách";
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            //Add column Sale Total Price
            index++;
            dataGridView.Columns[index].Name = "SalePrice";
            dataGridView.Columns[index].DataPropertyName = "sale_price_by_unit";
            dataGridView.Columns[index].HeaderText = "Giá bán theo đơn vị";
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // Add column Quantity By Stock
            index++;
            dataGridView.Columns[index].Name = "QuantityByStock";
            dataGridView.Columns[index].DataPropertyName = "quantity_by_stock";
            dataGridView.Columns[index].HeaderText = "Số lượng theo quy cách";

            // Add column Quantity By Unit
            index++;
            dataGridView.Columns[index].Name = "QuantityByUnit";
            dataGridView.Columns[index].DataPropertyName = "quantity_by_unit";
            dataGridView.Columns[index].HeaderText = "Số lượng theo đơn vị";
            
            // Add column Total item Price
            index++;
            dataGridView.Columns[index].Name = "TotalItemPrice";
            dataGridView.Columns[index].DataPropertyName = "TotalItemPrice";
            dataGridView.Columns[index].HeaderText = "Tổng";
            dataGridView.Columns[index].ReadOnly = true;
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        private void BindData(SaleOrder order)
        {
            prodsList.Clear();
            // Update order state
            if (order.order_status.Trim() == "Done")
                orderState = (int)OrderState.Done;
            else
                orderState = (int)OrderState.OnProgress;

            // Updatee order information
            var so = db.SaleOrders.Where(s => s.order_id.Equals(order.order_id))
                .Include(s => s.SaleOrderItems)
                .First();
            txtCustName.Text = so.Customer.cust_name;
            txtAddress.Text = so.Customer.cust_address;
            txtPhone.Text = so.Customer.cust_phone;
            cbbStatus.SelectedItem = so.order_status.Trim();
            if (so.date_ship != null)
                dtpShipDate.Value = so.date_ship;

            lblTotal.Text = so.total_price.ToString("#,##0.000");
            txtTakenMoney.Text = so.taken_money.ToString("#,##0.000");
            lblDept.Text = (so.total_price - so.taken_money).ToString("#,##0.000");

            // Bind DataSource
            var soi = db.SaleOrderItems.Where(p => p.order_id.Equals(order.order_id))
                .Include(p => p.Product);
            foreach (var item in soi)
            {
                item.ProductName = item.Product.prod_name;
                item.TotalItemPrice = item.quantity_by_stock * item.sale_price_by_stock +
                    item.quantity_by_unit * item.sale_price_by_unit;

                // Add product to product list
                Product prod = db.Products.Where(p => p.prod_id.Equals(item.Product.prod_id))
                    .Include(p => p.Stocks)
                    .First();
                prodsList.Add(prod);
            }
            dataGridView.DataSource = soi.ToList();

            // Assign newOrder to current order
            saleOrder = so;
        }

        public void SetSaleOrderDelegateCallbackFn(POSManagement.Models.SaleOrder newOrder)
        {
            // Ask User to save current order
            if (saleOrder.SaleOrderItems.Count > 0)
            {
                var result = MessageBox.Show(this, "Bạn có muốn lưu đơn hàng hiện tại?",
                    "Lưu đơn hàng",
                    MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // Validate Order before save
                    if(validateOrder())
                        saveOrder();
                }
                else
                {
                    BindData(newOrder);
                }
            }
            else
            {
                BindData(newOrder);
            }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            saveOrder();
        }

        private void saveOrder()
        {
            // Validate order
            if (!validateOrder())
            {
                return;
            }

            // Save validated order
            switch (orderState)
            {
                case (int) OrderState.New:
                    saveNewOrder();
                    newOrder();
                    break; 
                case (int) OrderState.Done:
                case (int) OrderState.OnProgress:
                    saveRepairOrder();
                    newOrder();
                    break;
            }
        }

        private void saveStocks(SMEntities db)
        {
            // Update Stocks
            foreach (var item in saleOrder.SaleOrderItems)
            {
                Product prod = prodsList.Where(p => p.prod_id.Equals(item.prod_id))
                        .First();
                prod.Stocks.OrderBy(s => s.base_price_by_stock);

                // Proceed stocks
                if (item.quantity_by_stock > 0)
                {
                    int remainStockNeedProceed = item.quantity_by_stock;
                    // Take lowest price stock first
                    foreach (var stock in prod.Stocks)
                    {
                        if (stock.quantity_by_stock == 0)// Proceed next stock
                            continue;
                        if (remainStockNeedProceed <= 0)// Proceed complete
                            break;
                        remainStockNeedProceed = remainStockNeedProceed - stock.quantity_by_stock;
                        if (remainStockNeedProceed < 0)
                        {
                            stock.quantity_by_stock = -remainStockNeedProceed;
                            db.Entry(stock).State = EntityState.Modified;
                            break;// Proceed complete
                        }
                        else
                        {
                            // Proceed the stock
                            stock.quantity_by_stock = 0;
                            db.Entry(stock).State = EntityState.Modified;
                        }
                    }
                }

                // Proceed units
                if (item.quantity_by_unit > 0 &&
                    item.quantity_by_unit < item.quantity_control)
                {
                    int remainUnitNeedProceed = item.quantity_by_unit;
                    // Take lowest price stock first
                    foreach (var stock in prod.Stocks)
                    {
                        if (stock.quantity_by_unit == 0)// Proceed next stock
                            continue;
                        if (remainUnitNeedProceed <= 0)// Proceed complete
                            break;
                        remainUnitNeedProceed = remainUnitNeedProceed - stock.quantity_by_unit;
                        if (remainUnitNeedProceed <= 0)
                        {
                            stock.quantity_by_unit = -remainUnitNeedProceed;
                            db.Entry(stock).State = EntityState.Modified;
                            break;// Proceed complete
                        }
                        else
                        {
                            // Subtract from availabe stock 
                            if (stock.quantity_by_stock >= 1)
                            {
                                stock.quantity_by_stock -= 1;
                                stock.quantity_by_unit = stock.quantity_control - remainUnitNeedProceed;
                                db.Entry(stock).State = EntityState.Modified;
                            }
                            //else continue proceed next stock
                        }
                    }
                }

                // Update quantity the product
                prod.quantity_by_stock -= item.quantity_by_stock;
                if (prod.quantity_by_unit >= item.quantity_by_unit)
                    prod.quantity_by_unit -= item.quantity_by_unit;
                else
                {
                    prod.quantity_by_stock -= 1;
                    prod.quantity_by_unit = prod.quantity_control + prod.quantity_by_unit 
                        - item.quantity_by_unit;
                }
                db.Entry(prod).State = EntityState.Modified;
                if (prod.quantity_by_stock < 0 || prod.quantity_by_unit < 0)
                {
                    MessageBox.Show("Lỗi thực thi.");
                }
            }
        }

        private void saveNewOrder()
        {
            db.SaleOrders.Add(saleOrder);
            saveStocks(db);
            db.SaveChanges();
        }

        private void saveRepairOrder()
        {
            if (orderState.Equals((int) OrderState.OnProgress)) // Repair OnProgress-original order
            {
                if (saleOrder.order_status.Equals("Done"))
                {
                    db.Entry(saleOrder).State = EntityState.Modified;
                    foreach (var item in saleOrder.SaleOrderItems)
                    {
                        db.Entry(item).State = EntityState.Modified;
                    }
                    // Save stock
                    saveStocks(db);
                }
                else
                {
                    db.Entry(saleOrder).State = EntityState.Modified;
                    foreach (var item in saleOrder.SaleOrderItems)
                    {
                        db.Entry(item).State = EntityState.Modified;
                    }// Do not save stocks
                }
            }
            else// Edit done-original order
            {
                // Only save state of order. Not order items.
                db.Entry(saleOrder).State = EntityState.Modified;
            }

            // Save changes
            db.SaveChanges();
        }

        private bool validateOrder()
        {
            // Check input texts
            if(txtTakenMoney.Text == string.Empty ||
                cbbStatus.SelectedItem == null)
            {
                MessageBox.Show("Không thể lưu khi thông tin chưa đầy đủ. Mời nhập.");
                return false;
            }

            saleOrder.taken_money = Convert.ToDecimal(txtTakenMoney.Text.ToString());
            saleOrder.order_status = cbbStatus.SelectedItem.ToString();
            saleOrder.date_ship = dtpShipDate.Value;

            //string errorStr = "Lỗi thông tin đơn hàng.";
            if (saleOrder.SaleOrderItems.Count <= 0)
            {
                MessageBox.Show("Không thể lưu khi danh sách hàng đang trống. Mời nhập.");
                return false;
            }
            var cust = db.Customers.Find(saleOrder.cust_id);
            if (cust == null)
            {
                MessageBox.Show("Không tìm thấy thông tin khách hàng. Mời nhập.");
                return false;
            }
            return true;
        }

        private void addProduct()
        {

            if (orderState.Equals((int)OrderState.Done))
            {
                MessageBox.Show("Hàng đã bán, bạn không thể chỉnh sửa danh sách hàng.");
                return;
            }

            ProductSearchDialog dialog = new ProductSearchDialog();
            dialog.ShowDialog();
            if (dialog.DialogResult == DialogResult.OK)
            {
                Product prod = dialog.ReturnProduct();
                if (prodsList.Any(p => p.prod_id.Equals(prod.prod_id)))
                {
                    MessageBox.Show("Sản phảm đã có trong danh sách.");
                }
                else
                {
                    // Add product to product list
                    prod = db.Products.Where(p => p.prod_id.Equals(prod.prod_id))
                        .Include(p => p.Stocks)
                        .First();
                    // Get all non-empty stock
                    //prod.Stocks = db.Stocks
                    //    .Where(s => s.prod_id.Equals(prod.prod_id) & (s.quantity_by_stock > 0 || s.quantity_by_unit > 0))
                    //    .ToList();
                    prodsList.Add(prod);

                    // Create new Item
                    SaleOrderItem item = db.SaleOrderItems.Create();
                    item.prod_id = prod.prod_id;
                    item.sale_price_by_stock = prod.sale_price_by_stock;
                    item.sale_price_by_unit = prod.sale_price_by_unit;
                    item.quantity_by_stock = 0;
                    item.quantity_by_unit = 0;
                    item.quantity_control = prod.quantity_control;
                    item.TotalItemPrice = 0;
                    item.ProductName = prod.prod_name;

                    saleOrder.SaleOrderItems.Add(item);
                    dataGridView.DataSource = saleOrder.SaleOrderItems.ToList();
                }
            }
        }

        private void removeProduct()
        {
            if (orderState.Equals((int)OrderState.Done))
            {
                MessageBox.Show("Hàng đã bán, bạn không thể chỉnh sửa danh sách hàng.");
                return;
            }

            if (dataGridView.CurrentRow != null)
            {
                SaleOrderItem item = (SaleOrderItem)dataGridView.CurrentRow.DataBoundItem;
                saleOrder.SaleOrderItems.Remove(item);
                prodsList.Remove(prodsList.Where(p => p.prod_id.Equals(item.prod_id)).First());

                dataGridView.DataSource = saleOrder.SaleOrderItems.ToList();
                updateLblTotal();
            }
        }

        private void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (orderState.Equals((int)OrderState.Done))
            {
                MessageBox.Show("Hàng đã bán, bạn không thể chỉnh sửa danh sách hàng.");
                e.Cancel = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            addProduct();
            updateLblTotal();
        }

        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = dataGridView.CurrentCell;
            SaleOrderItem item = (SaleOrderItem)dataGridView.CurrentRow.DataBoundItem;

            if (cell.ColumnIndex == 3 || cell.ColumnIndex == 4)// Edit Sale price
            {
                decimal num;
                bool parsed = decimal.TryParse(cell.Value.ToString(), out num);
                if (!parsed || num < 0)
                {
                    // Get back the sale price of the product
                    Product prod = prodsList.Where(p => p.prod_id.Equals(item.prod_id)).First();
                    cell.Value = prod.sale_price_by_stock.ToString();
                    item.sale_price_by_stock = prod.sale_price_by_stock;

                    MessageBox.Show("Chỉ chấp nhận nhập số dương.");
                }
                // Update item price
                item.TotalItemPrice = item.sale_price_by_stock * item.quantity_by_stock +
                    item.sale_price_by_unit * item.quantity_by_unit;
                dataGridView.Rows[cell.RowIndex].Cells[7].Value = item.TotalItemPrice.ToString();

                // Update total price
                updateLblTotal();
            }

            if (cell.ColumnIndex == 5)// Edit quantity_by_stock
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
                    // Check available stocks
                    Product prod = prodsList.Where(p => p.prod_id.Equals(item.prod_id)).First();
                    int availableUnit = prod.quantity_by_stock * prod.quantity_control + prod.quantity_by_unit;
                    int numUnitSale = num * prod.quantity_control + item.quantity_by_unit;
                    if (availableUnit < numUnitSale)
                    {
                        MessageBox.Show(String.Format("Không đủ hàng. Trong kho còn {0} thùng, lẻ {1}",
                            prod.quantity_by_stock, prod.quantity_by_unit));
                        cell.Value = 0;
                    }
                }

                // Update item price
                item.TotalItemPrice = item.sale_price_by_stock * item.quantity_by_stock +
                    item.sale_price_by_unit * item.quantity_by_unit;
                dataGridView.Rows[cell.RowIndex].Cells[7].Value = item.TotalItemPrice.ToString();

                //dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);

                updateLblTotal();
            }

            if (cell.ColumnIndex == 6)//Edit quantity_by_unit
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
                    // Check available stocks
                    if (item.quantity_control <= num)
                    {
                        cell.Value = 0;
                        MessageBox.Show("Số lượng hàng lẻ vượt quá quy cách");
                    }
                    else
                    {
                        Product prod = prodsList.Where(p => p.prod_id.Equals(item.prod_id)).First();
                        int availableUnit = prod.quantity_by_stock * prod.quantity_control + prod.quantity_by_unit;
                        int numUnitSale = item.quantity_by_stock * prod.quantity_control + num;

                        if (availableUnit < numUnitSale)
                        {
                            MessageBox.Show(String.Format("Không đủ hàng. Trong kho còn {0} thùng, lẻ {1}",
                                prod.quantity_by_stock, prod.quantity_by_unit));
                            cell.Value = 0;
                        }
                    }
                }

                // Update item price
                item.TotalItemPrice = item.sale_price_by_stock * item.quantity_by_stock +
                                item.sale_price_by_unit * item.quantity_by_unit;
                dataGridView.Rows[cell.RowIndex].Cells[7].Value = item.TotalItemPrice.ToString();

                updateLblTotal();
            }
            // Update Total price
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Giá trị nhập không phù hợp. Mời nhập lại.");
        }

        private decimal updateLblTotal()
        {
            decimal total = 0;
            decimal itemPrice = 0;
            foreach (SaleOrderItem item in saleOrder.SaleOrderItems)
            {
                itemPrice = item.sale_price_by_stock * item.quantity_by_stock +
                    item.sale_price_by_unit * item.quantity_by_unit;
                total += itemPrice;
            }

            saleOrder.total_price = total;
            lblTotal.Text = total.ToString("#,##0.000");
            txtTakenMoney.Text = total.ToString("#,##0.000");

            return total;
        }

        private void dataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip.Show(this.dataGridView, e.Location);
                contextMenuStrip.Show(Cursor.Position);
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addProduct();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            removeProduct();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            removeProduct();
        }

        private void toolStripCreatNewButton_Click(object sender, EventArgs e)
        {
            ClearEntries();
            newOrder();
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 || e.ColumnIndex == 1) // Format Product ID
            {
                e.Value = e.Value.ToString().Trim();
            }
            if (e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 7) // Format Sale Price
            {
                e.Value = Convert.ToDecimal(e.Value).ToString("#,##0.000");
            }
        }

        private void txtTakenMoney_Leave(object sender, EventArgs e)
        {
            decimal takenMoney;
            bool parsed = decimal.TryParse(txtTakenMoney.Text.ToString(), out takenMoney);
            decimal total = decimal.Parse(lblTotal.Text.ToString());
            lblDept.Text = (total - takenMoney).ToString("#,##0.000");
        }

        private void cbbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (orderState.Equals((int)OrderState.Done))
            {
                if (cbbStatus.SelectedItem.Equals("On progress")) // Changed from 'Done' to 'On Progress'
                {
                    MessageBox.Show("Không thể thay đổi trạng thái của đơn hàng khi đã giao.");
                    cbbStatus.SelectedItem = "Done";
                }
                else // Change order status from 'On progress' to 'Done'
                {
                    if (saleOrder != null)
                        saleOrder.order_status = cbbStatus.SelectedItem.ToString();
                }
            }
            else
            {
                if (saleOrder != null)
                  saleOrder.order_status = cbbStatus.SelectedItem.ToString();
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
                    saleOrder.cust_id = cust.cust_id;
                    // Update Sale order
                }
            }
        }

        private void btnQuickSearch_Click(object sender, EventArgs e)
        {
            var customer = db.Customers.Where(c => c.cust_name.Equals("Unknown")).First();
            if (customer != null)
            {
                cust = customer;
                saleOrder.cust_id = cust.cust_id;
                txtCustName.Text = cust.cust_name.Trim();
                txtAddress.Text = cust.cust_address.Trim();
                txtPhone.Text = cust.cust_phone.Trim();
            }
        }
    }
}
