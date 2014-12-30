using System.Windows.Forms;

namespace TrafficSignal {
	public class DoubleBufferPanel : Panel {
		public DoubleBufferPanel() {
			DoubleBuffered = true;
			SetStyle(ControlStyles.AllPaintingInWmPaint |
			ControlStyles.UserPaint |
			ControlStyles.OptimizedDoubleBuffer, true);
			UpdateStyles();
		}
	}
}
