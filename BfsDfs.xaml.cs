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
        public Button Create;
        public Button Edit;
        public Button Finish;
        public Button Start;
        Double x = 0, y = 0;
        bool[] edgeNode = new bool[100];
        AbsoluteLayout panel = new AbsoluteLayout();
        public Node[] arr = new Node[100];
        int counter = 0;
        public int j,o,c,p,first,second;
        public Graph mainGraph = new Graph();
        public PanGestureRecognizer panGesture = new PanGestureRecognizer();
        public TapGestureRecognizer tapGesture = new TapGestureRecognizer();
        string temp;
        public LabaGraphPage()
        {
            
            for (int e = 0; e < 20; e++)
            {
                temp="LabaGraph.ImagesOfNumbers."+e.ToString()+".jpg";
                arr[e] = new Node();
                arr[e].image = new CircleImage();
                arr[e].image.Source = ImageSource.FromResource(temp);
            }
          /*  arr[0].image.Source = ImageSource.FromResource("LabaGraph.ImagesOfNumbers.0.jpg");
            arr[1].image.Source = ImageSource.FromResource("LabaGraph.ImagesOfNumbers.1.jpg");
            arr[2].image.Source = ImageSource.FromResource("LabaGraph.ImagesOfNumbers.2.jpg");
            arr[3].image.Source = ImageSource.FromResource("LabaGraph.ImagesOfNumbers.3.jpg");
            arr[4].image.Source = ImageSource.FromResource("LabaGraph.ImagesOfNumbers.4.jpg");
            arr[5].image.Source = ImageSource.FromResource("LabaGraph.ImagesOfNumbers.5.jpg");
            arr[6].image.Source = ImageSource.FromResource("LabaGraph.ImagesOfNumbers.6.jpg");
            arr[7].image.Source = ImageSource.FromResource("LabaGraph.ImagesOfNumbers.7.jpg");
            arr[0].image.Source = ImageSource.FromResource("LabaGraph.ImagesOfNumbers.0.jpg");*/
            BackgroundColor = Color.LimeGreen;
            Padding = new Thickness(20);
            Create = new Button()
            {
                Text = "Create a node",
                BackgroundColor = Color.DarkRed,
                IsEnabled = true,
                TextColor = Color.Yellow,
            };
            panel.Children.Add(Create);
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
            Finish.Clicked += FinishEdit;
            this.Content = panel;
            Start.Clicked += GraphAnime;
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
                    temp="LabaGraph.InvertedNumbers." + current[a].ToString() + ".jpg";
                    arr[current[a]].image.Source=ImageSource.FromResource(temp);
                    a++;
                    return true;
                }
            });
        }


        void FinishEdit(object sender, EventArgs e)
        {
            Finish.IsVisible = false;
            Finish.IsEnabled = false;
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
                Color = SKColors.White,
                StrokeWidth = 20
            };
            linePaint.StrokeCap = SKStrokeCap.Round;
            canvas.DrawLine(((float)arr[first].x + 20+50*(first/9))*2, ((float)arr[first].y + 20 + 50 * (first % 9))*2 , ((float)arr[second].x + 20+50*(second/9))*2 , ((float)arr[second].y + 20 + 50 * (second % 9))*2 , linePaint);

        }

        public void EditGraph(object sender, EventArgs e)
        {

            Create.IsVisible = false;
            Create.IsEnabled = false;
            Edit.IsVisible = false;
            Edit.IsEnabled = false;
            Finish.IsVisible = true;
            Finish.IsEnabled = true;
            mainGraph = new Graph(counter);
            tapGesture.Tapped += OnTapDetected;
            for (c = 0; c <= counter; c++)
            {
                if (arr[c].image != null)
                {
                    arr[c].image.GestureRecognizers.Add(tapGesture);
                }
            }
        }


        void CreateNode(object sender, EventArgs e)
        {
            arr[counter].image.WidthRequest = 40;
            arr[counter].image.HeightRequest = 40;
           // arr[counter].x = x*2;
           // arr[counter].y = y*2;
            // Source = ImageSource.FromUri(new Uri("https://upload.wikimedia.org/wikipedia/commons/thumb/e/e9/Felis_silvestris_silvestris_small_gradual_decrease_of_quality.png/240px-Felis_silvestris_silvestris_small_gradual_decrease_of_quality.png")),
            arr[counter].image.Aspect = Aspect.Fill;
            panel.Children.Add(arr[counter].image);
            AbsoluteLayout.SetLayoutBounds(arr[counter].image, new Rectangle(x, y, 40, 40));
            panGesture.PanUpdated += OnPanUpdated;
            for (c = 0; c <= counter; c++)
            {
                if (arr[c].image != null)
                {
                    arr[c].image.GestureRecognizers.Add(panGesture);
                }
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
        public void OnTapDetected(object sender ,EventArgs e)
        {
            var obj = sender as Image;
            for (p = 0; p < counter; p++)
            {
                if (obj.Equals(arr[p].image))
                {
                    edgeNode[p] = true;
                    for (o = 0; o < counter; o++)
                    {
                        if (o != p && edgeNode[o])
                        {
                            this.DisplayAlert("Alert", "You created an edge", "OK");
                            mainGraph.graph[p, o] = 1;
                            mainGraph.graph[o, p] = 1;
                            first = o;
                            second = p;
                            SKCanvasView canvasView = new SKCanvasView();
                            canvasView.HeightRequest = Application.Current.MainPage.Height;
                            canvasView.WidthRequest = Application.Current.MainPage.Width;
                            canvasView.PaintSurface += OnCanvasViewPaintSurface;
                            panel.Children.Add(canvasView);
                            for (int z = 0; z < counter; z++)
                                edgeNode[z] = false;
                            for (int z = 0; z < counter; z++)
                                panel.RaiseChild(arr[z].image);
                            panel.RaiseChild(Finish);
                            panel.RaiseChild(Start);
                        }
                    }
                }
            }
        }
        public void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            for (int p = 0; p < counter; p++)
            {
                if (sender.Equals(arr[p].image))
                {
                    var obj = sender as CircleImage;
                    switch (e.StatusType)
                    {
                        case GestureStatus.Running:

                            arr[p].image.TranslationX = 
                               Math.Max(arr[p].x + e.TotalX, -Math.Abs(arr[p].image.Width - Application.Current.MainPage.Width));
                            arr[p].image.TranslationY = 
                                Math.Max(arr[p].y + e.TotalY, -Math.Abs(arr[p].image.Width - Application.Current.MainPage.Height));
                            break;

                        case GestureStatus.Completed:
                            arr[p].y = arr[p].image.TranslationY;
                            arr[p].x = arr[p].image.TranslationX;
                            break;
                    }
                }
            }
        }
    }
    public class Node
    {
        public double x, y;
        public CircleImage image;
        public Node()
        {
            for (int i = 0; i < 100;i++)
            {
                this.image=new CircleImage();
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

