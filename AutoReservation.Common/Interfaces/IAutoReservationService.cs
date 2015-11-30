using AutoReservation.Common.DataTransferObjects;
using System.Collections.Generic;

namespace AutoReservation.Common.Interfaces
{
    public interface IAutoReservationService
    {
        IList<AutoDto> GetAutos();

        AutoDto GetAuto(int id);

        void AddAuto(AutoDto auto);

        void DeleteAuto(AutoDto auto);

        void UpdateAuto(AutoDto modified, AutoDto original);

        IList<ReservationDto> GetReservations();

        ReservationDto GetReservation(int id);

        void AddReservation(ReservationDto reservation);

        void DeleteReservation(ReservationDto reservation);

        void UpdateReservation(ReservationDto modified, ReservationDto original);

        IList<KundeDto> GetKunden();

        KundeDto GetKunde(int id);

        void AddKunde(KundeDto kunde);

        void DeleteKunde(KundeDto kunde);

        void UpdateKunde(KundeDto modified, KundeDto original);
    }
}
