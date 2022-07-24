using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitter.Core.Extentions
{
    public static class LoggingExtensions
    {
        
        public static void LogWithParameters(this ILogger logger, LogLevel logLevel, string message, Dictionary<string, object> parameters)
        {
            LogWithParameters(logger, logLevel, null, message, parameters);
        }

       
        public static void LogWithParameters(this ILogger logger, LogLevel logLevel, Exception exception, string message, Dictionary<string, object> parameters)
        {
            List<object> parametersArray = new List<object>();
            var paramLog = "";

            if (parameters != null && parameters.Count > 0)
            {
                // Prepend the parameters to the log entry's message.
                var count = 0;
                paramLog += "["; // Start the param log with a '['
                foreach (var param in parameters)
                {
                    count++;
                    if (count > 1)
                    {
                        paramLog += " | "; // Add a pipe seperator between each param.
                    }
                    parametersArray.Add(string.Format("{0}: {1}", param.Key, param.Value.ToString()));  // Add the key and value to parametersArray
                    paramLog += "{Param" + count + "}"; // Add the placeholder.
                }
                paramLog += "] :: "; // End the param log with a ']' and two ':'.
            }

            // Log the entry with parameters and the message.
            logger.Log(logLevel, exception, string.Format("{0}{1}", paramLog, message), parametersArray.ToArray());
        }
    }
}

    

