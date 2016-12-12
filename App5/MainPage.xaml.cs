using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;

using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App5
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            int i = textBoxHandleList.Text.Split('\r').Length;

            String[] subHandles = textBoxHandleList.Text.Split('\r');
            foreach (var subHandle in subHandles)
            {
                string msg = handleCleanup(subHandle);
                var dialog = new MessageDialog(msg);
                await dialog.ShowAsync();
            }
        }
        public string handleCleanup(string handle)
        {
            string urlPatternGood = @"http(\:|s\:)\/\/(www\.twitter\.com|twitter\.com)\/[A-Za-z0-9_]{1,15}";
            string urlPatternLongHandle = @"http(\:|s\:)\/\/(www\.twitter\.com|twitter\.com)\/@([A-Za-z0-9_]+){16,}|([A-Za-z0-9_]+){16,}";
            string urlPatternGoodHandle = @"^@([A-Za-z0-9_]+){1,15}|([A-Za-z0-9_]+){1,15}";
            string urlPatternBadHandle = @"^(@[A-Za-z0-9_]+){16,}|([A-Za-z0-9_]+){16,}";
            if (System.Text.RegularExpressions.Regex.IsMatch(handle, urlPatternGood))
                return handle;
            else if (System.Text.RegularExpressions.Regex.IsMatch(handle, urlPatternLongHandle))
                return "Sorry, but " + handle + "is not a valid twitter link.";
            else if (System.Text.RegularExpressions.Regex.IsMatch(handle, urlPatternGoodHandle))
                return "https://www.twitter.com/" + handle;
        }
    }
}
