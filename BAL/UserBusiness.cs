using DAL;
using DAL.DTos;
using DAL.DTOs;

namespace BAL
{
    public class UserBusiness
    {
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

        public static async Task<UserLogInResult?> LogIn(UserLogInDTO user)
        {
            try
            {
                return await clsUserData.LogIn(user);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while logging in.", ex);
            }
        }

        public static CreatedUser SignUp(UserSignUpDTO user)
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
    }
}
