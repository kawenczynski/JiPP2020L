﻿namespace UnitConverter.Library.TaskUtil.Parameter
{
    /// <summary>
    /// Klasa, która ma za zadanie utworzyć nową instancję parametru, w którym możemy dononać wyboru
    /// </summary>
    /// <param name="_name">Nazwa parametru</param>
    /// <param name="_label">Wyraz lub ciąg wyrazowy, służący do wyswietlenia danej opcji na ekaknie aplikacji</param>
    /// <param name="_required">Pole przechowujące informacje, czy dany parametr jest wymagany do wprowadzenia</param>
    /// <param name="_options">Lista opcji dla danego parametru</param>
    /// <see cref="SelectableTaskParameterOption"/>
    public class SelectableTaskParameterBuilder
    {
        private string _name;
        private string _label;
        private bool _required;
        private SelectableTaskParameterOption[] _options;

        public SelectableTaskParameterBuilder()
        {
            this._name = "";
            this._label = "";
            this._required = true;
            this._options = new SelectableTaskParameterOption[]{ };
        }


        /// <summary>
        /// Metoda ustawiająca nazwę parametru
        /// </summary>
        /// <param name="name">Nazwa parametru</param>
        /// <returns></returns>
        public SelectableTaskParameterBuilder name(string name)
        {
            this._name = name;
            return this;
        }



        /// <summary>
        /// Metoda ustawiająca wyraz lub ciąg wyrazowy, służący do wyswietlenia danej opcji na ekaknie aplikacji
        /// </summary>
        /// <param name="label">
        ///     Wyraz lub ciąg wyrazowy, służący do wyswietlenia danej opcji na ekaknie aplikacji
        /// </param>
        /// <returns></returns>
        public SelectableTaskParameterBuilder label(string label)
        {
            this._label = label;
            return this;
        }



        /// <summary>
        /// Metoda ustalająca, czy dany parametr jest wymagany do wprowadzenia
        /// </summary>
        /// <param name="required">Pole przechowujące informacje, czy dany parametr jest wymagany do wprowadzenia</param>
        /// <returns></returns>
        public SelectableTaskParameterBuilder required(bool required)
        {
            this._required = required;
            return this;
        }



        /// <summary>
        /// Metoda dodająca nowe opcje do parametru
        /// </summary>
        /// <param name="options">Lista opcji do dodania</param>
        /// <returns></returns>
        /// <see cref="SelectableTaskParameterOption"/>
        public SelectableTaskParameterBuilder options(params SelectableTaskParameterOption[] options)
        {
            this._options = options;
            return this;
        }



        /// <summary>
        /// Metoda tworzy i zwraca nowyy obiekt
        /// </summary>
        /// <returns>Zwraca utworzony obiekt parametru</returns>
        /// <see cref="SelectableTaskParameter"/>
        public SelectableTaskParameter build()
        {
            SelectableTaskParameter res = new SelectableTaskParameter(_name, _label, _required);
            res.addOptions(_options);

            return res;
        }
    }
}
