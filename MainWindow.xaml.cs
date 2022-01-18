using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Threading;
using System.Timers;
using System.IO;

namespace Cellular_Automata
{

    public class cellGrid
    {
        //this, for storing the rectangle UI objects of the grid and their values (alive or dead for each cell)
        public static List<List<Rectangle>> thisGrid = new List<List<Rectangle>>();
        public static List<List<int>> thisGridVals = new List<List<int>>();
    }

    public static class ExtensionMethods
    {
        private static readonly Action EmptyDelegate = delegate { };
        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, EmptyDelegate);
        }
    }

    public partial class MainWindow : Window
    {

        Random r = new Random();
        public MainWindow()
        {
            InitializeComponent();

            System.Console.WriteLine("This is written last.");
            Brush WhiteBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            //loop through rows
            for (int e = 0; e < 30; e++)
            {
                List<Rectangle> row = new List<Rectangle>();
                List<int> rowVals = new List<int>();

                //loop through columns
                for (int i = 0; i < 30; i++)
                {
                    //create rectangle with parameters
                    Rectangle newRec = new Rectangle();
                    newRec.Width = 10;
                    newRec.Height = 10;
                    newRec.StrokeThickness = (5/3);
                    newRec.Stroke = Brushes.Black;
                    newRec.Fill = WhiteBrush;

                    //set rectangle position
                    Canvas.SetLeft(newRec, (240 + (i * 10)));
                    Canvas.SetTop(newRec, 85 + (e * 10));

                    //add new rectangle to canvas for render
                    MyCanvas.Children.Add(newRec);

                    //store rectangle and cell value to temporary row
                    row.Add(newRec);
                    rowVals.Add(0);

                }
                //add temporary row of rectangles and temporary row of cell values to cell grid
                cellGrid.thisGrid.Add(row);
                cellGrid.thisGridVals.Add(rowVals);
            }

            //randomly fill 5/6 of the grid with enabled cells
            for (int i = 0; i < (cellGrid.thisGrid.Count * cellGrid.thisGrid[0].Count); i++)
            {
                int selectx = r.Next(0,30);
                int selecty = r.Next(0,30);
                cellGrid.thisGridVals[selecty][selectx] = 1;
            }
            
            //set timer for refreshing the grid (every 200 millisecs)
            System.Timers.Timer _timer = new System.Timers.Timer(50);
            _timer.Enabled = true;
            _timer.Elapsed += new ElapsedEventHandler(OnElapsedEvent);

            //set timer for running new generation (every 200 millisecs)
            System.Timers.Timer _timer2 = new System.Timers.Timer(50);
            _timer2.Enabled = true;
            _timer2.Elapsed += new ElapsedEventHandler(OnElapsedEvent2);
            

        }

        public void OnElapsedEvent(object source, ElapsedEventArgs e)
        {
            updateGrid();
        }

        public void OnElapsedEvent2(object source, ElapsedEventArgs e)
        { 
            wowUpdate();
        }

        private void click(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Rectangle)
            {
                //identify the rectangle that was clicked on
                Rectangle activeRec = (Rectangle)e.OriginalSource;

                //loop through all the rectangles and try to find the index of the one that was clicked on
                for (int i = 0; i < cellGrid.thisGrid.Count; i++)
                {
                    for (int a = 0; a < cellGrid.thisGrid[i].Count; a++)
                    {
                        //if the rectangles match...
                        if (cellGrid.thisGrid[i][a] == activeRec)
                        {
                            //enable a a diamond of five cells where the user clicked
                            cellGrid.thisGridVals[i][a] = 1;
                            cellGrid.thisGridVals[i][a+1] = 1;
                            cellGrid.thisGridVals[i][a-1] = 1;
                            cellGrid.thisGridVals[i+1][a] = 1;
                            cellGrid.thisGridVals[i - 1][a] = 1;

                            //change colours and refresh
                            activeRec.Fill = Brushes.Red;
                            activeRec.Refresh();
                        }
                    }
                }

            }
        }

        public void wowUpdate()
        {
            //loop through rows of cells
            for (int i = 0; i < cellGrid.thisGrid.Count; i++)
            {
                //loop through columns per row
                for (int a = 0; a < cellGrid.thisGrid[i].Count; a++)
                {
                    //if cell's value is 1, turn red, otherwise turn white
                    Rectangle activeRec = (Rectangle)cellGrid.thisGrid[i][a];
                    if (cellGrid.thisGridVals[i][a] == 0)
                    {
                        //invoke dispatcher to update colour of cell (otherwise unable to access rectangle due to being on a different thread) :D
                        Dispatcher.Invoke(new Action(() => {
                            activeRec.Fill = Brushes.White;
                        }));
                    }
                    else
                    {
                        cellGrid.thisGridVals[i][a] = 1;
                        Dispatcher.Invoke(new Action(() => {
                            activeRec.Fill = Brushes.Red;
                        }));
                    }

                }
            }
        }

        public void updateGrid()
        {
            //create duplicate of main grid
            List<List<int>> temporaryGrid = new List<List<int>>();
            temporaryGrid = cellGrid.thisGridVals;

            //create grid of numNeighbour values (stores the number of neighbours each cell has)
            List<List<int>> temporaryGridNeighbours = new List<List<int>>();

            //loop through rows
            for (int i = 0; i < temporaryGrid.Count; i++)
            {
                //create row of numNeighbour values
                List<int> temporaryRowNeighbours = new List<int>();

                //loop through columns
                for (int a = 0; a < temporaryGrid[i].Count; a++)
                {
                    //set number of neighbours
                    int numNeighbours = 0;

                    //add neighbour if cell to the right is enabled
                    try
                    {
                        if (temporaryGrid[i][a + 1] == 1)
                        {
                            numNeighbours += 1;
                        }
                    }
                    catch { }

                    //add neighbour if cell to the left is enabled
                    try
                    {
                        if (temporaryGrid[i][a-1] == 1)
                        {
                            numNeighbours += 1;
                        }
                    }
                    catch { }

                    //add neighbour if cell above is enabled
                    try
                    {
                        if (temporaryGrid[i - 1][a] == 1)
                        {
                            numNeighbours += 1;
                        }
                    }
                    catch { }

                    //add neighbour if cell below is enabled
                    try
                    {
                        if (temporaryGrid[i + 1][a] == 1)
                        {
                            numNeighbours += 1;
                        }
                    }
                    catch { }

                    //add neighbour if cell top left is enabled
                    try
                    {
                        if (temporaryGrid[i - 1][a - 1] == 1)
                        {
                            numNeighbours += 1;
                        }
                    }
                    catch { }

                    //add neighbour is cell top right is enabled
                    try
                    {
                        if (temporaryGrid[i - 1][a + 1] == 1)
                        {
                            numNeighbours += 1;
                        }
                    }
                    catch { }

                    //add neighbour if bottom left is enabled
                    try
                    {
                        if (temporaryGrid[i + 1][a - 1] == 1)
                        {
                            numNeighbours += 1;
                        }
                    }
                    catch { }

                    //add neighbour is bottom right is enabled
                    try
                    {
                        if (temporaryGrid[i + 1][a + 1] == 1)
                        {
                            numNeighbours += 1;
                        }
                    }
                    catch { }

                    //add number of neighbours this cell has to the temporary row
                    temporaryRowNeighbours.Add(numNeighbours);

                }
                //add row of neighbour values to the neighbours grid
                temporaryGridNeighbours.Add(temporaryRowNeighbours);
            }

            //loop through rows of cells
            for (int i = 0; i < temporaryGrid.Count; i++)
            {
                //loop through columns
                for (int a = 0; a < temporaryGrid[i].Count; a++)
                {
                    //find out how many neighbours that cell had from the neighbours grid
                    int howManyNeighbours = temporaryGridNeighbours[i][a];

                    //if the cell is dead it can be born if it has three neighbours, no more and no less
                    if (temporaryGrid[i][a] == 0)
                    {
                        if (howManyNeighbours == 3)
                        {
                            temporaryGrid[i][a] = 1;
                        }
                    }

                    //if the cell is already alive, it only survives if it has exactly two or three neighbours, otherwise it dies
                    if (temporaryGrid[i][a] == 1)
                    {
                        if (howManyNeighbours == 2 || howManyNeighbours==3)
                        {
                            temporaryGrid[i][a] = 1;
                        }
                        else
                        {
                            temporaryGrid[i][a] = 0;
                        }
                    }

                }
            }

            //update main grid by duplicate grid
            cellGrid.thisGridVals = temporaryGrid;

        }
       
    }
}
