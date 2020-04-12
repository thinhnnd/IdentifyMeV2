using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Configuration;
using Hyperledger.Aries.Features.DidExchange;
using IdentifyMe.MVVM;
using IdentifyMe.MVVM.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentifyMe.ViewModels.Connections
{

    public class AcceptInvitationViewModel : BaseNavigationViewModel, IPopupViewModel
    {
        private ConnectionInvitationMessage _invitation;
        private IConnectionService _connectionService;
        private IMessageService _messageService;
        private readonly IProvisioningService _provisioningService;
        public AcceptInvitationViewModel(
           IConnectionService connectionService,
           IProvisioningService provisioningService,
           IMessageService messageService)
        {
            //InvitationTitle = $"Connect to {invitation.Label}?";
            //InvitationImageUrl = invitation.ImageUrl;
            //InvitationContents = $"{invitation.Label} has invited you to connect?";
            //InvitationLabel = invitation.Label;
            //_invitation = invitation;
        }

        public ConnectionInvitationMessage InvitationMessage
        {
            get => _invitation;
            set => RaiseAndUpdate(ref _invitation, value);
        }

        private string _invitationTitle;
        public string InvitationTitle
        {
            get => _invitationTitle;
            set => this.RaiseAndUpdate(ref _invitationTitle, value);
        }

        private string _invitationContents = "Someone wants to connect?";
        public string InvitationContents
        {
            get => _invitationContents;
            set => this.RaiseAndUpdate(ref _invitationContents, value);
        }

        private string _invitationImageUrl;
        public string InvitationImageUrl
        {
            get => _invitationImageUrl;
            set => this.RaiseAndUpdate(ref _invitationImageUrl, value);
        }

        private string _invitationLabel;

        public string InvitationLabel
        {
            get => _invitationLabel;
            set => this.RaiseAndUpdate(ref _invitationLabel, value);
        }
    }
  
}
