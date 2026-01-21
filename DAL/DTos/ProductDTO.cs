using System.Text.Json.Serialization;
using DAL.DTos;

public class ProductDTO
{
    public int ProductID { get; set; }
    public string ProductName { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public int Quantity { get; set; }
    public string ProductImage { get; set; }
    public CategoryDTO Category { get; set; }

    [JsonConstructor]
    public ProductDTO(int productID, string productName, string title, string description, float price, int quantity, string productImage, CategoryDTO category)
    {
        ProductID = productID;
        ProductName = productName;
        Title = title;
        Description = description;
        Price = price;
        Quantity = quantity;
        ProductImage = productImage;
        Category = category;
    }
}


public class Product_Created
{
    public int ProductID { get; set; }
    public string ProductName { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public int Quantity { get; set; }
    public string ProductImage { get; set; }
    public int CategoryID { get; set; }

    public Product_Created(int productID, string productName, string title, string description, float price, int quantity, string productImage, int categoryID)
    {
        ProductID = productID;
        ProductName = productName;
        Title = title;
        Description = description;
        Price = price;
        Quantity = quantity;
        ProductImage = productImage;
        CategoryID = categoryID;
    }
}
