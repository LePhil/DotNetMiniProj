using AutoReservation.Common.Extensions;
using AutoReservation.Common.DataTransferObjects.Core;
using System.Text;

namespace AutoReservation.Common.DataTransferObjects
{
    public class AutoDto : DtoBase<AutoDto>
    {
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (id == value)
                {
                    return;
                }
                id = value;
                this.OnPropertyChanged(p => p.Id);
            }
        }

        private string marke;
        public string Marke
        {
            get { return marke; }
            set
            {
                if (marke == value)
                {
                    return;
                }
                marke = value;
                this.OnPropertyChanged(p => p.Marke);
            }
        }

        private int tagestarif;
        public int Tagestarif
        {
            get { return tagestarif; }
            set
            {
                if (tagestarif == value)
                {
                    return;
                }
                tagestarif = value;
                this.OnPropertyChanged(p => p.Tagestarif);
            }
        }

        private AutoKlasse autoKlasse;
        public AutoKlasse AutoKlasse {
            get { return autoKlasse; }
            set
            {
                if (autoKlasse == value) {
                    return;
                }
                autoKlasse = value;
                this.OnPropertyChanged(p => p.AutoKlasse);
            }
        }


        private int basistarif;
        public int Basistarif
        {
            get { return basistarif; }
            set
            {
                if (basistarif == value)
                {
                    return;
                }
                basistarif = value;
                this.OnPropertyChanged(p => p.Basistarif);
            }
        }

        public override string Validate()
        {
            StringBuilder error = new StringBuilder();
            if (string.IsNullOrEmpty(marke))
            {
                error.AppendLine("- Marke ist nicht gesetzt.");
            }
            if (tagestarif <= 0)
            {
                error.AppendLine("- Tagestarif muss grösser als 0 sein.");
            }
            if (AutoKlasse == AutoKlasse.Luxusklasse && basistarif <= 0)
            {
                error.AppendLine("- Basistarif eines Luxusautos muss grösser als 0 sein.");
            }

            if (error.Length == 0) { return null; }
                   return error.ToString();
        }

        public override AutoDto Clone()
        {
            return new AutoDto
            {
                Id = Id,
                Marke = Marke,
                Tagestarif = Tagestarif,
                AutoKlasse = AutoKlasse,
                Basistarif = Basistarif
            };
        }

        public override string ToString()
        {
            return string.Format(
                "{0}; {1}; {2}; {3}; {4}",
                Id,
                Marke,
                Tagestarif,
                Basistarif,
                AutoKlasse);
        }
    }
}
