﻿using System;
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
                if (_hitList.Count == 1)
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
                    if (targetGridEast == "Water" || shipNames.Contains(targetGridEast))
                    {
                        potentialHits.Add(2);
                    }
                    if (targetGridSouth == "Water" || shipNames.Contains(targetGridSouth))
                    {
                        potentialHits.Add(3);
                    }
                    if (targetGridWest == "Water" || shipNames.Contains(targetGridWest))
                    {
                        potentialHits.Add(4);
                    }
                    if (potentialHits.Count != 0)
                    {
                        int direction;
                        if (potentialHits.Count == 1)
                        {
                            direction = potentialHits[0];
                        }
                        else
                        {
                            direction = potentialHits[_random.Next(potentialHits.Count)];
                        }
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
                if (_hitList.Count >= 2)
                {
                    List<int> tryVertical = new List<int>();
                    List<int> tryHorizontal = new List<int>();
                    foreach (int val in _hitList)
                    {
                        if (val % 10 == _hitList[0] % 10)
                        {
                            tryVertical.Add(val);
                        }
                    }
                    if (tryVertical.Count == 1)
                    {
                        tryVertical.Clear();
                        foreach (int val in _hitList)
                        {
                            if (val % 10 == _hitList[_hitList.Count - 1])
                            {
                                tryVertical.Add(val);
                            }
                        }
                    }
                    for (int i = 0; i < _hitList.Count; i++)
                    {
                        if (i == 0)
                        {
                            tryHorizontal.Add(_hitList[i]);
                        }
                        else
                        {
                            if (_hitList[i] == _hitList[i - 1] + 1)
                            {
                                tryHorizontal.Add(_hitList[i]);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    string gameGridChildEast = "";
                    string gameGridChildWest = "";
                    string gameGridChildNorth = "";
                    string gameGridChildSouth = "";
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
                        if ((tryHorizontal[0] - 1) % 10 != 9)
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
                    if (tryHorizontal.Count >= 2)
                    {
                        if ((gameGridChildEast == "Water" || shipNames.Contains(gameGridChildEast)) || (gameGridChildWest == "Water" || shipNames.Contains(gameGridChildWest)))
                        {
                            //East

                            if ((gameGridChildEast == "Water" || shipNames.Contains(gameGridChildEast)) && ((_hitList[_hitList.Count - 1]) + 1) % 10 != 0)
                            {
                                return tryHorizontal[tryHorizontal.Count - 1] + 1;
                            }
                            //West

                            if ((gameGridChildWest == "Water" || shipNames.Contains(gameGridChildWest)) && (_hitList[0] - 1) % 10 != 9)
                            {
                                return tryHorizontal[0] - 1;
                            }
                        }
                    }
                    if (tryVertical.Count >= 2)
                    {
                        if ((gameGridChildNorth == "Water" || shipNames.Contains(gameGridChildNorth)) || (gameGridChildSouth == "Water" || shipNames.Contains(gameGridChildSouth)))
                        {
                            //North
                            if (gameGridChildNorth == "Water" || shipNames.Contains(gameGridChildNorth))
                            {
                                return _hitList[0] - 10;
                            }
                            //South

                            if (gameGridChildSouth == "Water" || shipNames.Contains(gameGridChildSouth))
                            {
                                return tryVertical[tryVertical.Count - 1] + 10;
                            }
                        }
                    }
                }
                foreach (int item in _hitList)
                {
                    return ValidateShot(item);
                }
                #endregion
                return -1;
            }

        }
        public int ValidateShot(int coordinate)
        {
            int returnVal = -1;
            foreach (var item in _hitList)
            {
                string potentialHitNorth = "";
                try
                {
                    potentialHitNorth = _gridDictionary[coordinate - 10];
                }
                catch
                {
                }
                if (potentialHitNorth == "Water" || shipNames.Contains(potentialHitNorth))
                {
                    returnVal = coordinate - 10;
                }
                string potentialHitEast = "";
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
                if (potentialHitEast == "Water" || shipNames.Contains(potentialHitEast))
                {
                    returnVal = coordinate + 1;
                }
                string potentialHitSouth = "";

                try
                {
                    potentialHitSouth = _gridDictionary[coordinate + 10];
                }
                catch
                {
                }
                if (potentialHitSouth == "Water" || shipNames.Contains(potentialHitSouth))
                {
                    returnVal = coordinate + 10;
                }
                string potentialHitWest = "";
                try
                {
                    if ((coordinate - 1) % 10 != 9)
                    {
                        potentialHitWest = _gridDictionary[coordinate - 1];
                    }
                }
                catch
                {
                }   
                if (potentialHitWest == "Water" || shipNames.Contains(potentialHitWest))
                {
                    returnVal = coordinate - 1;
                }
                if (returnVal != -1)
                {
                    return returnVal;
                }
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
