using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.Xml.Linq;

namespace izsu
{
    public partial class MainPage : PhoneApplicationPage
    {
        public class item
        {
            public string title { get; set; }
            public string link { get; set; }
            public string description { get; set; }
            public string pubDate { get; set; }
        }

        public static List<item> Items = new List<item>();

        public MainPage()
        {
            InitializeComponent();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            WebClient client = new WebClient();
            client.OpenReadCompleted += client_OpenReadCompleted;
            client.OpenReadAsync(new Uri("http://www.izsu.gov.tr/pages/rss.aspx?rssId=2", UriKind.Absolute));
        }
        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null)
                return;
            Stream str = e.Result;
            try
            {
                String eve = "item";
                XDocument loadedData = XDocument.Load(str);

                foreach (var x in loadedData.Descendants(eve))
                {
                    try
                    {
                        item c = new item();
                        c.title = x.Element("title").Value;
                        c.link = x.Element("link").Value;
                        c.description = x.Element("description").Value;
                        c.pubDate = x.Element("pubDate").Value;
                        Items.Add(c);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                list.ItemsSource = Items;
            }
            catch (System.Xml.XmlException ex)
            {
                MessageBox.Show("Bağlantı Hatası!\nLütfen Tekrar Deneyin.");
            }
        }
    }
}