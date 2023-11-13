using Microsoft.Extensions.Logging;
using CafeShopFPT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeShopFPT.LogUlti {
    public class Logger {
        private static volatile Logger instance;
        private static object syncRoot = new Object();

        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Logger));

        public static Logger Instance {
            get {
                if (instance == null) {
                    lock (syncRoot) {
                        if (instance == null)
                            instance = new Logger();
                    }
                }

                return instance;
            }
        }
    }
}
