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
        private Random _random = new Random();
        private List<int> _hitList;
        private Grid _gameGrid;
        private List<string> shipNames = new List<string>() { "Destroyer", "Submarine", "Cruiser", "Battleship", "Carrier" };
        private static readonly Object lockObj = new object();
        public HuntAlgorithm(List<int> hitList, Grid gameGrid)
        {
            _hitList = hitList;
            _gameGrid = gameGrid;
        }
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
                if (_hitList.Count == 1 || (_hitList[0] % 10 != _hitList[1] % 10 && _hitList[0] % 10 != (_hitList[1] % 10) - 1 && _hitList[0] % 10 != (_hitList[1] % 10) + 1))
                {
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
                        targetGridNorth = _gameGrid.Children[_hitList[0] - 10] as Grid;
                    }
                    catch { }
                    try
                    {
                        targetGridEast = _gameGrid.Children[_hitList[0] + 1] as Grid;
                    }
                    catch { }
                    try
                    {
                        targetGridSouth = _gameGrid.Children[_hitList[0] + 10] as Grid;
                    }
                    catch { }
                    try
                    {
                        targetGridWest = _gameGrid.Children[_hitList[0] - 1] as Grid;
                    }
                    catch { }
                    if (targetGridNorth.Tag.ToString() == "Water" || shipNames.Contains(targetGridNorth.Tag.ToString()))
                    {
                        potentialHits.Add(1);
                    }
                    if (targetGridEast.Tag.ToString() == "Water" || shipNames.Contains(targetGridEast.Tag.ToString()))
                    {
                        potentialHits.Add(2);
                    }
                    if (targetGridSouth.Tag.ToString() == "Water" || shipNames.Contains(targetGridSouth.Tag.ToString()))
                    {
                        potentialHits.Add(3);
                    }
                    if (targetGridWest.Tag.ToString() == "Water" || shipNames.Contains(targetGridWest.Tag.ToString()))
                    {
                        potentialHits.Add(4);
                    }
                    if (potentialHits.Count != 0)
                    {
                        int direction = potentialHits[_random.Next(potentialHits.Count - 1)];
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
                #region Multiple count, no pattern
                bool isVertical = false;
                try
                {
                    isVertical = (_hitList[0] % 10 == _hitList[1] % 10);
                }
                catch { }
                if (isVertical)
                {
                    Grid targetGridNorth = new Grid();
                    Grid targetGridSouth = new Grid();
                    targetGridNorth.Tag = "";
                    targetGridSouth.Tag = "";
                    try
                    {
                        targetGridNorth = _gameGrid.Children[_hitList[0] + 10] as Grid;
                    }
                    catch { }
                    try
                    {
                        targetGridSouth = _gameGrid.Children[_hitList[_hitList.Count - 1] - 10] as Grid;
                    }
                    catch { }
                    if ((targetGridNorth.Tag.ToString() != "Water" || !shipNames.Contains(targetGridNorth.Tag.ToString())) && (targetGridSouth.Tag.ToString() != "Water" || !shipNames.Contains(targetGridSouth.Tag.ToString())))
                    {
                        foreach (var item in _hitList)
                        {
                            Grid targetGridEast = new Grid();
                            Grid targetGridWest = new Grid();
                            targetGridEast.Tag = "";
                            targetGridWest.Tag = "";
                            try
                            {
                                targetGridEast = _gameGrid.Children[item + 1] as Grid;
                            }
                            catch { }
                            try
                            {
                                targetGridWest = _gameGrid.Children[item - 1] as Grid;
                            }
                            catch { }
                            if (targetGridEast.Tag.ToString() == "Water" || shipNames.Contains(targetGridEast.Tag.ToString()))
                            {
                                return item + 1;
                            }
                            else if (targetGridWest.Tag.ToString() == "Water" || shipNames.Contains(targetGridWest.Tag.ToString()))
                            {
                                return item - 1;
                            }
                        }
                    }
                }
                else
                {
                    Grid targetGridEast = new Grid();
                    Grid targetGridWest = new Grid();
                    targetGridEast.Tag = "";
                    targetGridWest.Tag = "";
                    try
                    {
                        targetGridEast = _gameGrid.Children[_hitList[_hitList.Count - 1] + 1] as Grid;
                    }
                    catch { }
                    try
                    {
                        targetGridWest = _gameGrid.Children[_hitList[0] - 1] as Grid;
                    }
                    catch { }
                    if ((targetGridWest.Tag.ToString() != "Water" || shipNames.Contains(targetGridWest.Tag.ToString())) && (targetGridEast.Tag.ToString() != "Water" || !shipNames.Contains(targetGridEast.Tag.ToString())))
                    {
                        foreach (var item in _hitList)
                        {
                            Grid targetGridNorth = new Grid();
                            Grid targetGridSouth = new Grid();
                            targetGridNorth.Tag = "";
                            targetGridSouth.Tag = "";
                            try
                            {
                                targetGridNorth = _gameGrid.Children[item + 10] as Grid;
                            }
                            catch { }

                            try
                            {
                                targetGridSouth = _gameGrid.Children[item - 10] as Grid;
                            }
                            catch { }
                            if (targetGridNorth.Tag.ToString() == "Water" || shipNames.Contains(targetGridNorth.Tag.ToString()))
                            {
                                return item + 10;
                            }
                            if (targetGridSouth.Tag.ToString() == "Water" || shipNames.Contains(targetGridSouth.Tag.ToString()))
                            {
                                return item - 10;
                            }
                        }
                    }
                }
                #endregion
                #region Multiple count, pattern
                int hitListFirst = _hitList[0];
                int hitListSecond = _hitList[1];
                int hitListLast = _hitList[_hitList.Count - 1];
                int hitListSecondToLast = -1;
                Grid gameGridChild = new Grid();
                gameGridChild.Tag = "";
                //North
                try
                {
                    gameGridChild = _gameGrid.Children[_hitList[0] - 10] as Grid;
                }
                catch { }
                if (hitListFirst == hitListSecond - 10 && (gameGridChild.Tag.ToString() == "Water" || shipNames.Contains(gameGridChild.Tag.ToString())))
                {
                    return _hitList[0] - 10;
                }
                //East
                try
                {
                    hitListSecondToLast = _hitList[_hitList.Count - 2] + 1;
                }
                catch { }
                try
                {
                    gameGridChild = _gameGrid.Children[(_hitList.Count - 1) + 1] as Grid;
                }
                catch { }
                if (hitListLast == hitListSecondToLast && (gameGridChild.Tag.ToString() == "Water" || shipNames.Contains(gameGridChild.Tag.ToString())))
                {
                    return _hitList[_hitList.Count - 1] + 1;
                }
                //South
                try
                {
                    hitListSecondToLast = _hitList[_hitList.Count - 2] + 10;
                }
                catch { }
                try
                {
                    gameGridChild = _gameGrid.Children[(_hitList.Count - 1) + 10] as Grid;
                }
                catch { }
                if (hitListLast == hitListSecondToLast && (gameGridChild.Tag.ToString() == "Water" || shipNames.Contains(gameGridChild.Tag.ToString())))
                {
                    return _hitList[_hitList.Count - 1] + 10;
                }
                //West
                try
                {
                    gameGridChild = _gameGrid.Children[_hitList[0] - 1] as Grid;
                }
                catch { }
                if (hitListFirst == hitListSecond - 1 && (gameGridChild.Tag.ToString() == "Water" || shipNames.Contains(gameGridChild.Tag.ToString())))
                {
                    return _hitList[0] - 1;
                }
                #endregion
                return -1;
            }
        }
        #region OnPropertyChanged code
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
