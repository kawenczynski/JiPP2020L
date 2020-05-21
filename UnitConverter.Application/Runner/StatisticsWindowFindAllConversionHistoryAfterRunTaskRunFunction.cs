﻿using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using UnitConverter.Application.AppWindow;
using UnitConverter.Library.History;
using UnitConverter.Library.TaskUtil;
using UnitConverter.Library.TypeUtil.Number;
using System;
using System.Diagnostics;
using System.Windows;

namespace UnitConverter.Application.Runner
{
    /// <summary>
    /// Klasa implementująca interfejs <see cref="TaskRunFunction"/>, reprezentująca funkcję, 
    /// która będzie uruchamiana po wykonaniu operacji wyszukiwania historii konwersacji.
    /// Klasa ta jest odpowiedzialna za wyświetlenie wszytkich wpisów z historii konwersji
    /// w tabeli danych.
    /// </summary>
    /// <see cref="StatisticsWindow"/>
    /// <see cref="TaskRunFunction"/>
    public class StatisticsWindowFindAllConversionHistoryAfterRunTaskRunFunction : TaskRunFunction
    {
        public void apply(IRunnable runnable)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                StatisticsWindow statisticsWindow = (StatisticsWindow)runnable.getParameter("statisticsWindow").value;
                List<object> results = (List<object>)runnable.getResult();

                List<ConversionHistory> wholeHistory = (List<ConversionHistory>)results[0];
                List<KeyValuePair<ConversionHistory, int>> topThreeConversions = (List<KeyValuePair<ConversionHistory, int>>)results[1];

                CustomInteger currentPage = (CustomInteger)runnable.getParameter("currentPage").value;
                CustomInteger pageSize = (CustomInteger)runnable.getParameter("pageSize").value;

                if (pageSize < wholeHistory.Count)
                {
                    statisticsWindow.paginationDockPanelRowDefinition.Height = new System.Windows.GridLength(50);
                    statisticsWindow.paginationSwitcher.pages = (wholeHistory.Count / pageSize.toPrimitiveValue()) + 1;

                    wholeHistory = wholeHistory
                                    .Skip(((currentPage - 1) * pageSize).toPrimitiveValue())
                                    .Take(Math.Min((pageSize + (pageSize * (currentPage - 1))).toPrimitiveValue(), wholeHistory.Count))
                                    .ToList();
                }
                else
                {
                    statisticsWindow.paginationDockPanelRowDefinition.Height = new System.Windows.GridLength(0);
                }

                Trace.WriteLine("StatisticsWindowFindAllConversionHistoryAfterRunTaskRunFunction :: Pages ammmount: " + statisticsWindow.paginationSwitcher.pages);

                statisticsWindow.statisticsDataDrid.ItemsSource = wholeHistory;

                statisticsWindow.rowCountStatusbar.Content = string.Format("Łączna liczba rekordów: {0}", wholeHistory.Count);

                statisticsWindow.conversionNamesStackPanel.Children.Clear();

                topThreeConversions.ForEach(conversion =>
                {
                    statisticsWindow.conversionNamesStackPanel.Children.Add(
                        new Label()
                        {
                            Content = string.Format(" - {0} ({1})", conversion.Key.converterName, conversion.Value)
                        }
                    );
                });

                statisticsWindow.statisticsWindowLoadingSpinner.Visibility = Visibility.Hidden;
            });
        }
    }
}
