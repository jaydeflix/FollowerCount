using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Net;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.Web;
using Windows.Web.Http;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;
using HtmlAgilityPack;
using System.Threading.Tasks;
using System.Threading;




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
            if (textBoxHandleList.Text.ToString().StartsWith("URL,Twitter Handle,Following,Followers"))
            {
                MessageDialog dialog = new MessageDialog("It looks like you've already run the process. Please ensure the text block is only twitter urls/handles.", "Information");
                await dialog.ShowAsync();
                return;
            }
            int i;
            i = textBoxHandleList.Text.Split('\r').Length;
            String outputHandles = "URL,Twitter Handle,Following,Followers";
            outputHandles = outputHandles + '\r';
            String[] subHandles = textBoxHandleList.Text.Split('\r');
            foreach (var subHandle in subHandles)
            {
    
                string msg = handleCleanup(subHandle);
                if (System.Text.RegularExpressions.Regex.IsMatch(msg, @"^Sorry,"))
                    outputHandles = outputHandles + msg + '\r';
                else if (System.Text.RegularExpressions.Regex.IsMatch(msg, @"^blankhandle"))
                { }
                else
                {
                    string value = await (pageScrape(msg));
                    outputHandles = outputHandles + value + '\r';
                }

            }
            textBoxHandleList.Text = outputHandles;
            CopyDataMsgBox();

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
                return "Sorry, but " + handle + "is not a valid twitter link.\r";
            else if (System.Text.RegularExpressions.Regex.IsMatch(handle, urlPatternGoodHandle))
                return "https://www.twitter.com/" + handle;
            else if (System.Text.RegularExpressions.Regex.IsMatch(handle, urlPatternBadHandle))
                return "Sorry, but " + handle + "is not a valid twitter handle.";
            else if (handle.Length < 1)
                return "blankhandle";
            else
                return "Sorry, but " + handle + "is not recognized as a valid twitter connection";
        }
        public async Task<string> pageScrape(string url)
        {
            string target = url;
            System.Uri twitterURL = new System.Uri(target);
            string returnValues = target + ',' + Regex.Replace(twitterURL.AbsolutePath.ToString(), @"/", "") + ',';
            var uri = new Uri(target);
            var httpClient = new HttpClient();

            // Always catch network exceptions for async methods
            try
            {
                var result = await httpClient.GetStringAsync(uri);
                Match followingCount = System.Text.RegularExpressions.Regex.Match(result, @"title\=\""([0-9,].*) Following");
                Match followerCount = System.Text.RegularExpressions.Regex.Match(result, @"title\=\""([0-9,].*) Followers");
                returnValues = returnValues + Regex.Replace(followingCount.Groups[1].ToString(),",","") + "," + Regex.Replace(followerCount.Groups[1].ToString(), ",","");
            }
            catch
            {
                // Details in ex.Message and ex.HResult.       
            }
            httpClient.Dispose();
            Random r = new Random();
            int rInt = r.Next(5, 15);
            await System.Threading.Tasks.Task.Delay(rInt * 100); 
            return returnValues;
        }

        async void CopyDataMsgBox()
        {

            var messageDialog = new MessageDialog("Would you like to copy the data to the clipboard?");

            messageDialog.Title = "Data Collection Complete";
            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand(
                "Yes",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));
            messageDialog.Commands.Add(new UICommand(
                "No",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            // Display message showing the label of the command that was invoked

            if ( command.Label == "Yes" )
            {
                DataPackage CopyData = new DataPackage();
                CopyData.SetText(textBoxHandleList.Text.ToString());
                Clipboard.SetContent(CopyData);
            }
        }


        }
    }


