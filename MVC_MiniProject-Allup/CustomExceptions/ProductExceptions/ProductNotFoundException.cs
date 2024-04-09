namespace MVC_MiniProject_Allup.CustomExceptions.ProductExceptions
{
    public class ProductNotFoundException:Exception
    {
        public ProductNotFoundException() { }
        public ProductNotFoundException(string message) : base(message) { }
    }
}
