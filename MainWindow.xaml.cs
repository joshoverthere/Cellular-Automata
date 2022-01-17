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


            for (int e = 0; e < 30; e++)
            {
                List<Rectangle> row = new List<Rectangle>();
                List<int> rowVals = new List<int>();
                for (int i = 0; i < 30; i++)
                {

                    Rectangle newRec = new Rectangle();
                    newRec.Width = 10;
                    newRec.Height = 10;
                    newRec.StrokeThickness = (5/3);
                    newRec.Stroke = Brushes.Black;
                    newRec.Fill = WhiteBrush;


                    Canvas.SetLeft(newRec, (240 + (i * 10)));
                    Canvas.SetTop(newRec, 85 + (e * 10));

                    MyCanvas.Children.Add(newRec);
                    row.Add(newRec);
                    rowVals.Add(0);

                }
                cellGrid.thisGrid.Add(row);
                cellGrid.thisGridVals.Add(rowVals);
            }

            cellGrid.thisGridVals[25][9] = 1;

            System.Timers.Timer _timer = new System.Timers.Timer(1000); //Updates every quarter second.
            _timer.Enabled = true;
            _timer.Elapsed += new ElapsedEventHandler(updateGrid);

        }

        private void updateGrid(object sender, ElapsedEventArgs e)
        {

            Random r = new Random();
            int selectx = r.Next(0, 29);
            int selecty = r.Next(0, 29);
            cellGrid.thisGridVals[selecty][selectx] = 1;


            //calculate number of cell neighbours
            if (cellGrid.thisGrid[0].Count == 30)
            {
                for (int i = 0; i < cellGrid.thisGrid.Count; i++)
                {
                    for (int a = 0; a < cellGrid.thisGrid[i].Count; a++)
                    {
                        Rectangle activeRec = (Rectangle)cellGrid.thisGrid[i][a];
                        //Random r = new Random();
                        //Brush Custombrush = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255),(byte)r.Next(1, 255), (byte)r.Next(1, 233)));
                        int numNeighbours = 0;

                        if (a == 0)
                        {
                            if (cellGrid.thisGridVals[i][29] == 1)
                            {
                                numNeighbours += 1;
                            }
                        }
                        else if (a == 29)
                        {
                            if (cellGrid.thisGridVals[i][0] == 1)
                            {
                                numNeighbours += 1;
                            }
                        }
                        else
                        {
                            if (cellGrid.thisGridVals[i][a + 1] == 1)
                            {
                                numNeighbours += 1;
                            }
                            if (cellGrid.thisGridVals[i][a - 1] == 1)
                            {
                                numNeighbours += 1;
                            }
                        }

                    }
                }
            }

            List<List<int>> wowsocool = new List<List<int>>();

            for (int i = 0; i < cellGrid.thisGridVals.Count; i++)
            {
                List<int> wowsocoolrow = new List<int>();
                for (int a = 0; a < cellGrid.thisGridVals[i].Count; a++)
                {
                    wowsocoolrow.Add(cellGrid.thisGridVals[i][a]);
                    if (cellGrid.thisGridVals[i][a] != 0)
                    {
                        Rectangle activeRec = (Rectangle)cellGrid.thisGrid[i][a];
                        //cellGrid.thisGridVals[i][a];
                        MessageBox.Show(cellGrid.thisGridVals[i][a].ToString());
                        activeRec.Fill = Brushes.Red;
                        activeRec.Refresh();
                    }
                }
                wowsocool.Add(wowsocoolrow);

            }

            string joined = "";
            foreach (int i in wowsocool[5])
            {
                joined += i.ToString();
            }
            MessageBox.Show(joined);
        }



       
    }
}
