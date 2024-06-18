using Cassandra;

namespace TrafficAccidents.Classes
{
    public class CassandraDb
    {
        public ISession Session { get; set; }
        public CassandraDb()
        {
            var cluster = Cluster.Builder()
                                 .AddContactPoints("127.0.0.1")
                                 .Build();
            Session = cluster.Connect("traffic");
        }
    }
}
