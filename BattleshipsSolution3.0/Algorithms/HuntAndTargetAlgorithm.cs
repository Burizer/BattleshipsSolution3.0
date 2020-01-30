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
        private Grid _gameGrid;
        private List<int> _hitList = new List<int>();
        private static Random _random;
        private HuntAlgorithm _hunt;
        public HuntAndTargetAlgorithm(Grid gameGrid)
        {
            _gameGrid = gameGrid;
            _hunt = new HuntAlgorithm(_hitList, gameGrid);
        }
        public int Coordinate
        {
            get
            {
                if (_hitList.Count != 0)
                {
                    return _hunt.Coordinate;
                }
                return _random.Next(0, 99);
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
        public List<int> HitList
        {
            get { return _hitList; }
            set
            {
                _hitList = value;
                OnPropertyChanged();
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
