using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSManagement.Models;
using POSManagement.Repositories;
using POSManagement.Views;
using System.Windows.Forms;


namespace POSManagement
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.Run(new MainView());
        }
    }

}
