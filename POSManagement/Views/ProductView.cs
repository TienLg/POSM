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
    public partial class ProductView : Form
    {
        public ProductView()
        {
            InitializeComponent();
            //pnlMain.FlowDirection = FlowDirection.LeftToRight;
            ////pnlMain.AutoSize = true;
            ProductSearchControl sc = new ProductSearchControl();
            sc.Dock = DockStyle.Fill;
            ProductEditorControl pec = new ProductEditorControl();
            pec.Dock = DockStyle.Fill;

            // Add Call back to bridge Product object between 2 controls
            sc.addCallbacksFn(pec);

            splitContainer.Panel1.Controls.Add(sc);
            splitContainer.Panel2.Controls.Add(pec);
            //splitContainer.SuspendLayout();

            //sc.Dock = DockStyle.Bottom;
            //pec.Dock = DockStyle.Fill;
            //pnlMain.Controls.Add(sc);
            //pnlMain.Controls.Add(pec);
            //pnlMain.SuspendLayout();

        }
    }
}
