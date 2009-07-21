using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Ink;
using System.Windows.Input;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Mapper
{
    public class Region : INotifyPropertyChanged
    {
        Polygon _polygon;
        PolygonWindow _window;

        public Region(PolygonWindow window)
        {
            _window = window;
            _polygon = new Polygon();

            SetOpenStyle();
        }

        public Region(Polygon polygon, PolygonWindow window)
        {
            _window = window;
            _polygon = polygon;
            SetClosedStyle();
        }

        private void SetOpenStyle()
        {
            _polygon.Style = (Style)_window.FindResource("OpenPolygonStyle");
        }

        private void SetClosedStyle()
        {
            _polygon.Style = (Style)_window.FindResource("PolygonStyle");
        }

        public string Description
        {
            get {
                StringBuilder result = new StringBuilder();
                foreach (Point point in _polygon.Points)
                    result.Append((result.Length > 0 ? " | " : "") + point.ToString());

                return result.ToString();
            }
            set { ; }
        }

        public Polygon Polygon
        {
            get { return _polygon; }
        }


        internal void Close()
        {
            _polygon.Style = (Style)_window.FindResource("PolygonStyle");
        }

        internal void Add(Point point)
        {
            _polygon.Points.Add(point);
            OnPropertyChanged("Description");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }

    public class Regions : ObservableCollection<Region>
    {
        public Regions ()
	    {

	    }
    }

    class Vector : IComparable<Vector>
    {
        public Vector(Point point, double distance)
        {
            this.Point = point;
            this.Distance = distance;
        }

        internal double Distance;
        internal Point Point;

        public int CompareTo(Vector other)
        {
            return this.Distance.CompareTo(other.Distance);
        }

        public override string ToString()
        {
            return String.Format("{0} : {1}", Point, Distance);
        }
    }

    class Vectors : List<Vector>
    {
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            
            foreach (Vector vector in this)
                result.Append(vector.ToString() + "\n");

            return result.ToString();
        }
    }

	public partial class PolygonWindow
	{
        private const int cSCALE = 250;

        public PolygonWindow()
		{
            _regions = new Regions();

			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
            LoadList(@"..\..\map.txt");

            SetTransforms();
		}

        private void SetTransforms()
        {
            TransformGroup group = new TransformGroup();
            _translate = new TranslateTransform(0, 0);
            _scale = new ScaleTransform(1, 1);

            group.Children.Add(_translate);
            group.Children.Add(_scale);
            
            cvsMap.RenderTransform = group;
        }

        bool                _leftCtrl;
        Region              _currentRegion;
        ScaleTransform      _scale;
        TranslateTransform  _translate;
        Regions             _regions;

        private void MouseLeftClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_currentRegion == null)
            {
                _currentRegion = new Region(this);
                cvsMap.Children.Add(_currentRegion.Polygon);
                _regions.Add(_currentRegion);
            }

            _currentRegion.Add(Mouse(e.GetPosition(sender as Image), false));
        }

        public Point Mouse(Point point, bool offsetPanel)
        {
            return new Point(point.X - _translate.X - (offsetPanel ? icRegions.ActualWidth : 0), point.Y - _translate.Y);
        }
	
        private void Image_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_currentRegion == null)
                return;

            _currentRegion.Close();
            _currentRegion = null;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl)
                _leftCtrl = !_leftCtrl;
            else
            {
                if (e.Key == Key.Space)
                {
                    SaveList();
                    e.Handled = true;
                }

                bool translationRequired = false;
                Storyboard sb = new Storyboard();

/*
                if (e.Key == Key.PageUp)
                {
                    _scale.ScaleX += 0.1f;
                    _scale.ScaleY += 0.1f;
                }
                if (e.Key == Key.PageDown)
                {
                    _scale.ScaleX -= 0.1f;
                    _scale.ScaleY -= 0.1f;
                }
*/
                if (e.Key == Key.Right)
                {
                    DoubleAnimation ia = new DoubleAnimation(_translate.X - cSCALE, new Duration(new TimeSpan(0, 0, 0, 0, 200)));

                    Storyboard.SetTargetProperty(ia, new PropertyPath(PolygonWindow.TranslateXProperty));
                    Storyboard.SetTargetName(ia, this.Name);
                    sb.Children.Add(ia);
                    translationRequired = true;
                }

                if (e.Key == Key.Left)
                {

                    DoubleAnimation ia = new DoubleAnimation(
                        _translate.X + cSCALE, 
                        new Duration(new TimeSpan(0, 0, 0, 0, 200))
                    );

                    Storyboard.SetTargetProperty(ia, new PropertyPath(PolygonWindow.TranslateXProperty));
                    Storyboard.SetTargetName(ia, this.Name);
                    sb.Children.Add(ia);
                    translationRequired = true;
                }

                if (e.Key == Key.Up)
                {
                    DoubleAnimation ia = new DoubleAnimation(
                        _translate.Y + cSCALE, 
                        new Duration(new TimeSpan(0, 0, 0, 0, 500))
                    );

                    Storyboard.SetTargetProperty(ia, new PropertyPath(PolygonWindow.TranslateYProperty));
                    Storyboard.SetTargetName(ia, this.Name);
                    sb.Children.Add(ia);
                    translationRequired = true;
                }

                if (e.Key == Key.Down)
                {

                    DoubleAnimation ia = new DoubleAnimation(
                        _translate.Y - cSCALE, 
                        new Duration(new TimeSpan(0, 0, 0, 0, 500))
                    );

                    Storyboard.SetTargetProperty(ia, new PropertyPath(PolygonWindow.TranslateYProperty));
                    Storyboard.SetTargetName(ia, this.Name);
                    sb.Children.Add(ia);
                    translationRequired = true;
                }

                if (translationRequired)
                    sb.Begin(this);
            }
        }

        public static readonly DependencyProperty TranslateXProperty = 
            DependencyProperty.Register(
                "TranslateX", 
                typeof(double), 
                typeof(PolygonWindow),
                new PropertyMetadata(TranslateXChanged)
            );

        public static void TranslateXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PolygonWindow window = (PolygonWindow)d;
            window._translate.X = (double)e.NewValue;
            
        }

        public double TranslateX
        {
            get { return (double)this.GetValue(TranslateXProperty); }
            set { this.SetValue(TranslateXProperty, value); }
        }

        public static readonly DependencyProperty TranslateYProperty =
            DependencyProperty.Register(
                "TranslateY",
                typeof(double),
                typeof(PolygonWindow),
                new PropertyMetadata(TranslateYChanged)
            );

        public static void TranslateYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PolygonWindow window = (PolygonWindow)d;
            window._translate.Y = (double)e.NewValue;

        }

        public double TranslateY
        {
            get { return (double)this.GetValue(TranslateYProperty); }
            set { this.SetValue(TranslateYProperty, value); }
        }

        private void SaveList()
        {
            int index = 0;
            foreach (UIElement element in cvsMap.Children)
            {
                if (element is Polygon)
                {
                    index++;
                    Console.WriteLine(String.Format("Polygon {0, -2}: {1}", index, ((Polygon)element).Points));
                }
            }
        }

        private void LoadList(string fileName)
        {

            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.ASCII))
                {
                    string paths = sr.ReadToEnd();

                    PointConverter pc = new PointConverter();

                    string[] polygons = paths.Replace("\r", "").Split('\n');
                    foreach (string polygon in polygons)
                    {
                        if (polygon.Length == 0)
                            break;
                        Polygon poly = new Polygon();
                        poly.Style = (Style)FindResource("PolygonStyle");
                        foreach (string pair in polygon.Split(' '))
                            poly.Points.Add((Point)pc.ConvertFromString(pair));

                        cvsMap.Children.Add(poly);
                        _regions.Add(new Region(poly, this));
                    }
                }
            }

            int index = 0;
            foreach (UIElement element in cvsMap.Children)
                if (element is Polygon)
                {
                    index++;
                    Console.WriteLine(String.Format("Polygon {0, -2}: {1}", index, ((Polygon)element).Points));
                }
        }

        private double GetDistance(Point current, Point point)
        {
            const double cSQUARED = 2.0;

            double xSquared = Math.Pow(current.X - point.X, cSQUARED);
            double ySquared = Math.Pow(current.Y - point.Y, cSQUARED);

            return Math.Sqrt(xSquared + ySquared);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point current = Mouse(e.GetPosition(sender as Canvas), true);
            Point newPoint = this.PointToScreen(current);

            if (!_leftCtrl)
            {
                Vectors vectors = new Vectors();

                foreach (UIElement element in cvsMap.Children)
                    if (element is Polygon)
                        foreach (Point point in ((Polygon)element).Points)
                        {
                            double distance = GetDistance(current, point);
                            if (distance > 0 && distance < 5)
                                vectors.Add(new Vector(point, distance));
                        }
                
                if (vectors.Count > 0)
                {
                    vectors.Sort();

                    newPoint = this.PointToScreen(vectors[0].Point);

                    System.Windows.Forms.Cursor.Position =
                        new System.Drawing.Point(
                            (int)(newPoint.X + icRegions.ActualWidth + _translate.X),
                            (int)newPoint.Y
                        );
                }
            }
            this.Title = String.Format("Mouse: {0} Sticky: {1}", current, newPoint);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MouseLeftClick(sender, e);
        }

        public Regions Regions
        {
            get { return _regions; }
            set {}
        }
	}
}