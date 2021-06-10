using FlightCashbox.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM
{
    class ViewModel : INotifyPropertyChanged
    {
        public TrainTicketEntities ctx = new TrainTicketEntities();

        public ViewModel()
        {
            FillSeat();
            FillStationName();
            FillTypeTrainName();
            FillTypeRailwayCarriageName();
        }
        private void FillSeat()
        {
            var q = (from a in ctx.Seat
                     .Where(a => a.Sold == null || (bool)a.Sold == false)
                     select a).ToList();
            Seat = q;
        }

        private void FillStationName()
        {
            var q = (from a in ctx.Station
                     select a.Name).ToList();
            StationName = q;
        }

        private void FillTypeTrainName()
        {
            var q = (from a in ctx.TypeTrain
                     select a.Name).ToList();
            TypeTrainName = q;
        }

        private void FillTypeRailwayCarriageName()
        {
            var q = (from a in ctx.TypeRailwayCarriage
                     select a.Name).ToList();
            TypeRailwayCarriageName = q;
        }

        private List<Seat> _seat;
        public List<Seat> Seat
        {
            get { return _seat; }
            set
            {
                _seat = value;
                SeatDateGrid = new List<SeatDateGrid>();
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
                        NumFlight = seat.NumFlight,
                        Price = Price.Get((from a in ctx.Flight.Where(a => a.NumFlight == seat.NumFlight)select a.Train.Type).FirstOrDefault(),
                                           (from a in ctx.RailwayCarriage.Where(a => a.NumRailwayCarriage == seat.NumRailwayCarriage && a.NumFlight == seat.NumFlight)select a.Type).FirstOrDefault(),
                                           (bool)seat.Upper,
                                           (bool)seat.Side)
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

        private string _tBName;
        public string TBName
        {
            get { return _tBName; }
            set
            {
                _tBName = value;
                NotifyPropertyChanged();
            }
        }

        private string _tBSurname;
        public string TBSurname
        {
            get { return _tBSurname; }
            set
            {
                _tBSurname = value;
                NotifyPropertyChanged();
            }
        }

        private string _tBPatronymic;
        public string TBPatronymic
        {
            get { return _tBPatronymic; }
            set
            {
                _tBPatronymic = value;
                NotifyPropertyChanged();
            }
        }

        private string _tBPassport = "0";
        public string TBPassport
        {
            get { return _tBPassport; }
            set
            {
                if (int.TryParse(value, out _))
                {
                    _tBPassport = value;
                }
                NotifyPropertyChanged();
            }
        }

        private List<string> _stationName;
        public List<string> StationName
        {
            get { return _stationName; }
            set
            {
                _stationName = value;
                NotifyPropertyChanged();
            }
        }

        private List<string> _typeTrainName;
        public List<string> TypeTrainName
        {
            get { return _typeTrainName; }
            set
            {
                _typeTrainName = value;
                NotifyPropertyChanged();
            }
        }

        private List<string> _typeRailwayCarriageName;
        public List<string> TypeRailwayCarriageName
        {
            get { return _typeRailwayCarriageName; }
            set
            {
                _typeRailwayCarriageName = value;
                NotifyPropertyChanged();
            }
        }

        private SeatDateGrid _selectedSeat;
        public SeatDateGrid SelectedSeat
        {
            get { return _selectedSeat; }
            set
            {
                _selectedSeat = value;
                NotifyPropertyChanged();
            }
        }

        private string _selectedDepartureStationName;
        public string SelectedDepartureStationName
        {
            get { return _selectedDepartureStationName; }
            set
            {
                _selectedDepartureStationName = value;
                NotifyPropertyChanged();
            }
        }

        private string _selectedArrivalStationName;
        public string SelectedArrivalStationName
        {
            get { return _selectedArrivalStationName; }
            set
            {
                _selectedArrivalStationName = value;
                NotifyPropertyChanged();
            }
        }

        private string _selectedTypeTrainName;
        public string SelectedTypeTrainName
        {
            get { return _selectedTypeTrainName; }
            set
            {
                _selectedTypeTrainName = value;
                NotifyPropertyChanged();
            }
        }

        private string _selectedTypeRailwayCarriageName;
        public string SelectedTypeRailwayCarriageName
        {
            get { return _selectedTypeRailwayCarriageName; }
            set
            {
                _selectedTypeRailwayCarriageName = value;
                NotifyPropertyChanged();
            }
        }

        private bool _checkBoxUpperSeat;
        public bool CheckBoxUpperSeat
        {
            get { return _checkBoxUpperSeat; }
            set
            {
                _checkBoxUpperSeat = value;
                NotifyPropertyChanged();
            }
        }

        private bool _checkBoxSideSeat;
        public bool CheckBoxSideSeat
        {
            get { return _checkBoxSideSeat; }
            set
            {
                _checkBoxSideSeat = value;
                NotifyPropertyChanged();
            }
        }

        private RelayCommand _sort;
        public RelayCommand Sort
        {
            get
            {
                return _sort ??
                (_sort = new RelayCommand(obj =>
                {
                    FillSeat();
                    List<SeatDateGrid> listSeatDateGrids = SeatDateGrid;
                    List<Route> listRoute;
                    List<int> FlightNumberList = (from a in ctx.Flight
                                                  select a.NumFlight).Distinct().ToList();
                    int numberInRouteDeparture = 0;
                    int numberInRouteArrival = 0;
                    string stationName;
                    if (SelectedDepartureStationName != null && SelectedArrivalStationName != null) //Выбрана станция Отправления и Прибытия
                    {
                        foreach (var numberFlight in FlightNumberList)
                        {
                            listRoute = ctx.Flight.Where(a => a.NumFlight == numberFlight).First().Route.ToList();

                            foreach (var route in listRoute)
                            {
                                stationName = ctx.Station.Where(a => a.idStation == route.idStation).First().Name;
                                if (stationName == SelectedDepartureStationName) numberInRouteDeparture = route.NumberInRoute;
                                else if (stationName == SelectedArrivalStationName) numberInRouteArrival = route.NumberInRoute;
                            }
                            if (numberInRouteDeparture == 0 || numberInRouteArrival == 0 || numberInRouteDeparture > numberInRouteArrival)
                            { 
                                listSeatDateGrids = listSeatDateGrids.Where(a => a.NumFlight != numberFlight).ToList(); 
                            }
                        }
                    }
                    else if (SelectedDepartureStationName != null && SelectedArrivalStationName == null) //Выбрана только станция Отправления
                    {
                        foreach (var numberFlight in FlightNumberList)
                        {
                            listRoute = ctx.Flight.Where(a => a.NumFlight == numberFlight).First().Route.ToList();

                            foreach (var route in listRoute)
                            {
                                stationName = ctx.Station.Where(a => a.idStation == route.idStation).First().Name;
                                if (stationName == SelectedDepartureStationName) numberInRouteDeparture = route.NumberInRoute;
                            }
                            if (numberInRouteDeparture == 0 || numberInRouteDeparture == listRoute.Count()) 
                            { 
                                listSeatDateGrids = listSeatDateGrids.Where(a => a.NumFlight != numberFlight).ToList(); 
                            }
                        }
                    }
                    else if (SelectedDepartureStationName == null && SelectedArrivalStationName != null) //Выбрана только станция Прибытия
                    {
                        foreach (var numberFlight in FlightNumberList)
                        {
                            listRoute = ctx.Flight.Where(a => a.NumFlight == numberFlight).First().Route.ToList();

                            foreach (var route in listRoute)
                            {
                                stationName = ctx.Station.Where(a => a.idStation == route.idStation).First().Name;
                                if (stationName == SelectedArrivalStationName) numberInRouteArrival = route.NumberInRoute;
                            }
                            if (numberInRouteArrival == 0 || numberInRouteArrival == 1)
                            {
                                listSeatDateGrids = listSeatDateGrids.Where(a => a.NumFlight != numberFlight).ToList();
                            }
                        }
                    }
                    if (SelectedTypeTrainName != null) listSeatDateGrids = listSeatDateGrids.Where(a => a.TypeTrain == SelectedTypeTrainName).ToList();
                    if (SelectedTypeRailwayCarriageName != null) listSeatDateGrids = listSeatDateGrids.Where(a => a.TypeRailwayCarriage == SelectedTypeRailwayCarriageName).ToList();
                    if (CheckBoxUpperSeat) listSeatDateGrids = listSeatDateGrids.Where(a => !a.Upper).ToList();
                    if (CheckBoxSideSeat) listSeatDateGrids = listSeatDateGrids.Where(a => !a.Side).ToList();
                    SeatDateGrid = listSeatDateGrids;
                }));
            }
        }

        private RelayCommand _resetSort;
        public RelayCommand ResetSort
        {
            get
            {
                return _resetSort ??
                (_resetSort = new RelayCommand(obj =>
                {
                    FillSeat();
                    SelectedDepartureStationName = null;
                    SelectedArrivalStationName = null;
                    SelectedTypeTrainName = null;
                    SelectedTypeRailwayCarriageName = null;
                    CheckBoxUpperSeat = false;
                    CheckBoxSideSeat = false;
                }));
            }
        }

        private RelayCommand _sell;
        public RelayCommand Sell
        {
            get
            {
                return _sell ??
                (_sell = new RelayCommand(obj =>
                {
                    if (SelectedSeat == null) { MessageBox.Show("Место не выбрано.", "Ошибка!"); return; };
                    if (TBName == null) { MessageBox.Show("Имя покупателя не задано.", "Ошибка!"); return; };
                    if (TBSurname == null) { MessageBox.Show("Фамилия покупателя не задана.", "Ошибка!"); return; };
                    if (TBPatronymic == null) { MessageBox.Show("Отчество покупателя не задано.", "Ошибка!"); return; };
                    if (TBPassport == null) { MessageBox.Show("Серия и Номер паспорта не заданы.", "Ошибка!"); return; };
                    Flight flight = ctx.Flight.Where(a => a.NumFlight == SelectedSeat.NumFlight).First();
                    Passenger passenger = new Passenger() { Name = TBName, Surname = TBSurname, Patronymic = TBPatronymic, Passport = Convert.ToInt32(TBPassport) };
                    Ticket ticket = new Ticket();
                    ticket.Passenger = passenger;
                    ticket.Price = Price.Get((from a in ctx.Flight.Where(a => a.NumFlight == SelectedSeat.NumFlight) select a.Train.Type).FirstOrDefault(),
                                            (from a in ctx.RailwayCarriage.Where(a => a.NumRailwayCarriage == SelectedSeat.NumRailwayCarriage && a.NumFlight == SelectedSeat.NumFlight) select a.Type).FirstOrDefault(),
                                            (bool)SelectedSeat.Upper,
                                            (bool)SelectedSeat.Side);
                    ticket.DepartureDateTime = flight.DepartureDateTime;
                    ticket.ArrivalDateTime = flight.DepartureDateTime.AddHours(33);
                    ticket.idDepartureStation = ctx.Route.Where(a => a.NumFlight == SelectedSeat.NumFlight && a.NumberInRoute == 1).First().idStation;
                    List<Route> route = ctx.Route.Where(a => a.NumFlight == SelectedSeat.NumFlight).ToList();
                    var r1 = route.Count();
                    ticket.idArrivalStation = ctx.Route.Where(a => a.NumFlight == SelectedSeat.NumFlight && a.NumberInRoute == r1).First().idStation;
                    ticket.NumTrain = flight.NumTrain;
                    ticket.NumFlight = SelectedSeat.NumFlight;
                    ticket.NumRailwayCarrige = SelectedSeat.NumRailwayCarriage;
                    ticket.NumSeat = SelectedSeat.NumSeat;

                    Seat seat = ctx.Seat.Where(a => a.NumFlight == SelectedSeat.NumFlight && a.NumRailwayCarriage == SelectedSeat.NumRailwayCarriage && a.NumSeat == SelectedSeat.NumSeat).First();
                    seat.Sold = true;
                    ctx.Ticket.Add(ticket);
                    ctx.SaveChanges();
                    MessageBox.Show("Билет успешно продан.", "Выполнено!");
                    FillSeat();
                }));
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
