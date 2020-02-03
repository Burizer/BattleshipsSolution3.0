﻿using BattleshipsSolution3._0.Algorithms.Helpers;
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
    class MyParityAlgorithm : IBaseAI
    {
        private int _counter = 0;
        private Grid _gameGrid;
        private List<int> _hitList = new List<int>();
        private HuntAlgorithm _hunt;
        private List<string> shipNames = new List<string>() { "Destroyer", "Submarine", "Cruiser", "Battleship", "Carrier" };
        private static readonly Object lockObj = new object();
        public MyParityAlgorithm(Grid gameGrid)
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
                    lock (lockObj)
                    {
                        _hunt.GameGrid = _gameGrid;
                        _hunt.HitList = _hitList;
                    }
                    return _hunt.Coordinate;
                }
                else
                {
                    bool viableHit = false;
                    while (!viableHit)
                    {
                        _counter += 2;
                        Grid targetGrid = _gameGrid.Children[_counter] as Grid;
                        if (targetGrid.Tag.ToString() == "Water" || shipNames.Contains(targetGrid.Tag.ToString()))
                        {
                            viableHit = true;
                        }
                    }

                    if (_counter % 20 < 11 || _counter % 20 == 0)
                    {
                        return _counter - 1;
                    }
                    else
                    {
                        return _counter - 2;
                    }
                }
            }
        }

        public Grid GameGrid
        {
            get
            {
                lock (lockObj)
                { return _gameGrid; }
            }
            set
            {
                _gameGrid = value;
                OnPropertyChanged();
            }
        }
        public List<int> HitList
        {
            get
            {
                lock (lockObj)
                {
                    return _hitList;
                }
            }
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
