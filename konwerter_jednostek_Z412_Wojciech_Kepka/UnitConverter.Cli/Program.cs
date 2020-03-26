﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnitConverter.Lib;
using static UnitConverter.Lib.Units;

namespace UnitConverter.Cli
{

    class Converter
    {
        double inpVal; Unit inpUnit;
        List<Tuple<double, Unit>> outVals = new List<Tuple<double, Unit>>();

        string inpStr; TimeFormat inpFormat;
        string outStr;
        bool timeConversion = false;

        bool calculated = false;
        bool cmd = false;
        Dictionary<DateTime, Record> history = new Dictionary<DateTime, Record> { };

        List<IConverter<double, Unit>> converters = new List<IConverter<double, Unit>>()
        {
            new DistanceConverter(),
            new MassConverter(),
            new SpeedConverter(),
            new TemperatureConverter(),
        };
        TimeConverter tConv = new TimeConverter();

        Converter()
        {
            Console.WriteLine("Konwerter jednostek");
            Console.WriteLine("\nKonwertuje wybraną jednostkę na jej odpowiednik");
            PrintHelp();
        }

        static void PrintHelp()
        {
            Console.WriteLine("#########################################");
            Console.WriteLine("Dostępne Jednostki:");
            Console.WriteLine("\tTemperatura:");
            Console.WriteLine("\t\tC\t(Stopnie Celsjusza)");
            Console.WriteLine("\t\tF\t(Stopnie Fahrenheita)");
            Console.WriteLine("\t\tK\t(Stopnie Kelvina)");
            Console.WriteLine("\tMasa:");
            Console.WriteLine("\t\tkg\t(Kilogramy)");
            Console.WriteLine("\t\tlb\t(Funty)");
            Console.WriteLine("\t\toz\t(Uncje)");
            Console.WriteLine("\tDystans:");
            Console.WriteLine("\t\tkm\t(Kilometry)");
            Console.WriteLine("\t\tmi\t(Mile)");
            Console.WriteLine("\tPredkosc:");
            Console.WriteLine("\t\tkm/h\t(Kilometry na godzine)");
            Console.WriteLine("\t\tmi/h\t(Mile na godzine)");
            Console.WriteLine("\t\tm/s\t(Metry na sekunde)");
            Console.WriteLine("\t\tknots\t(Wezly)");
            Console.WriteLine("\tCzas:");
            Console.WriteLine("\t\th       - 24 godzinny format");
            Console.WriteLine("\t\tam | pm - 12 godzinny format");
            Console.WriteLine("Przykładowy input:");
            Console.WriteLine("\t10 kg");
            Console.WriteLine("\t-3.14 F");
            Console.WriteLine("\t23:50 h");
            Console.WriteLine("\t7:42 am");
            Console.WriteLine("Dostępne komendy:");
            Console.WriteLine("\thelp\tDrukuj pomoc");
            Console.WriteLine("\tclear\tWyczyść okno");
            Console.WriteLine("\thistory\tHistoria wyników");
            Console.WriteLine("#########################################");
        }
        bool SetInpVal(string userInp)
        {
            try
            {
                this.inpVal = Double.Parse(userInp);
                return true;
            }
            catch (System.FormatException)
            {
                Console.WriteLine($"'{userInp}' nie jest poprawną liczbą. Spróbuj ponownie");
                return false;
            }
        }
        bool SetInpUnit(string userInp)
        {
            try
            {
                inpUnit = UnitFromString(userInp);
                return true;
            }
            catch (UnexpectedEnumValueException<Unit> e)
            {
                Console.WriteLine($"Podana jednostka '{userInp}' jest nieobsługiwana - {e}. Spróbuj ponownie.");
                return false;
            }
        }
        bool SetInpFormat(string userInp)
        {
            try
            {
                inpFormat = TimeFormatFromString(userInp);
                return true;
            }
            catch (InvalidTimeFormat e)
            {
                Console.WriteLine($"Podany format czasu '{userInp}' jest nieobsługiwany - {e}. Spróbuj ponownie.");
                return false;
            }
        }
        bool Parse(string userInp)
        // Parses input like '10 kg', '15.5 C'...
        {
            switch (userInp)
            {
                case "help":
                    PrintHelp();
                    cmd = true;
                    return true;
                case "clear":
                    Console.Clear();
                    cmd = true;
                    return true;
                case "history":
                    PrintHistory();
                    cmd = true;
                    return true;
                default:
                    String[] inp = userInp.Split(' ');
                    cmd = false;
                    try
                    {
                        if (inp.Length == 2)
                        {
                            if (inp[1] == "am" | inp[1] == "pm" | inp[1] == "h")
                            {
                                if (SetInpFormat(inp[1]))
                                {
                                    if (inp[1] == "h")
                                    { 
                                        inpStr = inp[0];
                                    } else
                                    {
                                        inpStr = userInp;
                                    }

                                    timeConversion = true;
                                    return true;
                                }
                            }
                            if (SetInpVal(inp[0]) && SetInpUnit(inp[1])) { return true; }
                            else { return false; }
                        }

                    }
                    catch (System.IndexOutOfRangeException) { }
                    Console.WriteLine($"Podana komenda '{userInp}' nie istnieje.");
                    return false;
            }
        }
        void GetInp()
        {
            while (true)
            {
                Console.Write("=> ");
                string user_inp = Console.ReadLine();
                if (Parse(user_inp)) { break; }
            }
        }
        void Convert()
        {
            if (!cmd)
            {
                if (timeConversion)
                {
                    try
                    {
                        outStr = tConv.Convert(inpStr, inpFormat, OppositeFormat(inpFormat)).Item1;
                    }
                    catch (InvalidTimeFormat e)
                    {
                        Console.WriteLine(e);
                    }
                } else
                {
                    foreach (IConverter<double, Unit> conv in converters)
                    {
                        if (conv.SupportedUnits.Contains(inpUnit))
                        {
                            foreach (Unit u in conv.SupportedUnits)
                            {
                                if (u != inpUnit)
                                {
                                    outVals.Add(conv.Convert(inpVal, inpUnit, u));
                                }
                            }
                        }
                    }
                    history.Add(DateTime.Now, new Record(inpVal, inpUnit, new List<Tuple<double, Unit>>(outVals)));
                }
                calculated = true;
            }

        }
        string OutStr()
        {
            if (timeConversion)
            {
                return outStr;
            } else
            {
                StringBuilder s = new StringBuilder();
                foreach (Tuple<double, Unit> out_ in outVals)
                {
                    s.Append($"{inpVal} {UnitName(inpUnit)} = {out_.Item1} {UnitName(out_.Item2)}\n");
                }
                return s.ToString().Trim('\n');
            }
        }
        void PrintOut()
        {
            if (!cmd)
            {
                if (!calculated) { Convert(); }
                Console.WriteLine(OutStr());
            }
        }
        void PrintHistory()
        {
            foreach (KeyValuePair<DateTime, Record> entry in history)
            {
                Console.WriteLine($"{entry.Key}");
                Record r = entry.Value;
                Console.Write($"\t{r.inpVal} {UnitName(r.inpUnit)} =");
                foreach (Tuple<double, Unit> out_ in r)
                {
                    Console.WriteLine($"\t\t{out_.Item1} {UnitName(out_.Item2)}");
                }
            }
        }
        void Clear()
        {
            inpVal = default; inpUnit = default;
            outVals.Clear();
            calculated = false;
        }
        //################################################
        public class Record : IEnumerable<Tuple<double, Unit>>
        {
            public double inpVal; public Unit inpUnit;
            public List<Tuple<double, Unit>> outVal;
            public Record(double v, Unit u, List<Tuple<double, Unit>> out_v)
            {
                inpVal = v;
                inpUnit = u;
                this.outVal = out_v;
            }

            public IEnumerator<Tuple<double, Unit>> GetEnumerator()
            {
                return outVal.GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return outVal.GetEnumerator();
            }
        }
        //################################################

        static void Main(string[] args)
        {
            Converter conv = new Converter();
            while (true)
            {

                conv.GetInp();
                conv.Convert();
                conv.PrintOut();
                conv.Clear();
            }

        }
    }
}
