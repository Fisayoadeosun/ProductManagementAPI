namespace ProductManagementAPI.Data.ViewModel
{
    public class AuthResultVM
    {
        public string Token { get; set; }
        public string RefreshTokens { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
