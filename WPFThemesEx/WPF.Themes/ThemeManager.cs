using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace WPF.ThemesEx
{
    public static class ThemeManager
    {
        //public static readonly DependencyProperty ThemeProperty =
        //    DependencyProperty.RegisterAttached("Theme", typeof(string), typeof(ThemeManager),
        //        new FrameworkPropertyMetadata(string.Empty,
        //            OnThemeChanged));

        private static ResourceDictionary GetThemeResourceDictionary(string theme)
        {
            try
            {
                if (theme != null)
                {
                    string packUri = String.Format(@"/WPF.ThemesEx;component/{0}/Theme.xaml", theme);
                    return Application.LoadComponent(new Uri(packUri, UriKind.Relative)) as ResourceDictionary;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        public static IEnumerable<string> GetThemes()
        {
            var themes = new[]
            {
                "BubbleCreme",
                "ShinyBlue",
                "ExpressionDark",
                "MetroLight",
                "MetroDark",
                "ExpressionLight",
                "ShinyRed",
                "ShinyDarkTeal",
                "ShinyDarkPurple",
                "DavesGlossyControls",
                "WhistlerBlue",
                "BureauBlack",
                "BureauBlue",
                "TwilightBlue",
                "UXMusingsRed",
                "UXMusingsGreen",
                "UXMusingsRoughRed",
                "UXMusingsRoughGreen",
                "UXMusingsBubblyBlue"
            };
            return themes;
        }

        public static void ApplyTheme(this Application app, string theme)
        {
            try
            {
                ResourceDictionary dictionary = GetThemeResourceDictionary(theme);

                if (dictionary != null)
                {
                    app.Resources.MergedDictionaries.Clear();
                    app.Resources.MergedDictionaries.Add(dictionary);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static void ApplyTheme(this ContentControl control, string theme)
        {
            try
            {
                ResourceDictionary dictionary = GetThemeResourceDictionary(theme);

                if (dictionary != null)
                {
                    control.Resources.MergedDictionaries.Clear();
                    control.Resources.MergedDictionaries.Add(dictionary);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        //public static string GetTheme(DependencyObject d)
        //{
        //    return (string)d.GetValue(ThemeProperty);
        //}

        //public static void SetTheme(DependencyObject d, string value)
        //{
        //    d.SetValue(ThemeProperty, value);
        //}

        //private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    try
        //    {
        //        var theme = e.NewValue as string;
        //        if (theme == string.Empty)
        //        {
        //            return;
        //        }
        //        var control = d as ContentControl;
        //        if (control != null)
        //        {
        //            control.ApplyTheme(theme);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //    }
        //}
    }
}