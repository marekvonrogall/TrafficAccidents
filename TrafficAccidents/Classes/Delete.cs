using Cassandra;
using Cassandra.Mapping;
using System;
using System.Windows;

namespace TrafficAccidents.Classes
{
    public class Delete
    {
        public void DeleteEntryById(ISession session, int id)
        {
            var mapper = new Mapper(session, MappingConfiguration.Global);
            try
            {
                mapper.Delete<Accident>("WHERE id_unfall = ?", id);
                MessageBox.Show("Eintrag erfolgreich gelöscht!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Meldung: {ex.Message}", "Fehler beim Löschen");
            }
        }
    }
}
