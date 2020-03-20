namespace BlogService.DataContexts
{
    public class Settings
    {
        public string Database { get; set; }
      
        private string _connectionString = string.Empty;
        public string ConnectionString
        {
            get
            {
                if (IsContained && Development)
                    return Container;

                return _connectionString;

                //return $@"mongodb://root:example@127.0.0.1";
            }
            set { _connectionString = value; }
        }

        public string Container { get; set; }

        public bool IsContained { get; set; }

        public bool Development { get; set; }

    }

    public class MongoDBConfig
    {
        public string Database { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string ConnectionString
        {
            get
            {

               // return "mongodb://root:example@172.18.0.2:27017/blogdb";

                if (string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Password))
                    return $@"mongodb://{Host}:{Port}";

                return $@"mongodb://{User}:{Password}@{Host}:{Port}";
            }
        }
    }
}
