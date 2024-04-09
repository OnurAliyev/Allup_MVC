namespace MVC_MiniProject_Allup.Models;

public class ProductImages : BaseEntity
{
    public Product Product { get; set; }
    public int ProductId {  get; set; }
    public bool? IsCover {  get; set; } // true => cover | false => hover | null = multiple
    public string ImageUrl {  get; set; }
}
