﻿using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ApplicationServices.Logger
    {
        public class Logger : ILogger
        {
            private ILog log;

            public Logger()
            {
                log = LogManager.GetLogger("Logger");
            }

            public void Log(string msg, string fileName)
            {
                log.Info(msg);
            }

            public void Log(string msg, string fileName, Exception ex)
            {
                log.Error(msg, ex);
            }

            public void Log(string msg, string fileName, Exception ex, int level)
            {
                var message = "[Niveau " + level + "] - " + msg;
                switch (level)
                {
                    case 1: log.Error(message, ex); break;
                    case 2: log.Warn(message, ex); break;
                    default:
                        log.Info(message, ex); break;
                }
            }

            public void Log(string msg, string fileName, int level)
            {
                var message = "[Niveau " + level + "] - " + msg;
                switch (level)
                {
                    case 1: log.Error(message); break;
                    case 2: log.Warn(message); break;
                    default:
                        log.Info(message); break;
                }
            }
        }

    }
