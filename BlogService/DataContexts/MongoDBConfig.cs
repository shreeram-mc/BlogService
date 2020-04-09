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
            }
            set { _connectionString = value; }
        }

        public string Container { get; set; }

        public bool IsContained { get; set; }

        public bool Development { get; set; }

        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

    }   
}
