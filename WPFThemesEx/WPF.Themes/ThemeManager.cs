using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WPF.ThemesEx
{
    public static class ThemeManager
    {
        private static ResourceDictionary GetThemeResourceDictionary(string theme)
        {
            try
            {
                if (theme != null)
                {
                    return Application.LoadComponent(
                        new Uri(@"/WPF.ThemesEx;component/" + theme + "/Theme.xaml", UriKind.Relative))
                        as ResourceDictionary;
                }
            }
            catch
            { }
            return null;
        }

        public static IEnumerable<string> GetThemes()
        {
            var themes = new[]
            {
                "BubbleCreme","ShinyBlue","ExpressionDark","MetroLight","MetroDark",
                "ExpressionLight","ShinyRed","ShinyDarkTeal","ShinyDarkPurple",
                "DavesGlossyControls", "TwilightBlue","UXMusingsRed",
                "UXMusingsGreen","UXMusingsRoughRed", "UXMusingsRoughGreen","UXMusingsBubblyBlue"
            };
            return themes;
        }

        public static void ApplyTheme(this Application app, string theme)
        {
            try
            {
                var dictionary = GetThemeResourceDictionary(theme);
                if (dictionary == null)
                {
                    return;
                }
                app.Resources.MergedDictionaries.Clear();
                app.Resources.MergedDictionaries.Add(dictionary);
            }
            catch
            { }
        }

        public static void ApplyTheme(this ContentControl control, string theme)
        {
            try
            {
                var dictionary = GetThemeResourceDictionary(theme);
                if (dictionary == null)
                {
                    return;
                }
                control.Resources.MergedDictionaries.Clear();
                control.Resources.MergedDictionaries.Add(dictionary);
            }
            catch
            { }
        }
    }
}