using BattleshipsSolution3._0.Algorithms;
using BattleshipsSolution3._0.Classes;
using BattleshipsSolution3._0.Interfaces;
using BattleshipsSolution3._0.MVVM_tools.GameStart;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BattleshipsSolution3._0.MVVM_tools
{
    public class MyViewModel
    {
        private Grid _affixGrid;
        private Grid _gameAndScoreboardGrid;
        private Grid _setupGrid;
        private Grid _initializationGrid;
        private int _numberOfGrids;
        private List<string> _labelFillers = new List<string>() { "Algoritme: ", "Gennemsnit: ", "Minimum: ", "Maximum: ", "Total tid: ", "Tid per spil: " };
        private AiContainer _aiContainer = new AiContainer();
        public MyViewModel()
        {
            _affixGrid = App.Current.Windows[0].FindName("MainGrid") as Grid;
            AssignGrids();
            SetInitiationGrid();
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
        public AiContainer AiList
        {
            get { return _aiContainer; }
            set
            {
                _aiContainer = value;
                OnPropertyChanged();
            }
        }
        private void AssignGrids()
        {
            _gameAndScoreboardGrid = AffixGrid.Children[0] as Grid;
            _setupGrid = AffixGrid.Children[1] as Grid;
            _initializationGrid = AffixGrid.Children[2] as Grid;
        }
        private void SetInitiationGrid()
        {
            for (int i = 0; i < 3; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(30);
                _initializationGrid.RowDefinitions.Add(rowDef);
            }
            Thickness initiationGridMargin = _initializationGrid.Margin;
            initiationGridMargin.Top = 10;
            initiationGridMargin.Left = 10;
            _initializationGrid.Margin = initiationGridMargin;
            Label initLabel = new Label();
            initLabel.SetValue(Grid.RowProperty, 0);
            initLabel.Content = "Vælg antal spil";
            initLabel.Height = Double.NaN;
            initLabel.HorizontalAlignment = HorizontalAlignment.Left;
            initLabel.VerticalAlignment = VerticalAlignment.Center;
            ComboBox initCombo = new ComboBox();
            initCombo.SetValue(Grid.RowProperty, 1);
            for (int i = 0; i < 4; i++)
            {
                ComboBoxItem initItem = new ComboBoxItem();
                initItem.Content = Convert.ToInt32(i + 1);
                initCombo.Items.Add(initItem);
            }
            initCombo.Height = Double.NaN;
            initCombo.HorizontalAlignment = HorizontalAlignment.Left;
            initCombo.VerticalAlignment = VerticalAlignment.Center;
            initCombo.SelectedItem = initCombo.Items[0];
            Button initButton = new Button();
            initButton.Content = "Gå til setup";
            initButton.SetValue(Grid.RowProperty, 2);
            initButton.Width = 80;
            initButton.Height = Double.NaN;
            initButton.HorizontalAlignment = HorizontalAlignment.Left;
            initButton.VerticalAlignment = VerticalAlignment.Center;
            initButton.MouseDown += InitiateButton_Click;
            _initializationGrid.Children.Add(initLabel);
            _initializationGrid.Children.Add(initCombo);
            _initializationGrid.Children.Add(initButton);
        }
        private void InitiateButton_Click(object sender, RoutedEventArgs e)
        {
            ComboBox combo = _initializationGrid.Children[1] as ComboBox;
            var vari = combo.SelectedItem as ComboBoxItem;
            NumberOfGrids = Convert.ToInt32(vari.Content);
            for (int i = 0; i < NumberOfGrids; i++)
            {
                _setupGrid.RowDefinitions.Add(new RowDefinition());
                Grid setupGrid = new Grid();
                Thickness setupGridThickness = setupGrid.Margin;
                if (i == 0)
                {
                    setupGridThickness.Top = 10;
                }
                setupGridThickness.Left = 10;
                setupGrid.Margin = setupGridThickness;
                setupGrid.SetValue(Grid.RowProperty, i);
                for (int j = 0; j < 4; j++)
                {
                    RowDefinition setupRowDef = new RowDefinition();
                    setupRowDef.Height = new GridLength(30);
                    setupGrid.RowDefinitions.Add(setupRowDef);
                }

                Label TextLabel1 = new Label();
                TextLabel1.Content = "Vælg algoritme.";
                TextLabel1.SetValue(Grid.RowProperty, 0);
                TextLabel1.HorizontalAlignment = HorizontalAlignment.Left;
                TextLabel1.VerticalAlignment = VerticalAlignment.Center;
                ComboBox Combo = new ComboBox();
                Combo.SetValue(Grid.RowProperty, 1);
                Combo.HorizontalAlignment = HorizontalAlignment.Left;
                Combo.VerticalAlignment = VerticalAlignment.Center;
                Combo.ItemsSource = _aiContainer.AIList;
                Combo.SelectedItem = Combo.Items[0];
                Label TextLabel2 = new Label();
                TextLabel2.Content = "Vælg antal gennemkørsler.";
                TextLabel2.SetValue(Grid.RowProperty, 2);
                TextLabel2.HorizontalAlignment = HorizontalAlignment.Left;
                TextLabel2.VerticalAlignment = VerticalAlignment.Center;
                TextBox IterationTextBox = new TextBox();
                IterationTextBox.SetValue(Grid.RowProperty, 3);
                IterationTextBox.HorizontalAlignment = HorizontalAlignment.Left;
                IterationTextBox.VerticalAlignment = VerticalAlignment.Center;
                IterationTextBox.Width = 100;
                setupGrid.Children.Add(TextLabel1);
                setupGrid.Children.Add(Combo);
                setupGrid.Children.Add(TextLabel2);
                setupGrid.Children.Add(IterationTextBox);
                _setupGrid.Children.Add(setupGrid);
            }
            _setupGrid.RowDefinitions.Add(new RowDefinition());
            Button finishSetup = new Button();
            finishSetup.Content = "Start alle spil!";
            finishSetup.SetValue(Grid.RowProperty, _setupGrid.RowDefinitions.Count - 1);
            finishSetup.MouseDown += FinishSetup_ClickAsync;
            finishSetup.HorizontalAlignment = HorizontalAlignment.Left;
            finishSetup.VerticalAlignment = VerticalAlignment.Center;
            Thickness finishSetupMargin = finishSetup.Margin;
            finishSetupMargin.Left = 10;
            finishSetup.Margin = finishSetupMargin;
            _setupGrid.Children.Add(finishSetup);
            _initializationGrid.Visibility = Visibility.Hidden;
            _setupGrid.Visibility = Visibility.Visible;
        }
        private async void FinishSetup_ClickAsync(object sender, RoutedEventArgs e)
        {
            List<GameHandler> baseAIs = new List<GameHandler>();
            List<int> iterationList = new List<int>();
            for (int i = 0; i < NumberOfGrids; i++)
            {
                _gameAndScoreboardGrid.RowDefinitions.Add(new RowDefinition());
                Grid gameAndBoardGrid = new Grid();
                gameAndBoardGrid.RowDefinitions.Add(new RowDefinition());
                gameAndBoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
                gameAndBoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
                gameAndBoardGrid.SetValue(Grid.RowProperty, i);
                Grid GameGrid = new Grid();
                GameGrid.SetValue(Grid.ColumnProperty, 0);
                GameGrid.Width = 300;
                GameGrid.Height = 300;
                Thickness gameGridMargin = GameGrid.Margin;
                gameGridMargin.Top = 10;
                gameGridMargin.Left = 10;
                GameGrid.Margin = gameGridMargin;
                for (int j = 0; j < 10; j++)
                {
                    RowDefinition newRow = new RowDefinition();
                    GameGrid.RowDefinitions.Add(newRow);
                    ColumnDefinition newColumn = new ColumnDefinition();
                    GameGrid.ColumnDefinitions.Add(newColumn);
                }
                GameGrid.Children.Add(new TextBlock());
                Grid ScoreBoard = new Grid();
                ScoreBoard.SetValue(Grid.RowProperty, i);
                ScoreBoard.SetValue(Grid.ColumnProperty, 1);
                ScoreBoard.Width = 300;
                ScoreBoard.Height = 300;
                Thickness scoreBoardMargin = ScoreBoard.Margin;
                scoreBoardMargin.Left = 10;
                scoreBoardMargin.Top = 10;
                ScoreBoard.Margin = scoreBoardMargin;
                ColumnDefinition leftColumn = new ColumnDefinition();
                leftColumn.Width = new GridLength(100);
                ColumnDefinition rightColumn = new ColumnDefinition();
                rightColumn.Width = new GridLength(190);
                ScoreBoard.ColumnDefinitions.Add(leftColumn);
                ScoreBoard.ColumnDefinitions.Add(rightColumn);
                for (int k = 0; k < 6; k++)
                {
                    RowDefinition rowDef = new RowDefinition();
                    rowDef.Height = new GridLength(30);
                    ScoreBoard.RowDefinitions.Add(rowDef);
                    Label newLabel = new Label();
                    newLabel.SetValue(Grid.RowProperty, k);
                    newLabel.SetValue(Grid.ColumnProperty, 0);
                    newLabel.HorizontalAlignment = HorizontalAlignment.Left;
                    newLabel.VerticalAlignment = VerticalAlignment.Center;
                    newLabel.Content = _labelFillers[k];
                    ScoreBoard.Children.Add(newLabel);
                    TextBlock newTextBlock = new TextBlock();
                    newTextBlock.SetValue(Grid.RowProperty, k);
                    newTextBlock.SetValue(Grid.ColumnProperty, 1);
                    newTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                    newTextBlock.VerticalAlignment = VerticalAlignment.Center;
                    ScoreBoard.Children.Add(newTextBlock);
                }
                Grid setup = _setupGrid.Children[i] as Grid;
                ComboBox combo = setup.Children[1] as ComboBox;
                IBaseAI gameAI = combo.SelectedItem as IBaseAI;
                TextBox text = setup.Children[3] as TextBox;
                string algorithm = combo.SelectedItem.ToString();
                int iterations;
                try
                {
                    iterations = Convert.ToInt32(text.Text);
                }
                catch
                {
                    _gameAndScoreboardGrid.Children.Clear();
                    return;
                }
                gameAndBoardGrid.Children.Add(GameGrid);
                gameAndBoardGrid.Children.Add(ScoreBoard);
                _gameAndScoreboardGrid.Children.Add(gameAndBoardGrid);
                var newHandler = new GameHandler(gameAI, iterations);
                var _algorithmNameBlock = VisualTreeHelper.GetChild(ScoreBoard, 1) as TextBlock;
                string[] typeNameSplit = gameAI.GetType().ToString().Split(Convert.ToChar("."));
                _algorithmNameBlock.Text = typeNameSplit[3];
                var _shotsAverageBlock = VisualTreeHelper.GetChild(ScoreBoard, 3) as TextBlock;
                Binding shotsAverageBinding = new Binding();
                shotsAverageBinding.Source = newHandler.ShotsAverage;
                _shotsAverageBlock.SetBinding(TextBlock.TextProperty, shotsAverageBinding);
                var _shotsMinimumBlock = VisualTreeHelper.GetChild(ScoreBoard, 5) as TextBlock;
                Binding shotsMinimumBinding = new Binding();
                shotsMinimumBinding.Source = newHandler.ShotsMinimum;
                _shotsMinimumBlock.SetBinding(TextBlock.TextProperty, shotsMinimumBinding);
                var _shotsMaximumBlock = VisualTreeHelper.GetChild(ScoreBoard, 7) as TextBlock;
                Binding shotsMaximumBinding = new Binding();
                shotsMaximumBinding.Source = newHandler.ShotsMaximum;
                _shotsMaximumBlock.SetBinding(TextBlock.TextProperty, shotsMaximumBinding);
                var _timeElapsedBlock = VisualTreeHelper.GetChild(ScoreBoard, 9) as TextBlock;
                Binding timeElapsedBinding = new Binding();
                timeElapsedBinding.Source = newHandler.TimeElapsed;
                _timeElapsedBlock.SetBinding(TextBlock.TextProperty, timeElapsedBinding);
                var _timeAverageBlock = VisualTreeHelper.GetChild(ScoreBoard, 11) as TextBlock;
                Binding timeAverageBinding = new Binding();
                timeAverageBinding.Source = newHandler.TimeAverage;
                _timeAverageBlock.SetBinding(TextBlock.TextProperty, timeAverageBinding);
                Binding turnsBinding = new Binding();
                turnsBinding.Source = newHandler.Turns;

                baseAIs.Add(newHandler);
            }
            _setupGrid.Visibility = Visibility.Hidden;
            _gameAndScoreboardGrid.Visibility = Visibility.Visible;
            var gameGrids = new List<Dictionary<int, string>>();
            foreach (GameHandler game in baseAIs)
            {
                await Task.Run(() => gameGrids.Add(game.PlayGame));
            }
            for (int i = 0; i < gameGrids.Count; i++)
            {
                var topGrid = _affixGrid.Children[i] as Grid;
                var gameGrid = topGrid.Children[0] as Grid;
                gameGrid = buildGrid(gameGrids[i]);
            }
        }
        private Grid buildGrid(Dictionary<int, string> dictionary)
        {
            var gameGrid = new Grid();
            gameGrid.SetValue(Grid.ColumnProperty, 0);
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Rectangle newRect = new Rectangle();
                    newRect.SetValue(Grid.RowProperty, y);
                    newRect.SetValue(Grid.ColumnProperty, x);
                    newRect.Width = 30;
                    newRect.Height = 30;
                    newRect.Tag = "Water";
                    newRect.Stroke = new SolidColorBrush(Colors.Black);
                    newRect.StrokeThickness = 2;
                    string caseString = dictionary[(x + y * 10)];
                    switch (caseString)
                    { 
                        case "Water":
                            newRect.Fill = new SolidColorBrush(Colors.LightBlue);
                            break;
                        case "Hit":
                            newRect.Fill = new SolidColorBrush(Colors.Green);
                            break;
                        case "Miss":
                            newRect.Fill = new SolidColorBrush(Colors.Red);
                            break;
                    }
                    gameGrid.Children.Add(newRect);
                }
            }
            return gameGrid;
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
