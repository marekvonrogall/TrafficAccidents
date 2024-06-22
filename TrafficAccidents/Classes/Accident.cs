namespace TrafficAccidents.Classes
{
    public class Accident
    {
        public string GeoPoint { get; set; }
        public string GeoShape { get; set; }
        public int IdUnfall { get; set; }
        public string Typ { get; set; }
        public string Schwere { get; set; }
        public int Jahr { get; set; }
        public int Monat { get; set; }
        public string Wochentag { get; set; }
        public int? Stunde { get; set; }
        public string Strasseart { get; set; }
        public bool FussggBet { get; set; }
        public bool FahrrdBet { get; set; }
        public bool MotordBet { get; set; }
    }
}
