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
        private Grid _gameGrid;
        private List<int> _hitList = new List<int>();
        private HuntAlgorithm _hunt;
        private Random _random = new Random();
        private List<string> shipNames = new List<string>() { "Destroyer", "Submarine", "Cruiser", "Battleship", "Carrier" };
        #endregion
        #region Constructor
        public MyParityAlgorithm()
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

                else if (_counter < 100)
                {
                    bool viableHit = false;
                    Grid targetGrid = new Grid();
                    targetGrid.Tag = "";
                    while (!viableHit)
                    {
                        _counter += 2;
                        if (_counter % 20 < 11)
                        {
                            try
                            {
                                targetGrid = _gameGrid.Children[_counter - 1] as Grid;
                            }
                            catch { }
                            if ((targetGrid.Tag.ToString() == "Water" || shipNames.Contains(targetGrid.Tag.ToString())) && targetGrid.Tag.ToString() != "Hit")
                            {
                                viableHit = true;
                            }
                        }
                        else
                        {
                            try
                            {
                                targetGrid = _gameGrid.Children[_counter - 2] as Grid;
                            }
                            catch { }
                            if ((targetGrid.Tag.ToString() == "Water" || shipNames.Contains(targetGrid.Tag.ToString())) && targetGrid.Tag.ToString() != "Hit")
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
                    Grid targetGrid = new Grid();
                    targetGrid.Tag = "";
                    while (!viableHit)
                    {
                        target = _random.Next(0, 99);
                        try
                        {
                            targetGrid = _gameGrid.Children[target] as Grid;
                        }
                        catch
                        {

                        }
                        if ((targetGrid.Tag.ToString() == "Water" || shipNames.Contains(targetGrid.Tag.ToString())) && targetGrid.Tag.ToString() != "Hit")
                        {
                            viableHit = true;
                        }
                    }
                    return target;
                }
            }
        }

        public Grid GameGrid
        {
            get
            {
                return _gameGrid;
            }
            set
            {
                _gameGrid = value;
                OnPropertyChanged();
                _hunt = new HuntAlgorithm(_hitList, value);
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
