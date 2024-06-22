using System.Windows;
using Microsoft.Web.WebView2.Core;
using Cassandra;
using TrafficAccidents.Classes;
using System;
using System.Windows.Media;


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
            HideAllCanvases();
            canvasDisplayData.Visibility = Visibility.Visible;
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
            if (hour == "") hour = "/";

            string date = $"{dayOfWeek}, der {number}.{month}.{year} ({hour})";
            labelDate.Content = date;

            labelStreetType.Content = read.Accidents[0].Strasseart;

            if (read.Accidents[0].FussggBet == true) { checkPedestrian.IsChecked = true; }
            else checkPedestrian.IsChecked = false;

            if (read.Accidents[0].FahrrdBet == true) { checkBicycle.IsChecked = true; }
            else checkBicycle.IsChecked = false;

            if (read.Accidents[0].MotordBet == true) { checkMotorcycle.IsChecked = true; }
            else checkMotorcycle.IsChecked = false;
        }

        private void buttonDisplayEntry_Click(object sender, RoutedEventArgs e)
        {
            RemoveButtonHighlight();
            buttonDisplayEntry.Background = new SolidColorBrush(Color.FromRgb(147, 141, 189));
            HideAllCanvases();
            canvasDisplayData.Visibility = Visibility.Visible;
        }

        private void buttonAddEntry_Click(object sender, RoutedEventArgs e)
        {
            RemoveButtonHighlight();
            buttonAddEntry.Background = new SolidColorBrush(Color.FromRgb(147, 141, 189));
            HideAllCanvases();
            canvasAddData.Visibility = Visibility.Visible;
        }

        private void buttonModifyEntry_Click(object sender, RoutedEventArgs e)
        {
            RemoveButtonHighlight();
            buttonModifyEntry.Background = new SolidColorBrush(Color.FromRgb(147, 141, 189));
        }

        private void buttonDeleteEntry_Click(object sender, RoutedEventArgs e)
        {
            RemoveButtonHighlight();
            buttonDeleteEntry.Background = new SolidColorBrush(Color.FromRgb(147, 141, 189));
        }

        private void RemoveButtonHighlight()
        {
            buttonDisplayEntry.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            buttonModifyEntry.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            buttonDeleteEntry.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            buttonAddEntry.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
        }

        private void HideAllCanvases()
        {
            canvasDisplayData.Visibility = Visibility.Hidden;
            canvasAddData.Visibility = Visibility.Hidden;
        }
    }
}
