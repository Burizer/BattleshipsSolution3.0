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
        public HuntAlgorithm(List<int> hitList, Grid gameGrid)
        {
            _hitList = hitList;
            _gameGrid = gameGrid;
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
        public Grid GameGrid
        {
            get { return _gameGrid; }
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
                if (_hitList.Count == 1)
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
                    if (targetGridNorth.Tag.ToString() == "Water")
                    {
                        potentialHits.Add(1);
                    }
                    if (targetGridEast.Tag.ToString() == "Water")
                    {
                        potentialHits.Add(2);
                    }
                    if (targetGridSouth.Tag.ToString() == "Water")
                    {
                        potentialHits.Add(3);
                    }
                    if (targetGridWest.Tag.ToString() == "Water")
                    {
                        potentialHits.Add(4);
                    }
                    if (potentialHits.Count != 0)
                    {
                        int direction = potentialHits[_random.Next(potentialHits.Count)];
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
                bool isHorizontal = false;
                try
                {
                    isHorizontal = (_hitList[0] % 10 == _hitList[1] % 10);
                }
                catch { }
                if (isHorizontal)
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
                    if (targetGridNorth.Tag.ToString() != "Water" && targetGridSouth.Tag.ToString() != "Water")
                    {
                        foreach (var item in _hitList)
                        {
                            var targetGridEast = _gameGrid.Children[item + 1] as Grid;
                            var targetGridWest = _gameGrid.Children[item - 1] as Grid;
                            if (targetGridEast.Tag.ToString() == "Water")
                            {
                                return item + 1;
                            }
                            else if (targetGridWest.Tag.ToString() == "Water")
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
                    if (targetGridWest.Tag.ToString() != "Water" && targetGridEast.Tag.ToString() != "Water")
                    {
                        foreach (var item in _hitList)
                        {
                            Grid targetGridNorth = new Grid();
                            Grid targetGridSouth = new Grid();
                            targetGridNorth.Tag = "";
                            targetGridSouth.Tag = "";
                            try { 
                            targetGridNorth = _gameGrid.Children[item + 10] as Grid;
                            }
                            catch { }
                            
                            try { 
                            targetGridSouth = _gameGrid.Children[item - 10] as Grid;
                            }
                            catch { }
                            if (targetGridNorth.Tag.ToString() == "Water")
                            {
                                return item + 10;
                            }
                            if (targetGridSouth.Tag.ToString() == "Water")
                            {
                                return item - 10;
                            }
                        }
                    }
                }
                if (_hitList[_hitList.Count - 1] == _hitList[_hitList.Count - 2] - 10 && _gameGrid.Children[(_hitList.Count - 1) - 10] != null)
                {
                    return _hitList[_hitList.Count - 1] - 10;
                }
                if (_hitList[_hitList.Count - 1] == _hitList[_hitList.Count - 2] + 1 && _gameGrid.Children[(_hitList.Count - 1) + 1] != null)
                {
                    return _hitList[_hitList.Count - 1] + 1;
                }
                if (_hitList[_hitList.Count - 1] == _hitList[_hitList.Count - 2] + 10 && _gameGrid.Children[(_hitList.Count - 1) + 10] != null)
                {
                    return _hitList[_hitList.Count - 1] + 10;
                }
                if (_hitList[_hitList.Count - 1] == _hitList[_hitList.Count - 2] - 1 && _gameGrid.Children[(_hitList.Count - 1) - 1] != null)
                {
                    return _hitList[_hitList.Count - 1] - 1;
                }
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
