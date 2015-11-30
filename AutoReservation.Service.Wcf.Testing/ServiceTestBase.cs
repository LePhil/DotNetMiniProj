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
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        public void Test_UpdateAuto()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        public void Test_UpdateKunde()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        public void Test_UpdateReservation()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<AutoDto>))]
        public void Test_UpdateAutoWithOptimisticConcurrency()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<KundeDto>))]
        public void Test_UpdateKundeWithOptimisticConcurrency()
        {
            Assert.Inconclusive("Test not implemented.");
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ReservationDto>))]
        public void Test_UpdateReservationWithOptimisticConcurrency()
        {
            Assert.Inconclusive("Test not implemented.");
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
