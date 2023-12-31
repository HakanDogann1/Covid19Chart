﻿using System;

namespace Covid19Chart.Api.Models
{
    public enum Ecity
    {
        Istanbul = 1,
        Ankara = 2,
        Izmir = 3,
        Konya = 4,
        Antalya = 5
    }
    public class Covid
    {
        public int Id { get; set; }
        public Ecity Ecity { get; set; }
        public int Count { get; set; }
        public DateTime CovidDate { get; set; }
    }
}
