using System.Collections.Generic;
using System.Drawing;
using TrafficSignal.Settings;

namespace TrafficSignalTests.Builders {
	public class CarSettingsBuilder<T> where T : CarSettings, new() {
		private int x = 1200;
		private int y = 265;
		private int timerDelay = 1000;
		private int timerInterval = 8;
		private int pixelsToMove = 1;
		private int width = 77;
		private int height = 77;
		private Point location = new Point(1200, 265);
		private StreetSettings streetSettings = new StreetSettings { NorthBorder = 250, SouthBorder = 450, WestBorder = 450, EastBorder = 750 };
		private List<CarSettings> crossTraffic = new List<CarSettings>();

		public T Build() {
			return new T {
				X = x,
				Y = y,
				TimerDelay = timerDelay,
				TimerInterval = timerInterval,
				PixelsToMove = pixelsToMove,
				Width = width,
				Height = height,
				Location = location,
				StreetSettings = streetSettings,
				CrossTraffic = crossTraffic
			};
		}

		public CarSettingsBuilder<T> WithX(int x) {
			this.x = x;
			return this;
		}

		public CarSettingsBuilder<T> WithY(int y) {
			this.y = y;
			return this;
		}

		public CarSettingsBuilder<T> WithTimerDelay(int timerDelay) {
			this.timerDelay = timerDelay;
			return this;
		}

		public CarSettingsBuilder<T> WithTimerInterval(int timerInterval) {
			this.timerInterval = timerInterval;
			return this;
		}

		public CarSettingsBuilder<T> WithPixelsToMove(int pixelsToMove) {
			this.pixelsToMove = pixelsToMove;
			return this;
		}

		public CarSettingsBuilder<T> WithWidth(int width) {
			this.width = width;
			return this;
		}

		public CarSettingsBuilder<T> WithHeight(int height) {
			this.height = height;
			return this;
		}

		public CarSettingsBuilder<T> WithLocation(Point location) {
			this.location = location;
			return this;
		}

		public CarSettingsBuilder<T> WithStreetSettings(StreetSettings streetSettings) {
			this.streetSettings = streetSettings;
			return this;
		}

		public CarSettingsBuilder<T> WithCrossTraffic(List<CarSettings> crossTraffic) {
			this.crossTraffic = crossTraffic;
			return this;
		}

		public static implicit operator T (CarSettingsBuilder<T> instance) {
			return instance.Build();
		}
	}
}
