namespace FingerCrew.Model
{
    public class JWTTokenResponse
    {
        public string? status { get; set; }
        public string? remarks { get; set; }
        public string? Token { get; set; }
        public string tokenKey { get; set; }
        public string? ExpireTime { get; set; }
    }
}
