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
        private List<int> _totalShots = new List<int>();
        private List<string> _shipTypeList = new List<string>();
        private Dictionary<int, string> _hitCoordinateAndType;
        private static readonly Object lockObj = new Object();
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
        public GameHandler(IBaseAI gameAi, Grid gameAndBoardGrid, int iterations)
        {
            _gameAi = gameAi;
            _gameAndBoardGrid = gameAndBoardGrid;
            _gameAi.HitList = _hitList;
            _gameGrid = VisualTreeHelper.GetChild(_gameAndBoardGrid, 0) as Grid;
            _scoreGrid = VisualTreeHelper.GetChild(_gameAndBoardGrid, 1) as Grid;
            //PopulateGrids();
            //PlaceShips(new Shiplist());
            SetScoreBoardControls();
            PlayGame(iterations);
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
        private void PopulateGrids()
        {
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Grid newGrid = new Grid();
                    newGrid.SetValue(Grid.RowProperty, y);
                    newGrid.SetValue(Grid.ColumnProperty, x);
                    newGrid.Width = 30;
                    newGrid.Height = 30;
                    newGrid.Tag = "Water";
                    newGrid.Background = new SolidColorBrush(Colors.AliceBlue);
                    newGrid.MouseDown += GameGrid_MouseDown;
                    _gameGrid.Children.Add(newGrid);
                }
            }
        }
        private void SetScoreBoardControls()
        {
            _algorithmNameBlock = VisualTreeHelper.GetChild(_scoreGrid.Children[1], 0) as TextBlock;
            _shotsAverageBlock = VisualTreeHelper.GetChild(_scoreGrid.Children[3], 0) as TextBlock;
            _shotsMinimumBlock = VisualTreeHelper.GetChild(_scoreGrid.Children[5], 0) as TextBlock;
            _shotsMaximumBlock = VisualTreeHelper.GetChild(_scoreGrid.Children[7], 0) as TextBlock;
            _timeElapsedBlock = VisualTreeHelper.GetChild(_scoreGrid.Children[9], 0) as TextBlock;
            _timeAverageBlock = VisualTreeHelper.GetChild(_scoreGrid.Children[11], 0) as TextBlock;

        }

        private void GameGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Grid targetGrid = sender as Grid;
            CheckHit(-1);
        }
        public void PlayGame(int iterations)
        {
            DateTime gameTimer = new DateTime();
            gameTimer = DateTime.Now;
            for (int i = 0; i < iterations; i++)
            {
                Turn(i);
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
        private void Turn(int loopVar)
        {
            int shotsFired = 0;
            _ships = new Shiplist();
            _hitCoordinateAndType = new Dictionary<int, string>();
            _gameGrid.Children.Clear();
            PopulateGrids();
            PlaceShips(_ships);
            while (!GameWon)
            {
                _gameAi.GameGrid = _gameGrid;
                _gameAi.HitList = _hitList;
                int checkHitValue = _gameAi.Coordinate;
                CheckHit(checkHitValue);
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
        public void PlaceShips(Shiplist shipList)
        {
            foreach (Ship item in shipList.Ships)
            {
                ValidateShipPlacement(item);
            }
        }
        private void ValidateShipPlacement(Ship ship)
        {
            bool placementValid = false;
            int placeMentIndexer = 0;
            int shipStartIndex = 0;
            while (!placementValid)
            {
                Grid gridVar = new Grid();
                gridVar.Tag = "";
                int squaresValidated;
                shipStartIndex = _random.Next(0, 99);
                try
                {
                    gridVar = VisualTreeHelper.GetChild(_gameGrid, shipStartIndex) as Grid;
                }
                catch { }
                if (gridVar.Tag.ToString() == "Water")
                {
                    squaresValidated = 0;
                    int direction = _random.Next(1, 4);
                    switch (direction)
                    {
                        ///North
                        case 1:
                            placeMentIndexer = -10;
                            for (int i = 0; i < ship.Length; i++)
                            {
                                Grid shipLengthGrid = new Grid();
                                shipLengthGrid.Tag = "";
                                if ((shipStartIndex < 100 || shipStartIndex + (placeMentIndexer * i) > -1))
                                {
                                    try
                                    {
                                        shipLengthGrid = VisualTreeHelper.GetChild(_gameGrid, shipStartIndex + (placeMentIndexer * i)) as Grid;
                                    }
                                    catch
                                    {
                                        break;
                                    }
                                }
                                if (shipLengthGrid.Tag.ToString() != "Water")
                                {
                                    break;
                                }
                                squaresValidated++;
                            }
                            if (squaresValidated == ship.Length)
                            {
                                placementValid = true;
                            }
                            break;
                        ///East
                        case 2:
                            placeMentIndexer = 1;
                            for (int i = 0; i < ship.Length; i++)
                            {
                                Grid shipLengthGrid = new Grid();
                                shipLengthGrid.Tag = "";
                                if (shipStartIndex + (placeMentIndexer * i) < 100 || shipStartIndex > -1)
                                {
                                    try
                                    {
                                        shipLengthGrid = VisualTreeHelper.GetChild(_gameGrid, shipStartIndex + (placeMentIndexer * i)) as Grid;
                                    }
                                    catch
                                    {
                                        break;
                                    }
                                }
                                if (i != 0 && ((shipStartIndex + (placeMentIndexer * i)) % 10 != 0 || shipLengthGrid.Tag.ToString() != "Water"))
                                {
                                    break;
                                }
                                squaresValidated++;
                            }
                            if (squaresValidated == ship.Length)
                            {
                                placementValid = true;
                            }
                            break;
                        ///South
                        case 3:
                            placeMentIndexer = -10;
                            for (int i = 0; i < ship.Length; i++)
                            {
                                Grid shipLengthGrid = new Grid();
                                shipLengthGrid.Tag = "";
                                if ((shipStartIndex < 100 || shipStartIndex + (placeMentIndexer * i) > -1))
                                {
                                    try
                                    {
                                        shipLengthGrid = VisualTreeHelper.GetChild(_gameGrid, shipStartIndex + (placeMentIndexer * i)) as Grid;
                                    }
                                    catch
                                    {
                                        break;
                                    }
                                }
                                if (shipLengthGrid.Tag.ToString() != "Water")
                                {
                                    break;
                                }
                                squaresValidated++;
                            }
                            if (squaresValidated == ship.Length)
                            {
                                placementValid = true;
                            }
                            break;
                        ///West
                        case 4:
                            placeMentIndexer = -1;
                            for (int i = 0; i < ship.Length; i++)
                            {
                                Grid shipLengthGrid = new Grid();
                                shipLengthGrid.Tag = "";
                                if (shipStartIndex + (placeMentIndexer * i) < 100 || shipStartIndex > -1)
                                {
                                    try
                                    {
                                        shipLengthGrid = VisualTreeHelper.GetChild(_gameGrid, shipStartIndex + (placeMentIndexer * i)) as Grid;
                                    }
                                    catch
                                    {
                                        break;
                                    }
                                }
                                if (i != 0 && ((shipStartIndex + (placeMentIndexer * i)) % 10 == 1 || shipLengthGrid.Tag.ToString() != "Water"))
                                {
                                    break;
                                }
                                squaresValidated++;
                            }
                            if (squaresValidated == ship.Length)
                            {
                                placementValid = true;
                            }
                            break;
                    }
                }
            }
            for (int i = 0; i < ship.Length; i++)
            {
                Grid selectedGrid = VisualTreeHelper.GetChild(_gameGrid, shipStartIndex + (placeMentIndexer * i)) as Grid;
                selectedGrid.Tag = ship.Name;
                selectedGrid.Background = new SolidColorBrush(Colors.SandyBrown);
            }
        }
        public void CheckHit(int coord)
        {
            bool shipIsSunk = false;
            Grid hitGrid = new Grid();
            hitGrid.Tag = "";
            hitGrid = VisualTreeHelper.GetChild(_gameGrid, coord) as Grid;
            if (hitGrid.Tag.ToString() == "Water")
            {
                hitGrid.Tag = "Miss";
                hitGrid.Background = new SolidColorBrush(Colors.LightSalmon);
            }
            else
            {
                _hitList.Add(coord);
                _hitCoordinateAndType.Add(coord, hitGrid.Tag.ToString());
                foreach (Ship item in _ships.Ships)
                {
                    if (item.Name == hitGrid.Tag.ToString())
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
                hitGrid.Tag = "Hit";
                hitGrid.Background = new SolidColorBrush(Colors.LightGreen);
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
