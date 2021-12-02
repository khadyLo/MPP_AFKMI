using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement
{
    public static class Utilities
    {
        public static string GetReferenceByCompteur(string tableCompteur, IOrganizationService organizationService)
        {
            //Récupérer le compteur courant sur la candidature
            QueryExpression query = new QueryExpression();
            query.EntityName = "t344_compteur";
            query.ColumnSet = new ColumnSet(new string[] { "t344_prefixe", "t344_compteur", "t344_separateur", "t344_table" });
            query.Criteria = new FilterExpression();
            query.Criteria.AddCondition(new ConditionExpression("t344_table", ConditionOperator.Equal, tableCompteur));

            Entity compteur = new Entity();
            int nextCompteur;
            string finalRef;

            EntityCollection entityCollection = organizationService.RetrieveMultiple(query);

            if (entityCollection.Entities.Count == 0 || entityCollection.Entities.Count > 1)
            {
                throw new InvalidPluginExecutionException("La collection doit retourner une seule valeur");
            }
            compteur = entityCollection.Entities[0];

            string prefix = compteur.GetAttributeValue<string>("t344_prefixe");
            string separateur = compteur.GetAttributeValue<string>("t344_separateur");
            int compteurCourant = compteur.GetAttributeValue<int>("t344_compteur");

            nextCompteur = ++compteurCourant;

            string day = DateTime.Now.Day.ToString("00");
            string month = DateTime.Now.Month.ToString("00");
            string year = DateTime.Now.Year.ToString("0000");

            finalRef = prefix + separateur + year + month + day + separateur + nextCompteur.ToString("000");

            //Incrémentation du compteur courant dans la table des compteurs
            compteur["t344_compteur"] = nextCompteur;
            organizationService.Update(compteur);

            return finalRef;
        }
    }
}
