using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BattleshipsSolution3._0.Algorithms.Helpers
{

    public class HuntAlgorithm
    {
        #region Instance fields
        private Random _random = new Random();
        private List<int> _hitList;
        private Grid _gameGrid;
        private List<string> shipNames = new List<string>() { "Destroyer", "Submarine", "Cruiser", "Battleship", "Carrier" };
        private static readonly Object lockObj = new object();
        #endregion
        #region Constructor
        public HuntAlgorithm(List<int> hitList, Grid gameGrid)
        {
            _hitList = hitList;
            _gameGrid = gameGrid;
        }
        #endregion
        #region Properties
        public List<int> HitList
        {
            get
            {
                lock (lockObj)
                { return _hitList; }
            }
            set
            {
                _hitList = value;
                OnPropertyChanged();
            }
        }
        public Grid GameGrid
        {
            get
            {
                lock (lockObj)
                {
                    return _gameGrid;
                }
            }
            set
            {
                _gameGrid = value;
                OnPropertyChanged();
            }
        }
        public int Coordinate
        {
            get
            {
                _hitList.Sort();
                #region Single count
                if (_hitList.Count == 1 || (_hitList[0] % 10 != _hitList[1] % 10 && _hitList[0] % 10 != (_hitList[1] % 10) + 1))
                {
                    int currentGrid = _hitList[0];
                    List<int> potentialHits = new List<int>();
                    Grid targetGridNorth = new Grid();
                    targetGridNorth.Tag = "";
                    Grid targetGridEast = new Grid();
                    targetGridEast.Tag = "";
                    Grid targetGridSouth = new Grid();
                    targetGridSouth.Tag = "";
                    Grid targetGridWest = new Grid();
                    targetGridWest.Tag = "";
                    try
                    {
                        targetGridNorth = _gameGrid.Children[currentGrid - 10] as Grid;
                    }
                    catch { }
                    try
                    {
                        targetGridEast = _gameGrid.Children[currentGrid + 1] as Grid;
                    }
                    catch { }
                    try
                    {
                        targetGridSouth = _gameGrid.Children[currentGrid + 10] as Grid;
                    }
                    catch { }
                    try
                    {
                        targetGridWest = _gameGrid.Children[currentGrid - 1] as Grid;
                    }
                    catch { }
                    if (targetGridNorth.Tag.ToString() == "Water" || shipNames.Contains(targetGridNorth.Tag.ToString()))
                    {
                        potentialHits.Add(1);
                    }
                    if ((targetGridEast.Tag.ToString() == "Water" || shipNames.Contains(targetGridEast.Tag.ToString())) && currentGrid + 1 % 10 != 0)
                    {
                        potentialHits.Add(2);
                    }
                    if (targetGridSouth.Tag.ToString() == "Water" || shipNames.Contains(targetGridSouth.Tag.ToString()))
                    {
                        potentialHits.Add(3);
                    }
                    if ((targetGridWest.Tag.ToString() == "Water" || shipNames.Contains(targetGridWest.Tag.ToString())) && currentGrid - 1 % 10 != 1)
                    {
                        potentialHits.Add(4);
                    }
                    if (potentialHits.Count != 0)
                    {
                        int direction = potentialHits[_random.Next(0, potentialHits.Count - 1)];
                        switch (direction)
                        {
                            case 1:
                                return _hitList[0] - 10;
                            case 2:
                                return _hitList[0] + 1;
                            case 3:
                                return _hitList[0] + 10;
                            case 4:
                                return _hitList[0] - 1;
                        }
                    }
                }
                #endregion
                #region Multiple count
                int hitListFirst = _hitList[0];
                int hitListSecond = _hitList[1];
                int hitListSecondToLast = _hitList[_hitList.Count - 2];
                int hitListLast = _hitList[_hitList.Count - 1];
                List<int> tryVertical = new List<int>();
                List<int> tryHorizontal = new List<int>();

                Grid gameGridChild = new Grid();
                gameGridChild.Tag = "";
                foreach (int val in _hitList)
                {
                    if (val % 10 == hitListFirst % 10)
                    {
                        tryVertical.Add(val);
                    }
                }
                int tryIndexDef = -2;
                for (int i = 0; i < _hitList.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        tryHorizontal.Add(_hitList[i]);
                    }
                    else
                    {
                        tryIndexDef = _hitList[i - 1] + 1;
                    }
                    if (_hitList[i] == tryIndexDef)
                    {
                        tryHorizontal.Add(_hitList[i]);
                    }
                    else
                    {
                        break;
                    }
                }
                if (tryHorizontal.Count >= tryVertical.Count)
                {
                    //East
                    try
                    {
                        if ((tryHorizontal[tryHorizontal.Count - 1] + 1) % 10 != 0)
                        {
                            gameGridChild = _gameGrid.Children[tryHorizontal[tryHorizontal.Count - 1] + 1] as Grid;
                        }
                    }
                    catch { }
                    if (tryHorizontal.Count > 1 && (gameGridChild.Tag.ToString() == "Water" || shipNames.Contains(gameGridChild.Tag.ToString())) && (hitListLast + 1) % 10 != 0)
                    {
                        return tryHorizontal[tryHorizontal.Count - 1] + 1;
                    }
                    //West
                    try
                    {
                        if ((tryHorizontal[0] - 1) % 10 != 1)
                        {
                            gameGridChild = _gameGrid.Children[tryHorizontal[0] - 1] as Grid;
                        }
                    }
                    catch { }
                    if (tryHorizontal.Count > 1 && (gameGridChild.Tag.ToString() == "Water" || shipNames.Contains(gameGridChild.Tag.ToString())) && (hitListFirst - 1) % 10 != 1)
                    {
                        return tryHorizontal[0] - 1;
                    }
                }
                //North
                try
                {
                    gameGridChild = _gameGrid.Children[tryVertical[0] - 10] as Grid;
                }
                catch { }
                if (tryVertical.Count > 1 && (gameGridChild.Tag.ToString() == "Water" || shipNames.Contains(gameGridChild.Tag.ToString())))
                {
                    return _hitList[0] - 10;
                }
                //South
                try
                {
                    gameGridChild = _gameGrid.Children[tryVertical[tryVertical.Count - 1] + 10] as Grid;
                }
                catch { }
                if (tryVertical.Count > 1 && (gameGridChild.Tag.ToString() == "Water" || shipNames.Contains(gameGridChild.Tag.ToString())))
                {
                    return _hitList[_hitList.Count - 1] + 10;
                }
                #endregion
                return -1;
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
