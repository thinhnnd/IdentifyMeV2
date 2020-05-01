using System;
using System.Collections.Generic;
using System.Text;

namespace IdentifyMe.Events
{

    public enum ApplicationEventType
    {
        ConnectionsUpdated,
        CredentialsUpdated,
        GotCredentialOffer,
        GotProofRequestMessage
    }
    public class ApplicationEvent
    {
        public ApplicationEventType Type { get; set; }

    }
}
