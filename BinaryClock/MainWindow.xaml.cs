#region

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

#endregion

namespace BinaryClock
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _previousSec = -1;
        private int _previousHour = -1;
        private int _previousMin = -1;
        private readonly BinaryCircle[] _hours;
        private readonly BinaryCircle[] _minutes;
        private readonly RadialGradientBrush _radialGradientBlack = new RadialGradientBrush();
        private readonly RadialGradientBrush _radialGradientGreen = new RadialGradientBrush();
        private readonly BinaryCircle[] _seconds;
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            _radialGradientGreen.GradientStops.Add(new GradientStop(Color.FromRgb(0, 162, 14), 0.0));
            _radialGradientGreen.GradientStops.Add(new GradientStop(Color.FromRgb(0, 100, 7), 0.95));

            _radialGradientBlack.GradientStops.Add(new GradientStop(Color.FromRgb(50, 50, 50), 0.0));
            _radialGradientBlack.GradientStops.Add(new GradientStop(Color.FromRgb(0, 0, 0), 0.95));
            _hours = new[] {CirHour1, CirHour2, CirHour3, CirHour4, CirHour5};
            _minutes = new[] {CirMin1, CirMin2, CirMin3, CirMin4, CirMin5, CirMin6};
            _seconds = new[] {CirSec1, CirSec2, CirSec3, CirSec4, CirSec5, CirSec6};
            _timer.Tick += Timer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var currentHour = DateTime.Now.Hour;
            var currentSec = DateTime.Now.Second;
            var currentMin = DateTime.Now.Minute;

            if (_previousHour != currentHour)
            {
                _previousHour = currentHour;
                CycleHour(currentHour);
            }

            if (_previousMin != currentMin)
            {
                _previousMin = currentMin;
                CycleMinute(currentMin);
            }

            if (_previousSec != currentSec)
            {
                _previousSec = currentSec;
                CycleSecond(currentSec);
            }
        }

        private void CycleHour(int currentHour)
        {
            var hour = Convert.ToString(currentHour, 2).PadLeft(5, '0').ToCharArray();
            var hourTick = 0;
            foreach (var val in hour)
            {
                _hours[hourTick].CirMid.Fill = val == '1' ? _radialGradientGreen : _radialGradientBlack;

                hourTick++;
            }
        }

        private void CycleMinute(int currentMin)
        {
            var min = Convert.ToString(currentMin, 2).PadLeft(6, '0').ToCharArray();
            var minTick = 0;
            foreach (var val in min)
            {
                _minutes[minTick].SetFill = val == '1' ? _radialGradientGreen : _radialGradientBlack;

                minTick++;
            }
        }

        private void CycleSecond(int currentSec)
        {
            var sec = Convert.ToString(currentSec, 2).PadLeft(6, '0').ToCharArray();
            var secTick = 0;

            foreach (var val in sec)
            {
                _seconds[secTick].SetFill = val == '1' ? _radialGradientGreen : _radialGradientBlack;
                secTick++;
            }
        }
    }
}