﻿using Ionic.Zip;
using ISudio.Data.Provider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml.Serialization;
using WPF.ThemesEx;
using Application = System.Windows.Application;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;
using RichTextBox = System.Windows.Forms.RichTextBox;

namespace ImpressionStudio
{
    public partial class MainWindow
    {
        private readonly RichTextBox _rtb;
        private ObservableCollection<ImpressSlide> _slides = new ObservableCollection<ImpressSlide>();

        public MainWindow()
        {
            InitializeComponent();
            _rtb = new RichTextBox();
            host.Child = _rtb;
        }

        private void ThemesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                this.ApplyTheme(e.AddedItems[0].ToString());
                Application.Current.ApplyTheme(e.AddedItems[0].ToString());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                themes.ItemsSource = ThemeManager.GetThemes();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            lstSlides.ItemsSource = _slides;
            lstSlides.DisplayMemberPath = "Header";

            cmbSlideType.Items.Add("Step Slide");
            cmbSlideType.Items.Add("Step");
        }

        private void btnCreateNewSlide_Click(object sender, RoutedEventArgs e)
        {
            var newSlideForm = new NewSlide();
            newSlideForm.ShowDialog();
            if (newSlideForm.GetSlide() != null)
            {
                _slides.Add(newSlideForm.GetSlide());
            }
            else
            {
                MessageBox.Show("No slide is created",
                    "Empty slide",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void menuSave_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(@"temp"))
            {
                Directory.CreateDirectory(@"temp");
            }
            if (_slides.Count == 0)
            {
                MessageBox.Show("No Slides Present",
                    "Cannot save empty project",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            using (var saveFile = new SaveFileDialog { Filter = "IStudio Project Files|*.isproj" })
            {
                if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (_slides.Count != 0)
                    {
                        var serializer1 = new XmlSerializer(_slides.GetType());
                        FileStream fs1 = File.OpenWrite(@"temp\tempSlides.ist");
                        serializer1.Serialize(fs1, _slides);
                        fs1.Close();
                    }
                    using (var zip = new ZipFile())
                    {
                        if (File.Exists(@"temp\tempSlides.ist"))
                        {
                            zip.AddFile(@"temp\tempSlides.ist", string.Empty);
                        }
                        zip.Save(saveFile.FileName);
                    }
                    try
                    {
                        if (File.Exists(@"temp\tempSlides.ist"))
                        {
                            File.Delete(@"temp\tempSlides.ist");
                        }
                    }
                    catch
                    { }
                }
            }
        }

        private void menuGenerateSlideshow_Click(object sender, RoutedEventArgs e)
        {
            if (_slides.Count == 0)
            {
                MessageBox.Show("No Slides Present",
                    "Cannot generate empty project",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string destinationDirectory = string.Empty;
            using (var folder = new FolderBrowserDialog { Description = "Select output folder" })
            {
                if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    destinationDirectory = folder.SelectedPath;
                }
                else
                {
                    return;
                }
            }
            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }
            var zip = new ZipFile("data.pak");
            zip.ExtractAll(destinationDirectory, ExtractExistingFileAction.OverwriteSilently);
            var fileContents = new List<string>();
            foreach (String str in File.ReadAllLines(destinationDirectory + @"\index.html"))
            {
                fileContents.Add(str);
            }
            var title = new PresentationTitleWindow();
            title.ShowDialog();
            fileContents.Insert(2, title.PresentationTitle);
            var gen = new ImpressMarkupGenerator(_slides);
            List<string> markup = gen.GetMarkup();
            markup.Reverse();
            foreach (string str in markup)
            {
                fileContents.Insert(16, str);
            }
            using (File.Create(destinationDirectory + @"\index.html"))
            {
            }
            File.AppendAllLines(destinationDirectory + @"\index.html", fileContents);
            MessageBox.Show("Done", "Generated", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnFollowOnGithub_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://www.github.com/abhimanbhau");
        }

        private void btnDonate_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("mailto:abhimanbhau@gmail.com");
        }

        private void btnShareOnFacebook_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://www.facebook.com/abhimanbhau");
        }

        private void lstSlides_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_slides.Count == 0)
            {
                return;
            }
            if (lstSlides.SelectedIndex < 0)
                return;
            FillSlideData(_slides[lstSlides.SelectedIndex]);
        }

        private void FillSlideData(ImpressSlide arg)
        {
            _rtb.Clear();
            txtID.Text = arg.Id;
            txtDataX.Text = arg.DataX.ToString();
            txtDataY.Text = arg.DataY.ToString();
            txtDataZ.Text = arg.DataZ.ToString();
            txtDataRotateX.Text = arg.DataRotateX.ToString();
            txtDataRotateY.Text = arg.DataRotateY.ToString();
            txtDataRotateZ.Text = arg.DataRotateZ.ToString();
            txtDataRotate.Text = arg.DataRotate.ToString();
            txtDataScale.Text = arg.DataScale.ToString();
            txtSlideHeader.Text = arg.Header;
            _rtb.Lines = arg.SlideMatter;
            cmbSlideType.SelectedIndex = (arg.Class == "Step Slide") ? 0 : 1;
        }

        private void ResetSlideDataPanel()
        {
            _rtb.Clear();
            txtID.Text = string.Empty;
            txtDataX.Text = string.Empty;
            txtDataY.Text = string.Empty;
            txtDataZ.Text = string.Empty;
            txtDataRotateX.Text = string.Empty;
            txtDataRotateY.Text = string.Empty;
            txtDataRotateZ.Text = string.Empty;
            txtDataRotate.Text = string.Empty;
            txtDataScale.Text = string.Empty;
            txtSlideHeader.Text = string.Empty;
        }

        private void btnUpdateSlide_Click(object sender, RoutedEventArgs e)
        {
            if (
                   CheckNumeric(txtDataX.Text)
                && CheckNumeric(txtDataY.Text)
                && CheckNumeric(txtDataZ.Text)
                && CheckNumeric(txtDataRotateX.Text)
                && CheckNumeric(txtDataRotateY.Text)
                && CheckNumeric(txtDataRotateZ.Text)
                && CheckNumeric(txtDataRotate.Text)
                && CheckNumeric(txtDataScale.Text))
            {
                ImpressSlide temp = _slides[lstSlides.SelectedIndex];
                UpdateSlide(ref temp);
                _slides[lstSlides.SelectedIndex] = temp;
                MessageBox.Show("Successfully modified step slide", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("There are some invalid fields",
                    "Check Data Again",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateSlide(ref ImpressSlide arg)
        {
            arg.DataRotate = Convert.ToInt32(txtDataRotate.Text);
            arg.DataRotateX = Convert.ToInt32(txtDataRotateX.Text);
            arg.DataRotateY = Convert.ToInt32(txtDataRotateY.Text);
            arg.DataRotateZ = Convert.ToInt32(txtDataRotateZ.Text);
            arg.DataScale = Convert.ToInt32(txtDataScale.Text);
            arg.DataX = Convert.ToInt32(txtDataX.Text);
            arg.DataY = Convert.ToInt32(txtDataY.Text);
            arg.DataZ = Convert.ToInt32(txtDataZ.Text);
            arg.Header = txtSlideHeader.Text;
            arg.SlideMatter = _rtb.Lines;
            arg.Class = cmbSlideType.Text;
        }

        private Boolean CheckNumeric(String arg)
        {
            try
            {
                int.Parse(arg);
                return true;
            }
            catch
            {
            }
            return false;
        }

        private void btnDeleteSlide_Click(object sender, RoutedEventArgs e)
        {
            _slides.RemoveAt(lstSlides.SelectedIndex);
            ResetSlideDataPanel();
        }

        private void menuOpen_Click(object sender, RoutedEventArgs e)
        {
            string fileName;
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "IStudio Project File|*.isproj";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fileName = dialog.FileName;
                }
                else
                {
                    return;
                }
            }
            var zip = new ZipFile(fileName);
            if (!Directory.Exists(@"temp"))
            {
                Directory.CreateDirectory("temp");
            }
            else
            {
                Directory.Delete("temp", true);
                Directory.CreateDirectory(@"temp");
            }
            zip.ExtractAll("temp/");
            string[] files = Directory.GetFiles(@"temp\");
            bool valid = false;
            foreach (var file in files)
            {
                if ((file == @"temp\tempSlides.ist"))
                {
                    valid = true;
                }
            }
            if (!valid)
            {
                MessageBox.Show("This is not a valid IStudio Project File",
                       "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (MessageBox.Show("It Will Overwrite any slides currently present",
                    "Warning",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    return;
                }
                _slides = null;
                _slides = new ObservableCollection<ImpressSlide>();
                var ser = new XmlSerializer(_slides.GetType());
                FileStream fs = File.OpenRead(@"temp\tempSlides.ist");
                _slides = (ObservableCollection<ImpressSlide>)ser.Deserialize(fs);
                lstSlides.ItemsSource = _slides;
                lstSlides.DisplayMemberPath = "Header";
                MessageBox.Show("Successfully loaded Project into workspace", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void MenuAbout_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Impression Studio Beta\n" +
                "This is a free and open-source software\n" + 
                "This software uses Impress.js library\n" +
                "Copyright (c)2014 Abhimanbhau Kolte",
                "About Impression Studio",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}