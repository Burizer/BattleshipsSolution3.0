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
                if (_hitList.Count == 1 || (_hitList[0] % 10 != _hitList[1] % 10 && (_hitList[0] % 10 != (_hitList[1] % 10) + 1) && _hitList[0] + 10 != _hitList[1] + 1))
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
                List<int> tryVertical = new List<int>();
                List<int> tryHorizontal = new List<int>();
                foreach (int val in _hitList)
                {
                    if (val % 10 == _hitList[0] % 10)
                    {
                        tryVertical.Add(val);
                    }
                }
                if (tryVertical.Count <= 1)
                {
                    foreach (int val in _hitList)
                    {
                        if (val % 10 == _hitList[_hitList.Count - 1])
                        {
                            tryVertical.Add(val);
                        }
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
                Grid gameGridChildEast = new Grid();
                gameGridChildEast.Tag = "";
                Grid gameGridChildWest = new Grid();
                gameGridChildWest.Tag = "";
                Grid gameGridChildNorth = new Grid();
                gameGridChildNorth.Tag = "";
                Grid gameGridChildSouth = new Grid();
                gameGridChildSouth.Tag = "";
                try
                {
                    if ((tryHorizontal[tryHorizontal.Count - 1] + 1) % 10 != 0)
                    {
                        gameGridChildEast = _gameGrid.Children[tryHorizontal[tryHorizontal.Count - 1] + 1] as Grid;
                    }
                }
                catch
                {
                }
                try
                {
                    if ((tryHorizontal[0] - 1) % 10 != 1)
                    {
                        gameGridChildWest = _gameGrid.Children[tryHorizontal[0] - 1] as Grid;
                    }
                }
                catch
                {
                }
                try
                {
                    gameGridChildNorth = _gameGrid.Children[tryVertical[0] - 10] as Grid;
                }
                catch
                {
                }
                try
                {
                    gameGridChildSouth = _gameGrid.Children[tryVertical[tryVertical.Count - 1] + 10] as Grid;
                }
                catch
                {
                }
                if ((gameGridChildNorth.Tag.ToString() == "Water" || shipNames.Contains(gameGridChildNorth.ToString())) || (gameGridChildEast.Tag.ToString() == "Water" || shipNames.Contains(gameGridChildEast.ToString())) || (gameGridChildSouth.Tag.ToString() == "Water" || shipNames.Contains(gameGridChildSouth.ToString())) || (gameGridChildWest.Tag.ToString() == "Water" || shipNames.Contains(gameGridChildWest.ToString())))
                {
                    //East

                    if (tryHorizontal.Count > 1 && (gameGridChildEast.Tag.ToString() == "Water" || shipNames.Contains(gameGridChildEast.Tag.ToString())) && ((_hitList[_hitList.Count] - 1) + 1) % 10 != 0)
                    {
                        return tryHorizontal[tryHorizontal.Count - 1] + 1;
                    }
                    //West

                    if (tryHorizontal.Count > 1 && (gameGridChildWest.Tag.ToString() == "Water" || shipNames.Contains(gameGridChildWest.Tag.ToString())) && (_hitList[0] - 1) % 10 != 1)
                    {
                        return tryHorizontal[0] - 1;
                    }
                    //North
                    if (tryVertical.Count > 1 && (gameGridChildNorth.Tag.ToString() == "Water" || shipNames.Contains(gameGridChildNorth.Tag.ToString())))
                    {
                        return _hitList[0] - 10;
                    }
                    //South

                    if (tryVertical.Count > 1 && (gameGridChildSouth.Tag.ToString() == "Water" || shipNames.Contains(gameGridChildSouth.Tag.ToString())))
                    {
                        return tryVertical[tryVertical.Count - 1] + 10;
                    }
                }
                foreach (int item in _hitList)
                {
                    int shot = -1;
                    shot = ValidateShot(item);
                    if (shot != -1)
                    {
                        return shot;
                    }
                }
                #endregion
                return -1;
            }
        }
        public int ValidateShot(int coordinate)
        {
            Grid potentialHitNorth = new Grid();
            potentialHitNorth.Tag = "";
            Grid potentialHitEast = new Grid();
            potentialHitEast.Tag = "";
            Grid potentialHitSouth = new Grid();
            potentialHitSouth.Tag = "";
            Grid potentialHitWest = new Grid();
            potentialHitWest.Tag = "";
            try
            {
                potentialHitNorth = _gameGrid.Children[coordinate - 10] as Grid;
            }
            catch
            {
            }
            try
            {
                if ((coordinate + 1) % 10 != 0)
                {
                    potentialHitEast = _gameGrid.Children[coordinate + 1] as Grid;
                }
            }
            catch
            {
            }
            try
            {
                potentialHitSouth = _gameGrid.Children[coordinate + 10] as Grid;
            }
            catch
            {
            }
            try
            {
                if ((coordinate - 1) % 10 != 1)
                {
                    potentialHitWest = _gameGrid.Children[coordinate - 1] as Grid;
                }
            }
            catch
            {
            }
            if (potentialHitNorth.Tag.ToString() == "Water" || shipNames.Contains(potentialHitNorth.Tag.ToString()))
            {
                return coordinate - 10;
            }
            if (potentialHitEast.Tag.ToString() == "Water" || shipNames.Contains(potentialHitEast.Tag.ToString()))
            {
                return coordinate + 1;
            }
            if (potentialHitSouth.Tag.ToString() == "Water" || shipNames.Contains(potentialHitSouth.Tag.ToString()))
            {
                return coordinate + 10;
            }
            if (potentialHitWest.Tag.ToString() == "Water" || shipNames.Contains(potentialHitWest.Tag.ToString()))
            {
                return coordinate - 1;
            }
            int randShot = 0;
            bool randShotViable = false;
            while (!randShotViable)
            {
                Grid targetGrid = new Grid();
                targetGrid.Tag = "";
                randShot = _random.Next(0, 99);
                try
                {
                    targetGrid = _gameGrid.Children[randShot] as Grid;
                }
                catch { }
                if (targetGrid.Tag.ToString() == "Water" || shipNames.Contains(targetGrid.Tag.ToString()))
                {
                    randShotViable = true;
                }
            }
            return randShot;
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
