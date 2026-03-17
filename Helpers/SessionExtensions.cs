using Newtonsoft.Json;

namespace ClinicQueue.Helpers
{
    /// <summary>
    /// Extension methods for ISession to store and retrieve complex objects as JSON
    /// </summary>
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }

    /// <summary>
    /// Constants for session keys used throughout the application
    /// </summary>
    public static class SessionKeys
    {
        public const string Token = "JwtToken";
        public const string User = "CurrentUser";
        public const string UserRole = "UserRole";
        public const string UserName = "UserName";
        public const string ClinicName = "ClinicName";
    }
}
