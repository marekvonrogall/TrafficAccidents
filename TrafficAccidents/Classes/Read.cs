using Cassandra;

namespace TrafficAccidents.Classes
{
    public class Read
    {
        public void AllEntries(ISession session)
        {
            var rs = session.Execute("SELECT * FROM traffic.accidents");
            foreach (var row in rs) { /*Display*/ }
        }

        public void SpecificEntry(ISession session, int EntryId)
        {
           
        }
    }
}
