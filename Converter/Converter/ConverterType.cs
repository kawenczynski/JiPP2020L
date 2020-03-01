﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Converter
{
    enum ConverterType
    {
        [Description("degrees Celsius")]
        CELSIUS,
        [Description("in farenheit scale")]
        FARENHEIT,
        [Description("kilometers")]
        KILOMETER,
        [Description("miles")]
        MILE,
        [Description("kilograms")]
        KILOGRAMS,
        [Description("funts")]
        FUNT
    }
}
