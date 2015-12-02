using AutoReservation.Common.Interfaces;
using System;
using System.Diagnostics;
using AutoReservation.Common.DataTransferObjects;
using System.Collections.Generic;
using AutoReservation.BusinessLayer;
using System.ServiceModel;
using AutoReservation.Dal;

namespace AutoReservation.Service.Wcf
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class AutoReservationService : IAutoReservationService
    {
        private AutoReservationBusinessComponent busService;

        public AutoReservationService() {
            busService = new AutoReservationBusinessComponent();
        }

        public int AddAuto(AutoDto autoDto)
        {
            WriteActualMethod();
            return busService.AddAuto(DtoConverter.ConvertToEntity(autoDto));
        }

        public int AddKunde(KundeDto kundeDto)
        {
            WriteActualMethod();
            return busService.AddKunde(DtoConverter.ConvertToEntity(kundeDto));
        }

        public int AddReservation(ReservationDto reservationDto)
        {
            WriteActualMethod();
            return busService.AddReservation(DtoConverter.ConvertToEntity(reservationDto));
        }

        public void DeleteAuto(AutoDto autoDto)
        {
            WriteActualMethod();
            busService.DeleteAuto(DtoConverter.ConvertToEntity(autoDto));
        }

        public void DeleteKunde(KundeDto kundeDto)
        {
            WriteActualMethod();
            busService.DeleteKunde(DtoConverter.ConvertToEntity(kundeDto));
        }

        public void DeleteReservation(ReservationDto reservationDto)
        {
            WriteActualMethod();
            busService.DeleteReservation(DtoConverter.ConvertToEntity(reservationDto));
        }

        public AutoDto GetAuto(int id)
        {
            WriteActualMethod();
            return DtoConverter.ConvertToDto(busService.GetAuto(id));
        }

        public IList<AutoDto> GetAutos()
        {
            WriteActualMethod();
            return DtoConverter.ConvertToDtos(busService.GetAutos());
        }

        public KundeDto GetKunde(int id)
        {
            WriteActualMethod();
            return DtoConverter.ConvertToDto(busService.GetKunde(id));
        }

        public IList<KundeDto> GetKunden()
        {
            WriteActualMethod();
            return DtoConverter.ConvertToDtos(busService.GetKunden());
        }

        public ReservationDto GetReservation(int id)
        {
            WriteActualMethod();
            return DtoConverter.ConvertToDto(busService.GetReservation(id));
        }

        public IList<ReservationDto> GetReservations()
        {
            WriteActualMethod();
            return DtoConverter.ConvertToDtos(busService.GetReservations());
        }

        public void UpdateAuto(AutoDto modifiedDto, AutoDto originalDto)
        {
            try {
                WriteActualMethod();
                busService.UpdateAuto(DtoConverter.ConvertToEntity(modifiedDto), DtoConverter.ConvertToEntity(originalDto));
            }
            catch (LocalOptimisticConcurrencyException<Auto> ex)
            {
                throw new FaultException<AutoDto>(ex.MergedEntity.ConvertToDto(), ex.Message);
            }
        }

        public void UpdateKunde(KundeDto modifiedDto, KundeDto originalDto)
        {
            try {
                WriteActualMethod();
                busService.UpdateKunde(DtoConverter.ConvertToEntity(modifiedDto), DtoConverter.ConvertToEntity(originalDto));
            }
            catch (LocalOptimisticConcurrencyException<Kunde> ex)
            {
                throw new FaultException<KundeDto>(ex.MergedEntity.ConvertToDto(), ex.Message);
            }
        }

        public void UpdateReservation(ReservationDto modifiedDto, ReservationDto originalDto)
        {
            try {
                WriteActualMethod();
                busService.UpdateReservation(DtoConverter.ConvertToEntity(modifiedDto), DtoConverter.ConvertToEntity(originalDto));
            }
            catch (LocalOptimisticConcurrencyException<Reservation> ex)
            {
                throw new FaultException<ReservationDto>(ex.MergedEntity.ConvertToDto(), ex.Message);
            }
        }

        private static void WriteActualMethod()
        {
            Console.WriteLine("Calling: " + new StackTrace().GetFrame(1).GetMethod().Name);
        }
    }
}