using Microsoft.AspNetCore.Http;

namespace DAL.DTOs
{
    public class UserDTO
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string? Password { get; set; }

        public string? ImageURL { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Role { get; set; }
        public string? RefreshTokenHash { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public DateTime? RefreshTokenRevokedAt { get; set; }

        public UserDTO(int userID, string userName, string email, string? password, string? imageURL, DateTime createdAt, DateTime updatedAt, string role)
        {
            UserID = userID;
            UserName = userName;
            Email = email;
            Password = password;
            ImageURL = imageURL;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Role = role;
        }
        public UserDTO(int userID,string email,string role,string? rt,DateTime? rte,DateTime? rtr)
        {
            UserID =userID;
            Email = email;
            Role =role;
            RefreshTokenHash = rt;
            RefreshTokenExpiresAt = rte;
            RefreshTokenRevokedAt = rtr;
        }

        public UserDTO() { }



    }

    public class User_Updated_Request
    {
        public int UserID { get; set; }
        public string UserName { get; set; }    
        public IFormFile? UserImage { get; set; }
    }

    public class User_ChangePassword_Request
    {
        public int UserID { get; set; }
        public string password { get; set; }
    }

    public class User_ChangeRole_Request
    {
        public int AdminID { get; set; }
        public int UserID { get; set; }
        public bool Role { get; set; }
    }
}
