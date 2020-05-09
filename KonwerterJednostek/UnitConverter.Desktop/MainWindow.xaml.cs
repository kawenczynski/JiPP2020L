﻿using KonwerterJednostek;
using System;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UnitConverter.Logic;

namespace UnitConverter.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isClockVisible = false;
        bool isRecordsSortedByConverterTypeDescending = false;
        int paginationVariable=0;

        public MainWindow()
        {
            InitializeComponent();
            converterCombobox.ItemsSource = new ConverterService().GetConverters();

            ConvertCommand = new RelayCommand(obj => Convert(),
                obj => fromCombobox.SelectedItem !=null
                && toCombobox.SelectedItem != null 
                && string.IsNullOrEmpty(inputTextbox.Text)!=true);
            convertButton.Command = ConvertCommand;

            SearchDateCommand = new RelayCommand(obj => SearchDate(),
                obj => dateFrom.SelectedDate != null
                && dateTo.SelectedDate != null);
            dateFilter.Command = SearchDateCommand;

            ConverterTypeFilterCommand = new RelayCommand(obj => ConverterTypeFilter(),
                obj => typesOfConversions.SelectedItem != null);
            converterTypeFilter_valueFromCombobox.Command = ConverterTypeFilterCommand;

            BeginClockRotationCommand = new RelayCommand(obj => BeginClockRotation());
            button.Command = BeginClockRotationCommand;

            StopClockRotationCommand = new RelayCommand(obj => StopClockRotation());
            button1.Command = StopClockRotationCommand;

            ConverterTypeAscDescCommand = new RelayCommand(obj => ConverterTypeAscDesc());
            converterTypeFilterAsc.Command = ConverterTypeAscDescCommand;

            PaginationButtonPrevCommand = new RelayCommand(obj => PaginationButtonPrev());
            paginationButtonPrev.Command = PaginationButtonPrevCommand;

            PaginationButtonNextCommand = new RelayCommand(obj => PaginationButtonNext());
            paginationButtonNext.Command = PaginationButtonNextCommand;

            PopularConversionsCommand = new RelayCommand(obj => PopularConversions(),
                obj => dateFrom.SelectedDate != null
                && dateTo.SelectedDate != null);
            popularConversions.Command = PopularConversionsCommand;
        }     

        private void ConverterCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            string timeConverterName = new TimeConverter().Name;
            string selectedItemName = ((IConverter)converterCombobox.SelectedItem).Name.ToString();
            if (selectedItemName.Equals(timeConverterName) && isClockVisible==false)
            {
                //((Storyboard)Resources["clockVisibility"]).Begin();
                ((Storyboard)Resources["clockVisibility"]).AutoReverse = false;
                ((Storyboard)Resources["clockVisibility"]).Begin(this, false);
                isClockVisible = true;             
            }else if (!selectedItemName.Equals(timeConverterName) && isClockVisible == true)
            {
                ((Storyboard)Resources["clockVisibility"]).AutoReverse = true;
                ((Storyboard)Resources["clockVisibility"]).Begin(this, true);
                ((Storyboard)Resources["clockVisibility"]).Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.Duration);
                isClockVisible = false;
            }

            fromCombobox.ItemsSource =
                ((IConverter)converterCombobox.SelectedItem).Units;
            toCombobox.ItemsSource =
                ((IConverter)converterCombobox.SelectedItem).Units;
        }

        private RelayCommand ConvertCommand;
        private RelayCommand SearchDateCommand;
        private RelayCommand ConverterTypeFilterCommand;
        private RelayCommand BeginClockRotationCommand;
        private RelayCommand StopClockRotationCommand;
        private RelayCommand ConverterTypeAscDescCommand;
        private RelayCommand PaginationButtonPrevCommand;
        private RelayCommand PaginationButtonNextCommand;
        private RelayCommand PopularConversionsCommand;

        private void Convert()
        {
            string inputText = inputTextbox.Text;
            double inputValue;
            Double.TryParse(inputText, out inputValue);

            double result = ((IConverter)converterCombobox.SelectedItem).Convert(
                ((IConverter)converterCombobox.SelectedItem).Units
                .IndexOf((string)fromCombobox.SelectedItem),
                ((IConverter)converterCombobox.SelectedItem).Units
                .IndexOf((string)toCombobox.SelectedItem),
                inputValue);

            resultTextblock.Text = result.ToString();

            InsertDataToDatabase();

            double hourArrowValue = result > 12 ? result - 12 : result;
            double minuteArrowValue = (hourArrowValue - (int)hourArrowValue) * 100;

            hourRotation.Angle = ((hourArrowValue / 12) * 360) + 90;
            minuteRotation.Angle = ((minuteArrowValue / 60) * 360) + 90;
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        private void BeginClockRotation()
        {
            ((Storyboard)Resources["circleStoryboard"]).Begin();
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        private void StopClockRotation()
        {
            ((Storyboard)Resources["circleStoryboard"]).Stop();
        }

        private void InsertDataToDatabase()
        {
            using (ConverterDatabaseEntities context = new ConverterDatabaseEntities())
            {
                ConverterData newRecord = new ConverterData
                {
                    UsedConverter = ((IConverter)converterCombobox.SelectedItem).Name.ToString(),
                    UnitFrom = fromCombobox.SelectedItem.ToString(),
                    UnitTo = toCombobox.SelectedItem.ToString(),
                    InputValue = inputTextbox.Text,
                    OutputValue = resultTextblock.Text,
                    ConvertDate = DateTime.Now,
                    Rate = rateButtons.RateValue
                };
                context.ConverterDatas.Add(newRecord);
                context.SaveChanges();
            }
        }

        private void DataFromDatabase_Loaded(object sender, RoutedEventArgs e)
        {
            using (ConverterDatabaseEntities context = new ConverterDatabaseEntities())
            {
                dataFromDatabase.ItemsSource = context.ConverterDatas.ToList();
            }
        }

        //private void ConverterTypeFilterAsc_Click(object sender, RoutedEventArgs e)
        private void ConverterTypeAscDesc()
        {            
            using (ConverterDatabaseEntities context = new ConverterDatabaseEntities())
            {
                if (isRecordsSortedByConverterTypeDescending)
                {
                    dataFromDatabase.ItemsSource = context.ConverterDatas.
                        OrderBy(d => d.UsedConverter).ToList();
                    isRecordsSortedByConverterTypeDescending = false;
                }
                else
                {
                    dataFromDatabase.ItemsSource = context.ConverterDatas.
                        OrderByDescending(d => d.UsedConverter).ToList();
                    isRecordsSortedByConverterTypeDescending = true;
                }


            }
        }

        //private void DateFilter_Click(object sender, RoutedEventArgs e)
        private void SearchDate()
        {
            using (ConverterDatabaseEntities context = new ConverterDatabaseEntities())
            {
                int yearFrom = dateFrom.SelectedDate.Value.Year;
                int monthFrom = dateFrom.SelectedDate.Value.Month;
                int dayFrom = dateFrom.SelectedDate.Value.Day;
                int yearTo = dateTo.SelectedDate.Value.Year;
                int monthTo = dateTo.SelectedDate.Value.Month;
                int dayTo = dateTo.SelectedDate.Value.Day;
                
                dataFromDatabase.ItemsSource = context.ConverterDatas
                    .Where(d =>
                    d.ConvertDate >= new DateTime(yearFrom,monthFrom,dayFrom))
                    .Where(d =>
                    d.ConvertDate <= new DateTime(yearTo, monthTo, dayTo))
                    .ToList();
            }
        }

        //private void PaginationButtonPrev_Click(object sender, RoutedEventArgs e)
        private void PaginationButtonPrev()
        {
            using (ConverterDatabaseEntities context = new ConverterDatabaseEntities())
            {                
                dataFromDatabase.ItemsSource = context.ConverterDatas
                    .ToList()
                    .Skip(paginationVariable)
                    .Take(10)
                    .ToList();
                paginationVariable -= 10;
            }
        }

        //private void PaginationButtonNext_Click(object sender, RoutedEventArgs e)
        private void PaginationButtonNext()
        {
            using (ConverterDatabaseEntities context = new ConverterDatabaseEntities())
            {
                dataFromDatabase.ItemsSource = context.ConverterDatas
                    .ToList()
                    .Skip(paginationVariable)
                    .Take(10)
                    .ToList();
                paginationVariable += 10;
            }
        }

        private void TypesOfConversions_Loaded(object sender, RoutedEventArgs e)
        {
            using (ConverterDatabaseEntities context = new ConverterDatabaseEntities())
            {
                typesOfConversions.ItemsSource = context.ConverterDatas
                    .Select(d => d.UsedConverter)
                    .Distinct()
                    .ToList();
            }
        }

        //private void PopularConversions_Click(object sender, RoutedEventArgs e)
            private void PopularConversions()
        {
            using (ConverterDatabaseEntities context = new ConverterDatabaseEntities())
            {
                dataFromDatabase.ItemsSource = context.ConverterDatas
                    .ToList()
                    .Where(d => d.ConvertDate >= new DateTime(
                        dateFrom.SelectedDate.Value.Year,
                        dateFrom.SelectedDate.Value.Month,
                        dateFrom.SelectedDate.Value.Day))
                    .Where(d => d.ConvertDate <= new DateTime(
                        dateTo.SelectedDate.Value.Year,
                        dateTo.SelectedDate.Value.Month,
                        dateTo.SelectedDate.Value.Day))
                    .GroupBy(d => d.UsedConverter)
                    .Select(d => new { UsedConverter = d.Key, Count = d.Count() })
                    .OrderByDescending(d => d.Count)
                    .Take(3);
            }
        }

        //private void ConverterTypeFilter_valueFromCombobox_Click(object sender, RoutedEventArgs e)
        private void ConverterTypeFilter()
        {
            using (ConverterDatabaseEntities context = new ConverterDatabaseEntities())
            {
                dataFromDatabase.ItemsSource = context.ConverterDatas
                .Where(d => d.UsedConverter
                == typesOfConversions.SelectedItem.ToString())
                .ToList();
            }
        }

        private void rateControl_RateValueChanged(object sender, Common.Controls.RateEventArgs e)
        {            
            using (ConverterDatabaseEntities context = new ConverterDatabaseEntities())
            {
                //ConverterData newRecord = new ConverterData
                //{
                //    UsedConverter = "",
                //    UnitFrom = "",
                //    UnitTo = "",
                //    InputValue = "",
                //    OutputValue = "",
                //    ConvertDate = DateTime.Now,
                //    Rate = e.value
                //};
                //context.ConverterDatas.Add(newRecord);
                //context.SaveChanges();
                RateData newRecord = new RateData
                {
                    Rate = e.value,
                    RateDate = DateTime.Now
                };
                context.ConverterRates.Add(newRecord);
                context.SaveChanges();
            }
        }


        private void RateMe_Loaded(object sender, RoutedEventArgs e)
        {
            using (ConverterDatabaseEntities context = new ConverterDatabaseEntities())
            {
                int? lastRateFromDatabase = context.ConverterRates
                    .OrderByDescending(r => r.Id).FirstOrDefault().Rate;

                if (lastRateFromDatabase != null)
                    rateButtons.RateValue = lastRateFromDatabase ?? default(int);
            }
        }
    }
}
