
namespace Auth.BLL.Exceptions
{
    public class BadCredentialsException : Exception
    {
        public BadCredentialsException()
            : base("Bad credentials provided.") { }

        public BadCredentialsException(string message)
            : base(message) { }
    }
}
