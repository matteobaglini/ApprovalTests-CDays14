using System;

namespace LegacyWPF
{
    public class Shipping
    {
        public string Description { get; set; }
        public int Seal { get; set; }
        public int Weight { get; set; }
        public string Port { get; set; }
        public string Ship { get; set; }
        public string Date { get; set; }
        public string Forwarder { get; set; }

        public Shipping(String description, Int32 seal, Int32 weight, String port, String ship, DateTime date, String forwarder)
        {
            Description = description;
            Seal = seal;
            Weight = weight;
            Port = port;
            Ship = ship;
            Date = date.ToString("g");
            Forwarder = forwarder;
        }
    }
}
