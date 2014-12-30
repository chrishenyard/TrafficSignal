using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TrafficSignal.Settings;
using TrafficSignal.Strategy;

namespace TrafficSignal {
	public partial class TrafficSignalForm : Form {

		private TrafficSignalSettings _settings;

		public TrafficSignalForm() {
			if (components == null) components = new Container();
			components.Add(new Disposer(OnDispose));

			InitializeComponent();
			InitializeSettings();
		}

		private void InitializeSettings() {
			_settings = Configuration.Instance.AppSettings;

			var horizontalCarSettings = _settings.HorizontalCarSettings;
			var verticalCarSettings = _settings.VerticalCarSettings;

			// set street settings to determine position
			horizontalCarSettings.StreetSettings = verticalCarSettings.StreetSettings = _settings.StreetSettings;

			// cross traffic
			horizontalCarSettings.CrossTraffic.Add(verticalCarSettings);
			verticalCarSettings.CrossTraffic.Add(horizontalCarSettings);

			// image and timer
			horizontalCarSettings.Image = Properties.Resources.car_rtl;
			horizontalCarSettings.Timer = new System.Threading.Timer(HorizontalCarEventHandler, null, horizontalCarSettings.TimerDelay, horizontalCarSettings.TimerInterval);

			verticalCarSettings.Image = Properties.Resources.car_btt;
			verticalCarSettings.Timer = new System.Threading.Timer(VerticalCarEventHandler, null, verticalCarSettings.TimerDelay, verticalCarSettings.TimerInterval);

			// signal timer
			var signalTimerSettings = _settings.SignalTimerSettings;
			signalTimerSettings.Timer = new System.Threading.Timer(SignalEventHandler, null, signalTimerSettings.TimerDelay, signalTimerSettings.TimerInterval);
		}

		private void OnCanvasPaint(object sender, PaintEventArgs e) {
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			FillBackground(e.Graphics);
			DrawSidewalks(e.Graphics);
			DrawLanes(e.Graphics);
			DrawTrafficSignals(e.Graphics);
			DrawCars(e.Graphics);
		}

		private void FillBackground(Graphics g) {
			g.FillRectangle(Brushes.Black, 0, 0, TrafficSignalSettings.CanvasWidth, TrafficSignalSettings.CanvasHeight);
		}

		private void DrawSidewalks(Graphics g) {
			var settings = _settings.SidewalkSettings;
			var width = settings.Width;
			var height = settings.Height;
			var horizontalStreetWidth = settings.HorizontalStreetWidth;
			var verticalStreetWidth = settings.VerticalStreeWidth;

			// top left
			g.FillRectangle(Brushes.Aqua, 0, 0, width, height);

			// top right
			g.FillRectangle(Brushes.Aqua, width + verticalStreetWidth, 0, width, height);

			// bottom left
			g.FillRectangle(Brushes.Aqua, 0, height + horizontalStreetWidth, width, height);

			// bottom right
			g.FillRectangle(Brushes.Aqua, width + verticalStreetWidth, height + horizontalStreetWidth, width, height);
		}

		private void DrawLanes(Graphics g) {
			var settings = _settings.HorizontalLaneSettings;
            var y = settings.Y;
			var width = settings.Width;
			var height = settings.Height;

			// west 
			for (int i = 0; i < _settings.StreetSettings.WestBorder; i += width) {
				g.FillRectangle(Brushes.White, i, y, width - 10, height);
			}

			// east
			for (int i = _settings.StreetSettings.EastBorder; i < TrafficSignalSettings.CanvasWidth; i += width) {
				g.FillRectangle(Brushes.White, i + 10, y, width -10, height);
			}

			settings = _settings.VerticalLaneSettings;
			var x = settings.X;
			width = settings.Width;
			height = settings.Height;

			// north
			for (int i = 0; i < _settings.StreetSettings.NorthBorder; i += height) {
				g.FillRectangle(Brushes.White, x, i, width, height - 10);
			}

			// south
			for (int i = _settings.StreetSettings.SouthBorder; i < TrafficSignalSettings.CanvasHeight; i += height) {
				g.FillRectangle(Brushes.White, x, i + 10, width, height - 10);
			}
		}

		private void DrawCars(Graphics g) {
			var horizontalCar = _settings.HorizontalCarSettings;
			var verticalCar = _settings.VerticalCarSettings;

			g.DrawImage(horizontalCar.Image, horizontalCar.Location.X, horizontalCar.Location.Y);
			g.DrawImage(verticalCar.Image, verticalCar.Location.X, verticalCar.Location.Y);
		}

		private void DrawTrafficSignals(Graphics g) {
			var horizontalSignal = _settings.HorizontalSignalSettings;
			var verticalSignal = _settings.VerticalSignalSettings;

			// horizontal traffic signal
			g.FillRectangle(Brushes.DarkGreen, horizontalSignal.X, horizontalSignal.Y, horizontalSignal.Width, horizontalSignal.Height);
			var rectangle = new Rectangle(
				horizontalSignal.X + (horizontalSignal.Width / 2) - (horizontalSignal.LightWidth / 2),
				horizontalSignal.Y + (horizontalSignal.Height / 2) - (horizontalSignal.LightHeight / 2),
				horizontalSignal.LightWidth,
				horizontalSignal.LightHeight
			);
			Drawing.DrawSolidEllipse(g, horizontalSignal.LightColor, rectangle);

			// vertical traffic signal
			g.FillRectangle(Brushes.DarkGreen, verticalSignal.X, verticalSignal.Y, verticalSignal.Width, verticalSignal.Height);
			rectangle = new Rectangle(
				verticalSignal.X + (verticalSignal.Width / 2) - (verticalSignal.LightWidth / 2),
				verticalSignal.Y + (verticalSignal.Height / 2) - (verticalSignal.LightHeight / 2),
				verticalSignal.LightWidth,
				verticalSignal.LightHeight
				);
			Drawing.DrawSolidEllipse(g, verticalSignal.LightColor, rectangle);
		}

		private void HorizontalCarEventHandler(object state) {
			var move = _settings.HorizontalSignalSettings.LightColor == Color.LightGreen ? new GreenLightMove() as IMove : new RedLightMove();
            _settings.HorizontalCarSettings.SetMove(move);
			_settings.HorizontalCarSettings.Move();
			canvas.Invalidate();
		}

		private void VerticalCarEventHandler(object state) {
			var move = _settings.VerticalSignalSettings.LightColor == Color.LightGreen ? new GreenLightMove() as IMove : new RedLightMove();
			_settings.VerticalCarSettings.SetMove(move);
			_settings.VerticalCarSettings.Move();
			canvas.Invalidate();
		}

		private void SignalEventHandler(object state) {
            var horizontalSignalSettings = _settings.HorizontalSignalSettings;
			var verticalSignalSettings = _settings.VerticalSignalSettings;

			horizontalSignalSettings.LightColor = horizontalSignalSettings.LightColor == Color.LightGreen ? Color.Red : Color.LightGreen;
			verticalSignalSettings.LightColor = verticalSignalSettings.LightColor == Color.LightGreen ? Color.Red : Color.LightGreen;
            canvas.Invalidate();
		}

		private void OnDispose(bool disposing) {
			_settings.HorizontalCarSettings.Dispose();
			_settings.VerticalCarSettings.Dispose();
			_settings.SignalTimerSettings.Dispose();
		}
	}
}
