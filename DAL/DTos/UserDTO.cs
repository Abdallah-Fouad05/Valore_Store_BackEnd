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

        public bool IsAdmin { get; set; }

        public UserDTO(int userID, string userName, string email, string? password, string? imageURL, DateTime createdAt, DateTime updatedAt, bool isAdmin)
        {
            UserID = userID;
            UserName = userName;
            Email = email;
            Password = password;
            ImageURL = imageURL;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            IsAdmin = isAdmin;
        }
    }
}
