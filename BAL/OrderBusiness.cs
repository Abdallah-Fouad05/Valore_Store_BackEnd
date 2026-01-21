using DAL;
using DAL.DTos;
using System;
using System.Threading.Tasks;

namespace BAL
{
    public class OrderBusiness
    {
        public static async Task<orderDTO_Details?> GetOrdersByUserID(int userID)
        {
            try
            {
                return await clsOrderData.GetOrdersByUserID(userID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Service error while fetching orders for user ID {userID}.", ex);
            }
        }

        public static (bool success, int orderID) CreateOrder(OrderDTO_Created order)
        {
            try
            {
                return clsOrderData.CreateOrder(order);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while creating order.", ex);
            }
        }

        public static bool UpdateOrderStatus(OrderDTO_Updated order)
        {
            try
            {
                return clsOrderData.UpdateOrder(order);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Service error while updating order with ID {order.OrderID}.", ex);
            }
        }

        public static List<OrderItemDTO_Datelis> GetOrderItems(int userID)
        {
            try
            {
                return clsOrderData.GetOrderItems(userID); // Assuming your DAL class name
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Service error while fetching orders items for user ID {userID}.", ex);
            }
        }
    }
}
