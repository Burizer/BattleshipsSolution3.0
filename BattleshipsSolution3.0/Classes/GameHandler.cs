using BattleshipsSolution3._0.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BattleshipsSolution3._0.Classes
{
    public class GameHandler
    {
        #region Instance fields
        private IBaseAI _gameAi;
        private int _iterations;
        private Shiplist _ships;
        private Grid _gameGrid;
        private static Random _random = new Random();
        private List<int> _hitList = new List<int>();
        private Dictionary<int, string> _gridDictionary;
        private Dictionary<int, string> _hitCoordinateAndType;
        private List<int> _totalShots = new List<int>();
        private List<string> _shipTypeList = new List<string>();
        private string _algorithmName = "";
        private int _shotsAverage = 0;
        private int _shotsMinimum = 0;
        private int _shotsMaximum = 0;
        private string _timeElapsed = "";
        private string _timeAverage = "";
        private string _turns = "";
        #endregion
        #region Constructor
        public GameHandler(IBaseAI gameAi, int iterations)
        {
            _gameAi = gameAi;
            _gameAi.HitList = _hitList;
            _iterations = iterations;
        }
        #endregion
        #region Properties
        public IBaseAI GameAi
        {
            get { return _gameAi; }
        }
        public int Iterations
        {
            get { return _iterations; }
            set
            {
                _iterations = value;
                OnPropertyChanged();
            }
        }
        public Shiplist ShipList
        {
            get { return _ships; }
            set
            {
                _ships = value;
                OnPropertyChanged();
            }
        }
        public List<int> HitList
        {
            get { return _hitList; }
            set
            {
                _hitList = value;
                OnPropertyChanged();
            }
        }
        public string AlgorithmName
        {
            get { return _algorithmName; }
            set
            {
                _algorithmName = value;
                OnPropertyChanged();
            }
        }
        public int ShotsAverage
        {
            get { return _shotsAverage; }
            set
            {
                _shotsAverage = value;
                OnPropertyChanged();
            }
        }
        public int ShotsMaximum
        {
            get { return _shotsMaximum; }
            set
            {
                _shotsMaximum = value;
                OnPropertyChanged();
            }
        }
        public int ShotsMinimum
        {
            get { return _shotsMinimum; }
            set
            {
                _shotsMinimum = value;
                OnPropertyChanged();
            }
        }
        public string TimeElapsed
        {
            get { return _timeElapsed; }
            set
            {
                _timeElapsed = value;
                OnPropertyChanged();
            }
        }
        public string TimeAverage
        {
            get { return _timeAverage; }
            set
            {
                _timeAverage = value;
                OnPropertyChanged();
            }
        }
        public string Turns
        {
            get { return _turns; }
            set
            {
                _turns = value;
                OnPropertyChanged();
            }
        }
        public bool GameWon
        {
            get
            {
                return _ships.GameWon;
            }
        }
        #endregion
        #region Methods
        private Dictionary<int, string> ResetGridDictionary
        {
            get
            {
                Dictionary<int, string> dict = new Dictionary<int, string>();
                for (int i = 0; i < 100; i++)
                {
                    dict.Add(i, "Water");
                }
                return dict;
            }
        }
        

        //private void GameGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    Grid targetGrid = sender as Grid;
        //    CheckHit(-1);
        //}
        public Dictionary<int, string> PlayGame
        {
            get
            {
                DateTime gameTimer = new DateTime();
                gameTimer = DateTime.Now;
                for (int i = 0; i < _iterations; i++)
                {
                    if (i != _iterations - 1)
                    {
                        Turn(i, false);
                    }
                    else
                    {
                        Turn(i, true);
                    }
                    Turns = "Gennemkørsler: " + (i + 1).ToString();
                }
                TimeSpan gameEndTimer = new TimeSpan();
                gameEndTimer = DateTime.Now - gameTimer;
                _timeElapsed = gameEndTimer.ToString();
                long averageTime = gameEndTimer.Ticks / _iterations;
                TimeSpan averageSpan = TimeSpan.FromTicks(averageTime);
                _timeAverage = averageSpan.ToString();
                TimeElapsed = _timeElapsed;
                TimeAverage = _timeAverage;
                return _gridDictionary;
            }
        }
        private void Turn(int loopVar, bool lastIteration)
        {
            int shotsFired = 0;
            _ships = new Shiplist();
            _hitCoordinateAndType = new Dictionary<int, string>();
            _gridDictionary = ResetGridDictionary;
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                if (lastIteration == true)
                {
                    PlaceShips(_ships, true);
                }
                else
                {
                    PlaceShips(_ships, false);
                }
            });

            while (!GameWon)
            {
                _gameAi.GridDictionary = _gridDictionary;
                _gameAi.HitList = _hitList;
                int checkHitValue = _gameAi.Coordinate;
                CheckHit(checkHitValue, lastIteration);
                shotsFired++;
            }
            _totalShots.Add(shotsFired);
            ShotsAverage = _totalShots.Sum() / _totalShots.Count;
            if (loopVar == 0 || shotsFired < _shotsMinimum)
            {
                ShotsMinimum = shotsFired;
            }
            if (shotsFired > _shotsMaximum)
            {
                ShotsMaximum = shotsFired;
            }
        }
        public void PlaceShips(Shiplist shipList, bool lastIteration)
        {
            foreach (Ship item in shipList.Ships)
            {
                ValidateShipPlacement(item, lastIteration);
            }
        }
        private void ValidateShipPlacement(Ship ship, bool lastIteration)
        {
            bool placementValid = false;
            int placeMentIndexer = 0;
            int shipStartIndex = 0;
            while (!placementValid)
            {
                int squaresValidated = 0;
                int direction = _random.Next(1, 3);
                switch (direction)
                {
                    ///East
                    case 1:
                        placeMentIndexer = 1;
                        shipStartIndex = (_random.Next(10) * 10) + _random.Next(10 - (ship.Length - 1));
                        for (int i = 0; i < ship.Length; i++)
                        {
                            string shipLengthGrid = "";
                            int shipIndex = shipStartIndex + (placeMentIndexer * i);
                            if (shipIndex <= 99)
                            {
                                shipLengthGrid = _gridDictionary[shipIndex];
                                if (shipLengthGrid != "Water")
                                {
                                    break;
                                }
                                else
                                {
                                    squaresValidated++;
                                }
                            }
                            else
                            {
                                break;
                            }

                        }
                        if (squaresValidated == ship.Length)
                        {
                            placementValid = true;
                        }
                        break;
                    ///South
                    case 2:
                        placeMentIndexer = 10;
                        shipStartIndex = (_random.Next(10 - (ship.Length - 1)) * 10) + _random.Next(10);
                        for (int i = 0; i < ship.Length; i++)
                        {
                            string shipLengthGrid = "";
                            int shipIndex = shipStartIndex + (placeMentIndexer * i);
                            if (shipIndex <= 99)
                            {
                                shipLengthGrid = _gridDictionary[shipStartIndex + (placeMentIndexer * i)];
                                if (shipLengthGrid != "Water")
                                {
                                    break;
                                }
                                else
                                {
                                    squaresValidated++;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (squaresValidated == ship.Length)
                        {
                            placementValid = true;
                        }
                        break;
                }
            }
            for (int i = 0; i < ship.Length; i++)
            {
                _gridDictionary[shipStartIndex + (placeMentIndexer * i)] = ship.Name;
            }
        }
        public void CheckHit(int coord, bool lastIteration)
        {
            bool shipIsSunk = false;
            string hitGrid = _gridDictionary[coord];
            if (hitGrid == "Water")
            {
                _gridDictionary[coord] = "Miss";
            }
            else
            {
                _hitList.Add(coord);
                _hitCoordinateAndType.Add(coord, hitGrid);
                foreach (Ship item in _ships.Ships)
                {
                    if (item.Name == hitGrid)
                    {
                        item.HitRegistered();
                        if (item.IsSunk)
                        {
                            shipIsSunk = true;
                        }
                    }
                }
                if (shipIsSunk)
                {
                    var sunkenShips = _ships.Ships.Where(x => x.IsSunk).Select(x => x.Name);
                    _hitList = _hitCoordinateAndType.Where(x => !sunkenShips.Contains(x.Value)).Select(x => x.Key).ToList();

                }
                _gridDictionary[coord] = "Hit";
            }
        }
        #endregion
        #region OnPropertyChanged code
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
