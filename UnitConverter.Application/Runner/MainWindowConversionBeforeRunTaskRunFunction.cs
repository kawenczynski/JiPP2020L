﻿using System.Text.RegularExpressions;
using UnitConverter.Library.TaskUtil;
using UnitConverter.Library.TaskUtil.Parameter;
using UnitConverter.Library.TypeUtil.DateTimeType;

namespace UnitConverter.Application.Runner
{
    /// <summary>
    /// Klasa implementująca interfejs <see cref="TaskRunFunction"/>, który reprezentuję funkcję,
    /// która będzie uruchamiana przed wykonaniem operacji konwertowania jednostek miar
    /// </summary>
    /// <param name="mainWindow">
    ///     Pole przechowujące instancję obiektu okna głwnego
    /// </param>
    /// <see cref="TaskRunFunction"/>
    public class MainWindowConversionBeforeRunTaskRunFunction : TaskRunFunction
    {
        private MainWindow mainWindow;

        public MainWindowConversionBeforeRunTaskRunFunction(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }



        public void apply(IRunnable runnable)
        {
            string input = mainWindow.providedValueTextBoxUtils.getText();
            SelectableTask currentTask = (SelectableTask)mainWindow.selectedOperation.task;

            SelectableTaskParameterOption selectedFromConversionOption = TaskParameterUtils.toSelectableTaskParameter(
                    currentTask.getSelectedOperation().task.getParameter("fromConversion")
                ).getSelectedOption();

            if (selectedFromConversionOption.type == typeof(Custom12HTime))
            {
                if (mainWindow.providedValueTextBoxUtils.valid && !Regex.IsMatch(mainWindow.providedValueTextBoxUtils.getText(), @"\s([Aa]|[Pp])[Mm]$"))
                {
                    input += " " + mainWindow.timeFormatComboBoxUtils.getSelectedContent().ToString();
                }
            }

            runnable.setParameter("value", input);
        }
    }
}
