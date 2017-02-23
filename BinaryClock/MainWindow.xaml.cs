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
        private bool _hideNum;
        private bool _isTwelveHours;
        private int _previousHour = -1;
        private int _previousMin = -1;
        private int _previousSec = -1;
        private readonly IEnumerable<Label> _hourLabels;
        private readonly IEnumerable<BinaryCircle> _hours;
        private readonly IEnumerable<BinaryCircle> _minutes;
        private readonly RadialGradientBrush _radialGradientBlack;
        private readonly RadialGradientBrush _radialGradientGreen;
        private readonly IEnumerable<BinaryCircle> _seconds;
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            _radialGradientGreen = FindResource("RadialGradientGreen") as RadialGradientBrush;
            _radialGradientBlack = FindResource("RadialGradientBlack") as RadialGradientBrush;
            _seconds = FindLogicalChildren<BinaryCircle>(mainWindow).Where(cir => cir.Name.StartsWith("CirSec"));
            _minutes = FindLogicalChildren<BinaryCircle>(mainWindow).Where(cir => cir.Name.StartsWith("CirMin"));
            _hours = FindLogicalChildren<BinaryCircle>(mainWindow).Where(cir => cir.Name.StartsWith("CirHour"));
            _hourLabels =
                FindLogicalChildren<Label>(mainWindow)
                    .Where(lbl => lbl.Name.StartsWith("lblH"))
                    .SkipWhile(element => ReferenceEquals(element, lblH1));
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

            lblAMPM.Visibility =_isTwelveHours ? Visibility.Hidden : Visibility.Visible;
            var hourEnum = _isTwelveHours
                ? _hours.SkipWhile(element => ReferenceEquals(element, CirHour5)).GetEnumerator()
                : _hours.GetEnumerator();
            if (_isTwelveHours)
            {
                lblAMPM.Content = currentHour >= 12 ? "PM" : "AM";
                currentHour = currentHour > 12 ? currentHour - 12 : currentHour;
                hour = Convert.ToString(currentHour, 2).PadLeft(4, '0').ToCharArray();
            }
            else
            {
                hour = Convert.ToString(currentHour, 2).PadLeft(5, '0').ToCharArray();
            }

            foreach (var val in hour)
            {
                hourEnum.MoveNext();
                hourEnum.Current.SetFill = val == '1' ? _radialGradientGreen : _radialGradientBlack;
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

        private static IEnumerable<T> FindLogicalChildren<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj != null)
            {
                if (obj is T)
                {
                    yield return (T) obj;
                }

                foreach (
                    var c in
                        LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().SelectMany(FindLogicalChildren<T>)
                    )
                {
                    yield return c;
                }
            }
        }



        private void ChkHideNum_Click(object sender, RoutedEventArgs e)
        {
            _hideNum ^= true;

            foreach (var lbl in FindLogicalChildren<Label>(mainWindow).Where(lbl => lbl.Name != "lblAMPM"))
            {
                lbl.Visibility = _hideNum ? Visibility.Hidden : Visibility.Visible;
            }
        }

        private void ChkTwelveHour_Click(object sender, RoutedEventArgs e)
        {
            _isTwelveHours ^= true;
            CycleHour(DateTime.Now.Hour);
            CirHour5.Visibility = _isTwelveHours? Visibility.Hidden : Visibility.Visible;
            lblH1.Visibility = _isTwelveHours ? Visibility.Hidden : Visibility.Visible;
            var startNum = _isTwelveHours ? 1 : 2;
            foreach (var lbl in _hourLabels)
            {
                lbl.Content = startNum;
                startNum *= 2;
            }
        }
    }
}