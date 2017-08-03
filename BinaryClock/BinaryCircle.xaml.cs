#region
using System.Windows.Media;

#endregion

namespace BinaryClock
{
    /// <summary>
    ///    Custom LED Control
    /// </summary>
    internal sealed partial class BinaryCircle
    {
        public BinaryCircle()
        {
            InitializeComponent();
        }

        public Brush SetFill
        {
            set => CirMid.Fill = value;
            get => CirMid.Fill;
        }

        public Brush FillRing
        {
            set => CirOuter.Fill = value;
            get => CirOuter.Fill;
        }
    }
}