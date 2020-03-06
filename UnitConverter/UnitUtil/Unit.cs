﻿namespace UnitConverter.UnitUtil
{

    /// <summary>
    /// Klasa reprezentująca typ, która przechowje dane odnoście jednostki miary
    /// <param name="name">Nazwa jednostki</param>
    /// <param name="frombaseUnitFormula">
    ///     Jest to referencja do metody, która przechowuje wzór na skonwertowanie 
    ///     jednostki bazowej na jednostkę, w której obecnie się znajdujemy.
    ///     Np. Jeżeli chcemy skonwertować kilometry na mile, to 'bazową' jednostką dla obu miar będzie metr.
    ///         Wobec czego w tym przykładzie ta formułka posłuży do skonwertowania metrów na kilometry
    /// </param>
    /// <param name="toBaseUnitFormula">
    ///     Jest to referencja do metody, która przechowuje wzór na skonwertowanie 
    ///     jednostki, w której obecnie się znajdujemy na jednostkę bazową.
    ///     Np. Jeżeli chcemy skonwertować kilometry na mile, to 'bazową' jednostką dla obu miar będzie metr.
    ///         Wobec czego w tym przykładzie ta formułka posłuży do skonwertowania kilometrów na metry
    /// </param>
    /// </summary>
    /// <see cref="UnitFormula"/>
    public class Unit
    {
        public string name { get; set; }
        public UnitFormula fromBaseUnitFormula { get; set; }
        public UnitFormula toBaseUnitFormula { get; set; }


        public Unit(string name, UnitFormula fromBaseUnitFormula, UnitFormula toBaseUnitFormula)
        {
            this.name = name;
            this.fromBaseUnitFormula = fromBaseUnitFormula;
            this.toBaseUnitFormula = toBaseUnitFormula;
        }
    }
}