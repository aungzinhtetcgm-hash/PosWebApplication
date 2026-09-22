namespace PosWebApplication.Exceptions
{
    public class UnauthorizedAdminException : Exception
    {
        public UnauthorizedAdminException(string message)
            : base(message)
        {
        }
    }
}