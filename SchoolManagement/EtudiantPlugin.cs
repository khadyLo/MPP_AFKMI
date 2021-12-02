using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement
{
    public class EtudiantPlugin : IPlugin
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
                            Entity etudiantEntity = executionContext.InputParameters["Target"] as Entity;
                            // Récupérer le compteur courant sur la candidature
                            QueryExpression query = new QueryExpression();
                            query.EntityName = "t344_compteur";
                            query.ColumnSet = new ColumnSet(new string[] { "t344_compteur", "t344_separateur", "t344_table" });
                            query.Criteria = new FilterExpression();
                            query.Criteria.AddCondition(new ConditionExpression("t344_table", ConditionOperator.Equal, "Etudiant"));
                            
                            EntityCollection entityCollection = organizationService.RetrieveMultiple(query);

                            if (entityCollection.Entities.Count == 0 || entityCollection.Entities.Count > 1)
                            {
                                throw new InvalidPluginExecutionException("La collection doit retourner une seule valeur");
                            }

                            Entity compteur = entityCollection.Entities[0];
                            
                            string nomEtudiant = etudiantEntity.GetAttributeValue<string>("t344_nom");
                            string prenomEtudiant = etudiantEntity.GetAttributeValue<string>("t344_prenom");
                            DateTime dateNaissance = etudiantEntity.GetAttributeValue<DateTime>("t344_datedenaissance");

                            DateTime tiko = new DateTime();
                            serviceTrace.Trace("Date naissance => " +dateNaissance.ToString());
                            serviceTrace.Trace("Date naissance Tiko Ticket => " +tiko.ToString());

                            string month = dateNaissance.Month.ToString("00");

                            string initial = nomEtudiant.Substring(0,1) + prenomEtudiant.Substring(0,1);
                            string separateur = compteur.GetAttributeValue<string>("t344_separateur");
                            int oldCompteur = compteur.GetAttributeValue<int>("t344_compteur");

                            int newCompteur = ++oldCompteur;

                            string numbEtudiant = initial + separateur + month + separateur + newCompteur.ToString("000");

                            etudiantEntity["t344_numeroetudiant"] = numbEtudiant;
                            compteur["t344_compteur"] = newCompteur;
                            organizationService.Update(compteur);
                   
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
