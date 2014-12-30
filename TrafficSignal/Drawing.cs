using System.Drawing;

namespace TrafficSignal {
	public static class Drawing {
		public static void DrawSolidEllipse(Graphics g, Color color, Rectangle rectangle) {
			using (var brush = new SolidBrush(color)) {
				g.FillEllipse(brush, rectangle);
			}
		}
	}
}
