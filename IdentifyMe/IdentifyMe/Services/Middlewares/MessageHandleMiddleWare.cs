using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hyperledger.Aries.Models.Records;
using Hyperledger.Aries.Storage;
using Plugin.LocalNotification;

namespace IdentifyMe.Services.Middlewares
{
    public class MessageHandleMiddleWare : IAgentMiddleware
    {
        private readonly IConnectionService _connectionService;
        private readonly IWalletRecordService _recordSerice;
        private readonly ICredentialService _credentialService;
        public MessageHandleMiddleWare(ICredentialService credentialService, IWalletRecordService recordService)
        {
            _credentialService = credentialService;
            _recordSerice = recordService;
        }

        private string GetMessageContent(string type)
        {
            //Check type to show message:
            switch(type)
            {
                case "Hello":
                    return "Hello world!";
                default:
                    return "You've got a message from mediator";
            }
        }

        public async Task OnMessageAsync(IAgentContext agentContext, UnpackedMessageContext messageContext)
        {
            await _credentialService.ListAsync(agentContext);
            //if (messageContext.GetMessageType() == MessageTypes.IssueCredentialNames.OfferCredential)
            //{
            //    await _credentialService.ListAsync(agentContext);
            //    var wallet = agentContext.Wallet;
            //    CredentialOfferMessage msg = messageContext.GetMessage<CredentialOfferMessage>();
            //    if (messageContext.ContextRecord != null)
            //    {
            //        CredentialRecord credentialRecord = (CredentialRecord)messageContext.ContextRecord;



            //        Console.WriteLine("Accept Credential");




            //        if (credentialRecord.State == Hyperledger.Aries.Features.IssueCredential.CredentialState.Offered)
            //        {
            //            var (request, record) = await _credentialService.CreateRequestAsync(agentContext, credentialRecord.Id);
            //            string res = await _credentialService.ProcessCredentialRequestAsync(agentContext, request, messageContext.Connection);
            //            //await _recordSerice.AddAsync<CredentialRecord>(wallet, credentialRecord);



            //            //var (request, record) = await _credentialService.CreateRequestAsync(agentContext, credentialRecord.CredentialId);
            //            //await _credentialService.CreateRequestAsync(agentContext, msg);



            //            //string res = await _credentialService.ProcessOfferAsync(agentContext, msg, messageContext.Connection);



            //            //var rc = _credentialService.GetAsync(agentContext, record.CredentialId);
            //            Console.WriteLine($"Process Result: {res}");



            //        }



            //    }
            //    // CredentialOfferMessage credentialOfferMessage = messageContext.Payload();
            //    //messageContext.Payload
            //    //_credentialService.ProcessOfferAsync(agentContext, messageContext.ContextRecord, messageContext.Connection);
            //}
            var messageType = messageContext.GetMessageType();
            var content = GetMessageContent(messageType);
            var notification = new NotificationRequest
            {
                NotificationId = 100,
                Title = "Test",
                Description = content,
                ReturningData = "Dummy data", // Returning data when tapped on notification.
            };
            NotificationCenter.Current.Show(notification);
            Console.WriteLine($"thinhnnd - Message Type: {messageContext.GetMessageType()}");
        }
    }
}