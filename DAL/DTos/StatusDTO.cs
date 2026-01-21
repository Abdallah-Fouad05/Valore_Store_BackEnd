using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTos
{
    public class StatusDTO
    {
        public int StatusID { get; set; }
        public string StatusName { get; set; }

        public StatusDTO(int statusID, string statusName)
        {
            StatusID = statusID;
            StatusName = statusName;
        }
    }
}
