﻿using POSManagement.Views.Controls;
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
    public partial class ImportReportView : Form
    {
        public ImportReportView()
        {
            InitializeComponent();
            ImportReportControl sc = new ImportReportControl();
            sc.Dock = DockStyle.Fill;
            this.Controls.Add(sc);
        }
    }
}
