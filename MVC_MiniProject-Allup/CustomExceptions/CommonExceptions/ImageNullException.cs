namespace MVC_MiniProject_Allup.CustomExceptions.CommonExceptions
{
    public class ImageNullException:Exception
    {
        public string PropertyName { get; set; }
        public ImageNullException() { }
        public ImageNullException(string message) : base(message) { }
        public ImageNullException(string propertyName,string message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}
