using DAL;
using DAL.DTos;
using DAL.DTOs;

namespace BAL
{
    public class UserBusiness
    {
        public static async Task<List<UserDTO>> GetAllUsers()
        {
            try
            {
                return await clsUserData.GetAllUsers();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task<UserDTO?> GetUserByID(int userID)
        {
            try
            {
                return await clsUserData.GetUserByID(userID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Service error while fetching user with ID {userID}.", ex);
            }
        }

        public static (bool success, int userID) CreateUser(UserDTO user)
        {
            try
            {
                return clsUserData.CreateUser(user);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while creating user.", ex);
            }
        }

        public static bool UpdateUser(UserDTO user)
        {
            try
            {
                return clsUserData.UpdateUser(user);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Service error while updating user with ID {user.UserID}.", ex);
            }
        }
        public static bool ChangePassword(UserDTO user)
        {
            try
            {
                return clsUserData.ChangePassword(user);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Service error while change Password with ID {user.UserID}.", ex);
            }
        }
        public static bool DeleteUser(int userID)
        {
            try
            {
                return clsUserData.DeleteUser(userID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Service error while deleting user with ID {userID}.", ex);
            }
        }

        public static async Task<UserDTO?> GetRefreshTokenByEmail(string email)
        {
            return await clsUserData.GetRefreshTokenByEmail(email);
        }

        public static async Task<bool> UpdateRefreshToken(string email, string refreshTokenHash, DateTime expiresAt)
        {
            return await clsUserData.UpdateRefreshToken( email, refreshTokenHash, expiresAt);
        }

        public static async Task<UserDTO?> LogIn(LoginRequest user)
        {
        
                return await clsUserData.LogIn(user);
            
        }

        public static UserDTO SignUp(SignInRequest user)
        {
            try
            {
                return clsUserData.CreateUser(user);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while signing up.", ex);
            }
        }

        public static Task<bool> ChangeUserRole(int adminID,int UserID,bool role)
        {
            try
            {
                return clsUserData.ChangeUserRole(adminID, UserID, role);
            }
            catch (Exception ex) 
            {
                throw new ApplicationException("Service error while updating role");
            
            }
        }
    }
}
