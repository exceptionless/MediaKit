using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using NLog;

namespace ExceptionDrivenDemo.Controllers {
    public class ValuesController : ApiController {
        private readonly Logger _logger = LogManager.GetLogger("ValuesController");

        // GET api/values
        public IEnumerable<string> Get() {
            _logger.Log(LogLevel.Trace, "GET()");
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id) {
            _logger.Log(LogLevel.Trace, $"GET({id})");
            if (id == 1)
                File.Open(id.ToString(), FileMode.Open);

            return "value" + id;
        }
    }
}