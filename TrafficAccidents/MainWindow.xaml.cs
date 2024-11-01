﻿using System.Windows;
using TrafficAccidents.Classes;
using System;
using System.Windows.Media;
using System.Globalization;


namespace TrafficAccidents
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CassandraDb cassandraDb = new CassandraDb();
        private Read read = new Read();
        private Add add = new Add();
        private Delete delete = new Delete();
        private Modify modify = new Modify();

        public MainWindow()
        {
            InitializeComponent();
            HideAllCanvases();
            canvasDisplayData.Visibility = Visibility.Visible;
        }

        private async void ShowCoordinatesOnMap_Read(string mlat, string mlon)
        {
            await accidentMap_Read.EnsureCoreWebView2Async(null);
            accidentMap_Read.CoreWebView2.Navigate($"https://www.openstreetmap.org/?mlat={mlat}&mlon={mlon}");
        }

        private async void ShowCoordinatesOnMap_Add(string mlat, string mlon)
        {
            await accidentMap_Add.EnsureCoreWebView2Async(null);
            accidentMap_Add.CoreWebView2.Navigate($"https://www.openstreetmap.org/?mlat={mlat}&mlon={mlon}");
        }

        private async void ShowCoordinatesOnMap_Modify(string mlat, string mlon)
        {
            await accidentMap_Modify.EnsureCoreWebView2Async(null);
            accidentMap_Modify.CoreWebView2.Navigate($"https://www.openstreetmap.org/?mlat={mlat}&mlon={mlon}");
        }

        private void BoxAccidentID_showDS_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            labelAccidentID_showDS.Visibility = Visibility.Hidden;
            try
            {
                int id = Convert.ToInt32(boxAccidentID_showDS.Text);
                read.GetEntryById(cassandraDb.Session, id);
                if (read.Accident != null)
                {
                    DisplayAccident();
                }
                else { throw new Exception(); }
            }
            catch { labelAccidentID_showDS.Visibility = Visibility.Visible; }
        }

        private void DisplayAccident()
        {
            string[] coordinates = read.Accident.GeoPoint.Split(',');
            string mlat = coordinates[0].Trim();
            string mlon = coordinates[1].Trim();
            ShowCoordinatesOnMap_Read(mlat, mlon);

            labelType.Content = read.Accident.Typ;
            labelSeriousness.Content = read.Accident.Schwere;

            string[] numberDayOfWeek = read.Accident.Wochentag.Split(' ');
            string number = Convert.ToInt32(numberDayOfWeek[0].Trim()).ToString("D2");
            string dayOfWeek = numberDayOfWeek[1].Trim();
            string year = read.Accident.Jahr.ToString();
            string month = read.Accident.Monat.ToString("D2");
            string hour = read.Accident.Stunde.ToString();
            if (hour == "") hour = "/";

            string date = $"{dayOfWeek}, der {number}.{month}.{year} ({hour})";
            labelDate.Content = date;

            labelStreetType.Content = read.Accident.Strasseart;

            if (read.Accident.FussggBet == true) { checkPedestrian.IsChecked = true; }
            else checkPedestrian.IsChecked = false;

            if (read.Accident.FahrrdBet == true) { checkBicycle.IsChecked = true; }
            else checkBicycle.IsChecked = false;

            if (read.Accident.MotordBet == true) { checkMotorcycle.IsChecked = true; }
            else checkMotorcycle.IsChecked = false;
        }

        private void ButtonDisplayEntry_Click(object sender, RoutedEventArgs e)
        {
            RemoveButtonHighlight();
            buttonDisplayEntry.Background = new SolidColorBrush(Color.FromRgb(147, 141, 189));
            HideAllCanvases();
            canvasDisplayData.Visibility = Visibility.Visible;
        }

        private void ButtonAddEntry_Click(object sender, RoutedEventArgs e)
        {
            RemoveButtonHighlight();
            buttonAddEntry.Background = new SolidColorBrush(Color.FromRgb(147, 141, 189));
            HideAllCanvases();
            canvasAddData.Visibility = Visibility.Visible;

            boxAdd_accidentID.Text = read.GetNextAvailableId(cassandraDb.Session).ToString();
        }

        private void ButtonModifyEntry_Click(object sender, RoutedEventArgs e)
        {
            RemoveButtonHighlight();
            buttonModifyEntry.Background = new SolidColorBrush(Color.FromRgb(147, 141, 189));
            HideAllCanvases();
            canvasModifyData.Visibility = Visibility.Visible;
        }

        private void ButtonDeleteEntry_Click(object sender, RoutedEventArgs e)
        {
            RemoveButtonHighlight();
            buttonDeleteEntry.Background = new SolidColorBrush(Color.FromRgb(147, 141, 189));
            HideAllCanvases();
            canvasDeleteData.Visibility = Visibility.Visible;
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
            canvasModifyData.Visibility = Visibility.Hidden;
            canvasDeleteData.Visibility = Visibility.Hidden;
        }

        private void ButtonCreateAndAddEntry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime selectedDate = datePickerAdd_date.SelectedDate.Value;

                int year = selectedDate.Year;
                int month = selectedDate.Month;
                string day = selectedDate.Day.ToString();
                string dayOfWeek = selectedDate.ToString("dddd", new CultureInfo("de-DE"));
                string weekday = $"{day} {dayOfWeek}";
                int? hour = null;
                if (boxAdd_hour.Text != "") hour = Convert.ToInt32(boxAdd_hour.Text);

                Accident accident = new Accident
                {
                    GeoPoint = $"{boxAdd_mlat.Text}, {boxAdd_mlon.Text}",
                    GeoShape = $"{{\"coordinates\": [{boxAdd_mlon.Text}, {boxAdd_mlat.Text}], \"type\": \"Point\"}}",
                    IdUnfall = read.GetNextAvailableId(cassandraDb.Session),
                    Typ = boxAdd_type.Text,
                    Schwere = boxAdd_seriousness.Text,
                    Jahr = year,
                    Monat = month,
                    Wochentag = weekday,
                    Stunde = hour,
                    Strasseart = boxAdd_streetType.Text,
                    FussggBet = checkAdd_pedestrian.IsChecked ?? false,
                    FahrrdBet = checkAdd_bicycle.IsChecked ?? false,
                    MotordBet = checkAdd_motorcycle.IsChecked ?? false
                };

                add.AddEntry(cassandraDb.Session, accident);
                ClearAddPageElements();
            }
            catch
            {
                MessageBox.Show("Ungültige Eingaben erkannt.", "Fehler beim Hinzufügen");
            }
        }

        private void ClearAddPageElements()
        {
            boxAdd_mlat.Clear();
            boxAdd_mlon.Clear();
            boxAdd_accidentID.Text = read.GetNextAvailableId(cassandraDb.Session).ToString();
            boxAdd_type.Clear();
            boxAdd_seriousness.Clear();
            datePickerAdd_date.SelectedDate = null;
            boxAdd_hour.Clear();
            boxAdd_streetType.Clear();
            checkAdd_pedestrian.IsChecked = false;
            checkAdd_bicycle.IsChecked = false;
            checkAdd_motorcycle.IsChecked = false;
        }

        private void BoxAccidentID_deleteDS_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            labelAccidentID_deleteDS.Visibility = Visibility.Hidden;
            buttonDeleteEntryCanvas.IsEnabled = true;
            try
            {
                int id = Convert.ToInt32(boxAccidentID_deleteDS.Text);
                read.GetEntryById(cassandraDb.Session, id);
                if (read.Accident == null) throw new Exception();
            }
            catch
            {
                labelAccidentID_deleteDS.Visibility = Visibility.Visible;
                buttonDeleteEntryCanvas.IsEnabled = false;
            }
        }

        private void ButtonDeleteEntryCanvas_Click(object sender, RoutedEventArgs e)
        {
            delete.DeleteEntryById(cassandraDb.Session, Convert.ToInt32(boxAccidentID_deleteDS.Text));
            boxAccidentID_deleteDS.Clear();
        }

        private void BoxAccidentID_modifyDS_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            labelAccidentID_modifyDS.Visibility = Visibility.Hidden;
            buttonCreateAndModifyEntry.IsEnabled = true;
            try
            {
                int id = Convert.ToInt32(boxAccidentID_modifyDS.Text);
                read.GetEntryById(cassandraDb.Session, id);
                DisplayEntryInModifyForm();
                if (read.Accident == null) throw new Exception();
            }
            catch
            {
                labelAccidentID_modifyDS.Visibility = Visibility.Visible;
                buttonCreateAndModifyEntry.IsEnabled = false;
            }
        }

        private void DisplayEntryInModifyForm()
        {
            string[] coordinates = read.Accident.GeoPoint.Split(',');
            boxModify_mlat.Text = coordinates[0].Trim();
            boxModify_mlon.Text = coordinates[1].Trim();

            boxModify_type.Text = read.Accident.Typ;
            boxModify_seriousness.Text = read.Accident.Schwere;

            string[] numberDayOfWeek = read.Accident.Wochentag.Split(' ');
            int number = Convert.ToInt32(numberDayOfWeek[0].Trim());

            DateTime entryDate = new DateTime(read.Accident.Jahr, read.Accident.Monat, number);
            datePickerModify_date.SelectedDate = entryDate;

            boxModify_hour.Text = read.Accident.Stunde.ToString();

            boxModify_streetType.Text = read.Accident.Strasseart;

            if (read.Accident.FussggBet == true) { checkModify_pedestrian.IsChecked = true; }
            else checkModify_pedestrian.IsChecked = false;

            if (read.Accident.FahrrdBet == true) { checkModify_bicycle.IsChecked = true; }
            else checkModify_bicycle.IsChecked = false;

            if (read.Accident.MotordBet == true) { checkModify_motorcycle.IsChecked = true; }
            else checkModify_motorcycle.IsChecked = false;
        }

        private void ButtonCreateAndModifyEntry_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime selectedDate = datePickerModify_date.SelectedDate.Value;

                int year = selectedDate.Year;
                int month = selectedDate.Month;
                string day = selectedDate.Day.ToString();
                string dayOfWeek = selectedDate.ToString("dddd", new CultureInfo("de-DE"));
                string weekday = $"{day} {dayOfWeek}";
                int? hour = null;
                if (boxModify_hour.Text != "") hour = Convert.ToInt32(boxModify_hour.Text);

                Accident accident = new Accident
                {
                    GeoPoint = $"{boxModify_mlat.Text}, {boxModify_mlon.Text}",
                    GeoShape = $"{{\"coordinates\": [{boxModify_mlon.Text}, {boxModify_mlat.Text}], \"type\": \"Point\"}}",
                    IdUnfall = Convert.ToInt32(boxAccidentID_modifyDS.Text),
                    Typ = boxModify_type.Text,
                    Schwere = boxModify_seriousness.Text,
                    Jahr = year,
                    Monat = month,
                    Wochentag = weekday,
                    Stunde = hour,
                    Strasseart = boxModify_streetType.Text,
                    FussggBet = checkModify_pedestrian.IsChecked ?? false,
                    FahrrdBet = checkModify_bicycle.IsChecked ?? false,
                    MotordBet = checkModify_motorcycle.IsChecked ?? false
                };

                modify.UpdateEntry(cassandraDb.Session, accident);
            }
            catch
            {
                MessageBox.Show("Ungültige Eingaben erkannt.", "Fehler beim Hinzufügen");
            }
        }

        private void BoxAdd_coordinates_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ShowCoordinatesOnMap_Add(boxAdd_mlat.Text, boxAdd_mlon.Text);
        }

        private void BoxModify_coordinates_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ShowCoordinatesOnMap_Modify(boxModify_mlat.Text, boxModify_mlon.Text);
        }
    }
}
