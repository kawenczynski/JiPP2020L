﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using Konwerter.Logic;


namespace Konwerter.Desktop
{
    /// <summary>
    /// Interaction logic for HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {

        private List<ConversionHistory> HistoryList = new List<ConversionHistory>();

        public HistoryWindow()
        {
            InitializeComponent();

            ConvertersComboBox.ItemsSource = (new Konwertery()).WybierzKonwerter();
        }


        private IQueryable<ConversionHistory> Filter(IQueryable<ConversionHistory> items)
        {
            if (StartDateDatePicker.SelectedDate.HasValue)
                items = items.Where(i => DbFunctions.TruncateTime(i.Date) >= StartDateDatePicker.SelectedDate);
            if (EndDateDatePicker.SelectedDate.HasValue)
                items = items.Where(i => DbFunctions.TruncateTime(i.Date) <= EndDateDatePicker.SelectedDate);
            if (ConvertersComboBox.SelectedIndex != -1)
                items = items.Where(i => i.Converter == ((IKonwerter)ConvertersComboBox.SelectedItem).Nazwa);


            return items;

        }


        public void LoadHistory()
        {
            using (var db = new jippEntities())
            {

                var items = db.ConversionHistory.AsQueryable<ConversionHistory>();


                items = Filter(items);


                int count = items.Count();

                PageSelectorComboBox.Items.Clear();

                for (int i = 0, j = 1; i < count; i += 20, j++)
                {
                    PageSelectorComboBox.Items.Add(j);
                }

                PageSelectorComboBox.SelectedIndex = 0;

                HistoryDataGrid.ItemsSource = items.OrderBy(i => i.Id).Take(20).ToList();

                var mostPopular = items.GroupBy(i => new { i.Converter, i.UnitFrom, i.UnitTo }).Select(i => new { Type = i.Key, Count = i.Count() }).OrderByDescending(i => i.Count).Take(3).ToList();


                MostPopularTextBlock.Text = "";

                foreach (var a in mostPopular)
                    MostPopularTextBlock.Text += a.Type.Converter + " [" + a.Type.UnitFrom + " -> " + a.Type.UnitTo + "] (" + a.Count + ")\n";

            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void filterButon_Click(object sender, RoutedEventArgs e)
        {
            LoadHistory();
        }

        private void ClearfilterButon_Click(object sender, RoutedEventArgs e)
        {
            ConvertersComboBox.SelectedIndex = -1;
            StartDateDatePicker.SelectedDate = null;
            EndDateDatePicker.SelectedDate = null;
            LoadHistory();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadHistory();
        }

        private void PageSelectorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void HistoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}


