namespace CityPowerAndLight.Models
{
    public interface Model
    {
        public string GetPayload();
        public static string GeneratePayload() => throw new NotImplementedException();
    }
}
