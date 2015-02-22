using System.Collections.Generic;
using System.Linq;
using AltBeaconOrg.BoundBeacon;
using Android.App;
using Android.OS;
using Android.Widget;

namespace DroidReceiver
{
	[Activity (Label = "DroidReceiver", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, IBeaconConsumer, IRangeNotifier
	{
		private readonly Region _region = new Region("iOS beacon", Identifier.Parse("1EBE566A-56A0-4EEC-80A7-8BB73D4CDE5F"), null, null);
		private BeaconManager _beaconManager;
		private TextView _status;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Main);

			_status = FindViewById<TextView>(Resource.Id.Status);

			_beaconManager = BeaconManager.GetInstanceForApplication(this);

			var iBeaconParser = new BeaconParser();
			iBeaconParser.SetBeaconLayout("m:2-3=0215,i:4-19,i:20-21,i:22-23,p:24-24");
			_beaconManager.BeaconParsers.Add(iBeaconParser);

			_beaconManager.SetRangeNotifier(this);
			_beaconManager.Bind(this);
		}

		public void OnBeaconServiceConnect()
		{
			_beaconManager.StartRangingBeaconsInRegion(_region);
		}

		public void DidRangeBeaconsInRegion(ICollection<Beacon> beacons, Region region)
		{
			if (!beacons.Any())
				return;

			var beacon = beacons.First();

			RunOnUiThread(() =>
			{
				if (beacon.Distance < 0.5)
					_status.SetBackgroundColor(Android.Graphics.Color.Green);
				else if (beacon.Distance < 10)
					_status.SetBackgroundColor(Android.Graphics.Color.Yellow);
				else
					_status.SetBackgroundColor(Android.Graphics.Color.Blue);

				_status.Text = beacon.Distance.ToString();
			});
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();

			_beaconManager.StopRangingBeaconsInRegion(_region);
			_beaconManager.Unbind(this);
		}
	}
}