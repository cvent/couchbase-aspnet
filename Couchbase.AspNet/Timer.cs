using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Newtonsoft.Json;

namespace Couchbase.AspNet
{
    class Timer
    {
        private DateTime _start, _end;
        private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // To Start the timer
        public void Start()
        {
            _start = DateTime.Now;
        }

        // To stop the timer
        public void End()
        {
            _end = DateTime.Now;
        }

        public double GetTotalMilliseconds()
        {
            return (_end - _start).TotalMilliseconds;
        } 


        //Log creator
        //message: description about the time logged, eg: SET Header, GET Header etc.
        //id: SessionID
        public void WriteLog(string message, string id = "Not specified")
        {

            if (_log.IsDebugEnabled)
            {
                _log.Debug(JsonConvert.SerializeObject(
                    new
                    {
                        Key = id,
                        Type = message,
                        Time = GetTotalMilliseconds()
                    }));
            }
        }
    }
}
