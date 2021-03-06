﻿using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Common.Interfaces;
using AutoReservation.TestEnvironment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;

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
            IEnumerable<AutoDto> cars = Target.GetAutos();
            Assert.AreEqual(3, Enumerable.Count(cars) );
        }

        [TestMethod]
        public void Test_GetKunden()
        {
            IEnumerable<KundeDto> customers = Target.GetKunden();
            Assert.AreEqual(4, Enumerable.Count(customers) );
        }

        [TestMethod]
        public void Test_GetReservationen()
        {
            IEnumerable<ReservationDto> reservations = Target.GetReservations();
            Assert.AreEqual(3, Enumerable.Count(reservations) );
        }

        [TestMethod]
        public void Test_GetAutoById()
        {
            var cars = Target.GetAutos();

            Assert.IsTrue(Enumerable.Count(cars) > 0);

            AutoDto car = cars.First();
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
            var customers = Target.GetKunden();
            Assert.IsTrue(Enumerable.Count(customers) > 0);

            KundeDto cust = customers.First();
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
            var reservations  = Target.GetReservations();
            Assert.IsTrue(Enumerable.Count(reservations) > 0);

            ReservationDto res = reservations.First();
            Assert.IsNotNull(res);
            ReservationDto resByID = Target.GetReservation(res.ReservationNr);
            Assert.IsNotNull(resByID);

            Assert.AreEqual(res.ReservationNr, resByID.ReservationNr);
            Assert.AreEqual(res.Kunde.Id, resByID.Kunde.Id);
            Assert.AreEqual(res.Von, resByID.Von);
            Assert.AreEqual(res.Bis, resByID.Bis);
            Assert.AreEqual(res.Auto.Id, resByID.Auto.Id);
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
                Auto = Target.GetAutos().First(),
                Kunde = Target.GetKunden().First(),
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
            AutoDto newCar = new AutoDto
            {
                Marke = "Fiat Panda",
                AutoKlasse = AutoKlasse.Luxusklasse,
                Tagestarif = 222,
                Basistarif = 321
            };

            int id = Target.AddAuto(newCar);

            AutoDto original = Target.GetAuto(id);
            AutoDto updated = Target.GetAuto(id);

            updated.Marke = "Peugeot 207";
            updated.Tagestarif = 111;
            updated.Basistarif = 123;


            Target.UpdateAuto(updated, original);

            AutoDto toTest = Target.GetAuto(id);

            Assert.AreEqual(updated.Id, toTest.Id);
            Assert.AreEqual(updated.Marke, toTest.Marke);
            Assert.AreEqual(updated.Tagestarif, toTest.Tagestarif);
            Assert.AreEqual(updated.Basistarif, toTest.Basistarif);
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
            Assert.IsTrue(Enumerable.Count(Target.GetReservations()) > 0);

            ReservationDto resOrig = Target.GetReservations().First();
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
            AutoDto original = Target.GetAutos().First();
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
            KundeDto original = Target.GetKunden().First();
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
            ReservationDto original = Target.GetReservations().First();
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
            Assert.AreEqual(4, Enumerable.Count(Target.GetKunden()));

            KundeDto toDelete = Target.GetKunden().First();
            Target.DeleteKunde(toDelete);

            Assert.IsNull(Target.GetKunde(toDelete.Id));
            Assert.AreEqual(3, Enumerable.Count(Target.GetKunden()));
        }

        [TestMethod]
        public void Test_DeleteAuto()
        {
            Assert.AreEqual(3, Enumerable.Count( Target.GetAutos() ) );

            AutoDto toDelete = Target.GetAutos().First();
            Target.DeleteAuto(toDelete);

            Assert.IsNull(Target.GetAuto(toDelete.Id));
            Assert.AreEqual(2, Enumerable.Count(Target.GetAutos()));
        }

        [TestMethod]
        public void Test_DeleteReservation()
        {
            Assert.AreEqual(3, Enumerable.Count(Target.GetReservations()));

            ReservationDto toDelete = Target.GetReservations().First();
            Target.DeleteReservation(toDelete);

            Assert.IsNull(Target.GetReservation(toDelete.ReservationNr));
            Assert.AreEqual(2, Enumerable.Count(Target.GetReservations()));
        }
    }
}
