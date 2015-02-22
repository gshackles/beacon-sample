using System;
using System.Linq;
using CoreLocation;
using Foundation;
using UIKit;

namespace IosReceiver
{
	public partial class IosReceiverViewController : UIViewController
	{
		private readonly CLLocationManager _locationManager = new CLLocationManager();
		private readonly CLBeaconRegion _beaconRegion = new CLBeaconRegion(new NSUuid("1EBE566A-56A0-4EEC-80A7-8BB73D4CDE5F"), "iOS Beacon");

		public IosReceiverViewController(IntPtr handle) 
			: base (handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			_locationManager.RequestWhenInUseAuthorization();

			_locationManager.DidRangeBeacons += (sender, e) => 
			{
				if (!e.Beacons.Any())
					return;

				var beacon = e.Beacons.First();

				InvokeOnMainThread(() =>
				{
					switch (beacon.Proximity) 
					{
						case CLProximity.Far:
							View.BackgroundColor = UIColor.Blue;
							break;
						case CLProximity.Near:
							View.BackgroundColor = UIColor.Yellow;
							break;
						case CLProximity.Immediate:
							View.BackgroundColor = UIColor.Green;
							break;
						case CLProximity.Unknown:
							View.BackgroundColor = UIColor.Red;
							break;
					}

					Status.Text = beacon.Accuracy.ToString();
				});
			};

			_locationManager.StartRangingBeacons(_beaconRegion);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			_locationManager.StopRangingBeacons(_beaconRegion);
		}
	}
}