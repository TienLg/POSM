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
    public partial class SaleOrderView : Form
    {
        public SaleOrderView()
        {
            InitializeComponent();
            SaleOrderSearchControl sosc = new SaleOrderSearchControl();
            SaleOrderControl soc = new SaleOrderControl();

            sosc.Dock = DockStyle.Fill;
            soc.Dock = DockStyle.Fill;
            // Add fn callback
            sosc.addCallbacksFn(soc);

            this.splitContainer.Panel1.Controls.Add(sosc);
            this.splitContainer.Panel2.Controls.Add(soc);

        }
    }
}
