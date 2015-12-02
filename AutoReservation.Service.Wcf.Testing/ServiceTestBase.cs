using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.Interfaces;
using AutoReservation.TestEnvironment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace AutoReservation.Service.Wcf.Testing
{
    [TestClass]
    public abstract class ServiceTestBase
    {
        protected abstract IAutoReservationService Target { get; }

        [TestInitialize]
        public void InitializeTestData()
        {
            TestEnvironmentHelper.InitializeTestData();
        }

        [TestMethod]
        public void Test_GetAutos()
        {
            IList<AutoDto> cars = Target.GetAutos();
            Assert.AreEqual(3, cars.Count);
        }

        [TestMethod]
        public void Test_GetKunden()
        {
            IList<KundeDto> customers = Target.GetKunden();
            Assert.AreEqual(4, customers.Count);
        }

        [TestMethod]
        public void Test_GetReservationen()
        {
            IList<ReservationDto> reservations = Target.GetReservations();
            Assert.AreEqual(3, reservations.Count);
        }

        [TestMethod]
        public void Test_GetAutoById()
        {
            Assert.IsTrue(Target.GetAutos().Count > 0);

            AutoDto car = Target.GetAutos()[0];
            Assert.IsNotNull(car);
            AutoDto carByID = Target.GetAuto(car.Id);
            Assert.IsNotNull(carByID);


            Assert.AreEqual(car.Id, carByID.Id);
            Assert.AreEqual(car.Marke, carByID.Marke);
            Assert.AreEqual(car.Tagestarif, carByID.Tagestarif);
            Assert.AreEqual(car.AutoKlasse, carByID.AutoKlasse);
        }

        [TestMethod]
        public void Test_GetKundeById()
        {
            Assert.IsTrue(Target.GetKunden().Count > 0);

            KundeDto cust = Target.GetKunden()[0];
            Assert.IsNotNull(cust);
            KundeDto custByID = Target.GetKunde(cust.Id);
            Assert.IsNotNull(custByID);

            Assert.AreEqual(cust.Id, custByID.Id);
            Assert.AreEqual(cust.Vorname, custByID.Vorname);
            Assert.AreEqual(cust.Nachname, custByID.Nachname);
            Assert.AreEqual(cust.Geburtsdatum, custByID.Geburtsdatum);
        }

        [TestMethod]
        public void Test_GetReservationByNr()
        {
            Assert.IsTrue(Target.GetReservations().Count > 0);

            ReservationDto res = Target.GetReservations()[0];
            Assert.IsNotNull(res);
            ReservationDto resByID = Target.GetReservation(res.ReservationNr);
            Assert.IsNotNull(resByID);

            Assert.AreEqual(res.ReservationNr, resByID.ReservationNr);
            Assert.AreEqual(res.Kunde, resByID.Kunde);
            Assert.AreEqual(res.Von, resByID.Von);
            Assert.AreEqual(res.Bis, resByID.Bis);
            Assert.AreEqual(res.Auto, resByID.Auto);
        }

        [TestMethod]
        public void Test_GetReservationByIllegalNr()
        {
            ReservationDto reservation = Target.GetReservation(-42);
            Assert.IsNull(reservation);
        }

        [TestMethod]
        public void Test_InsertAuto()
        {
            AutoDto car = new AutoDto
            {
                Marke = "Fiat Panda",
                AutoKlasse = AutoKlasse.Standard,
                Tagestarif = 10
            };

            int id = Target.AddAuto(car);
            AutoDto toTest = Target.GetAuto(id);

            Assert.IsNotNull(toTest);
            Assert.AreEqual(id, toTest.Id);
            Assert.AreNotEqual(0, toTest.Id);
            Assert.AreEqual(car.Marke, toTest.Marke);
            Assert.AreEqual(car.AutoKlasse, toTest.AutoKlasse);
            Assert.AreEqual(car.Tagestarif, toTest.Tagestarif);
        }

        [TestMethod]
        public void Test_InsertKunde()
        {
            KundeDto customer = new KundeDto
            {
                Nachname = "Müller",
                Vorname = "Markus",
                Geburtsdatum = new DateTime(1980, 1, 1)
            };

            int id = Target.AddKunde(customer);

            KundeDto toTest = Target.GetKunde(id);

            Assert.IsNotNull(toTest);
            Assert.AreEqual(id, toTest.Id);
            Assert.AreNotEqual(0, toTest.Id);
            Assert.AreEqual(customer.Nachname, toTest.Nachname);
            Assert.AreEqual(customer.Vorname, toTest.Vorname);
            Assert.AreEqual(customer.Geburtsdatum, toTest.Geburtsdatum);
        }

        [TestMethod]
        public void Test_InsertReservation()
        {
            ReservationDto res = new ReservationDto{
                Auto = Target.GetAutos()[0],
                Kunde = Target.GetKunden()[0],
                Von = DateTime.Today,
                Bis = DateTime.Today.AddDays(2)
            };
            int id = Target.AddReservation(res);

            Assert.AreNotEqual(0, id);

            ReservationDto toTest = Target.GetReservation(id);

            Assert.IsNotNull(toTest);
            Assert.AreNotEqual(0, toTest.ReservationNr);
            Assert.AreEqual(id, toTest.ReservationNr);
            Assert.AreEqual(res.Auto.Id, toTest.Auto.Id);
            Assert.AreEqual(res.Kunde.Id, toTest.Kunde.Id);
            Assert.AreEqual(res.Von, toTest.Von);
            Assert.AreEqual(res.Bis, toTest.Bis);

            
        }

        [TestMethod]
        public void Test_UpdateAuto()
        {
            AutoDto newCar = new AutoDto {
                Marke = "Smart",
                Tagestarif = 22,
                AutoKlasse = AutoKlasse.Standard
            };
            int id = Target.AddAuto( newCar );

            AutoDto original = Target.GetAuto( id );
            AutoDto updated = Target.GetAuto( id );

            Assert.IsNotNull(original);
            Assert.IsNotNull(updated);

            updated.AutoKlasse = AutoKlasse.Luxusklasse;
            updated.Basistarif = 999;
            updated.Marke = "Bugatti Veyron oder so";

            Target.UpdateAuto(updated, original);

            AutoDto toTest = Target.GetAuto(id);

            Assert.IsNotNull(toTest);

            Assert.AreEqual(updated.Id, toTest.Id);
            Assert.AreEqual(updated.Marke, toTest.Marke);
            Assert.AreEqual(updated.Basistarif, toTest.Basistarif);
            Assert.AreEqual(updated.AutoKlasse, toTest.AutoKlasse);

            Assert.AreEqual(original.Id, toTest.Id);
            Assert.AreNotEqual(original.Marke, toTest.Marke);
            Assert.AreNotEqual(original.Basistarif, toTest.Basistarif);
            Assert.AreNotEqual(original.AutoKlasse, toTest.AutoKlasse);
        }

        [TestMethod]
        public void Test_UpdateKunde()
        {
            KundeDto newKunde = new KundeDto
            {
                Geburtsdatum = new DateTime(1990, 1, 1),
                Nachname = "Takahashi",
                Vorname = "Akira"

            };
            int id = Target.AddKunde(newKunde);

            KundeDto original = Target.GetKunde(id);
            KundeDto updated = Target.GetKunde(id);

            updated.Geburtsdatum = new DateTime(2000,2,2);
            updated.Nachname = "Rösli";
            updated.Vorname = "Heidi";

            Target.UpdateKunde(updated, original);

            KundeDto toTest = Target.GetKunde(id);

            Assert.IsNotNull(toTest);

            Assert.AreEqual(updated.Id, toTest.Id);
            Assert.AreEqual(updated.Geburtsdatum, toTest.Geburtsdatum);
            Assert.AreEqual(updated.Nachname, toTest.Nachname);
            Assert.AreEqual(updated.Vorname, toTest.Vorname);

            Assert.AreEqual(original.Id, toTest.Id);
            Assert.AreNotEqual(original.Geburtsdatum, toTest.Geburtsdatum);
            Assert.AreNotEqual(original.Nachname, toTest.Nachname);
            Assert.AreNotEqual(original.Vorname, toTest.Vorname);
        }

        [TestMethod]
        public void Test_UpdateReservation()
        {
            Assert.IsTrue(Target.GetReservations().Count > 0);

            ReservationDto resOrig = Target.GetReservations()[0];
            Assert.IsNotNull(resOrig);

            ReservationDto resNew = resOrig.Clone();
            resNew.Bis.AddHours(2);

            Target.UpdateReservation(resNew, resOrig);
            ReservationDto check = Target.GetReservation(resOrig.ReservationNr);

            Assert.AreEqual(resNew.ReservationNr, check.ReservationNr);
            Assert.AreEqual(resNew.Kunde.Id, check.Kunde.Id);
            Assert.AreEqual(resNew.Von, check.Von);
            Assert.AreEqual(resNew.Bis, check.Bis);
            Assert.AreEqual(resNew.Auto.Id, check.Auto.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<AutoDto>))]
        public void Test_UpdateAutoWithOptimisticConcurrency()
        {
            AutoDto original = Target.GetAutos()[0];
            AutoDto clone1 = original.Clone();
            AutoDto clone2 = original.Clone();

            clone1.Tagestarif = 999;
            clone2.Tagestarif = 111;

            Target.UpdateAuto(clone1, original);

            try
            {
                Target.UpdateAuto(clone2, original);
            }
            catch (FaultException<AutoDto> fe)
            {
                Assert.AreEqual(fe.Detail.Tagestarif, clone1.Tagestarif);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<KundeDto>))]
        public void Test_UpdateKundeWithOptimisticConcurrency()
        {
            KundeDto original = Target.GetKunden()[0];
            KundeDto clone1 = original.Clone();
            KundeDto clone2 = original.Clone();

            clone1.Nachname = "Robert";
            clone2.Nachname = "Hürlimann";

            Target.UpdateKunde(clone1, original);

            try
            {
                Target.UpdateKunde(clone2, original);
            }
            catch (FaultException<KundeDto> fe)
            {
                Assert.AreEqual(fe.Detail.Nachname, clone1.Nachname);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ReservationDto>))]
        public void Test_UpdateReservationWithOptimisticConcurrency()
        {
            ReservationDto original = Target.GetReservations()[0];
            ReservationDto clone1 = original.Clone();
            ReservationDto clone2 = original.Clone();

            clone1.Bis = new DateTime(2222, 2, 2);
            clone2.Bis = new DateTime(3333, 3, 3);

            Target.UpdateReservation(clone1, original);

            try
            {
                Target.UpdateReservation(clone2, original);
            }
            catch (FaultException<ReservationDto> fe)
            {
                Assert.AreEqual(fe.Detail.Bis, clone1.Bis);
                throw;
            }
        }

        [TestMethod]
        public void Test_DeleteKunde()
        {
            Assert.AreEqual(4, Target.GetKunden().Count);

            KundeDto toDelete = Target.GetKunden()[0];
            Target.DeleteKunde(toDelete);

            Assert.IsNull(Target.GetKunde(toDelete.Id));
            Assert.AreEqual(3, Target.GetKunden().Count);
        }

        [TestMethod]
        public void Test_DeleteAuto()
        {
            Assert.AreEqual(3, Target.GetAutos().Count);

            AutoDto toDelete = Target.GetAutos()[0];
            Target.DeleteAuto(toDelete);

            Assert.IsNull(Target.GetAuto(toDelete.Id));
            Assert.AreEqual(2, Target.GetAutos().Count);
        }

        [TestMethod]
        public void Test_DeleteReservation()
        {
            Assert.AreEqual(3, Target.GetReservations().Count);

            ReservationDto toDelete = Target.GetReservations()[0];
            Target.DeleteReservation(toDelete);

            Assert.IsNull(Target.GetReservation(toDelete.ReservationNr));
            Assert.AreEqual(2, Target.GetReservations().Count);
        }
    }
}
