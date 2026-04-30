namespace Eneru.Services
{
    public static class AdminGuard
    {
        // Admin credentials are hardcoded — no registration needed
        public const string AdminEmail = "admin@eneru.com";
        public const string AdminPassword = "admin123";

        public static bool IsAdmin(ISession session)
        {
            return session.GetString("UserEmail") == AdminEmail;
        }
    }
}