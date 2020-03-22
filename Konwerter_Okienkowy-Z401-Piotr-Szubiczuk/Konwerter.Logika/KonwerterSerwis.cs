﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konwerter
{
    public class KonwerterSerwis
    {
        public List<Ikonwenter> NazwyK()
        {
            return new List<Ikonwenter>()
            {
                new CnF(),
                new FnC(),
                new FnKG(),
                new KGnF(),
                new KMnM(),
                new MnKM()
            };
        }
    }
}