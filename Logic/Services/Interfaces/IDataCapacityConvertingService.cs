﻿namespace Logic.Services.Interfaces
{
    public interface IDataCapacityConvertingService
    {
        float Bits { get; set; }
        float KiloBits { get; set; }
        float MegaBits { get; set; }
        float Bytes { get; set; }
        float KiloBytes { get; set; }
        float MegaBytes { get; set; }
    }
}
