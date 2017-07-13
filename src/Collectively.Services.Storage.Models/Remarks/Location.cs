namespace Collectively.Services.Storage.Models.Remarks
{
    public class Location
    {
        public string Address { get; set; }
        public double[] Coordinates { get; set; }
        public double Longitude => Coordinates[0];
        public double Latitude => Coordinates[1];
        public string Type { get; set; }
    }
}