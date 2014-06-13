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

namespace ImpressionStudio
{
    /// <summary>
    /// Interaction logic for SlideTitle.xaml
    /// </summary>
    public partial class PresentationTitleWindow : Window
    {
        public string PresentationTitle { get; set; }
        public PresentationTitleWindow()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            PresentationTitle = "<title>" + txtTitle.Text + "</title>";
            Close();
        }
    }
}
