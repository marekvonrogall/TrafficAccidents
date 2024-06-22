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

        private void boxAccidentID_showDS_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            labelAccidentID_showDS.Visibility = Visibility.Hidden;
            try
            {
                int id = Convert.ToInt32(boxAccidentID_showDS.Text);
                read.GetEntryById(cassandraDb.Session, id);
                if (read.Accidents[0] != null)
                {
                    DisplayAccident();
                }
                else { throw new Exception(); }
            }
            catch { labelAccidentID_showDS.Visibility = Visibility.Visible; }
        }

        private void DisplayAccident()
        {
            string[] coordinates = read.Accidents[0].GeoPoint.Split(',');
            string mlat = coordinates[0].Trim();
            string mlon = coordinates[1].Trim();
            ShowCoordinatesOnMap(mlat, mlon);

            labelType.Content = read.Accidents[0].Typ;
            labelSeriousness.Content = read.Accidents[0].Schwere;

            string[] numberDayOfWeek = read.Accidents[0].Wochentag.Split(' ');
            string number = Convert.ToInt32(numberDayOfWeek[0].Trim()).ToString("D2");
            string dayOfWeek = numberDayOfWeek[1].Trim();
            string year = read.Accidents[0].Jahr.ToString();
            string month = read.Accidents[0].Monat.ToString("D2");
            string hour = read.Accidents[0].Stunde.ToString();

            string date = $"{dayOfWeek}, der {number}.{month}.{year}";
            labelDate.Content = date;

            if(hour != "") { labelHour.Content = hour; }
            else { labelHour.Content = "/"; }
        }

        private void buttonAddEntry_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonModifyEntry_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonDeleteEntry_Click(object sender, RoutedEventArgs e)
        {

        }
        private void buttonDisplayEntry_Click(object sender, RoutedEventArgs e)
        {
            canvasDisplayData.Visibility = Visibility.Visible;
        }
    }
}
