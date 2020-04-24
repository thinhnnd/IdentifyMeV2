using System;
using System.Collections.Generic;
using System.Text;

namespace IdentifyMe.Events
{

    public enum ApplicationEventType
    {
        ConnectionsUpdated,
        CredentialsUpdated
    }
    public class ApplicationEvent
    {
        public ApplicationEventType Type { get; set; }

    }
}
