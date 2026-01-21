using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTos
{
    public class CategoryDTO
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImage { get; set; }

        public CategoryDTO(int categoryID, string categoryName, string categoryImage)
        {
            CategoryID = categoryID;
            CategoryName = categoryName;
            CategoryImage = categoryImage;
        }
    }
}
