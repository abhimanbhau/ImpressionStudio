using System.Windows;

namespace ImpressionStudio
{
    /// <summary>
    ///     Interaction logic for SlideTitle.xaml
    /// </summary>
    public partial class PresentationTitleWindow : Window
    {
        public PresentationTitleWindow()
        {
            InitializeComponent();
        }

        public string PresentationTitle { get; set; }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            PresentationTitle = "<title>" + txtTitle.Text + "</title>";
            Close();
        }
    }
}