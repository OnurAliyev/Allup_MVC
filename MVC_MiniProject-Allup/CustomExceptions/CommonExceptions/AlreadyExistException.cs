namespace MVC_MiniProject_Allup.CustomExceptions.CommonExceptions
{
    public class AlreadyExistException:Exception
    {
        public string PropertyName;
        public AlreadyExistException() { }
        public AlreadyExistException(string message):base(message) { }
        public AlreadyExistException(string propertyName , string message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}
