#region

using System.Windows.Controls;
using System.Windows.Media;

#endregion

namespace BinaryClock
{
    /// <summary>
    ///     Interaction logic for BinaryCircle.xaml
    /// </summary>
    public partial class BinaryCircle : UserControl
    {
        public BinaryCircle()
        {
            InitializeComponent();
        }

        public Brush SetFill
        {
            set { cirMid.Fill = value; }
        }
    }
}