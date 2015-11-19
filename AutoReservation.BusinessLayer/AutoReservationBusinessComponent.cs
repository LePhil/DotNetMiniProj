using AutoReservation.Dal;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace AutoReservation.BusinessLayer
{
    public class AutoReservationBusinessComponent
    {
        private AutoReservationEntities context;

        public AutoReservationBusinessComponent() {
            context = new AutoReservationEntities();
        }

        private static void HandleDbConcurrencyException<T>(AutoReservationEntities context, T original) where T : class
        {
            var databaseValue = context.Entry(original).GetDatabaseValues();
            context.Entry(original).CurrentValues.SetValues(databaseValue);

            throw new LocalOptimisticConcurrencyException<T>(string.Format("Update {0}: Concurrency-Fehler", typeof(T).Name), original);
        }

        public IList<Auto> GetAutos() {
            var autos = from a in context.Autos select a;
            return autos.ToList();
        }

        public Auto GetAuto(int id) {
            return context.Autos.Find(id);
        }

        public void AddAuto(Auto auto) {
            context.Autos.Add(auto);
            context.SaveChanges();
        }

        public void DeleteAuto(Auto auto)
        {
            context.Autos.Attach(auto);
            context.Autos.Remove(auto);
            context.SaveChanges();
        }

        public void UpdateAuto(Auto modified, Auto original) {
            try
            {
                context.Autos.Attach(original);
                context.Entry(original).CurrentValues.SetValues(modified);
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException) {
                HandleDbConcurrencyException(context, original);
            }
            
        }

        public IList<Reservation> GetReservations()
        {
            var reservationen = from a in context.Reservationen select a;
            return reservationen.ToList();
        }

        public Reservation GetReservation(int id)
        {
            return context.Reservationen.Find(id);
        }

        public void AddReservation(Reservation reservation)
        {
            context.Reservationen.Add(reservation);
            context.SaveChanges();
        }

        public void DeleteReservation(Reservation reservation)
        {
            context.Reservationen.Attach(reservation);
            context.Reservationen.Remove(reservation);
            context.SaveChanges();
        }

        public void UpdateReservation(Reservation modified, Reservation original)
        {
            try { 
                context.Reservationen.Attach(original);
                context.Entry(original).CurrentValues.SetValues(modified);
                context.SaveChanges();
            } catch (DbUpdateConcurrencyException) {
                HandleDbConcurrencyException(context, original);
            }
        }

        public IList<Kunde> GetKunden()
        {
            var reservationen = from a in context.Kunden select a;
            return reservationen.ToList();
        }

        public Kunde GetKunde(int id)
        {
            return context.Kunden.Find(id);
        }

        public void AddKunde(Kunde kunde)
        {
            context.Kunden.Add(kunde);
            context.SaveChanges();
        }

        public void DeleteKunde(Kunde kunde)
        {
            context.Kunden.Attach(kunde);
            context.Kunden.Remove(kunde);
            context.SaveChanges();
        }

        public void UpdateKunde(Kunde modified, Kunde original)
        {
            try
            {
                context.Kunden.Attach(original);
                context.Entry(original).CurrentValues.SetValues(modified);
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                HandleDbConcurrencyException(context, original);
            }
        }
    }
}