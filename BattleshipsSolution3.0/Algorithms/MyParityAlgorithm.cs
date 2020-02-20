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
        private Random _random = new Random();
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

                else if (_counter < 100)
                {
                    bool viableHit = false;
                    string targetGrid = "";
                    while (!viableHit)
                    {
                        _counter += 2;
                        if (_counter % 20 < 11 && _counter % 20 != 0)
                        {
                            try
                            {
                                targetGrid = _gridDictionary[_counter - 1];
                            }
                            catch { }
                            if ((targetGrid == "Water" || shipNames.Contains(targetGrid)) && targetGrid != "Hit")
                            {
                                viableHit = true;
                            }
                        }
                        else
                        {
                            try
                            {
                                targetGrid = _gridDictionary[_counter - 2];
                            }
                            catch { }
                            if ((targetGrid == "Water" || shipNames.Contains(targetGrid)) && targetGrid != "Hit")
                            {
                                viableHit = true;
                            }
                        }
                    }

                    if (_counter % 20 < 11)
                    {
                        return _counter - 1;
                    }
                    else
                    {
                        return _counter - 2;
                    }
                }
                else
                {
                    bool viableHit = false;
                    int target = -1;
                    string targetGrid = "";
                    while (!viableHit)
                    {
                        target = _random.Next(0, 100);
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
        #region OnPropertyChanged code
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
