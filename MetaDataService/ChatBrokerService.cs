using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using EasyNetQ.Management.Client;

namespace MetaDataService
{
    public class ChatBrokerService
    {

        private ManagementClient manager; 

        public  ChatBrokerService(String host, String user, String password)
        {
            manager = new ManagementClient(host, user, password);
        }
                

        public HashSet<String> getChatterNames()
        {          
            // Using Hashset to avoid adding dupes
            HashSet<String> chatterSet = null;
            if (manager != null)
            {
                var conns = manager.GetConnections();
                if (conns != null && conns.Count() > 0)
                {
                    chatterSet = new HashSet<string>();
                    foreach (var conn in conns)
                    {
                        var chatter = conn.ClientProperties.PropertiesDictionary["Chatter"] != null ? conn.ClientProperties.PropertiesDictionary["Chatter"].ToString() : String.Empty;
                        if (!String.IsNullOrEmpty(chatter))
                        {
                            chatterSet.Add(chatter);
                        }
                    
                    }
                }
            }

            return chatterSet;            
        }


        public Boolean isNameAvailable(String name)
        {
            if (getChatterNames() != null)
            {
                return !getChatterNames().Contains(name);
            }
            return true;
        }

        



    }
}
