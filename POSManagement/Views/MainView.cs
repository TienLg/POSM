using POSManagement.Views.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSManagement.Views
{
    public partial class MainView : Form
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void tsbNewProduct_Click(object sender, EventArgs e)
        {
            new ProductView().Show();
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            new ProductView().Show();
        }

        private void btnSale_Click(object sender, EventArgs e)
        {
            new SaleOrderView().Show();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            new ImportOrderView().Show();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomerSearchDialog dialog = new CustomerSearchDialog();
            dialog.ShowDialog(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Tồn kho
            Form f = new Form() { Text = "Tồn kho" };
            ProductSearchControl p = new ProductSearchControl();
            p.Dock = DockStyle.Fill;
            f.AutoSize = true;
            f.Controls.Add(p);
            f.Width = 800;
            f.Height = 600;
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Thông kê nhập
            new ImportReportView().Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Thống kê xuất
            new CustomerReportView().Show();
        }
    }
}
