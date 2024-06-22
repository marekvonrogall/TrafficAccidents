using Cassandra;
using System.Collections.Generic;
using System.Linq;

namespace TrafficAccidents.Classes
{
    public class Read
    {
        public List<Accident> Accidents { get; set; } = new List<Accident>();
        public void GetEntryById(ISession session, int id)
        {
            Accidents.Clear();
            var query = $"SELECT * FROM traffic.accidents WHERE id_unfall = {id}";
            var rs = session.Execute(query);
            foreach (var row in rs)
            {
                Accidents.Add(new Accident
                {
                    GeoPoint = row.GetValue<string>("geo_point_2d"),
                    GeoShape = row.GetValue<string>("geo_shape"),
                    IdUnfall = row.GetValue<int>("id_unfall"),
                    Typ = row.GetValue<string>("typ"),
                    Schwere = row.GetValue<string>("schwere"),
                    Jahr = row.GetValue<int>("jahr"),
                    Monat = row.GetValue<int>("monat"),
                    Wochentag = row.GetValue<string>("wochentag"),
                    Stunde = row.GetValue<int?>("stunde"),
                    Strasseart = row.GetValue<string>("strasseart"),
                    FussggBet = row.GetValue<bool>("fussgg_bet"),
                    FahrrdBet = row.GetValue<bool>("fahrrd_bet"),
                    MotordBet = row.GetValue<bool>("motord_bet")
                });
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
