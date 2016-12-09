#region

using System;
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
    public partial class MainWindow : Window
    {
        private int _previousHour = -1;
        private int _previousSec = -1;
        private int _previousMin = -1;
        private readonly BinaryCircle[] _hours;
        private readonly BinaryCircle[] _twelveHours;
        private readonly BinaryCircle[] _minutes;
        private readonly BinaryCircle[] _seconds;

        private readonly Control[] _binaryLabels;
        private readonly RadialGradientBrush _radialGradientBlack;
        private readonly RadialGradientBrush _radialGradientGreen;
     
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            _radialGradientGreen = FindResource("RadialGradientGreen") as RadialGradientBrush;
            _radialGradientBlack = FindResource("RadialGradientBlack") as RadialGradientBrush;

            _hours = new[] {CirHour1, CirHour2, CirHour3, CirHour4, CirHour5};
            _twelveHours = new [] { CirHour1, CirHour2, CirHour3, CirHour4};
            _minutes = new[] {CirMin1, CirMin2, CirMin3, CirMin4, CirMin5, CirMin6};
            _seconds = new[] {CirSec1, CirSec2, CirSec3, CirSec4, CirSec5, CirSec6};
            _binaryLabels = new Control[] {lblH1,lblH2,lblH3,lblH4,lblH5,lblM1,lblM2,lblM3,lblM4,lblM5,lblM6,lblS1,lblS2,lblS3,lblS4,lblS5,lblS6};
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
            if (ChkTwelveHour.IsChecked == true)
            {
                lblAMPM.Content = currentHour >= 12 ? "PM" : "AM";
                currentHour = currentHour > 12 ? currentHour - 12 : currentHour;
                hour = Convert.ToString(currentHour, 2).PadLeft(4, '0').ToCharArray();
                lblAMPM.Visibility = Visibility.Visible;
            }
            else
            {
                
            lblAMPM.Visibility = Visibility.Hidden;
            hour = Convert.ToString(currentHour, 2).PadLeft(5, '0').ToCharArray();
            }
            var hourTick = 0;
           
            foreach (var val in hour)
            {
                if (ChkTwelveHour.IsChecked == true)
                {
                    _twelveHours[hourTick].CirMid.Fill = val == '1' ? _radialGradientGreen : _radialGradientBlack;
                }
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

        private void chkHideNum_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var lab in _binaryLabels)
            {
                lab.Visibility = Visibility.Visible;
            }
        }

        private void chkHideNum_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var lab in _binaryLabels)
            {
                lab.Visibility = Visibility.Hidden;
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
            lblH1.Visibility = Visibility.Visible;
            //eventually change to just loop over controls and programmaticly change 
            lblH5.Content = "16";
            lblH4.Content = "8";
            lblH3.Content = "4";
            lblH2.Content = "2";
        }
    }
}