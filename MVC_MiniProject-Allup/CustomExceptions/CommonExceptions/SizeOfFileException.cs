namespace MVC_MiniProject_Allup.CustomExceptions.CommonExceptions
{
    public class SizeOfFileException:Exception
    {
        public string PropertyName { get; set; }
        public SizeOfFileException() { }
        public SizeOfFileException(string message) : base(message) { }
        public SizeOfFileException(string propertyName, string message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}
