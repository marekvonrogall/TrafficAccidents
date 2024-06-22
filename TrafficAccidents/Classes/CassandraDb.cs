using Cassandra;
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
            }
            catch
            {
                MessageBox.Show("Stelle sicher, dass die Datenbank läuft (http://localhost:9042/) und der Keyspace \"traffic\" existiert.", "Fehler beim Verbinden zur Datenbank");
            }
        }
    }
}
