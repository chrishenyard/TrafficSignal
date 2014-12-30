using System;
using System.ComponentModel;

namespace TrafficSignal {
	internal class Disposer : Component {
		private Action<bool> _dispose;

		internal Disposer(Action<bool> disposeCallback) {
			_dispose = disposeCallback;
		}

		protected override void Dispose(bool disposing) {
			base.Dispose(disposing);
			_dispose(disposing);
		}
	}
}
