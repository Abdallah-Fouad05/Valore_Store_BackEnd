using DAL;
using DAL.DTos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL
{
    public class OrderItemBusiness
    {
        // Create OrderItem
        public static (bool success, int orderItemID) CreateOrderItem(OrderItemDTO_Created orderItem)
        {
            try
            {
                return clsOrderItemData.CreateOrderItem(orderItem);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while creating order item.", ex);
            }
        }

        // Update OrderItem
        public static bool UpdateOrderItem(OrderItemDTO_Updated orderItem)
        {
            try
            {
                return clsOrderItemData.UpdateOrderItem(orderItem);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while updating order item with ID {orderItem.OrderItemID}.", ex);
            }
        }

        // Delete OrderItem
        public static bool DeleteOrderItem(int orderItemID)
        {
            try
            {
                return clsOrderItemData.DeleteOrderItem(orderItemID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while deleting order item with ID {orderItemID}.", ex);
            }
        }
    }
}
