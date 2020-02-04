using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsSolution3._0.Classes
{
    public class Shiplist
    {
        private List<Ship> _ships;
        public   string _shipstatus;
        public Shiplist()
        {
            PopulateShiplist();
        }
        public Shiplist(List<Ship> shipList)
        {
            _ships = shipList;
        }
        public void PopulateShiplist()
        {
            List<Ship> ships = new List<Ship>() { new Ship("Destroyer", 2), new Ship("Submarine", 3), new Ship("Cruiser", 3), new Ship("Battleship", 4), new Ship("Carrier", 5) };
            _ships = ships;
        }
        public List<Ship> Ships
        {
            get { return _ships; }
            set { _ships = value; }
        }
        public bool GameWon
        {
            get
            {
                foreach (Ship ship in _ships)
                {
                    if (!ship.IsSunk)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public string CheckShipStatus
        {
            get
            {
                foreach (Ship ship in _ships)
                {
                    if (ship.IsSunk)
                    {
                        _shipstatus += ship.Name + " has been sunk." + Environment.NewLine;
                    }
                    else
                    {
                        _shipstatus += ship.Name + " has not been sunk." + Environment.NewLine;
                    }
                }
                return _shipstatus;
            }
        }
        
    }
}
