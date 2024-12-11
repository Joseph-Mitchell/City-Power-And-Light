namespace CityPowerAndLight.Models
{
    internal interface Model
    {
        public string GetPayload();
        public static string GeneratePayload() => throw new NotImplementedException();
    }
}
