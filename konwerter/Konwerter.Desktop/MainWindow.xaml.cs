﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using Common.Controls;
using konwerter;
using UnitConverter.Desktop;

namespace Konwerter.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> converters_names = new List<string>()
            {
                new MassConv().name.ToString(),
                new TemperatureConv().name.ToString(),
                new LenghConv().name.ToString(),
                new StorageConv().name.ToString()
            };
        List<IConverter> converters = new List<IConverter>()
            {
                new MassConv(),
                new TemperatureConv(),
                new LenghConv(),
                new StorageConv()
            };
        public MainWindow()
        {
            InitializeComponent();
            From_ChoiseMassUnitComboBox.ItemsSource = converters[0].Units;
            To_ChoiseMassUnitComboBox.ItemsSource = converters[0].Units;
            From_ChoiseTempUnitComboBox.ItemsSource = converters[1].Units;
            To_ChoiseTempUnitComboBox.ItemsSource = converters[1].Units;
            From_ChoiseLenghUnitComboBox.ItemsSource = converters[2].Units;
            To_ChoiseLenghUnitComboBox.ItemsSource = converters[2].Units;
            From_ChoiseDataUnitComboBox.ItemsSource = converters[3].Units;
            To_ChoiseDataUnitComboBox.ItemsSource = converters[3].Units;            

            using (var historia = new DataEntities())
            {
                var dane = historia.Rate_History.AsQueryable();
                List<Rate_History> tmp = dane.OrderByDescending(LP => LP.IDRate).Take(1).ToList();
                foreach (var a in tmp)
                {
                    RateControl.Wartosc_Oceny = a.RateValue;
                }
            }
            TempConvertCommand = new RelayCommand(obj => Temp_Convert(), obj => string.IsNullOrEmpty(InputTempTextBox.Text)
            != true && From_ChoiseTempUnitComboBox.SelectedItem != To_ChoiseTempUnitComboBox.SelectedValue);
            Temp_Button.Command = TempConvertCommand;

            MassConvertCommand = new RelayCommand(obj => Mass_Convert(), obj => string.IsNullOrEmpty(InputMassTextBox.Text)
            != true && From_ChoiseMassUnitComboBox.SelectedItem != To_ChoiseMassUnitComboBox.SelectedValue);
            Mass_Button.Command = MassConvertCommand;

            LenghtConvertCommand = new RelayCommand(obj => Lenght_Convert(), obj => string.IsNullOrEmpty(InputLenghTextBox.Text)
            != true && From_ChoiseLenghUnitComboBox.SelectedItem != To_ChoiseLenghUnitComboBox.SelectedValue);
            Lenght_Button.Command = LenghtConvertCommand;

            DataConvertCommand = new RelayCommand(obj => Data_Convert(), obj => string.IsNullOrEmpty(InputDataTextBox.Text)
            != true && From_ChoiseDataUnitComboBox.SelectedItem != To_ChoiseDataUnitComboBox.SelectedValue);
            Data_Button.Command = DataConvertCommand;

            TimeConvertCommand = new RelayCommand(obj => Time_Convert(), obj => string.IsNullOrEmpty(InputTimeHourTextBox.Text) != true
            && string.IsNullOrEmpty(InputTimeMinTextBox.Text) != true);
            Time_Button.Command = TimeConvertCommand;
        }

        public static void DisplayDataUsingEF()
        {
            using (DataEntities context = new DataEntities())
            {
                List<Konwert_History> history_conversion = context.Konwert_History.ToList();

            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private RelayCommand TempConvertCommand;

        private void Temp_Convert()
        {
            string inputValueTemp = InputTempTextBox.Text;
            decimal value_Temp;
            if (decimal.TryParse(inputValueTemp, out value_Temp) == true)
            {
                converters[1].Data_and_convert(From_ChoiseTempUnitComboBox.Text, To_ChoiseTempUnitComboBox.Text, value_Temp);
                ResultTempTextBlock.Text = Math.Round(converters[1].Convert(), 4, MidpointRounding.AwayFromZero).ToString();
            }
            else
            {
                ResultTempTextBlock.Text = "Error";
                MessageBox.Show("Podaj poprawną wartość liczbową");
            }
            using (DataEntities context = new DataEntities())
            {
                Konwert_History rec = new Konwert_History()
                {
                    Data = System.DateTime.Now,
                    Wartosc_z = decimal.Parse(InputTempTextBox.Text),
                    Jednostka_z = From_ChoiseTempUnitComboBox.Text,
                    Wartosc_do = decimal.Parse(ResultTempTextBlock.Text),
                    Jednostka_do = To_ChoiseTempUnitComboBox.Text
                };
                context.Konwert_History.Add(rec);
                context.SaveChanges();
            }
        }

        private RelayCommand MassConvertCommand;

        private void Mass_Convert()
        {
            string inputValueMass = InputMassTextBox.Text;
            decimal value_Mass;
            if (decimal.TryParse(inputValueMass, out value_Mass) == true)
            {
                converters[0].Data_and_convert(From_ChoiseMassUnitComboBox.Text, To_ChoiseMassUnitComboBox.Text, value_Mass);
                ResultMassTextBlock.Text = Math.Round(converters[0].Convert(), 4, MidpointRounding.AwayFromZero).ToString();
            }
            else
            {
                ResultMassTextBlock.Text = "Error";
                MessageBox.Show("Podaj poprawną wartość liczbową");
            }
            using (DataEntities context = new DataEntities())
            {
                Konwert_History rec = new Konwert_History()
                {
                    Data = System.DateTime.Now,
                    Wartosc_z = decimal.Parse(InputMassTextBox.Text),
                    Jednostka_z = From_ChoiseMassUnitComboBox.Text,
                    Wartosc_do = decimal.Parse(ResultMassTextBlock.Text),
                    Jednostka_do = To_ChoiseMassUnitComboBox.Text
                };
                context.Konwert_History.Add(rec);
                context.SaveChanges();
            }
        }

        private RelayCommand LenghtConvertCommand;

        private void Lenght_Convert()
        {
            string inputValueLengh = InputLenghTextBox.Text;
            decimal value_Lengh;
            if (decimal.TryParse(inputValueLengh, out value_Lengh) == true)
            {
                converters[2].Data_and_convert(From_ChoiseLenghUnitComboBox.Text, To_ChoiseLenghUnitComboBox.Text, value_Lengh);
                ResultLenghTextBlock.Text = Math.Round(converters[2].Convert(), 4, MidpointRounding.AwayFromZero).ToString();
            }
            else
            {
                ResultLenghTextBlock.Text = "Error";
                MessageBox.Show("Podaj poprawną wartość liczbową");
            }
            using (DataEntities context = new DataEntities())
            {
                Konwert_History rec = new Konwert_History()
                {
                    Data = System.DateTime.Now,
                    Wartosc_z = decimal.Parse(InputLenghTextBox.Text),
                    Jednostka_z = From_ChoiseLenghUnitComboBox.Text,
                    Wartosc_do = decimal.Parse(ResultLenghTextBlock.Text),
                    Jednostka_do = To_ChoiseLenghUnitComboBox.Text
                };
                context.Konwert_History.Add(rec);
                context.SaveChanges();
            }
        }

        private RelayCommand DataConvertCommand;

        private void Data_Convert()
        {
            string inputValueData = InputDataTextBox.Text;
            decimal value_Data;
            if (decimal.TryParse(inputValueData, out value_Data) == true)
            {
                converters[3].Data_and_convert(From_ChoiseDataUnitComboBox.Text, To_ChoiseDataUnitComboBox.Text, value_Data);
                ResultDataTextBlock.Text = Math.Round(converters[3].Convert(), 4, MidpointRounding.AwayFromZero).ToString();
            }
            else
            {
                ResultDataTextBlock.Text = "Error";
                MessageBox.Show("Podaj poprawną wartość liczbową");
            }
            using (DataEntities context = new DataEntities())
            {
                Konwert_History rec = new Konwert_History()
                {
                    Data = System.DateTime.Now,
                    Wartosc_z = decimal.Parse(InputDataTextBox.Text),
                    Jednostka_z = From_ChoiseDataUnitComboBox.Text,
                    Wartosc_do = decimal.Parse(ResultDataTextBlock.Text),
                    Jednostka_do = To_ChoiseDataUnitComboBox.Text
                };
                context.Konwert_History.Add(rec);
                context.SaveChanges();
            }
        }

        private RelayCommand TimeConvertCommand;

        private void Time_Convert()
        {
            string input_hour = InputTimeHourTextBox.Text;
            string input_min = InputTimeMinTextBox.Text;
            int hour, minutes;
            if (int.TryParse(input_hour, out hour) == true)
            {
                if (hour < 0 || hour > 24)
                {
                    MessageBox.Show("Podaj poprawną godzinę");
                }
                else
                {
                    if (hour >= 0 && hour <= 12)
                    {
                        ResultTimeHourTextBlock.Text = hour.ToString();
                        ResultTimePMTextBlock.Text = "AM";
                    }
                    if (hour > 12 && hour < 24)
                    {
                        hour -= 12;
                        ResultTimeHourTextBlock.Text = hour.ToString();
                        ResultTimePMTextBlock.Text = "PM";
                    }
                    if (hour == 24)
                    {
                        hour = 00;
                        ResultTimeHourTextBlock.Text = hour.ToString();
                        ResultTimePMTextBlock.Text = "AM";
                    }
                    double hours = hour * 30;
                    HourRotate.Angle = hours;
                }
            }
            if (int.TryParse(input_min, out minutes) == true)
            {
                if (minutes < 0 || minutes > 60)
                {
                    MessageBox.Show("Podaj poprawną wartość minut");
                }
                else
                {
                    ResultTimeMinTextBlock.Text = minutes.ToString();
                    double minut = minutes * 6;
                    MinuteRotate.Angle = minut;
                }
            }
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void History_Data_Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void Daj_historie(DateTime date_od, DateTime date_do, int biezaca)
        {           
            
            using (var historia = new DataEntities())
            {
                var dane = historia.Konwert_History.AsQueryable();
                if (date_od != null)
                {
                    dane = dane.Where(DATE => DATE.Data >= date_od);
                }
                if (date_do != null)
                {
                    dane = dane.Where(DATE => DATE.Data <= date_do);
                }
                var x = dane.OrderBy(LP => LP.ID).Skip(10 * (biezaca - 1)).Take(10).ToList();
                Dispatcher.Invoke(() =>
                {
                    History_Data_Grid.ItemsSource = x;
                });
            }              
            Task.Delay(5000).Wait();
        }
    
        private void Get_Button_Click(object sender, RoutedEventArgs e)
        {
            LoadingRectangle.Visibility = Visibility.Visible;
            LoaderCircle.Visibility = Visibility.Visible;
            string x = date_od.SelectedDate.ToString();
            DateTime data_od = DateTime.Parse(x);
            string y = date_do.SelectedDate.ToString();
            DateTime data_do = DateTime.Parse(y);            
            int biezaca;
            biezaca = int.Parse(strona.Content.ToString());            
            Task t1 = new Task(() => Daj_historie(data_od, data_do, biezaca));
            t1.Start();
            Task.WhenAll(t1).ContinueWith(t =>
            {
                LoaderCircle.Visibility = Visibility.Hidden;
                LoadingRectangle.Visibility = Visibility.Hidden;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Prev_Button_Click(object sender, RoutedEventArgs e)
        {
            int biezaca;
            biezaca = int.Parse(strona.Content.ToString());
            string x = date_od.SelectedDate.ToString();
            DateTime data_od = DateTime.Parse(x);
            string y = date_do.SelectedDate.ToString();
            DateTime data_do = DateTime.Parse(y);
            if (biezaca > 1)
            {
                strona.Content = biezaca - 1;
            }
            Daj_historie(data_od, data_do, biezaca);
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            int biezaca;
            biezaca = int.Parse(strona.Content.ToString());
            string x = date_od.SelectedDate.ToString();
            DateTime data_od = DateTime.Parse(x);
            string y = date_do.SelectedDate.ToString();
            DateTime data_do = DateTime.Parse(y);
            strona.Content = biezaca + 1;
            Daj_historie(data_od, data_do, biezaca);
        }

        private void Top_5_Click(object sender, RoutedEventArgs e)
        {
            using (var dane = new DataEntities())
            {
                var items = dane.Konwert_History.GroupBy(X => new { X.Jednostka_z, X.Wartosc_z, X.Jednostka_do, X.Wartosc_do });
                var top5 = items.Select(x => new { ile = x.Count(), x.Key.Jednostka_z, x.Key.Jednostka_do, x.Key.Wartosc_z, x.Key.Wartosc_do})
                    .OrderByDescending(x => x.ile)
                    .Take(5);
                History_Data_Grid.ItemsSource = top5.ToList();
            }
        }

        private void RateControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void RateControl_Wartosc_Oceny_Changed_1(object sender, Common.Controls.RateEventArgs e)
        {
            using (DataEntities cont = new DataEntities())
            {
                Rate_History rec = new Rate_History()
                {
                    RateValue = e.Value
                };
                cont.Rate_History.Add(rec);
                cont.SaveChanges();
            }
        }
    }
}