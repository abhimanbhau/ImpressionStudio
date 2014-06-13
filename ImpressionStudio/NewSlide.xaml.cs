using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using ISudio.Data.Provider;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace ImpressionStudio
{
    /// <summary>
    ///     Interaction logic for NewSlide.xaml
    /// </summary>
    public partial class NewSlide
    {
        private readonly RichTextBox _rtb;
        private bool _isValidData;
        private ImpressSlide _slide;

        public NewSlide()
        {
            InitializeComponent();
            _rtb = new RichTextBox();
            host.Child = _rtb;
        }

        private void NewSlideForm_Loaded(object sender, RoutedEventArgs e)
        {
            cmbSlideType.ItemsSource = new[] { "Step Slide", "Step" };
            cmbSlideType.SelectedIndex = 0;
        }

        private void horizontalScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                horizontalScroll.LineRight();
            }
            else
            {
                horizontalScroll.LineLeft();
            }
        }

        private void btnBold_Click(object sender, RoutedEventArgs e)
        {
            _rtb.SelectedText = Environment.NewLine + @"<B>Content</B>";
        }

        private void btnItalic_Click(object sender, RoutedEventArgs e)
        {
            _rtb.SelectedText = Environment.NewLine + @"<I>Content</I>";
        }

        private void btnUnderline_Click(object sender, RoutedEventArgs e)
        {
            _rtb.SelectedText = Environment.NewLine + @"<U>Content</U>";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _rtb.SelectedText = Environment.NewLine + @"<H1>Header</H1>";
        }

        private void btnH5_Click(object sender, RoutedEventArgs e)
        {
            _rtb.SelectedText = Environment.NewLine + @"<H5>Small_Header</H5>";
        }

        private void btnParagraph_Click(object sender, RoutedEventArgs e)
        {
            _rtb.SelectedText = Environment.NewLine + @"<P></P>";
        }

        private void btnDivision_Click(object sender, RoutedEventArgs e)
        {
            _rtb.SelectedText = Environment.NewLine + @"<DIV></DIV>";
        }

        private void btnLink_Click(object sender, RoutedEventArgs e)
        {
            char qoutes = Convert.ToChar(34);
            _rtb.SelectedText = Environment.NewLine + String.Format(@"<a href={0}link_here{1}>LINK_TITLE_HERE</a>",
                qoutes, qoutes);
        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            char q = Convert.ToChar(34);
            _rtb.SelectedText = Environment.NewLine +
                               String.Format(
                                   @"<img src={0}image_link_here{0} alt={0}error_message{0} height={0}100{0} width={0}200{0} title={0}Title”>",
                                   q);
        }

        private void btnOl_Click(object sender, RoutedEventArgs e)
        {
            _rtb.SelectedText = Environment.NewLine +
                               String.Format(@"<ol>{0}<li>Item1</li>{0}<li>Item2</li>{0}</ol>", Environment.NewLine);
        }

        private void btnUOl_Click(object sender, RoutedEventArgs e)
        {
            _rtb.SelectedText = Environment.NewLine +
                               String.Format(@"<ul>{0}<li>Item1</li>{0}<li>Item1</li>{0}</ul>", Environment.NewLine);
        }

        public ImpressSlide GetSlide()
        {
            if (_slide == null)
            {
                return null;
            }
            return _slide;
        }

        private void btnVerifyData_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text) || string.IsNullOrEmpty(_rtb.Text) ||
                String.IsNullOrEmpty(txtSlideHeader.Text.Trim()))
            {
                MessageBox.Show("Something appears to be missing",
                    "Check all values", MessageBoxButton.OK, MessageBoxImage.Error);
                _isValidData = false;
            }
            else
            {
                MessageBox.Show("Data appears to be valid\nPlease press submit button",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                _isValidData = true;
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!_isValidData)
            {
                MessageBox.Show("Click Verify Button First",
                    "Verify Slide Contents First",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _slide = new ImpressSlide
            {
                Id = txtID.Text.Trim(),
                DataX = Convert.ToInt16(txtDataX.Text.Trim()),
                DataY = Convert.ToInt16(txtDataY.Text.Trim()),
                DataZ = Convert.ToInt16(txtDataZ.Text.Trim()),
                DataRotateX = Convert.ToInt16(txtDataRotateX.Text.Trim()),
                DataRotateY = Convert.ToInt16(txtDataRotateY.Text.Trim()),
                DataRotateZ = Convert.ToInt16(txtDataRotateZ.Text.Trim()),
                DataScale = 1,
                SlideMatter = _rtb.Lines,
                DataRotate = Convert.ToInt16(DataRotate.Text.Trim()),
                Header = txtSlideHeader.Text.Trim()
            };
            _slide.Class = (cmbSlideType.Text == "Step Slide") ? "Step Slide" : "Step";
            MessageBox.Show("Successfully created new slide",
                "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}