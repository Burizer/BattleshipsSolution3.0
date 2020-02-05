using BattleshipsSolution3._0.Algorithms;
using BattleshipsSolution3._0.Classes;
using BattleshipsSolution3._0.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BattleshipsSolution3._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> _aiBaseList = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(x => typeof(IBaseAI).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(x => x.Name).ToList();
        private string _gridSelectionBox;
        private int NumberOfGrids { get; set; } = 0;

        public MainWindow()
        {
            InitializeComponent();
            PopulateLists();
        }
        public string GridSelectionBox
        {
            get { return _gridSelectionBox; }
            set { _gridSelectionBox = value; }
        }
        public List<string> GetIBaseAI
        {
            get { return _aiBaseList; }
            set
            {
                _aiBaseList = value;
                OnPropertyChanged();
            }
        }
        public void PopulateLists()
        {
            foreach (string item in _aiBaseList)
            {
                ComboBoxItem comboItem1 = new ComboBoxItem();
                comboItem1.Content = item;
                comboItem1.Tag = item;
                ComboBoxItem comboItem2 = new ComboBoxItem();
                comboItem2.Content = item;
                comboItem2.Tag = item;
                ComboBoxItem comboItem3 = new ComboBoxItem();
                comboItem3.Content = item;
                comboItem3.Tag = item;
                ComboBoxItem comboItem4 = new ComboBoxItem();
                comboItem4.Content = item;
                comboItem4.Tag = item;
                GameSetupSelection1.Items.Add(comboItem1);
                GameSetupSelection2.Items.Add(comboItem2);
                GameSetupSelection3.Items.Add(comboItem3);
                GameSetupSelection4.Items.Add(comboItem4);
            }
            GameSetupSelection1.SelectedItem = GameSetupSelection1.Items[0];
            GameSetupSelection2.SelectedItem = GameSetupSelection2.Items[0];
            GameSetupSelection3.SelectedItem = GameSetupSelection3.Items[0];
            GameSetupSelection4.SelectedItem = GameSetupSelection4.Items[0];
        }

        //int generatedGrids = -1;
        //Style newFullGridStyle = Application.Current.FindResource("") as Style;
        //Style newGridStyle = Application.Current.FindResource("") as Style;
        //Style newScoreBoardStyle = Application.Current.FindResource("") as Style;
        ///// <summary>
        ///// I denne funktion sker der følgende:
        ///// 1. Det nye spilgrid oprettes og gives et navn
        ///// 2. Der oprettes en ny grid row, som senere tilføjes til MainGrid
        ///// 3. Det nye gspilrid får egne row og column properties sat
        ///// 4. Der tilføjes 10 rows og columns til det nye spilgrid
        ///// 5. Der oprettes 100 nye mindre grids, som alle sættes ind i det nye spilgrid, med deres navne baseret på deres placering
        ///// 6. Der sættes style på elementerne
        ///// 7. Til sidst sættes det nye spil grid ind i MainGrid
        ///// </summary>
        //public void PopulateGrid()
        //{
        //    generatedGrids++;
        //    Grid newFullGrid = new Grid();
        //    newFullGrid.Name = "Grid" + generatedGrids.ToString();
        //    RowDefinition newRow = new RowDefinition();
        //    newFullGrid.SetValue(Grid.RowProperty, generatedGrids);
        //    newFullGrid.SetValue(Grid.ColumnProperty, 0);
        //    for (int i = 0; i < 10; i++)
        //    {
        //        newFullGrid.ColumnDefinitions.Add(new ColumnDefinition());
        //    }
        //    for (int j = 0; j < 10; j++)
        //    {
        //        newFullGrid.RowDefinitions.Add(new RowDefinition());
        //    }
        //    List<string> letters = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        //    for (int y = 0; y < 10; y++)
        //    {
        //        for (int x = 0; x < 10; x++)
        //        {
        //            Grid newGrid = new Grid();
        //            newGrid.Name = newFullGrid.Name.ToString() + "-" + letters[y] + x.ToString();
        //            newGrid.SetValue(Grid.RowProperty, y);
        //            newGrid.SetValue(Grid.ColumnProperty, x);
        //            newGrid.MouseDown += ClickEvent;
        //            newGrid.Style = newGridStyle;
        //            newFullGrid.Children.Add(newGrid);
        //            newFullGrid.Style = newFullGridStyle;
        //        }
        //    }
        //    MainGrid.RowDefinitions.Add(newRow);
        //    MainGrid.Children.Add(newFullGrid);
        //}


        //public void SetUpScoreboard()
        //{
        //    Grid newScoreBoard = new Grid();
        //    newScoreBoard.Name = "ScoreGrid" + generatedGrids.ToString();
        //    newScoreBoard.SetValue(Grid.RowProperty, generatedGrids);
        //    newScoreBoard.SetValue(Grid.ColumnProperty, 1);
        //    for (int i = 0; i < 2; i++)
        //    {
        //        newScoreBoard.ColumnDefinitions.Add(new ColumnDefinition());
        //    }
        //    for (int j = 0; j < 4; j++)
        //    {
        //        newScoreBoard.RowDefinitions.Add(new RowDefinition());
        //    }
        //    int newScoreGridRow = 0;
        //    List<String> labelList = new List<string>() { "Algoritme", "Gennemsnit", "Min", "Max" };
        //    for (int k = 0; k < 8; k++)
        //    {
        //        if (k % 2 == 1)
        //        {
        //            Label scoreGridLabel = new Label();
        //            scoreGridLabel.Name = newScoreBoard.Name + "-ScoreGridLabel" + generatedGrids.ToString();
        //            scoreGridLabel.Content = labelList[newScoreGridRow];
        //            scoreGridLabel.SetValue(Grid.RowProperty, newScoreGridRow);
        //            scoreGridLabel.SetValue(Grid.ColumnProperty, 0);
        //            newScoreBoard.Children.Add(scoreGridLabel);
        //        }
        //        else
        //        {
        //            TextBlock scoreGridTextBlock = new TextBlock();
        //            scoreGridTextBlock.Name = newScoreBoard.Name + "-ScoreGridTextBlock" + generatedGrids.ToString();
        //            scoreGridTextBlock.SetValue(Grid.RowProperty, newScoreGridRow);
        //            scoreGridTextBlock.SetValue(Grid.ColumnProperty, 1);
        //            newScoreBoard.Children.Add(scoreGridTextBlock);
        //            newScoreGridRow++;
        //        }
        //    }
        //    newScoreBoard.Style = newScoreBoardStyle;
        //    MainGrid.Children.Add(newScoreBoard);
        //}
        private void InitiateButton_Click(object sender, RoutedEventArgs e)
        {
            NumberOfGrids = Convert.ToInt32(InitiationDropdown.SelectedValue.ToString());
            for (int i = 1; i < NumberOfGrids + 1; i++)
            {
                var main = GameSetupGrids as DependencyObject;
                string NameOfGrid = "GameSetupGrid";
                var findNameVar = LogicalTreeHelper.FindLogicalNode(main, NameOfGrid + i.ToString());
                var newGrid = findNameVar as Grid;
                newGrid.Visibility = Visibility.Visible;
            }
            GameInitiationGrid.Visibility = Visibility.Hidden;
            GameSetupGrids.Visibility = Visibility.Visible;
            BeginGame.Visibility = Visibility.Visible;
        }

        private void BeginGame_Click(object sender, RoutedEventArgs e)
        {
            List<Tuple<IBaseAI, Grid, int>> gameHandlerParams = new List<Tuple<IBaseAI, Grid, int>>();
            for (int i = 1; i < NumberOfGrids + 1; i++)
            {
                var main = MainGrid as DependencyObject;
                string NameOfGrid = "GameAndBoardGrid";
                var findNameVar = LogicalTreeHelper.FindLogicalNode(main, NameOfGrid + i.ToString());
                var newGrid = findNameVar as Grid;
                Grid setupGrid = GameSetupGrids;
                Grid boxGrid = setupGrid.Children[i - 1] as Grid;
                ComboBox comboBox = boxGrid.Children[0] as ComboBox;
                TextBox textBox = boxGrid.Children[1] as TextBox;
                int iterations = -1;
                try
                {
                    iterations = Convert.ToInt32(textBox.Text);
                }
                catch
                {
                    return;
                }
                ComboBoxItem selected = comboBox.SelectedItem as ComboBoxItem;
                string algorithmName = selected.Tag.ToString();
                Type algorithmType = Type.GetType("BattleshipsSolution3._0.Algorithms." + algorithmName);
                var algorithm = Activator.CreateInstance(algorithmType) as IBaseAI;
                algorithm.GameGrid = newGrid;
                gameHandlerParams.Add(Tuple.Create(algorithm, newGrid, iterations));
            }
            foreach (var item in gameHandlerParams)
            {
                GameHandler gameHandler = new GameHandler(item.Item1, item.Item2, item.Item3);
                item.Item2.Visibility = Visibility.Visible;
            }
            GameSetupGrids.Visibility = Visibility.Hidden;
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
