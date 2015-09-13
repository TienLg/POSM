using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POSManagement.Views.Controls;

namespace POSManagement.Views
{
    public partial class CustomerReportView : Form
    {
        public CustomerReportView()
        {
            InitializeComponent();
            CustomerReportControl rc = new CustomerReportControl();
            rc.Dock = DockStyle.Fill;
            this.Controls.Add(rc);
        }
    }
}
