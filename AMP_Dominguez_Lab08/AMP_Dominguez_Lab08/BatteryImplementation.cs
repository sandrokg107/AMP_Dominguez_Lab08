using System;
using AMP_Dominguez_Lab08.Droid;
using Android.Content;
using Android.OS;
using Android.App;


[assembly: Xamarin.Forms.Dependency(typeof(BatteryImplementation))]
namespace AMP_Dominguez_Lab08.Droid
{
    public class BatteryImplementation : IBattery
    {
        public BatteryImplementation()
        {
        }

        public int RemainingChargePercent
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            var level = battery.GetIntExtra(BatteryManager.ExtraLevel, -1);
                            var scale = battery.GetIntExtra(BatteryManager.ExtraScale, -1);

                            return (int)Math.Floor(level * 100D / scale);
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }
            }
        }

        public AMP_Dominguez_Lab08.BatteryStatus Status
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            int status = battery.GetIntExtra(BatteryManager.ExtraStatus, -1);
                            var isCharging = status == (int)BatteryStatus.Charging || status == (int)BatteryStatus.Full;

                            var chargePlug = battery.GetIntExtra(BatteryManager.ExtraPlugged, -1);
                            var usbCharge = chargePlug == (int)BatteryPlugged.Usb;
                            var acCharge = chargePlug == (int)BatteryPlugged.Ac;
                            bool wirelessCharge = false;
                            wirelessCharge = chargePlug == (int)BatteryPlugged.Wireless;

                            isCharging = (usbCharge || acCharge || wirelessCharge);
                            if (isCharging)
                                return AMP_Dominguez_Lab08.BatteryStatus.Charging;

                            switch (status)
                            {
                                case (int)BatteryStatus.Charging:
                                    return AMP_Dominguez_Lab08.BatteryStatus.Charging;
                                case (int)BatteryStatus.Discharging:
                                    return AMP_Dominguez_Lab08.BatteryStatus.Discharging;
                                case (int)BatteryStatus.Full:
                                    return AMP_Dominguez_Lab08.BatteryStatus.Full;
                                case (int)BatteryStatus.NotCharging:
                                    return AMP_Dominguez_Lab08.BatteryStatus.NotCharging;
                                default:
                                    return AMP_Dominguez_Lab08.BatteryStatus.Unknown;
                            }
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }
            }
        }

        public PowerSource PowerSource
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            int status = battery.GetIntExtra(BatteryManager.ExtraStatus, -1);
                            var isCharging = status == (int)BatteryStatus.Charging || status == (int)BatteryStatus.Full;

                            var chargePlug = battery.GetIntExtra(BatteryManager.ExtraPlugged, -1);
                            var usbCharge = chargePlug == (int)BatteryPlugged.Usb;
                            var acCharge = chargePlug == (int)BatteryPlugged.Ac;

                            bool wirelessCharge = false;
                            wirelessCharge = chargePlug == (int)BatteryPlugged.Wireless;

                            isCharging = (usbCharge || acCharge || wirelessCharge);

                            if (!isCharging)
                                return AMP_Dominguez_Lab08.PowerSource.Battery;
                            else if (usbCharge)
                                return AMP_Dominguez_Lab08.PowerSource.Usb;
                            else if (acCharge)
                                return AMP_Dominguez_Lab08.PowerSource.Ac;
                            else if (wirelessCharge)
                                return AMP_Dominguez_Lab08.PowerSource.Wireless;
                            else
                                return AMP_Dominguez_Lab08.PowerSource.Other;
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }
            }
        }
    }
}

