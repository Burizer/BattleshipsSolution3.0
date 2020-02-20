using BattleshipsSolution3._0.Algorithms.Helpers;
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
    public class HuntAndTargetAlgorithm : IBaseAI
    {
        #region Instance fields
        private List<int> _hitList = new List<int>();
        private Dictionary<int, string> _gridDictionary;
        private static Random _random = new Random();
        private HuntAlgorithm _hunt = new HuntAlgorithm(); 
        private List<string> shipNames = new List<string>() { "Destroyer", "Submarine", "Cruiser", "Battleship", "Carrier" };
        #endregion
        #region Constructor
        public HuntAndTargetAlgorithm()
        {
        }
        #endregion
        #region Properties
        public int Coordinate
        {
            get
            {
                bool viableHit = false;
                int randomHit = -1;

                if (_hitList.Count != 0)
                {
                    _hunt.GridDictionary = _gridDictionary;
                    _hunt.HitList = _hitList;
                    return _hunt.Coordinate;
                }
                while (!viableHit)
                {
                    randomHit = _random.Next(0, 100);

                    if (_gridDictionary[randomHit] == "Water" || shipNames.Contains(_gridDictionary[randomHit]))
                    {
                        viableHit = true;
                    }
                }
                return randomHit;
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
