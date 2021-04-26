namespace Infrastructure.Logging
{
    public class LoggerFactory
    {
        /// <summary>
        /// Factory Class to get the instance of the logger implementation 
        /// </summary>
        private static ILogger _logger;

        /// <summary>
        /// Initializes the log factory.
        /// </summary>
        /// <param name="logger">The logger contract.</param>
        public static void InitializeLogFactory(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets the logger instance.
        /// </summary>
        /// <returns></returns>
        public static ILogger GetLogger()
        {
            return _logger;
        }
    }
}