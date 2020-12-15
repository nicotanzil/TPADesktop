using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPA_Desktop_NT20_2.Models
{
    public class DataAccess
    {
        public Employee GetEmployee(string key)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                return (from x in db.Employees
                        where x.EmployeeId == key
                        select x).FirstOrDefault(); 
            }
        }

        public Item GetItem(string key)
        {
            using (KongBuBankEntities db = new KongBuBankEntities())
            {
                return (from x in db.Items
                        where x.ItemId == key
                        select x).FirstOrDefault();
            }
        }
    }
}
