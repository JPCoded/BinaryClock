#region

using System;
using System.Threading;
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
        private readonly Control[] _hours;
        private readonly Control[] _minutes;
        private readonly Control[] _seconds;
        readonly DispatcherTimer _timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
          
            _hours = new Control[] {cirHour1, cirHour2, cirHour3, cirHour4, cirHour5};
            _minutes = new Control[] {cirMin1,cirMin2,cirMin3,cirMin4,cirMin5,cirMin6};
            _seconds = new Control[] {cirSec1,cirSec2,cirSec3,cirSec4,cirSec5,cirSec6};
            _timer.Tick += Timer_Tick;
            _timer.Interval = new TimeSpan(0,0,1);
            _timer.Start();
 
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var hour = Convert.ToString(DateTime.Now.Hour, 2).PadLeft(5,'0').ToCharArray();
            var min = Convert.ToString(DateTime.Now.Minute, 2).PadLeft(6, '0').ToCharArray();
            var sec = Convert.ToString(DateTime.Now.Second, 2).PadLeft(6, '0').ToCharArray();
            var hourTick = 0;
            var secTick = 0;
            var minTick = 0;
            foreach (var val in hour)
            {
               
               var temp =  (BinaryCircle) _hours[hourTick];
                temp.SetFill = val == '1' ? Brushes.Green : Brushes.Black;
                hourTick++;
            }
            foreach (var val in min)
            {
                var temp = (BinaryCircle)_minutes[minTick];
                temp.SetFill = val == '1' ? Brushes.Green : Brushes.Black;
                minTick++;
            }

            foreach (var val in sec)
            {
                var temp = (BinaryCircle)_seconds[secTick];
                temp.SetFill = val == '1' ? Brushes.Green : Brushes.Black;
                secTick++;
            }
         
        }
    }
}