﻿using Cassandra;
using Cassandra.Mapping;
using System;
using System.Windows;

namespace TrafficAccidents.Classes
{
    public class Add
    {
        public void AddEntry(ISession session, Accident accident)
        {
            var mapper = new Mapper(session, MappingConfiguration.Global);

            try
            {
                mapper.Insert(accident);
                MessageBox.Show("Eintrag erfolgreich hinzugefügt!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Meldung: {ex.Message}", "Fehler beim Hinzufügen");
            }
        }
    }
}
