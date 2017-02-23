#region
using System.Windows.Media;

#endregion

namespace BinaryClock
{
    /// <summary>
    ///    Custom LED Control
    /// </summary>
    public partial class BinaryCircle
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