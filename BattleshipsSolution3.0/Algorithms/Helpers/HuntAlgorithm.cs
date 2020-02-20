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
        private Dictionary<int, string> _gridDictionary;
        private List<string> shipNames = new List<string>() { "Destroyer", "Submarine", "Cruiser", "Battleship", "Carrier" };
        #endregion
        #region Constructor
        public HuntAlgorithm(List<int> hitList, Dictionary<int, string> gridDictionary)
        {
            _hitList = hitList;
            _gridDictionary = gridDictionary;
        }
        public HuntAlgorithm()
        {

        }
        #endregion
        #region Properties
        public List<int> HitList
        {
            get { return _hitList; }
            set
            {
                _hitList = value;
                OnPropertyChanged();
            }
        }
        public Dictionary<int, string> GridDictionary
        {
            get { return _gridDictionary; }
            set
            {
                _gridDictionary = value;
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
                    string targetGridNorth = "";
                    string targetGridEast = "";
                    string targetGridSouth = "";
                    string targetGridWest = "";
                    try
                    {
                        targetGridNorth = _gridDictionary[currentGrid - 10];
                    }
                    catch { }
                    try
                    {
                        targetGridEast = _gridDictionary[currentGrid + 1];
                    }
                    catch { }
                    try
                    {
                        targetGridSouth = _gridDictionary[currentGrid + 10];
                    }
                    catch { }
                    try
                    {
                        targetGridWest = _gridDictionary[currentGrid - 1];
                    }
                    catch { }
                    if (targetGridNorth == "Water" || shipNames.Contains(targetGridNorth))
                    {
                        potentialHits.Add(1);
                    }
                    if ((targetGridEast == "Water" || shipNames.Contains(targetGridEast)) && currentGrid + 1 % 10 != 0)
                    {
                        potentialHits.Add(2);
                    }
                    if (targetGridSouth == "Water" || shipNames.Contains(targetGridSouth))
                    {
                        potentialHits.Add(3);
                    }
                    if ((targetGridWest == "Water" || shipNames.Contains(targetGridWest)) && currentGrid - 1 % 10 != 1)
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
                string gameGridChildEast = "";
                string gameGridChildWest = "";
                string gameGridChildNorth = "";
                string gameGridChildSouth = "";
                //Grid gameGridChildEast = new Grid();
                //gameGridChildEast.Tag = "";
                //Grid gameGridChildWest = new Grid();
                //gameGridChildWest.Tag = "";
                //Grid gameGridChildNorth = new Grid();
                //gameGridChildNorth.Tag = "";
                //Grid gameGridChildSouth = new Grid();
                //gameGridChildSouth.Tag = "";
                try
                {
                    if ((tryHorizontal[tryHorizontal.Count - 1] + 1) % 10 != 0)
                    {
                        gameGridChildEast = _gridDictionary[tryHorizontal[tryHorizontal.Count - 1] + 1];
                    }
                }
                catch
                {
                }
                try
                {
                    if ((tryHorizontal[0] - 1) % 10 != 1)
                    {
                        gameGridChildWest = _gridDictionary[tryHorizontal[0] - 1];
                    }
                }
                catch
                {
                }
                try
                {
                    gameGridChildNorth = _gridDictionary[tryVertical[0] - 10];
                }
                catch
                {
                }
                try
                {
                    gameGridChildSouth = _gridDictionary[tryVertical[tryVertical.Count - 1] + 10];
                }
                catch
                {
                }
                if ((gameGridChildNorth == "Water" || shipNames.Contains(gameGridChildNorth.ToString())) || (gameGridChildEast == "Water" || shipNames.Contains(gameGridChildEast.ToString())) || (gameGridChildSouth == "Water" || shipNames.Contains(gameGridChildSouth)) || (gameGridChildWest == "Water" || shipNames.Contains(gameGridChildWest)))
                {
                    //East

                    if (tryHorizontal.Count > 1 && (gameGridChildEast == "Water" || shipNames.Contains(gameGridChildEast)) && ((_hitList[_hitList.Count] - 1) + 1) % 10 != 0)
                    {
                        return tryHorizontal[tryHorizontal.Count - 1] + 1;
                    }
                    //West

                    if (tryHorizontal.Count > 1 && (gameGridChildWest == "Water" || shipNames.Contains(gameGridChildWest)) && (_hitList[0] - 1) % 10 != 1)
                    {
                        return tryHorizontal[0] - 1;
                    }
                    //North
                    if (tryVertical.Count > 1 && (gameGridChildNorth == "Water" || shipNames.Contains(gameGridChildNorth)))
                    {
                        return _hitList[0] - 10;
                    }
                    //South

                    if (tryVertical.Count > 1 && (gameGridChildSouth == "Water" || shipNames.Contains(gameGridChildSouth)))
                    {
                        return tryVertical[tryVertical.Count - 1] + 10;
                    }
                }
                else
                {
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
                return -1;
            }
        }
        public int ValidateShot(int coordinate)
        {
            string potentialHitNorth = "";
            string potentialHitEast = "";
            string potentialHitSouth = "";
            string potentialHitWest = "";
            try
            {
                potentialHitNorth = _gridDictionary[coordinate - 10];
            }
            catch
            {
            }
            try
            {
                if ((coordinate + 1) % 10 != 0)
                {
                    potentialHitEast = _gridDictionary[coordinate + 1];
                }
            }
            catch
            {
            }
            try
            {
                potentialHitSouth = _gridDictionary[coordinate + 10];
            }
            catch
            {
            }
            try
            {
                if ((coordinate - 1) % 10 != 1)
                {
                    potentialHitWest = _gridDictionary[coordinate - 1];
                }
            }
            catch
            {
            }
            if (potentialHitNorth == "Water" || shipNames.Contains(potentialHitNorth))
            {
                return coordinate - 10;
            }
            if (potentialHitEast == "Water" || shipNames.Contains(potentialHitEast))
            {
                return coordinate + 1;
            }
            if (potentialHitSouth == "Water" || shipNames.Contains(potentialHitSouth))
            {
                return coordinate + 10;
            }
            if (potentialHitWest == "Water" || shipNames.Contains(potentialHitWest))
            {
                return coordinate - 1;
            }
            return -1;
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
