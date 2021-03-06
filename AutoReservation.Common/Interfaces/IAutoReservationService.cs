﻿using AutoReservation.Common.DataTransferObjects;
using System.Collections.Generic;
using System.ServiceModel;

namespace AutoReservation.Common.Interfaces
{
    [ServiceContract(Name = "AutoReservationService")]
    public interface IAutoReservationService
    {
        [OperationContract]
        IEnumerable<AutoDto> GetAutos();

        [OperationContract]
        AutoDto GetAuto(int id);

        [OperationContract]
        int AddAuto(AutoDto auto);

        [OperationContract]
        void DeleteAuto(AutoDto auto);

        [OperationContract]
        [FaultContract(typeof(AutoDto))]
        void UpdateAuto(AutoDto modified, AutoDto original);

        [OperationContract]
        IEnumerable<ReservationDto> GetReservations();

        [OperationContract]
        ReservationDto GetReservation(int id);

        [OperationContract]
        int AddReservation(ReservationDto reservation);

        [OperationContract]
        void DeleteReservation(ReservationDto reservation);

        [OperationContract]
        [FaultContract(typeof(ReservationDto))]
        void UpdateReservation(ReservationDto modified, ReservationDto original);

        [OperationContract]
        IEnumerable<KundeDto> GetKunden();

        [OperationContract]
        KundeDto GetKunde(int id);

        [OperationContract]
        int AddKunde(KundeDto kunde);

        [OperationContract]
        void DeleteKunde(KundeDto kunde);

        [OperationContract]
        [FaultContract(typeof(KundeDto))]
        void UpdateKunde(KundeDto modified, KundeDto original);
    }
}
