using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Drawing;
using TrafficSignalTests.Builders;
using TrafficSignal.Settings;
using TrafficSignal.Strategy;

namespace TrafficSignalTests {
	[TestClass]
	public class MoveTests {
		[TestMethod]
		public void GreenLightMove_CrossTraffic_InIntersection_HorizontalCar_Close_To_Intersection_Should_Not_Move() {
			VerticalCarSettings verticalCarSettings = new CarSettingsBuilder<VerticalCarSettings>()
				.WithLocation(new Point(655, 449));

			Assert.IsTrue(verticalCarSettings.States.InIntersection);

			HorizontalCarSettings horizontalCarSettings = new CarSettingsBuilder<HorizontalCarSettings>()
				.WithLocation(new Point(751, 265))
				.WithCrossTraffic(new List<CarSettings> { verticalCarSettings });

			Assert.IsTrue(horizontalCarSettings.States.CloseToIntersection);

			horizontalCarSettings.SetMove(new GreenLightMove());
			horizontalCarSettings.Move();

			Assert.AreEqual(751, horizontalCarSettings.Location.X);
		}

		[TestMethod]
		public void GreenLightMove_CrossTraffic_InIntersection_HorizontalCar_Not_Close_To_Intersection_Should_Move() {
			VerticalCarSettings verticalCarSettings = new CarSettingsBuilder<VerticalCarSettings>()
				.WithLocation(new Point(655, 449));

			Assert.IsTrue(verticalCarSettings.States.InIntersection);

			HorizontalCarSettings horizontalCarSettings = new CarSettingsBuilder<HorizontalCarSettings>()
				.WithLocation(new Point(800, 265))
				.WithCrossTraffic(new List<CarSettings> { verticalCarSettings });

			Assert.IsTrue(horizontalCarSettings.States.BeforeIntersection);

			horizontalCarSettings.SetMove(new GreenLightMove());
			horizontalCarSettings.Move();

			Assert.AreEqual(800 - horizontalCarSettings.PixelsToMove, horizontalCarSettings.Location.X);
		}

		[TestMethod]
		public void GreenLightMove_CrossTraffic_Not_InIntersection_HorizontalCar_Should_Move() {
			VerticalCarSettings verticalCarSettings = new CarSettingsBuilder<VerticalCarSettings>()
				.WithLocation(new Point(655, 600));

			Assert.IsTrue(verticalCarSettings.States.BeforeIntersection);

			HorizontalCarSettings horizontalCarSettings = new CarSettingsBuilder<HorizontalCarSettings>()
				.WithLocation(new Point(700, 265))
				.WithCrossTraffic(new List<CarSettings> { verticalCarSettings });

			Assert.IsTrue(horizontalCarSettings.States.InIntersection);

			horizontalCarSettings.SetMove(new GreenLightMove());
			horizontalCarSettings.Move();

			Assert.AreEqual(700 - horizontalCarSettings.PixelsToMove, horizontalCarSettings.Location.X);
		}

		[TestMethod]
		public void GreenLightMove_CrossTraffic_InIntersection_VerticalCar_Close_To_Intersection_Should_Not_Move() {
			HorizontalCarSettings horizontalCarSettings = new CarSettingsBuilder<HorizontalCarSettings>()
				.WithLocation(new Point(750, 265));

			Assert.IsTrue(horizontalCarSettings.States.InIntersection);

			VerticalCarSettings verticalCarSettings = new CarSettingsBuilder<VerticalCarSettings>()
				.WithLocation(new Point(655, 451))
				.WithCrossTraffic(new List<CarSettings> { horizontalCarSettings });

			Assert.IsTrue(verticalCarSettings.States.CloseToIntersection);

			verticalCarSettings.SetMove(new GreenLightMove());
			verticalCarSettings.Move();

			Assert.AreEqual(451, verticalCarSettings.Location.Y);
		}

