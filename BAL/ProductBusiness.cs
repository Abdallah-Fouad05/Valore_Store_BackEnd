using DAL;
using DAL.DTos;

namespace BAL
{
    public class ProductBusiness
    {
        public static async Task<List<ProductDTO>> GetAllProducts()
        {
            try
            {
                var products = await clsProdcutData.GetAllProduct();
                return products;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all products.", ex);
            }
        }

        public static async Task<List<ProductDTO>> GetAllProductByCategoryID(int categoryID)
        {
            try
            {
                var products = await clsProdcutData.GetAllProductCategory(categoryID);
                return products;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all products Category.", ex);
            }
        }

        public static async Task<ProductDTO> Find(int productID)
        {
            try
            {
                var product = await clsProdcutData.GetProductByID(productID);
                return product;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching  product.", ex);
            }
        }

        public static (bool success, int productID) CreateProduct(Product_Created product)
        {
            try
            {
                return clsProdcutData.CreateProduct(product);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while creating product.", ex);
            }
        }

        public static bool UpdateProduct(Product_Created product)
        {
            try
            {
                return clsProdcutData.UpdateProduct(product);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while updating product with ID {product.ProductID}.", ex);
            }
        }

        public static bool DeleteProduct(int productId)
        {
            try
            {
                return clsProdcutData.DeleteProduct(productId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while deleting product with ID {productId}.", ex);
            }
        }
    }
}
