namespace MVC_MiniProject_Allup.CustomExceptions.CategoryExceptions;

public class CategoryNotFoundException:Exception
{
    public CategoryNotFoundException() { }
    public CategoryNotFoundException(string message):base(message) { }
}
