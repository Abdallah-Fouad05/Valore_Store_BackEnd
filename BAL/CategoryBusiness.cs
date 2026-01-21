using DAL;
using DAL.DTos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL
{
    public class CategoryBusiness
    {
        public static async Task<List<CategoryDTO>> GetAllCategories()
        {
            try
            {
                var categories = await clsCategoryData.GetAllCategory();
                return categories;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all categories.", ex);
            }
        }

        public static async Task<CategoryDTO> Find(int categoryID)
        {
            try
            {
                var categories = await clsCategoryData.GetCategoryByID(categoryID);
                return categories;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while fetching category with ID {categoryID}.", ex);
            }
        }

        public static (bool success, int categoryID) CreateCategory(CategoryDTO category)
        {
            try
            {
                return clsCategoryData.CreateCategory(category);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while creating category.", ex);
            }
        }

        public static bool UpdateCategory(CategoryDTO category)
        {
            try
            {
                return clsCategoryData.UpdateCategory(category);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while updating category with ID {category.CategoryID}.", ex);
            }
        }

        public static bool DeleteCategory(int categoryID)
        {
            try
            {
                return clsCategoryData.DeleteCategory(categoryID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while deleting category with ID {categoryID}.", ex);
            }
        }
    }
}
