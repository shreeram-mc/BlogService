using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.DataContexts
{
    public class ServerConfig
    {
        public MongoDBConfig MongoDB { get; set; } = new MongoDBConfig();
    }
}
