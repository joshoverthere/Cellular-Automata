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

namespace Cellular_Automata
{

    public class cellGrid
    {

        public static List<List<Rectangle>> thisGrid = new List<List<Rectangle>>();
        public static List<List<int>> thisGridVals = new List<List<int>>();
        public static List<TextBlock> titlesList = new List<TextBlock>();
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

            //enable cell in row 25 column 9
            cellGrid.thisGridVals[25][9] = 1;


            System.Timers.Timer _timer = new System.Timers.Timer(1000); //Updates every quarter second.
            _timer.Enabled = true;
            _timer.AutoReset = false;
            _timer.Elapsed += new ElapsedEventHandler(OnElapsedEvent);


        }

        private void OnElapsedEvent(object source, ElapsedEventArgs e)
        {
            updateGrid();
        }

        private void updateGrid()
        {
            //generate random co-ordinates and enable cell at that position
            Random r = new Random();
            int selectx = r.Next(0, 29);
            int selecty = r.Next(0, 29);

            //attempt to change fill of rectangle at selected location to red and refresh render of rectangle
            try
            {
                Rectangle activeRec = (Rectangle)cellGrid.thisGrid[selecty][selectx];
                activeRec.Fill = Brushes.Red;
                activeRec.Refresh();
            }
            //or show an error message
            catch (Exception ex)
            {
                //convert error message
                string errorMessage = ex.ToString();
                MessageBox.Show(errorMessage);
            }

            


            /*
            //calculate number of cell neighbours
            if (cellGrid.thisGrid[0].Count == 30)
            {
                //loop through rows
                for (int i = 0; i < cellGrid.thisGrid.Count; i++)
                {
                    //loop through columns
                    for (int a = 0; a < cellGrid.thisGrid[i].Count; a++)
                    {
                        Rectangle activeRec = (Rectangle)cellGrid.thisGrid[i][a];
                        //Random r = new Random();
                        //Brush Custombrush = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255),(byte)r.Next(1, 255), (byte)r.Next(1, 233)));
                        int numNeighbours = 0;

                        //if a is the leftmost cell loop to rightmost on same row and check if enabled
                        if (a == 0)
                        {
                            if (cellGrid.thisGridVals[i][29] == 1)
                            {
                                numNeighbours += 1;
                            }
                        }
                        //if a is the rightmost cell loop to the leftmost on same row and check if enabled
                        else if (a == 29)
                        {
                            if (cellGrid.thisGridVals[i][0] == 1)
                            {
                                numNeighbours += 1;
                            }
                        }
                        //otherwise...
                        else
                        {
                            //if cell to the right is enabled add a neighbour
                            if (cellGrid.thisGridVals[i][a + 1] == 1)
                            {
                                numNeighbours += 1;
                            }
                            //if cell to the left is enabled add a neighbour
                            if (cellGrid.thisGridVals[i][a - 1] == 1)
                            {
                                numNeighbours += 1;
                            }
                        }

                    }
                }
            }
            */

        }


       
    }
}
