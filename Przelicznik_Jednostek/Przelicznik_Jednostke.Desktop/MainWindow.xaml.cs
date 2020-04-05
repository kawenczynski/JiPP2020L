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
using Przelicznik_Jednostek;

namespace Przelicznik_Jednostke.Desktop
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ComboBox_Unit.ItemsSource = new List<string>()
            {
                "Przelicz C na F",
                "Przelicz F na C",
                "Przelicz M na KM",
                "Przelicz KM na M",
                "Przelicz KM na Yd",
                "Przelicz Yd na KM",
                "Przelicz KG na Ibs",
                "Przelicz Ibs na KG",
            };
        }

        private void Button_Convert_Click(object sender, RoutedEventArgs e)
        {
            //Dzialania operacja = new Dzialania();
            if (ComboBox_Unit.SelectedItem == "Przelicz C na F")
            {
                string input = TexBox_Number.Text;
                int q = 2;
                double jednostka = double.Parse(input);
                 
                TextBlock_Converted.Text = new Dzialania().which_submition(q,jednostka);
            }
            if (ComboBox_Unit.SelectedItem == "Przelicz F na C")
            {
                string input = TexBox_Number.Text;
                int q = 1;
                double jednostka = double.Parse(input);
                
                TextBlock_Converted.Text = new Dzialania().which_submition(q,jednostka);
            }
            if (ComboBox_Unit.SelectedItem == "Przelicz M na KM")
            {
                string input = TexBox_Number.Text;
                int q = 3;
                double jednostka = double.Parse(input);
                TextBlock_Converted.Text = new Dzialania().which_submition(q,jednostka);

            }
            if (ComboBox_Unit.SelectedItem == "Przelicz KM na M")
            {
                string input = TexBox_Number.Text;
                int q = 4;
                double jednostka = double.Parse(input);
                 
                TextBlock_Converted.Text = new Dzialania().which_submition(q,jednostka);
            }
            if (ComboBox_Unit.SelectedItem == "Przelicz KG na Ibs")
            {
                string input = TexBox_Number.Text;
                int q = 6;
                double jednostka = double.Parse(input);
               
                TextBlock_Converted.Text = new Dzialania().which_submition(q,jednostka);
            }
            if (ComboBox_Unit.SelectedItem == "Przelicz Ibs na KG")
            {
                string input = TexBox_Number.Text;
                int q = 5;
                double jednostka = double.Parse(input);
               TextBlock_Converted.Text = new Dzialania().which_submition(q,jednostka);
                 
            }
            if (ComboBox_Unit.SelectedItem == "Przelicz KM na Yd")
            {
                string input = TexBox_Number.Text;
                int q = 7;
                double jednostka = double.Parse(input);
                TextBlock_Converted.Text = new Dzialania().which_submition(q, jednostka);

            }
            if (ComboBox_Unit.SelectedItem == "Przelicz Yd na KM")
            {
                string input = TexBox_Number.Text;
                int q = 8;
                double jednostka = double.Parse(input);
                TextBlock_Converted.Text = new Dzialania().which_submition(q, jednostka);

            }
        }
    }
}
