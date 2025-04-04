namespace Module_5.Exceptions
{
    public class GlobalException : Exception
    {
        public GlobalException()
        { }
        public GlobalException(string message) : base(message)
        {

        }
        public GlobalException(string message, Exception inner) : base(message, inner)
        {

        }

    }
}
