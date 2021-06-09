using FlightCashbox.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MVVM
{
    class ViewModel : INotifyPropertyChanged
    {
        public TrainTicketEntities ctx = new TrainTicketEntities();

        public ViewModel()
        {
            FillSeat();
        }
        private void FillSeat()
        {
            var q = (from a in ctx.Seat
                     select a).ToList();
            Seat = q;
        }

        private List<Seat> _seat;
        public List<Seat> Seat
        {
            get { return _seat; }
            set
            {
                _seat = value;

                foreach (var seat in value)
                {
                    SeatDateGrid.Add(new SeatDateGrid {
                        Upper = (bool)seat.Upper,
                        Side = (bool)seat.Side,
                        TypeTrain = (from a in ctx.Flight
                                                           .Where(a => a.NumFlight == seat.NumFlight)
                                                   select a.Train.Type).FirstOrDefault(),
                        TypeRailwayCarriage = (from a in ctx.RailwayCarriage
                                                           .Where(a => a.NumRailwayCarriage == seat.NumRailwayCarriage && a.NumFlight == seat.NumFlight)
                                                             select a.Type).FirstOrDefault(),
                        NumSeat = seat.NumSeat,
                        NumRailwayCarriage = seat.NumRailwayCarriage,
                        NumFlight = seat.NumFlight
                    });
                }
                NotifyPropertyChanged();
            }
        }

        private List<SeatDateGrid> _seatDateGrid = new List<SeatDateGrid>();
        public List<SeatDateGrid> SeatDateGrid
        {
            get { return _seatDateGrid; }
            set
            {
                _seatDateGrid = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
