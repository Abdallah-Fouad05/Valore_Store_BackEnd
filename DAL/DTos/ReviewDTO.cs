using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTOs;

namespace DAL.DTos
{
    public class ReviewDTO
    {
        public int ReviewID { get; set; }
        public int UsertID { get; set; }
        public int ProductID { get; set; }
        public float Ratting { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public ReviewDTO(int reviewID, int usertID, int productID, float ratting, string comment, DateTime createdAt)
        {
            ReviewID = reviewID;
            UsertID = usertID;
            ProductID = productID;
            Ratting = ratting;
            Comment = comment;
            CreatedAt = createdAt;
        }
    }
    public class ReviewDatailsDTO
    {
        public int ReviewID { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string Title { get; set; }
        public float Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public ReviewDatailsDTO(
            int reviewID,
            string userName,
            string userImage,
            string productName,
            string productImage,
            string title,
            float rating,
            string comment,
            DateTime createdAt)
        {
            ReviewID = reviewID;
            UserName = userName;
            UserImage = userImage;
            ProductName = productName;
            ProductImage = productImage;
            Title = title;
            Rating = rating;
            Comment = comment;
            CreatedAt = createdAt;
        }
    }

}

