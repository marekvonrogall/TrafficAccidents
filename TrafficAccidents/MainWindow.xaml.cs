using System.Windows;
using Microsoft.Web.WebView2.Core;
using Cassandra;
using TrafficAccidents.Classes;
using System;


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
            canvasDisplayData.Visibility = Visibility.Visible;
        }

        private void boxAccidentID_showDS_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            labelAccidentID_showDS.Visibility = Visibility.Hidden;
            try
            {
                int id = Convert.ToInt32(boxAccidentID_showDS.Text);
                read.GetEntryById(cassandraDb.Session, id);
                if (read.Accidents[0] != null)
                {
                    string[] coordinates = read.Accidents[0].GeoPoint.Split(',');
                    string mlat = coordinates[0].Trim();
                    string mlon = coordinates[1].Trim();
                    ShowCoordinatesOnMap(mlat, mlon);
                }
                else { throw new Exception(); }
            }
            catch { labelAccidentID_showDS.Visibility = Visibility.Visible; }
        }
    }
}
