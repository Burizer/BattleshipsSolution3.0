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
        private List<Tuple<int, string>> _hitCoordinateAndType;
        private string _algorithmName = "";
        private int _shotsAverage = 0;
        private int _shotsMinimum = 0;
        private int _shotsMaximum = 0;
        private string _timeElapsed = "";
        private TextBlock _algorithmNameBlock;
        private TextBlock _shotsAverageBlock;
        private TextBlock _shotsMinimumBlock;
        private TextBlock _shotsMaximumBlock;
        private TextBlock _timeElapsedBlock;
        #endregion

        public GameHandler(IBaseAI gameAi, Grid gameAndBoardGrid)
        {
            _gameAi = gameAi;
            _gameAndBoardGrid = gameAndBoardGrid;
            _gameAi.HitList = _hitList;
            _gameGrid = VisualTreeHelper.GetChild(_gameAndBoardGrid, 0) as Grid;
            _scoreGrid = VisualTreeHelper.GetChild(_gameAndBoardGrid, 1) as Grid;
            PopulateGrids();
            SetScoreBoardControls();
            PlayGame(100);
        }

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
        #endregion
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
            _algorithmNameBlock.Text = _algorithmName;
            _shotsAverageBlock.Text = _shotsAverage.ToString();
            _shotsMinimumBlock.Text = _shotsMinimum.ToString();
            _shotsMaximumBlock.Text = _shotsMaximum.ToString();
            _timeElapsedBlock.Text = _timeElapsed;
        }

        private void GameGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Grid targetGrid = sender as Grid;
            Shoot(1);
        }
        public void PlayGame(int iterations)
        {
            DispatcherTimer gameTimer = new DispatcherTimer();
            _algorithmName = GameGrid.GetType().ToString();
            gameTimer.Start();
            for (int i = 0; i < iterations; i++)
            {
                int shotsFired = 0;
                _ships = new Shiplist();
                _hitCoordinateAndType = new List<Tuple<int, string>>();
                foreach (Grid item in _gameGrid.Children)
                {
                    item.Tag = "Water";
                    item.Background = new SolidColorBrush(Colors.LightSkyBlue);

                }
                PlaceShips(_ships);
                while (!GameWon)
                {
                    Shoot(_gameAi.Coordinate);
                    shotsFired++;
                }
                _totalShots.Add(shotsFired);
                int averageShots = 0;
                foreach (int shot in _totalShots)
                {
                    averageShots += shot;
                }
                _shotsAverage = averageShots / i;
                if (i == 0 || shotsFired < _shotsMinimum)
                {
                    _shotsMinimum = shotsFired;
                }
                if (shotsFired > _shotsMaximum)
                {
                    _shotsMaximum = shotsFired;
                }
            }
            gameTimer.Stop();
            _timeElapsed = gameTimer.ToString();

        }
        public void PlaceShips(Shiplist shipList)
        {
            foreach (Ship item in shipList.Ships)
            {
                ValidateShipPlacement(item);
            }
        }
        public void Shoot(int coord)
        {
            var hitGrid = VisualTreeHelper.GetChild(_gameGrid, coord) as Grid;
            CheckHit(hitGrid, coord);
        }
        public void CheckHit(Grid grid, int coord)
        {
            if (grid.Tag.ToString() == "Water")
            {
                grid.Tag = "Miss";
                grid.Background = new SolidColorBrush(Colors.LightSalmon);
            }
            else
            {
                _hitCoordinateAndType.Add(new Tuple<int, string>(coord, grid.Tag.ToString()));
                foreach (Ship item in _ships.Ships)
                {
                    if (item.Name == grid.Tag.ToString())
                    {
                        item.Hits--;
                        if (item.IsSunk)
                        {
                            _hitList = _hitCoordinateAndType.Where(x => x.Item2 != item.Name).Select(x => x.Item1).ToList<int>();
                        }
                    }

                }
                grid.Tag = "Hit";
                grid.Background = new SolidColorBrush(Colors.LightGreen);
                _hitList.Add(coord);
            }
        }
        public void ValidateShipPlacement(Ship ship)
        {
            bool placementValid = true;
            int shipStartIndex = _random.Next(0, 100 - ship.Length);
            var gridVar = VisualTreeHelper.GetChild(_gameGrid, shipStartIndex) as Grid;
            int direction = _random.Next(1, 4);
            switch (direction)
            {
                ///North
                case 1:
                    if (gridVar.Tag.ToString() == "Water")
                    {
                        for (int i = 0; i < ship.Length; i++)
                        {
                            Grid shipLengthGrid = new Grid();
                            try
                            {
                                shipLengthGrid = VisualTreeHelper.GetChild(_gameGrid, shipStartIndex - (10 * i)) as Grid;
                            }
                            catch
                            {
                                ValidateShipPlacement(ship);
                                placementValid = false;
                                break;
                            }
                            if (shipLengthGrid.Tag.ToString() != "Water")
                            {
                                ValidateShipPlacement(ship);
                                placementValid = false;
                                break;
                            }
                        }
                        if (placementValid)
                        {
                            for (int j = 0; j < ship.Length; j++)
                            {
                                var shipPlacementGrid = VisualTreeHelper.GetChild(_gameGrid, shipStartIndex - (10 * j)) as Grid;
                                shipPlacementGrid.Tag = ship.Name;
                                shipPlacementGrid.Background = new SolidColorBrush(Colors.Brown);
                            }
                        }
                    }
                    else
                    {
                        ValidateShipPlacement(ship);
                    }
                    return;
                ///East
                case 2:
                    if (gridVar.Tag.ToString() == "Water")
                    {
                        for (int i = 0; i < ship.Length; i++)
                        {
                            Grid shipLengthGrid = new Grid();
                            try
                            {
                                shipLengthGrid = VisualTreeHelper.GetChild(_gameGrid, shipStartIndex + (1 * i)) as Grid;

                            }
                            catch
                            {
                                ValidateShipPlacement(ship);
                                placementValid = false;
                                break;
                            }
                            if (gridVar != shipLengthGrid && shipStartIndex + (1 * i) % 10 != 1 && (shipLengthGrid.Tag.ToString() != "Water" || shipLengthGrid == null))
                            {
                                ValidateShipPlacement(ship);
                                placementValid = false;
                                break;
                            }
                        }
                        if (placementValid)
                        {
                            for (int j = 0; j < ship.Length; j++)
                            {
                                var shipPlacementGrid = VisualTreeHelper.GetChild(_gameGrid, shipStartIndex + (1 * j)) as Grid;
                                shipPlacementGrid.Tag = ship.Name;
                                shipPlacementGrid.Background = new SolidColorBrush(Colors.Brown);
                            }
                        }
                    }
                    else
                    {
                        ValidateShipPlacement(ship);
                    }
                    return;
                ///South
                case 3:
                    if (gridVar.Tag.ToString() == "Water")
                    {
                        for (int i = 0; i < ship.Length; i++)
                        {
                            Grid shipLengthGrid = new Grid();
                            try
                            {
                                shipLengthGrid = VisualTreeHelper.GetChild(_gameGrid, shipStartIndex + (10 * i)) as Grid;
                            }
                            catch
                            {
                                ValidateShipPlacement(ship);
                                placementValid = false;
                                break;
                            }
                            if (shipLengthGrid.Tag.ToString() != "Water" || shipLengthGrid == null)
                            {
                                ValidateShipPlacement(ship);
                                placementValid = false;
                                break;
                            }
                        }
                        if (placementValid)
                        {
                            for (int j = 0; j < ship.Length; j++)
                            {
                                var shipPlacementGrid = VisualTreeHelper.GetChild(_gameGrid, shipStartIndex + (10 * j)) as Grid;
                                shipPlacementGrid.Tag = ship.Name;
                                shipPlacementGrid.Background = new SolidColorBrush(Colors.Brown);
                            }
                        }
                    }
                    else
                    {
                        ValidateShipPlacement(ship);
                    }
                    return;
                ///West
                case 4:
                    if (gridVar.Tag.ToString() == "Water")
                    {
                        for (int i = 0; i < ship.Length; i++)
                        {
                            Grid shipLengthGrid = new Grid();
                            try
                            {
                                shipLengthGrid = VisualTreeHelper.GetChild(_gameGrid, shipStartIndex - (1 * i)) as Grid;
                            }
                            catch
                            {
                                ValidateShipPlacement(ship);
                                placementValid = false;
                                break;
                            }
                            if (gridVar != shipLengthGrid && shipStartIndex - (1 * i) % 10 != 0 && (shipLengthGrid.Tag.ToString() != "Water" || shipLengthGrid.Name == null))
                            {
                                ValidateShipPlacement(ship);
                                placementValid = false;
                                break;
                            }
                        }
                        if (placementValid)
                        {
                            for (int j = 0; j < ship.Length; j++)
                            {
                                var shipPlacementGrid = VisualTreeHelper.GetChild(_gameGrid, shipStartIndex - (1 * j)) as Grid;
                                shipPlacementGrid.Tag = ship.Name;
                                shipPlacementGrid.Background = new SolidColorBrush(Colors.Brown);
                            }
                        }
                    }
                    else
                    {
                        ValidateShipPlacement(ship);
                    }
                    return;
            }
        }
        public bool GameWon
        {
            get
            {
                foreach (var ship in _ships.Ships)
                {
                    if (!ship.IsSunk)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        #region OnPropertyChanged code
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        //Grid something = ((MainWindow)System.Windows.Application.Current.MainWindow).
    }
}
