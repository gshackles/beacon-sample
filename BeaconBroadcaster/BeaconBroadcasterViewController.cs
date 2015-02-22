using System;
using CoreBluetooth;
using CoreFoundation;
using CoreLocation;
using Foundation;
using UIKit;

namespace BeaconBroadcaster
{
	public partial class BeaconBroadcasterViewController : UIViewController
	{
		private readonly CBPeripheralManager _peripheralManager;
		private readonly CLBeaconRegion _beaconRegion;

		public BeaconBroadcasterViewController(IntPtr handle) : base (handle)
		{
			_peripheralManager = new CBPeripheralManager(new PeripheralDelegate(), DispatchQueue.DefaultGlobalQueue);
			_beaconRegion = new CLBeaconRegion(new NSUuid("1EBE566A-56A0-4EEC-80A7-8BB73D4CDE5F"), "iOS Beacon");
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var locationManager = new CLLocationManager();
			locationManager.RequestWhenInUseAuthorization();

			var peripheralData = _beaconRegion.GetPeripheralData(null);

			_peripheralManager.StartAdvertising(peripheralData);
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);

			_peripheralManager.StopAdvertising();
		}

		private class PeripheralDelegate : CBPeripheralManagerDelegate 
		{
			public override void StateUpdated(CBPeripheralManager peripheral)
			{
				Console.WriteLine("State updated: " + peripheral.State);
			}
		}
	}
}