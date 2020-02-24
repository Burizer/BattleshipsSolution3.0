using BattleshipsSolution3._0.Algorithms.Helpers;
using BattleshipsSolution3._0.Classes;
using BattleshipsSolution3._0.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BattleshipsSolution3._0.Algorithms
{
    public class MyParityAlgorithm : IBaseAI
    {
        #region Instance fields
        private int _counter = 0;
        private List<int> _hitList = new List<int>();
        private Dictionary<int, string> _gridDictionary;
        private HuntAlgorithm _hunt = new HuntAlgorithm();
        private List<string> shipNames = new List<string>() { "Destroyer", "Submarine", "Cruiser", "Battleship", "Carrier" };
        #endregion
        #region Constructor
        public MyParityAlgorithm()
        {

        }
        #endregion
        #region Properties
        public Dictionary<int, string> GridDictionary
        {
            get { return _gridDictionary; }
            set
            {
                _gridDictionary = value;
                OnPropertyChanged();
            }
        }
        public HuntAlgorithm Hunt
        {
            get { return _hunt; }
            set
            {
                _hunt = value;
                OnPropertyChanged();
            }
        }
        public int Counter
        {
            get { return _counter; }
            set
            {
                _counter = value;
                OnPropertyChanged();
            }
        }
        public int Coordinate
        {
            get
            {
                if (_hitList.Count != 0)
                {
                    _hunt.GridDictionary = _gridDictionary;
                    _hunt.HitList = _hitList;
                    return _hunt.Coordinate;
                }

                else
                {
                    string targetGrid = "";
                    while (_counter < 100)
                    {
                        _counter += 2;
                        if (_counter % 20 < 11 && _counter % 20 != 0)
                        {
                            targetGrid = _gridDictionary[_counter - 1];
                            if (targetGrid == "Water" || shipNames.Contains(targetGrid))
                            {
                                return _counter - 1;
                            }
                        }
                        else
                        {
                            targetGrid = _gridDictionary[_counter - 2];
                            if (targetGrid == "Water" || shipNames.Contains(targetGrid))
                            {
                                return _counter - 2;
                            }
                        }
                    }
                    bool viableHit = false;
                    int target = -1;
                    targetGrid = "";
                    while (!viableHit)
                    {
                        target = StaticRandom.Rand(100);
                        try
                        {
                            targetGrid = _gridDictionary[target];
                        }
                        catch
                        {

                        }
                        if ((targetGrid == "Water" || shipNames.Contains(targetGrid)) && targetGrid != "Hit")
                        {
                            viableHit = true;
                        }
                    }
                    return target;
                }
            }
        }
        public List<int> HitList
        {
            get
            {
                return _hitList;
            }
            set
            {
                _hitList = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public void GameFinished()
        {
            _counter = 0;
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
