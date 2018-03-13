using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using ImageCircle.Forms.Plugin.Abstractions;

namespace LabaGraph
{
    public partial class LabaGraphPage : ContentPage
    {
        // public Button Create;
        public Button Edit;
        public Button Finish;
        public Button Start;
        Double x = 0, y = 0;
        bool[] edgeNode = new bool[100];
        AbsoluteLayout panel = new AbsoluteLayout();
        public Image[] arr = new Image[100];
        int counter = 0;
        public int j, o, cur;
        public Graph mainGraph = new Graph();
        PanGestureRecognizer panGesture = new PanGestureRecognizer();

        public LabaGraphPage()
        {
           // var image = new CircleImage { Source = ImageSource.FromUri(new Uri("https://upload.wikimedia.org/wikipedia/commons/thumb/e/e9/Felis_silvestris_silvestris_small_gradual_decrease_of_quality.png/240px-Felis_silvestris_silvestris_small_gradual_decrease_of_quality.png")) };
           // panel.Children.Add(image);
           // image.GestureRecognizers.Add(panGesture);
           // image.HeightRequest = 500;
           // image.WidthRequest = 500;
            BackgroundColor = Color.Chocolate;
            Padding = new Thickness(20);
           // image.Aspect = Aspect.AspectFill;
            var Create = new Button
            {
                Text = "Create a node",
                BackgroundColor = Color.DarkRed,
                IsEnabled = true,
                TextColor = Color.Yellow,
            };
            panel.Children.Add(Create);
          //  Create.GestureRecognizers.Add(panGesture);
            panel.Children.Add(Edit = new Button()
            {
                Text = "Edit Graph",
                BackgroundColor = Color.DarkRed,
                IsEnabled = true,
                TextColor = Color.Yellow,
            });
            panel.Children.Add(Start = new Button()
            {
                Text = "Start animation",
                BackgroundColor = Color.DarkRed,
                IsEnabled = true,
                TextColor = Color.Yellow,
            });
            AbsoluteLayout.SetLayoutBounds(Create, new Rectangle(240, 20, 110, 40));
            AbsoluteLayout.SetLayoutBounds(Edit, new Rectangle(240, 65, 110, 40));
            AbsoluteLayout.SetLayoutBounds(Start, new Rectangle(240, 110, 110, 40));
            panel.Children.Add(Finish = new Button()
            {
                Text = "Finish graph",
                BackgroundColor = Color.DarkRed,
                IsEnabled = false,
                IsVisible = false,
                TextColor = Color.Yellow,
            });
            AbsoluteLayout.SetLayoutBounds(Finish, new Rectangle(240, 65, 110, 40));
            Create.Clicked += CreateNode;
            Edit.Clicked += EditGraph;
            panGesture.PanUpdated += OnPanUpdated;
            for (int w = 0; w < counter; w++)
            {
                if (arr[w] != null)
                    arr[w].GestureRecognizers.Add(panGesture);
            }
            Finish.Clicked += FinishEdit;
            this.Content = panel;
            Start.Clicked += GraphAnime;
            //  Content.GestureRecognizers.Add(panGesture);
        }

