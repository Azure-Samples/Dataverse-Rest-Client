namespace Dataverse.RestClient
{
    public class DataverseClientOptions
    {
        /// <summary>
        /// The Url of the environment: https://org.api.crm.dynamics.com
        /// </summary>
        public string DataverseBaseUrl { get; set; }
        /// <summary>
        /// Whether to disable Affinity cookies to gain performance
        /// </summary>
        public bool DisableCookies { get; set; } = false;
        /// <summary>
        /// The version of the service to use
        /// </summary>
        public string Version { get; set; } = "9.2";
        /// <summary>
        /// How long to wait for a timeout
        /// </summary>
        public ushort TimeoutInSeconds { get; set; } = 120;
    }
}
