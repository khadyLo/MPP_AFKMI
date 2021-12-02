using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement
{
    internal class DevoirPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext executionContext = serviceProvider.GetService(typeof(IPluginExecutionContext)) as IPluginExecutionContext;
            ITracingService serviceTrace = serviceProvider.GetService(typeof(ITracingService)) as ITracingService;
            IOrganizationServiceFactory factory = serviceProvider.GetService(typeof(IOrganizationServiceFactory)) as IOrganizationServiceFactory;

            IOrganizationService organizationService = factory.CreateOrganizationService(executionContext.UserId) as IOrganizationService;

            try
            {
                switch(executionContext.MessageName.ToLower())
                {
                    case "create":
                        
                        break;


                }
                    
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
