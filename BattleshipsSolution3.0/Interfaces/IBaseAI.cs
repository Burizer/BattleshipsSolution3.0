using BattleshipsSolution3._0.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BattleshipsSolution3._0.Interfaces
{
    public interface IBaseAI
    {
        int Coordinate { get; }
        List<int> HitList { get; set; }
        Grid GameGrid { get; set; }
    }
}
