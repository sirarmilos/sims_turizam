using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace InitialProject
{
    class AppTheme
    {
        public static void ChangeTheme(Uri themeUri, Uri langUri)
        {
            ResourceDictionary Theme = new ResourceDictionary() { Source = themeUri };
            ResourceDictionary Language = new ResourceDictionary() { Source = langUri };

            App.Current.Resources.Clear();

            App.Current.Resources.MergedDictionaries.Add(Theme);
            App.Current.Resources.MergedDictionaries.Add(Language);
        }

    }
}
