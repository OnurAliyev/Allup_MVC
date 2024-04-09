namespace MVC_MiniProject_Allup.CustomExceptions.BrandExceptions
{
    public class BrandNotFoundException:Exception
    {
        public BrandNotFoundException() { }
        public BrandNotFoundException(string message) : base(message) { }
    }
}
