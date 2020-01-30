using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsSolution3._0.Classes
{
    public class Ship
    {
        private string _name;
        private int _length;
        private int _hits;
        public Ship(string name, int stats)
        {
            _name = name;
            _length = stats;
            _hits = stats;
        }
        public string Name
        {
            get { return _name; }
        }
        public int Length
        {
            get { return _length; }
        }
        public int Hits
        {
            get { return _hits; }
            set { _hits = value;
                OnPropertyChanged();
            }
        }
        public void HitRegistered()
        {
            _hits--;
        }
        public bool IsSunk
        {
            get { return _hits <= 0; }
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