		[TestMethod]
		public void GreenLightMove_CrossTraffic_InIntersection_VerticalCar_Not_Close_To_Intersection_Should_Move() {
			HorizontalCarSettings horizontalCarSettings = new CarSettingsBuilder<HorizontalCarSettings>()
				.WithLocation(new Point(750, 265));

			Assert.IsTrue(horizontalCarSettings.States.InIntersection);

			VerticalCarSettings verticalCarSettings = new CarSettingsBuilder<VerticalCarSettings>()
				.WithLocation(new Point(655, 500))
				.WithCrossTraffic(new List<CarSettings> { horizontalCarSettings });

			Assert.IsTrue(verticalCarSettings.States.BeforeIntersection);

			verticalCarSettings.SetMove(new GreenLightMove());
			verticalCarSettings.Move();

			Assert.AreEqual(500 - verticalCarSettings.PixelsToMove, verticalCarSettings.Location.Y);
		}

		[TestMethod]
		public void GreenLightMove_CrossTraffic_Not_InIntersection_VerticalCar_Should_Move() {
			HorizontalCarSettings horizontalCarSettings = new CarSettingsBuilder<HorizontalCarSettings>()
				.WithLocation(new Point(1100, 265));

			Assert.IsTrue(horizontalCarSettings.States.BeforeIntersection);

			VerticalCarSettings verticalCarSettings = new CarSettingsBuilder<VerticalCarSettings>()
				.WithLocation(new Point(655, 10))
				.WithCrossTraffic(new List<CarSettings> { horizontalCarSettings });

			Assert.IsFalse(verticalCarSettings.States.BeforeIntersection);
			Assert.IsFalse(verticalCarSettings.States.CloseToIntersection);
			Assert.IsFalse(verticalCarSettings.States.InIntersection);

			verticalCarSettings.SetMove(new GreenLightMove());
			verticalCarSettings.Move();

			Assert.AreEqual(10 - verticalCarSettings.PixelsToMove, verticalCarSettings.Location.Y);
		}

		[TestMethod]
		public void RedLightMove_HorizontalCar_Close_To_Intersection_Should_Not_Move() {
			HorizontalCarSettings horizontalCarSettings = new CarSettingsBuilder<HorizontalCarSettings>()
				.WithLocation(new Point(751, 265));

			Assert.IsTrue(horizontalCarSettings.States.CloseToIntersection);

			horizontalCarSettings.SetMove(new RedLightMove());
			horizontalCarSettings.Move();

			Assert.AreEqual(751, horizontalCarSettings.Location.X);
		}

		[TestMethod]
		public void RedLightMove_HorizontalCar_Not_Close_To_Intersection_Should_Move() {
			HorizontalCarSettings horizontalCarSettings = new CarSettingsBuilder<HorizontalCarSettings>()
				.WithLocation(new Point(800, 265));

			Assert.IsFalse(horizontalCarSettings.States.CloseToIntersection);

			horizontalCarSettings.SetMove(new RedLightMove());
			horizontalCarSettings.Move();

			Assert.AreEqual(800 - horizontalCarSettings.PixelsToMove, horizontalCarSettings.Location.X);
		}

		[TestMethod]
		public void RedLightMove_VerticalCar_Close_To_Intersection_Should_Not_Move() {
			VerticalCarSettings verticalCarSettings = new CarSettingsBuilder<VerticalCarSettings>()
				.WithLocation(new Point(655, 451));

			Assert.IsTrue(verticalCarSettings.States.CloseToIntersection);

			verticalCarSettings.SetMove(new RedLightMove());
			verticalCarSettings.Move();

			Assert.AreEqual(451, verticalCarSettings.Location.Y);
		}

		[TestMethod]
		public void RedLightMove_VerticalCar_Not_Close_To_Intersection_Should_Move() {
			VerticalCarSettings verticalCarSettings = new CarSettingsBuilder<VerticalCarSettings>()
				.WithLocation(new Point(655, 600));

			Assert.IsFalse(verticalCarSettings.States.CloseToIntersection);

			verticalCarSettings.SetMove(new RedLightMove());
			verticalCarSettings.Move();

			Assert.AreEqual(600 - verticalCarSettings.PixelsToMove, verticalCarSettings.Location.Y);
		}
	}
}
