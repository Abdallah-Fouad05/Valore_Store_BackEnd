using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTos;
using DAL;

namespace BAL
{
        public class StatusBusiness
        {
            public static async Task<List<StatusDTO>> GetAllStatus()
            {
                try
                {
                    return await clsStatusData.GetAllStatus();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Service error while fetching all statuses.", ex);
                }
            }

            public static async Task<StatusDTO?> GetStatusByID(int statusID)
            {
                try
                {
                    return await clsStatusData.GetStatusByID(statusID);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Service error while fetching status with ID {statusID}.", ex);
                }
            }

            public static (bool success, int statusID) CreateStatus(StatusDTO status)
            {
                try
                {
                    return clsStatusData.CreateStatus(status);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Service error while creating status.", ex);
                }
            }

            public static bool UpdateStatus(StatusDTO status)
            {
                try
                {
                    return clsStatusData.UpdateStatus(status);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Service error while updating status with ID {status.StatusID}.", ex);
                }
            }

            public static bool DeleteStatus(int statusID)
            {
                try
                {
                    return clsStatusData.DeleteStatus(statusID);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Service error while deleting status with ID {statusID}.", ex);
                }
            }
        }
    }

