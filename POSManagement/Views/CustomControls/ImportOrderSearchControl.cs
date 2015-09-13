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

namespace POSManagement.Views.Controls
{
    public partial class ImportOrderSearchControl : UserControl
    {
        public delegate void SetOrderDelegate(ImportOrder order);

        public SetOrderDelegate SetOrderDelegateCallback;
        public ImportOrderSearchControl()
        {
            InitializeComponent();
            AdjustGridView();
        }

        public void addCallbacksFn(ImportOrderControl editor)
        {
            this.SetOrderDelegateCallback += new SetOrderDelegate(editor.SetOrderDelegateCallbackFn);
        }

        private void AdjustGridView()
        {
            // Show specific columns of Product
            dataGridView.AutoGenerateColumns = false;
            dataGridView.ColumnCount = 3;
            int index = 0;

            //// Add column Saler Name
            //dataGridView.Columns[index].Name = "SalerName";
            //dataGridView.Columns[index].DataPropertyName = "UserName";
            //dataGridView.Columns[index].HeaderText = "Người nhập hàng";

            // Add column Total Price
            dataGridView.Columns[index].Name = "TotalPrice";
            dataGridView.Columns[index].DataPropertyName = "total_price";
            dataGridView.Columns[index].HeaderText = "Tổng giá";

            // Add column Status
            index++;
            dataGridView.Columns[index].Name = "Status";
            dataGridView.Columns[index].DataPropertyName = "order_status";
            dataGridView.Columns[index].HeaderText = "Tình trạng";

            // Add column Status
            index++;
            dataGridView.Columns[index].Name = "Status";
            dataGridView.Columns[index].DataPropertyName = "date_import";
            dataGridView.Columns[index].HeaderText = "Ngày nhập hàng";
            dataGridView.Columns[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void BindData()
        {
            using (var db = new SMEntities())
            {
                // Bind DataSource
                var so = db.ImportOrders.Where(o => true);
                if (chbDate.Checked)
                {
                    so = so.Where(o => o.date_import.Year >= dtpFrom.Value.Year &
                       o.date_import.Month >= dtpFrom.Value.Month &
                       o.date_import.Day >= dtpFrom.Value.Day &
                       o.date_import.Year <= dtpTo.Value.Year &
                       o.date_import.Month <= dtpTo.Value.Month &
                       o.date_import.Day <= dtpTo.Value.Day);
                }
                if (cbbStatus.SelectedItem != null && !cbbStatus.SelectedItem.Equals("All"))
                {
                    so = so.Where(o => o.order_status.Equals(cbbStatus.SelectedItem.ToString()));
                }
                //if (txtUser.Text != string.Empty)
                //    so = so.Where(p => p.User.user_name.Contains(txtUser.Text));
                //so = so.Include(o => o.User);
                dataGridView.DataSource = so.ToList();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            ImportOrder so = (ImportOrder)dataGridView.CurrentRow.DataBoundItem;
            if (so != null)
                SetOrderDelegateCallback(so);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void xemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(dataGridView.CurrentRow != null)
            {
                ImportOrder order = (ImportOrder)dataGridView.CurrentRow.DataBoundItem;
                SetOrderDelegateCallback(order);
            }
        }

        private void dataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip.Show(this.dataGridView, e.Location);
                contextMenuStrip.Show(Cursor.Position);
            }
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null)
                return;
            if (e.ColumnIndex == 0) // Format Sale Price
            {
                e.Value = Convert.ToDecimal(e.Value).ToString("#,##0.000");
            }
            if (e.ColumnIndex == 2) // Format Product Name
            {
                e.Value = String.Format("{0:MM/dd/yyyy}", e.Value);
            }
        }
    }
}
