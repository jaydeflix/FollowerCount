using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Net;
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
using HtmlAgilityPack;

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
            String outputHandles = "URL,Twitter Handle,Following,Followers";
            outputHandles = outputHandles + '\r';
            String[] subHandles = textBoxHandleList.Text.Split('\r');
            foreach (var subHandle in subHandles)
            {
                string msg = handleCleanup(subHandle);
                if (System.Text.RegularExpressions.Regex.IsMatch(msg, @"^Sorry,"))
                    outputHandles = outputHandles + msg + '\r';
                else 
                    outputHandles = outputHandles + pageScrape(msg) + '\r';

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
            else if (System.Text.RegularExpressions.Regex.IsMatch(handle, urlPatternBadHandle))
                return "Sorry, but " + handle + "is not a valid twitter handle.";
            else
                return "Sorry, but " + handle + "is not recognized as a valid twitter connection";
        }
        public string pageScrape(string url)
        {
            //http://dejanstojanovic.net/aspnet/2015/april/scraping-website-content-using-htmlagilitypack/ and https://www.codeproject.com/articles/659019/scraping-html-dom-elements-using-htmlagilitypack-h
            string target = url;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(target);
            HttpWebResponse response = (HttpWebResponse)request.BeginGetResponse();
            return "blah";
        }
    }
}
