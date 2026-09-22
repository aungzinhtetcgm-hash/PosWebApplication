using Newtonsoft.Json;
using PosWebApplication.Entity;
using System.Text.Json.Serialization;

namespace PosWebApplication.Services
{
    public class SessionService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ISession _session;

        public SessionService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _session = _contextAccessor.HttpContext!.Session;
        }

        public void SetUser(user user)
        {
            _session.SetString(
                "user",
                JsonConvert.SerializeObject(user)
            );
        }

        public user? GetUser()
        {
            var user = _session.GetString("user");

            return user is null
                ? null
                : JsonConvert.DeserializeObject<user>(user);
        }

        public void SetString(string key, string value)
        {
            _session.SetString(key, value);
        }

        public string? GetString(string key)
        {
            return _session.GetString(key);
        }

        public void SetInt32(string key, int value)
        {
            _session.SetInt32(key, value);
        }

        public int? GetInt32(string key)
        {
            return _session.GetInt32(key);
        }

        public void Remove(string key)
        {
            _session.Remove(key);
        }

        public void Clear()
        {
            _session.Clear();
        }
    }
}