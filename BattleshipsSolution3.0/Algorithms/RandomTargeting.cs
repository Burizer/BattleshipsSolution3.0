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
    class RandomTargeting : IBaseAI
    {
        #region Instance fields
        private Grid _gameGrid;
        private List<int> _hitList = new List<int>();
        private static Random _random = new Random();
        private HuntAlgorithm _hunt;
        private List<string> shipNames = new List<string>() { "Destroyer", "Submarine", "Cruiser", "Battleship", "Carrier" };
        #endregion
        #region Constructor
        public RandomTargeting()
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
                while (!viableHit)
                {
                    randomHit = _random.Next(0, 99);
                    Grid targetGrid = _gameGrid.Children[randomHit] as Grid;
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