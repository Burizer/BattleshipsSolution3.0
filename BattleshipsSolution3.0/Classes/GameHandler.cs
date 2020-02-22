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
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BattleshipsSolution3._0.Classes
{
    public class GameHandler
    {
        #region Instance fields
        private IBaseAI _gameAi;
        private Grid _gameAndBoardGrid;
        private Grid _gameGrid;
        private Grid _scoreGrid;
        private Shiplist _ships;
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
        private TextBlock _algorithmNameBlock;
        private TextBlock _shotsAverageBlock;
        private TextBlock _shotsMinimumBlock;
        private TextBlock _shotsMaximumBlock;
        private TextBlock _timeElapsedBlock;
        private TextBlock _timeAverageBlock;
        #endregion
        #region Constructor
        public GameHandler(IBaseAI gameAi, Grid gameAndBoardGrid)
        {
            _gameAi = gameAi;
            _gameAndBoardGrid = gameAndBoardGrid;
            _gameAi.HitList = _hitList;
            _gameGrid = VisualTreeHelper.GetChild(_gameAndBoardGrid, 0) as Grid;
            _scoreGrid = VisualTreeHelper.GetChild(_gameAndBoardGrid, 1) as Grid;
            SetScoreBoardControls();
        }
        #endregion
        #region Properties
        public IBaseAI GameAi
        {
            get { return _gameAi; }
        }
        public Grid GameAndBoardGrid
        {
            get { return _gameAndBoardGrid; }
            set
            {
                if (value != _gameAndBoardGrid)
                {
                    _gameAndBoardGrid = value;
                    OnPropertyChanged();
                }
            }
        }
        public Grid GameGrid
        {
            get { return _gameGrid; }
            set
            {
                _gameGrid = value;
                OnPropertyChanged();
            }
        }
        public Grid ScoreGrid
        {
            get { return _scoreGrid; }
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
        private void PopulateGrids()
        {
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Rectangle newRect = new Rectangle();
                    newRect.SetValue(Grid.RowProperty, y);
                    newRect.SetValue(Grid.ColumnProperty, x);
                    newRect.Width = 30;
                    newRect.Height = 30;
                    newRect.Tag = "Water";
                    newRect.Fill = new SolidColorBrush(Colors.AliceBlue);
                    newRect.Stroke = new SolidColorBrush(Colors.Black);
                    newRect.StrokeThickness = 2;
                    _gameGrid.Children.Add(newRect);
                }
            }
        }
        private void SetScoreBoardControls()
        {
            _algorithmNameBlock = VisualTreeHelper.GetChild(_scoreGrid, 1) as TextBlock;
            _shotsAverageBlock = VisualTreeHelper.GetChild(_scoreGrid, 3) as TextBlock;
            _shotsMinimumBlock = VisualTreeHelper.GetChild(_scoreGrid, 5) as TextBlock;
            _shotsMaximumBlock = VisualTreeHelper.GetChild(_scoreGrid, 7) as TextBlock;
            _timeElapsedBlock = VisualTreeHelper.GetChild(_scoreGrid, 9) as TextBlock;
            _timeAverageBlock = VisualTreeHelper.GetChild(_scoreGrid, 11) as TextBlock;

        }

        //private void GameGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    Grid targetGrid = sender as Grid;
        //    CheckHit(-1);
        //}
        public void PlayGame(int iterations)
        {
            DateTime gameTimer = new DateTime();
            gameTimer = DateTime.Now;
            for (int i = 0; i < iterations; i++)
            {
                if (i != iterations - 1)
                {
                    Turn(i, false);
                }
                else
                {
                    Turn(i, true);
                }
            }
            TimeSpan gameEndTimer = new TimeSpan();
            gameEndTimer = DateTime.Now - gameTimer;
            _timeElapsed = gameEndTimer.ToString();
            _timeElapsedBlock.Text = _timeElapsed;
            long averageTime = gameEndTimer.Ticks / iterations;
            TimeSpan averageSpan = TimeSpan.FromTicks(averageTime);
            _timeAverage = averageSpan.ToString();
            _timeAverageBlock.Text = _timeAverage;
        }
        private void Turn(int loopVar, bool lastIteration)
        {
            int shotsFired = 0;
            _ships = new Shiplist();
            _hitCoordinateAndType = new Dictionary<int, string>();
            _gridDictionary = ResetGridDictionary;
            if (lastIteration == true)
            {
                _gameGrid.Children.Clear();
                PopulateGrids();
                PlaceShips(_ships, true);
            }
            else
            {
                PlaceShips(_ships, false);
            }

            while (!GameWon)
            {
                _gameAi.GridDictionary = _gridDictionary;
                _gameAi.HitList = _hitList;
                int checkHitValue = _gameAi.Coordinate;
                CheckHit(checkHitValue, lastIteration);
                shotsFired++;
            }
            _totalShots.Add(shotsFired);
            _shotsAverage = _totalShots.Sum() / _totalShots.Count;
            if (loopVar == 0 || shotsFired < _shotsMinimum)
            {
                _shotsMinimum = shotsFired;
            }
            if (shotsFired > _shotsMaximum)
            {
                _shotsMaximum = shotsFired;
            }

            string[] typeNameSplit = _gameAi.GetType().ToString().Split(Convert.ToChar("."));
            _algorithmNameBlock.Text = typeNameSplit[3];
            _shotsAverageBlock.Text = _shotsAverage.ToString();
            _shotsMinimumBlock.Text = _shotsMinimum.ToString();
            _shotsMaximumBlock.Text = _shotsMaximum.ToString();
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
                if (lastIteration)
                {
                    Rectangle selectedGrid = VisualTreeHelper.GetChild(_gameGrid, shipStartIndex + (placeMentIndexer * i)) as Rectangle;
                    selectedGrid.Tag = ship.Name;
                    selectedGrid.Fill = new SolidColorBrush(Colors.SandyBrown);
                }
            }
        }
        public void CheckHit(int coord, bool lastIteration)
        {
            bool shipIsSunk = false;
            string hitGrid = _gridDictionary[coord];
            var targetGrid = new Rectangle();
            if (lastIteration)
            {
                targetGrid = _gameGrid.Children[coord] as Rectangle;
            }
            if (hitGrid == "Water")
            {
                _gridDictionary[coord] = "Miss";
                if (lastIteration)
                {
                    targetGrid.Tag = "Miss";
                    targetGrid.Fill = new SolidColorBrush(Colors.LightSalmon);
                }
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
                if (lastIteration)
                {
                    targetGrid.Tag = "Hit";
                    targetGrid.Fill = new SolidColorBrush(Colors.LightGreen);
                }
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
