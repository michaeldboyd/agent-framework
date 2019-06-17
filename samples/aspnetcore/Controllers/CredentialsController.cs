using System;
using System.Globalization;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AgentFramework.Core.Contracts;
using AgentFramework.Core.Extensions;
using AgentFramework.Core.Handlers;
using AgentFramework.Core.Messages.Connections;
using AgentFramework.Core.Models;
using AgentFramework.Core.Models.Connections;
using AgentFramework.Core.Models.Events;
using AgentFramework.Core.Models.Records.Search;
using AgentFramework.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WebAgent.Messages;
using WebAgent.Models;
using WebAgent.Protocols.BasicMessage;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAgent.Controllers
{
    public class CredentialsController : Controller
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ICredentialService _credentialService;
        private readonly IWalletService _walletService;
        private readonly IWalletRecordService _recordService;
        private readonly IProvisioningService _provisioningService;
        private readonly IAgentContextProvider _agentContextProvider;
        private readonly IMessageService _messageService;
        private readonly WalletOptions _walletOptions;

        public CredentialsController
            (
            IEventAggregator eventAggregator,
            ICredentialService credentialService,
            IWalletService walletService,
            IWalletRecordService recordService,
            IProvisioningService provisioningService,
            IAgentContextProvider agentContextProvider,
            IMessageService messageService,
            IOptions<WalletOptions> walletOptions)
        {
            _eventAggregator = eventAggregator;
            _credentialService = credentialService;
            _walletService = walletService;
            _recordService = recordService;
            _provisioningService = provisioningService;
            _agentContextProvider = agentContextProvider;
            _messageService = messageService;
            _walletOptions = walletOptions.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var context = await _agentContextProvider.GetContextAsync();

            // map credentialRecord to CredentialViewModel
            //TODO: Perhaps there is a better place to put this mapping?
            var credList = await _credentialService.ListAsync(context);
            var credVMList = new System.Collections.Generic.List<CredentialViewModel>();
            foreach (var record in credList)
            {
                credVMList.Add(new CredentialViewModel
                {
                    Name = record.TypeName,
                    State = record.State,
                    CreatedAt = record.CreatedAtUtc.Value
                });
            }
      
            return View(new CredentialsViewModel
            {
                Credentials = credVMList
            });
        }

        [HttpGet]
        public async Task<IActionResult> RequestCredential()
        {
            var context = await _agentContextProvider.GetContextAsync();

            return View();
        }
    }
}
