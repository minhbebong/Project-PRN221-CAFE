using log4net;
using Microsoft.EntityFrameworkCore;
using CafeShopFPT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CafeShopFPT.DAO.RoleDao {
    public class RoleDao {
        private static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static RoleDao instance;

        public static RoleDao Instance {
            get {
                if (instance == null) {
                    instance = new RoleDao();
                }
                return instance;
            }
            private set => instance = value;
        }

        private RoleDao() {
        }




        public List<RoleDto> LoadAllRoles() {
            try {
                var ObjTableList = from role in DataProvider.Ins.DB.Roles
                                   select new RoleDto {
                                  Id= role.Id,
                                  Name= role.Name,
                                   };
                return ObjTableList.ToList();
            } catch (System.Exception) {

                throw;
            }
        }

    }
}


