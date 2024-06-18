using System.Windows;
using Microsoft.Web.WebView2.Core;
using Cassandra;
using TrafficAccidents.Classes;


namespace TrafficAccidents
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CassandraDb cassandraDb = new CassandraDb();
        private Read read = new Read();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ShowCoordinatesOnMap(string mlat, string mlon)
        {
            await accidentMap.EnsureCoreWebView2Async(null);
            accidentMap.CoreWebView2.Navigate($"https://www.openstreetmap.org/?mlat={mlat}&mlon={mlon}");
        }

        private void buttonDisplayEntry_Click(object sender, RoutedEventArgs e)
        {
            read.AllEntries(cassandraDb.Session);
            //ShowCoordinatesOnMap("47.569294711177534", "7.590064633926254");
        }
    }
}
