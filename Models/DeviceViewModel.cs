namespace AssetSentry.Models
{
    public class DeviceViewModel
    {
        public DeviceViewModel()
        {
            NewDevice = new Device();
        }

        public List<Device>? Devices { get; set; }

        public List<Status>? Statuses { get; set; }

        public Device NewDevice { get; set; }
    }
}
