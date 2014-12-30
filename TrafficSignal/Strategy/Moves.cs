using System.Collections.Generic;
using TrafficSignal.Settings;

namespace TrafficSignal.Strategy {
	public struct States {
		public bool InIntersection;
		public bool BeforeIntersection;
		public bool CloseToIntersection;
	}

	public interface IMove {
		void Move(ref int axis, int pixels, int minimum, int start, States states, List<CarSettings> crossTraffic);
	}

	public class GreenLightMove : IMove {
		public void Move(ref int axis, int pixels, int minimum, int start, States states, List<CarSettings> crossTraffic) {
			var crossTrafficInIntersection = crossTraffic.Exists(t => t.InIntersection);

			if (states.CloseToIntersection && crossTrafficInIntersection) return;

			axis -= pixels;
			if (axis < -minimum) axis = start;
		}
	}
	public class RedLightMove : IMove {
		public void Move(ref int axis, int pixels, int minimum, int start, States states, List<CarSettings> crossTraffic) {
			if (states.CloseToIntersection) return;

			axis -= pixels;
			if (axis < -minimum) axis = start;
		}
	}
}
