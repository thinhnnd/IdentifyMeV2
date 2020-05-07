using Acr.UserDialogs;
using Hyperledger.Aries.Agents;
using IdentifyMe.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentifyMe.ViewModels.Setting
{
    public class SettingViewModel : ABaseViewModel
    {
        public SettingViewModel(IUserDialogs userDialogs, 
            INavigationService navigationService, IAgentContext agentContext, IAgentProvider agentProvider) : 
            base (nameof(SettingViewModel), userDialogs, navigationService)
        {
            Title = "Setting";
        }

    }
}
