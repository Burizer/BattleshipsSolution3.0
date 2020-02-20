using BattleshipsSolution3._0.Algorithms;
using BattleshipsSolution3._0.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsSolution3._0.MVVM_tools.GameStart
{
    public class AiContainer
    {
        private IBaseAI _baseAI;
        private ObservableCollection<IBaseAI> _aiList = new ObservableCollection<IBaseAI>() { new HuntAndTargetAlgorithm(), new MyParityAlgorithm(), new RandomTargeting() };
        public AiContainer()
        {
        }
        public IBaseAI BaseAI
        {
            get { return _baseAI; }
            set
            {
                _baseAI = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<IBaseAI> AIList
        {
            get { return _aiList; }
            set
            {
                _aiList = value;
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
