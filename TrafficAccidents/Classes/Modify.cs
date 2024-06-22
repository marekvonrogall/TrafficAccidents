using Cassandra.Mapping;
using Cassandra;
using System;
using System.Windows;

namespace TrafficAccidents.Classes
{
    public class Modify
    {
        public void UpdateEntry(ISession session, Accident accident)
        {
            var mapper = new Mapper(session, MappingConfiguration.Global);

            try
            {
                mapper.Update(accident);

                MessageBox.Show("Eintrag erfolgreich aktualisiert!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Meldung: {ex.Message}", "Fehler beim Aktualisieren");
            }
        }
    }
}
