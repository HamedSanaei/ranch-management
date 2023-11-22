using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Arash;
using Dam.Data;

namespace Dam
{
    /// <summary>
    /// Interaction logic for ReportMenu.xaml
    /// </summary>
    public partial class AboutUs : Window
    {
        public AboutUs()
        {
            InitializeComponent();
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {

            this.Close();

        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
