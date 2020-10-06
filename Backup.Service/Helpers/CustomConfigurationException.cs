using System;

namespace Scheduling.Service.Helpers
{
    [Serializable]
    public class CustomConfigurationException: Exception
    {
        public CustomConfigurationException() { }

        public CustomConfigurationException(string message): base(message) { }
    }
}
