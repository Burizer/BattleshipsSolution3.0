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
        private Grid _gameGrid;
        private List<int> _hitList = new List<int>();
        private static Random _random = new Random();
        private HuntAlgorithm _hunt;
        private int randomHit = -1;
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
                if (_hitList.Count != 0)
                {
                    _hunt.GameGrid = _gameGrid;
                    _hunt.HitList = _hitList;
                    return _hunt.Coordinate;
                }
                randomHit = 0;
                Grid targetGrid = new Grid();
                targetGrid.Tag = "";
                bool viableHit = false;
                while (!viableHit)
                {
                    randomHit = _random.Next(0, 100);
                    targetGrid = _gameGrid.Children[randomHit] as Grid;
                    if (targetGrid.Tag.ToString() == "Water" || shipNames.Contains(targetGrid.Tag.ToString()))
                    {
                        viableHit = true;
                    }
                }
                return randomHit;
            }
        }

        public Grid GameGrid
        {
            get { return _gameGrid; }
            set
            {
                _gameGrid = value;
                OnPropertyChanged();
                _hunt = new HuntAlgorithm(_hitList, value);
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
