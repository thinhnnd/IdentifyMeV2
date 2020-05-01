﻿using Acr.UserDialogs;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using IdentifyMe.Services.Interfaces;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IdentifyMe.ViewModels.Credentials
{
    public class CredentialViewModel : ABaseViewModel
    {
        private readonly CredentialRecord _credential;
        private readonly IConnectionService _connectionService;
        private readonly IAgentProvider _agentProvider;

        public CredentialViewModel(IUserDialogs userDialogs,
            INavigationService navigationService,
            CredentialRecord credential,
            IAgentProvider agentProvider,
            IConnectionService connectionService) :
            base(nameof(CredentialViewModel), userDialogs, navigationService)
        {
            _connectionService = connectionService;
            _agentProvider = agentProvider;

            Title = "Credential Detail";
            _credential = credential;
            _attributes = credential.CredentialAttributesValues;
            _credId = _credential.Id;
            _state = _credential.State;
            _credentialName = ConvertNameFromeSchemaId(_credential.SchemaId).ToTitleCase();
            if (_credential.CreatedAtUtc != null)
            {
                _issuedDate = _credential.CreatedAtUtc.ToString();
            }
        }

        public async override Task InitializeAsync(object navigationData)
        {
            await base.InitializeAsync(navigationData);
            var context = await _agentProvider.GetContextAsync();
            _relatedConnection = await _connectionService.GetAsync(context, this._credential.ConnectionId);
        }

        private string ConvertNameFromeSchemaId(string schemaId)
        {
            var arr = schemaId.Split(new string[] { ":" }, StringSplitOptions.None);
            if (arr[2] != null)
            {
                return arr[2];
            }
            return "NoName";
        }

        private ConnectionRecord _relatedConnection;

        public ConnectionRecord RelatedConnection
        {
            get => _relatedConnection;
            set => this.RaiseAndSetIfChanged(ref _relatedConnection, value);
        }

        private CredentialRecord _credentialRecord;

        public CredentialRecord CredentialRecord
        {
            get => _credentialRecord;
            set
            {
                this.RaiseAndSetIfChanged(ref _credentialRecord, value);
            }
        }

        private string _issuedDate;
        public string IssuedDate
        {
            get => _issuedDate;
            set => this.RaiseAndSetIfChanged(ref _issuedDate, value);
        }

        private string _credId;

        public string Id
        {
            get => _credId;
            set => this.RaiseAndSetIfChanged(ref _credId, value);
        }

        private CredentialState _state;

        public CredentialState State
        {
            get => _state;
            set => this.RaiseAndSetIfChanged(ref _state, value);
        }

        private string _credentialName;
        public string CredentialName
        {
            get => _credentialName;
            set => this.RaiseAndSetIfChanged(ref _credentialName, value);
        }

        private string _credentialType;
        public string CredentialType
        {
            get => _credentialType;
            set => this.RaiseAndSetIfChanged(ref _credentialType, value);
        }

        private string _credentialImageUrl;
        public string CredentialImageUrl
        {
            get => _credentialImageUrl;
            set => this.RaiseAndSetIfChanged(ref _credentialImageUrl, value);
        }

        private string _credentialSubtitle;
        public string CredentialSubtitle
        {
            get => _credentialSubtitle;
            set => this.RaiseAndSetIfChanged(ref _credentialSubtitle, value);
        }

        private bool _isNew;
        public bool IsNew
        {
            get => _isNew;
            set => this.RaiseAndSetIfChanged(ref _isNew, value);
        }

        private string _qRImageUrl;
        public string QRImageUrl
        {
            get => _qRImageUrl;
            set => this.RaiseAndSetIfChanged(ref _qRImageUrl, value);
        }

        private IEnumerable<CredentialPreviewAttribute> _attributes;
        public IEnumerable<CredentialPreviewAttribute> Attributes
        {
            get => _attributes;
            set => this.RaiseAndSetIfChanged(ref _attributes, value);
        }

        #region Command

        #endregion
    }
}
