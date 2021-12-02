using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement
{
    public class MatierePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {

            IPluginExecutionContext executionContext = serviceProvider.GetService(typeof(IPluginExecutionContext)) as IPluginExecutionContext;
            ITracingService serviceTrace = serviceProvider.GetService(typeof(ITracingService)) as ITracingService;
            IOrganizationServiceFactory factory = serviceProvider.GetService(typeof(IOrganizationServiceFactory)) as IOrganizationServiceFactory;

            IOrganizationService organizationService = factory.CreateOrganizationService(executionContext.UserId) as IOrganizationService;
            try
            {
                switch (executionContext.MessageName.ToLower())
                {
                    case "create":
                        if (executionContext.Stage == 20) //PréOpération
                        {
                            Entity matiereEntity = executionContext.InputParameters["Target"] as Entity;

                            matiereEntity["t344_referencematiere"] = Utilities.GetReferenceByCompteur("Matiére", organizationService);

                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.StackTrace);

            }
        }

    }
}

