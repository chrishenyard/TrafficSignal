using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using TrafficSignal.Serializers;
using TrafficSignal.Strategy;

namespace TrafficSignal.Settings {

	[DataContract]
	public abstract class CarSettings : IDisposable {
		protected IMove _move;

		[DataMember]
		public int X;
		[DataMember]
		public int Y;
		[DataMember]
		public int TimerDelay;
		[DataMember]
		public int TimerInterval;
		[DataMember]
		public int PixelsToMove;
		[DataMember]
		public int Width;
		[DataMember]
		public int Height;
		[DataMember]
		public Point Location;
		public Bitmap Image;
		public System.Threading.Timer Timer;
		public StreetSettings StreetSettings;
		public List<CarSettings> CrossTraffic { get; set; } = new List<CarSettings>();

		public void SetMove(IMove move) {
			_move = move;
		}

		public abstract void Move();
		public abstract bool InIntersection { get; }
		public abstract bool BeforeIntersection { get; }
		public abstract bool CloseToIntersection { get; }
		public abstract States States { get; }

		public void Dispose() {
			if (Timer != null) {
				Timer.Dispose();
				Timer = null;
			}

			if (Image != null) {
				Image.Dispose();
				Image = null;
			}
		}
	}

	/*
		Car travels from right to left.
		So the x value decreases as it moves.
	*/
	public class HorizontalCarSettings : CarSettings {
		public override bool BeforeIntersection {
			get {
				if (Location.X > StreetSettings.EastBorder + 5)
					return true;
				return false;
			}
		}

		public override bool CloseToIntersection {
			get {
				if (Location.X <= StreetSettings.EastBorder + 5 &&
					Location.X > StreetSettings.EastBorder)
					return true;
				return false;
			}
		}

		public override bool InIntersection {
			get {
				if (Location.X <= StreetSettings.EastBorder &&
					Location.X >= StreetSettings.WestBorder) {
					return true;
				}
				return false;
			}
		}

		public override States States {
			get {
				return new States {
					BeforeIntersection = BeforeIntersection,
					CloseToIntersection = CloseToIntersection,
					InIntersection = InIntersection
				};
			}
		}

		public override void Move() {
			int axis = Location.X;
			_move.Move(ref axis, PixelsToMove, Width, X, States, CrossTraffic);
			Location.X = axis;
		}
	}

	/*
		Car travels from bottom to top.
		So the y value decreases as it moves.
	*/
	public class VerticalCarSettings : CarSettings {
		public override bool BeforeIntersection {
			get {
				if (Location.Y > StreetSettings.SouthBorder + 5)
					return true;
				return false;
			}
		}

		public override bool CloseToIntersection {
			get {
				if (Location.Y <= StreetSettings.SouthBorder + 5 &&
					Location.Y > StreetSettings.SouthBorder)
					return true;
				return false;
			}
		}

		public override bool InIntersection {
			get {
				if (Location.Y <= StreetSettings.SouthBorder &&
					Location.Y >= StreetSettings.NorthBorder) {
					return true;
				}
				return false;
			}
		}

		public override States States {
			get {
				return new States {
					BeforeIntersection = BeforeIntersection,
					CloseToIntersection = CloseToIntersection,
					InIntersection = InIntersection
				};
			}
		}

		public override void Move() {
			int axis = Location.Y;
			_move.Move(ref axis, PixelsToMove, Height, Y, States, CrossTraffic);
			Location.Y = axis;
		}
	}

	[DataContract]
	public class SignalSettings {
		[DataMember]
		public int X;
		[DataMember]
		public int Y;
		[DataMember]
		public int Width;
		[DataMember]
		public int Height;
		[DataMember]
		public int LightWidth;
		[DataMember]
		public int LightHeight;
		[DataMember]
		public Color LightColor;
	}

	public class SignalTimerSettings : IDisposable {
		[DataMember]
		public int TimerDelay;
		[DataMember]
		public int TimerInterval;
		public System.Threading.Timer Timer;

		public void Dispose() {
			if (Timer != null) {
				Timer.Dispose();
				Timer = null;
			}
		}
	}

	[DataContract]
	public class SidewalkSettings {
		[DataMember]
		public int Width;
		[DataMember]
		public int Height;
		[DataMember]
		public int VerticalStreeWidth;
		[DataMember]
		public int HorizontalStreetWidth;
	}

	[DataContract]
	public class LaneSettings {
		[DataMember]
		public int X;
		[DataMember]
		public int Y;
		[DataMember]
		public int Width;
		[DataMember]
		public int Height;
	}

	[DataContract]
	public class StreetSettings {
		[DataMember]
		public int NorthBorder;
		[DataMember]
		public int SouthBorder;
		[DataMember]
		public int WestBorder;
		[DataMember]
		public int EastBorder;
	}

	[DataContract]
	public class TrafficSignalSettings {
		public const int CanvasWidth = 1200;
		public const int CanvasHeight = 700;

		[DataMember]
		public HorizontalCarSettings HorizontalCarSettings;
		[DataMember]
		public VerticalCarSettings VerticalCarSettings;

		[DataMember]
		public SignalSettings HorizontalSignalSettings;
		[DataMember]
		public SignalSettings VerticalSignalSettings;

		[DataMember]
		public SignalTimerSettings SignalTimerSettings;

		[DataMember]
		public SidewalkSettings SidewalkSettings;

		[DataMember]
		public LaneSettings HorizontalLaneSettings;
		[DataMember]
		public LaneSettings VerticalLaneSettings;

		[DataMember]
		public StreetSettings StreetSettings;
	}

	public sealed class Configuration {
		private static readonly Configuration _instance = new Configuration();
		private TrafficSignalSettings _trafficSignalSettings;

        private Configuration() { }

		public static Configuration Instance { get; } = _instance;

		public string FileName { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "settings.json");

		public TrafficSignalSettings AppSettings {
			get {
				if (_trafficSignalSettings == null) {
					var text = File.ReadAllText(FileName);
					_trafficSignalSettings = Json.Deserialize<TrafficSignalSettings>(text);
				}
				return _trafficSignalSettings;
			}
		}
	}
}
