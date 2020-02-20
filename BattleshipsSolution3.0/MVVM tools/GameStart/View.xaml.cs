using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BattleshipsSolution3._0.MVVM_tools.GameStart
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class View : Page
    {
        private Grid _affixGrid;
        private int _numberOfGrids;
        private List<string> _labelFillers = new List<string>() { "Algoritme: ", "Gennemsnit: ", "Minimum: ", "Maximum: ", "Total tid: ", "Tid per spil: " };
        public View()
        {
            _affixGrid = AffixingGrid;
            InitializeComponent();
        }
        public Grid AffixGrid
        {
            get { return _affixGrid; }
            set
            {
                _affixGrid = value;
                OnPropertyChanged();
            }
        }
        public int NumberOfGrids
        {
            get { return _numberOfGrids; }
            set
            {
                _numberOfGrids = value;
                OnPropertyChanged();
            }
        }

        private void InitiateButton_Click(object sender, RoutedEventArgs e)
        {
            NumberOfGrids = Convert.ToInt32(InitiationDropdown.SelectedValue.ToString());
            for (int i = 0; i < NumberOfGrids; i++)
            {
                GameSetupGrids.RowDefinitions.Add(new RowDefinition());
                Grid setupGrid = new Grid();
                setupGrid.SetValue(Grid.RowProperty, i);
                for (int j = 0; j < 4; j++)
                {
                    RowDefinition setupRowDef = new RowDefinition();
                    setupGrid.RowDefinitions.Add(setupRowDef);
                }

                Label TextLabel1 = new Label();
                TextLabel1.Content = "Vælg algoritme.";
                TextLabel1.SetValue(Grid.RowProperty, 0);
                ComboBox Combo = new ComboBox();
                Combo.SetValue(Grid.RowProperty, 1);
                Label TextLabel2 = new Label();
                TextLabel2.Content = "Vælg antal gennemkørsler.";
                TextLabel2.SetValue(Grid.RowProperty, 2);
                TextBox Text = new TextBox();
                Text.SetValue(Grid.RowProperty, 3);
                setupGrid.Children.Add(TextLabel1);
                setupGrid.Children.Add(Combo);
                setupGrid.Children.Add(Text);
                GameSetupGrids.Children.Add(setupGrid);
            }
            FinishSetup.SetValue(Grid.RowProperty, GameSetupGrids.RowDefinitions.Count - 1);
            GameStartGrid.Visibility = Visibility.Hidden;
            GameSetupGrids.Visibility = Visibility.Visible;
        }
        private void FinishSetup_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < NumberOfGrids; i++)
            {
                GameAndBoardGrids.RowDefinitions.Add(new RowDefinition());
                Grid GameGrid = new Grid();
                GameGrid.SetValue(Grid.RowProperty, i);
                GameGrid.SetValue(Grid.ColumnProperty, 0);
                Thickness gameGridMargin = GameGrid.Margin;
                gameGridMargin.Top = 10;
                gameGridMargin.Left = 10;
                GameGrid.Margin = gameGridMargin;
                GameGrid.ShowGridLines = true;
                for (int j = 0; j < 10; j++)
                {
                    GameGrid.RowDefinitions.Add(new RowDefinition());
                    GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    for (int k = 0; k < 10; k++)
                    {
                        Rectangle newRectangle = new Rectangle();
                        newRectangle.Width = 30;
                        newRectangle.Height = 30;
                        newRectangle.Stroke = new SolidColorBrush(Colors.Black);
                        newRectangle.StrokeThickness = 2;
                        newRectangle.SetValue(Grid.ColumnProperty, j);
                        newRectangle.SetValue(Grid.RowProperty, k);
                        newRectangle.MouseDown += OnRectangleClick();
                        GameGrid.Children.Add(newRectangle);
                    }
                }
                GameAndBoardGrids.Children.Add(GameGrid);
                Grid ScoreBoard = new Grid();
                ScoreBoard.SetValue(Grid.RowProperty, i);
                ScoreBoard.SetValue(Grid.ColumnProperty, 1);
                ScoreBoard.ColumnDefinitions.Add(new ColumnDefinition());
                ScoreBoard.ColumnDefinitions.Add(new ColumnDefinition());
                for (int j = 0; j < 6; j++)
                {
                    ScoreBoard.RowDefinitions.Add(new RowDefinition());
                    Label newLabel = new Label();
                    newLabel.SetValue(Grid.RowProperty, j);
                    newLabel.SetValue(Grid.ColumnProperty, 0);
                    newLabel.Content = _labelFillers[j];
                    ScoreBoard.Children.Add(newLabel);
                    TextBlock newTextBlock = new TextBlock();
                    newTextBlock.SetValue(Grid.RowProperty, j);
                    newTextBlock.SetValue(Grid.ColumnProperty, 1);
                    ScoreBoard.Children.Add(newTextBlock);
                }
                Grid game = GameSetupGrids.Children[i] as Grid;
                ComboBox combo = game.Children[1] as ComboBox;
                TextBox text = game.Children[3] as TextBox;
                string algorithm = combo.SelectedItem.ToString();
                try
                {
                    int iterations = Convert.ToInt32(text.Text);
                }
                catch
                {
                    GameAndBoardGrids.Children.Clear();
                    return;
                }
                GameSetupGrids.Visibility = Visibility.Hidden;
                GameAndBoardGrids.Visibility = Visibility.Visible;
            }
        }

        private MouseButtonEventHandler OnRectangleClick()
        {
            throw new NotImplementedException();
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
