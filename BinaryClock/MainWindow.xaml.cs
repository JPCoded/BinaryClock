#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

#endregion

namespace BinaryClock
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private int _previousHour = -1;
        private int _previousMin = -1;
        private int _previousSec = -1;
        private BinaryCircle[] _twelveHours;
        private readonly BinaryCircles _hours = new BinaryCircles();
        private readonly IEnumerable<BinaryCircle> _minutes;
        //  private readonly BinaryCircles _minutes = new BinaryCircles();
        private readonly RadialGradientBrush _radialGradientBlack;
        private readonly RadialGradientBrush _radialGradientGreen;
        private readonly IEnumerable<BinaryCircle> _seconds;
        //  private readonly BinaryCircles _seconds = new BinaryCircles();
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            _radialGradientGreen = FindResource("RadialGradientGreen") as RadialGradientBrush;
            _radialGradientBlack = FindResource("RadialGradientBlack") as RadialGradientBrush;
            _seconds = FindLogicalChildren<BinaryCircle>(mainWindow).Where(lbl => lbl.Name.StartsWith("CirSec"));
            _minutes = FindLogicalChildren<BinaryCircle>(mainWindow).Where(lbl => lbl.Name.StartsWith("CirMin"));
            _hours.Circles = new[] {CirHour1, CirHour2, CirHour3, CirHour4, CirHour5};

            _timer.Tick += Timer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var currentSec = DateTime.Now.Second;
            var currentMin = DateTime.Now.Minute;


            if (_previousMin != currentMin)
            {
                var currentHour = DateTime.Now.Hour;
                _previousMin = currentMin;
                CycleMinute(currentMin);

                if (_previousHour != currentHour)
                {
                    _previousHour = currentHour;
                    CycleHour(currentHour);
                }
            }

            if (_previousSec != currentSec)
            {
                _previousSec = currentSec;
                CycleSecond(currentSec);
            }
        }

        private void CycleHour(int currentHour)
        {
            // redo most of this to make it just better;
            char[] hour;
            _hours.Reset();
            lblAMPM.Visibility = ChkTwelveHour.IsChecked == true ? Visibility.Visible : Visibility.Hidden;

            if (ChkTwelveHour.IsChecked == true)
            {
                lblAMPM.Content = currentHour >= 12 ? "PM" : "AM";
                currentHour = currentHour > 12 ? currentHour - 12 : currentHour;
                hour = Convert.ToString(currentHour, 2).PadLeft(4, '0').ToCharArray();

                _twelveHours = _hours.Circles.SkipWhile(element => ReferenceEquals(element, CirHour5)).ToArray();
            }
            else
            {
                hour = Convert.ToString(currentHour, 2).PadLeft(5, '0').ToCharArray();
            }

            var hourTick = 0;

            foreach (var val in hour)
            {
                if (ChkTwelveHour.IsChecked == true)
                {
                    _twelveHours[hourTick].CirMid.Fill = val == '1' ? _radialGradientGreen : _radialGradientBlack;
                }
                _hours.Next().CirMid.Fill = val == '1' ? _radialGradientGreen : _radialGradientBlack;

                hourTick++;
            }
        }

        private void CycleMinute(int currentMin)
        {
            var min = Convert.ToString(currentMin, 2).PadLeft(6, '0').ToCharArray();
            var minEnum = _minutes.GetEnumerator();
            foreach (var val in min)
            {
                minEnum.MoveNext();
                minEnum.Current.SetFill = val == '1' ? _radialGradientGreen : _radialGradientBlack;
            }
        }

        private void CycleSecond(int currentSec)
        {
            var sec = Convert.ToString(currentSec, 2).PadLeft(6, '0').ToCharArray();
            var secEnum = _seconds.GetEnumerator();
            foreach (var val in sec)
            {
                secEnum.MoveNext();
                secEnum.Current.SetFill = val == '1' ? _radialGradientGreen : _radialGradientBlack;
            }
        }

        private void chkHideNum_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var lbl in FindLogicalChildren<Label>(mainWindow).Where(lbl => lbl.Name != "lblAMPM"))
            {
                lbl.Visibility = Visibility.Visible;
            }
        }

        private void chkHideNum_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var lbl in FindLogicalChildren<Label>(mainWindow).Where(lbl => lbl.Name != "lblAMPM"))
            {
                lbl.Visibility = Visibility.Hidden;
            }
        }

        private static IEnumerable<T> FindLogicalChildren<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj != null)
            {
                if (obj is T)
                    yield return (T) obj;

                foreach (
                    var c in
                        LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().SelectMany(FindLogicalChildren<T>)
                    )
                    yield return c;
            }
        }

        private void ChkTwelveHour_Checked(object sender, RoutedEventArgs e)
        {
            CycleHour(DateTime.Now.Hour);
            CirHour5.Visibility = Visibility.Hidden;
            lblH1.Visibility = Visibility.Hidden;
            //eventually change to just loop over controls and programmaticly change 
            lblH5.Content = "8";
            lblH4.Content = "4";
            lblH3.Content = "2";
            lblH2.Content = "1";
        }

        private void ChkTwelveHour_Unchecked(object sender, RoutedEventArgs e)
        {
            CycleHour(DateTime.Now.Hour);
            CirHour5.Visibility = Visibility.Visible;
            // lblH1.Visibility = Visibility.Visible;
            //eventually change to just loop over controls and programmaticly change 
            lblH5.Content = "16";
            lblH4.Content = "8";
            lblH3.Content = "4";
            lblH2.Content = "2";
        }
    }
}