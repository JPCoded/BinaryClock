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
            set { CirMid.Fill = value; }
            get { return CirMid.Fill; }
        }

        public Brush FillRing
        {
            set { CirOuter.Fill = value; }
            get { return CirOuter.Fill; }
        }
    }
}