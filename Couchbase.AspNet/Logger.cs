using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Couchbase.AspNet
{
    class Logger
    {
        private DateTime _start, _end;
        private static readonly log4net.ILog Log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // To Start the timer
        public void StartTimer()
        {
            _start = DateTime.Now;
        }

        // To stop the timer
        public void EndTimer()
        {
            _end = DateTime.Now;
        }

        //Log creator
        //message: description about the time logged, eg: SET Header, GET Header etc.
        //id: SessionID
        public void WriteLog(string message, string id = "Not specified")
        {
            var t = _end - _start;

            var arg = new Dictionary<string, object>
                        {
                            {"Request", message},
                            {"SessionID", id},
                            {"Milliseconds", t.TotalMilliseconds}
                        };
            Log.Debug(JsonConvert.SerializeObject((arg)));   

        }
    }
}
