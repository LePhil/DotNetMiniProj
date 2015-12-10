using AutoReservation.Dal;
using AutoReservation.TestEnvironment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoReservation.BusinessLayer.Testing
{
    [TestClass]
    public class BusinessLayerTest
    {
        private AutoReservationBusinessComponent target;
        private AutoReservationBusinessComponent Target
        {
            get
            {
                if (target == null)
                {
                    target = new AutoReservationBusinessComponent();
                }
                return target;
            }
        }


        [TestInitialize]
        public void InitializeTestData()
        {
            TestEnvironmentHelper.InitializeTestData();
        }
        
        [TestMethod]
        public void Test_UpdateAuto()
        {
            string neueMarke = "Peugeot 207";
            Auto auto = Target.GetAuto(1);
            auto.Marke = neueMarke;

            Target.UpdateAuto(auto, Target.GetAuto(1));
            Assert.AreEqual(neueMarke, Target.GetAuto(1).Marke);
        }

        [TestMethod]
        public void Test_UpdateKunde()
        {
            string neuerName = "Tester";
            Kunde kunde = Target.GetKunde(1);
            kunde.Nachname = neuerName;

            Target.UpdateKunde(kunde, Target.GetKunde(1));
            Assert.AreEqual(neuerName, Target.GetKunde(1).Nachname);
        }

        [TestMethod]
        public void Test_UpdateReservation()
        {
            Kunde neuerKunde = Target.GetKunde(2);
            Reservation reservation = Target.GetReservation(1);
            reservation.Kunde = neuerKunde;

            Target.UpdateReservation(reservation, Target.GetReservation(1));

            Reservation updated = Target.GetReservation(1);

            Assert.AreEqual(updated.ReservationsNr, reservation.ReservationsNr);
            Assert.AreEqual(updated.Von, reservation.Von);
            Assert.AreEqual(updated.Bis, reservation.Bis);
            Assert.AreEqual(updated.KundeId, reservation.KundeId);
            Assert.AreEqual(updated.AutoId, reservation.AutoId);
        }
    }
}
