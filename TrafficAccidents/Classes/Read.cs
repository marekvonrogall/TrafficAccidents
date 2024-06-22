using Cassandra;
using Cassandra.Mapping;
using System.Linq;

namespace TrafficAccidents.Classes
{
    public class Read
    {
        public Accident Accident { get; set; }
        public void GetEntryById(ISession session, int id)
        {
            var mapper = new Mapper(session, MappingConfiguration.Global);
            var accident = mapper.Single<Accident>($"SELECT * FROM traffic.accidents WHERE id_unfall = {id}");
            if (accident != null)
            {
                Accident = accident;
            }
        }

        public int GetNextAvailableId(ISession session)
        {
            var query = "SELECT max(id_unfall) AS max_id FROM traffic.accidents";
            var rs = session.Execute(query);

            var row = rs.FirstOrDefault();

            int maxId = row.GetValue<int>("max_id");
            return maxId +1;
        }
    }
}