        void GraphAnime(object sender, EventArgs e)
        {
            int[] current;
            int a = 0;
            current = mainGraph.DFS();
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (a == current.Length)
                {
                    return false;
                }
                else
                {
                    arr[current[a]].BackgroundColor = Color.OrangeRed;
                    a++;
                    return true;
                }
            });
        }


        void FinishEdit(object sender, EventArgs e)
        {
            Finish.IsVisible = false;
            Finish.IsEnabled = false;
            // for (int a = 0; a < counter; a++)
            //     arr[a].IsEnabled = false;
        }


        void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            var button = sender as Button;
            if (button != null)
            {
                cur = Int32.Parse(button.Text);
                edgeNode[cur] = true;
                for (o = 0; o < counter - 1; o++)
                {
                    if (o != cur && edgeNode[o])
                    {
                        this.DisplayAlert("Alert", "You created an edge", "OK");
                        mainGraph.graph[cur, o] = 1;
                        mainGraph.graph[o, cur] = 1;
                        SKCanvasView canvasView = new SKCanvasView();
                        canvasView.PaintSurface += OnCanvasViewPaintSurface;
                        //    panel.Children.Add(canvasView);
                        this.Content = panel;
                        //  button.BackgroundColor = Color.DarkOrange;
                        //  arr[i].BackgroundColor = Color.DarkOrange;
                        for (int z = 0; z < counter; z++)
                            edgeNode[z] = false;
                    }
                }
            }
        }
        public void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPaint linePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Yellow,
                StrokeWidth = 20
            };
            linePaint.StrokeCap = SKStrokeCap.Round;

            canvas.DrawLine((float)arr[cur].X + 20, (float)arr[cur].Y + 20, (float)arr[o].X + 20, (float)arr[o].Y + 20, linePaint);

        }

        public void EditGraph(object sender, EventArgs e)
        {
            for (int i = 0; i < counter; i++)
            {
                arr[i].IsEnabled = true;
            }
            // Create.IsVisible = false;
            // Create.IsEnabled = false;
            Edit.IsVisible = false;
            Edit.IsEnabled = false;
            Finish.IsVisible = true;
            Finish.IsEnabled = true;
            mainGraph = new Graph(counter);
          /*      for (j = 0; j < counter; j++)
                {
                    arr[j].Clicked += ButtonOnClick;
                }*/
            }


            void CreateNode(object sender, EventArgs e)
            {
            arr[counter] = new CircleImage()
            {
                WidthRequest = 40,
                HeightRequest = 40,
                Source = ImageSource.FromUri(new Uri("https://upload.wikimedia.org/wikipedia/commons/thumb/e/e9/Felis_silvestris_silvestris_small_gradual_decrease_of_quality.png/240px-Felis_silvestris_silvestris_small_gradual_decrease_of_quality.png")),
                Aspect = Aspect.Fill,
                };
                panel.Children.Add(arr[counter]);
                AbsoluteLayout.SetLayoutBounds(arr[counter], new Rectangle(x, y, 40, 40));
            for (int x = 0; x <=counter;x++)
            {
                if (arr[x] != null)
                    arr[x].GestureRecognizers.Add(panGesture);
            }
                if (y == 400)
                {
                    y = 0;
                    x += 50;
                }
                else
                    y += 50;
                counter++;
            }



            void OnPanUpdated(object sender, PanUpdatedEventArgs e)
            {
                var button = sender as Image;
                switch (e.StatusType)
                {
                    case GestureStatus.Running:
                        button.TranslationX =
                            Math.Max(x + e.TotalX, -Math.Abs(button.Width - Application.Current.MainPage.Width));
                        button.TranslationY =
                                  Math.Max(y + e.TotalY, -Math.Abs(button.Height - Application.Current.MainPage.Height));
                        break;

                    case GestureStatus.Completed:
                    x = e.TotalX;
                    y = e.TotalY;
                        break;
                }
            }
        }



        public class Graph
        {
            int size;
            public int[,] graph = new int[100, 100];
            public Graph(int x)
            {
                size = x;
                for (int a = 0; a < x; a++)
                    for (int b = 0; b < x; b++)
                        this.graph[a, b] = 0;
            }
            public Graph() { }
            public int[] BFS()
            {
                int[] local = new int[size];
                int q = 0;
                Random rand = new Random();
                var queue = new Queue<int>();
                int cur = rand.Next(0, size);
                queue.Enqueue(cur);
                bool[] used = new bool[size];
                used[cur] = true;
                while (queue.Count != 0)
                {
                    cur = queue.Peek();
                    local[q] = cur;
                    q++;
                    queue.Dequeue();
                    for (int i = 0; i < size; i++)
                    {
                        if (graph[cur, i] != 0 && !used[i])
                        {
                            queue.Enqueue(i);
                            used[i] = true;
                        }
                    }
                }
                return local;
            }
            public int[] DFS()
            {
                int[] local = new int[size];
                int q = 0;
                Random rand = new Random();
                var stack = new Stack<int>();
                int cur = rand.Next(0, size);
                bool[] used = new bool[size];
                used[cur] = true;
                stack.Push(cur);
                local[q] = cur;
                q++;
                while (stack.Count != 0)
                {
                    cur = stack.Peek();
                    int i = 0;
                    while (stack.Peek() == cur && i < size)
                    {
                        if (graph[cur, i] != 0 && !used[i])
                        {
                            stack.Push(i);
                            used[i] = true;
                            local[q] = i;
                            q++;
                        }
                        i++;
                    }
                    if (stack.Peek() == cur)
                        stack.Pop();
                }
                return local;
            }

        }


    }

