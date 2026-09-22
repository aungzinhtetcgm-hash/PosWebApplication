namespace PosWebApplication.Helper
{
    public class FilePathHelper
    {
        private readonly IWebHostEnvironment _environment;

        public FilePathHelper(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string GetUserImagePath()
        {
            return Path.Combine(
                _environment.WebRootPath,
                "images",
                "users"
            );
        }

        public string GetUserImageUrl(string imageName)
        {
            if (string.IsNullOrWhiteSpace(imageName))
            {
                return string.Empty;
            }

            return $"/images/users/{imageName}";
        }
    }
}