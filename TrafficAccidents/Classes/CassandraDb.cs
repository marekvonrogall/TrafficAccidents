using Cassandra;
using Cassandra.Mapping;
using System.Windows;

namespace TrafficAccidents.Classes
{
    public class CassandraDb
    {
        public ISession Session { get; set; }
        public CassandraDb()
        {
            try
            {
                var cluster = Cluster.Builder()
                                 .AddContactPoints("127.0.0.1")
                                 .Build();
                Session = cluster.Connect("traffic");

                MappingConfiguration.Global.Define(
                    new Map<Accident>()
                        .TableName("accidents")
                        .PartitionKey(u => u.IdUnfall)
                        .Column(u => u.GeoPoint, cm => cm.WithName("geo_point_2d"))
                        .Column(u => u.GeoShape, cm => cm.WithName("geo_shape"))
                        .Column(u => u.IdUnfall, cm => cm.WithName("id_unfall"))
                        .Column(u => u.Typ, cm => cm.WithName("typ"))
                        .Column(u => u.Schwere, cm => cm.WithName("schwere"))
                        .Column(u => u.Jahr, cm => cm.WithName("jahr"))
                        .Column(u => u.Monat, cm => cm.WithName("monat"))
                        .Column(u => u.Wochentag, cm => cm.WithName("wochentag"))
                        .Column(u => u.Stunde, cm => cm.WithName("stunde"))
                        .Column(u => u.Strasseart, cm => cm.WithName("strasseart"))
                        .Column(u => u.FussggBet, cm => cm.WithName("fussgg_bet"))
                        .Column(u => u.FahrrdBet, cm => cm.WithName("fahrrd_bet"))
                        .Column(u => u.MotordBet, cm => cm.WithName("motord_bet"))
                );
            }
            catch
            {
                MessageBox.Show("Stelle sicher, dass die Datenbank läuft (http://localhost:9042/) und der Keyspace \"traffic\" existiert.", "Fehler beim Verbinden zur Datenbank");
            }
        }
    }
}
