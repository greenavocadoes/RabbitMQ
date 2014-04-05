using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using EasyNetQ.Management.Client;

namespace MessagingMetadata
{
    public class ChatUserMetadataService
    {

        private ManagementClient manager; 

        public  ChatUserMetadataService(String host, String user, String password)
        {
            manager = new ManagementClient(host, user, password);
        }
                

        public HashSet<String> getChatUserNames()
        {          
            // Using Hashset to avoid adding dupes
            HashSet<String> chatUserSet = null;
            if (manager != null)
            {
                var conns = manager.GetConnections();
                if (conns != null && conns.Count() > 0)
                {
                    chatUserSet = new HashSet<string>();
                    foreach (var conn in conns)
                    {
                        var chatUser = conn.ClientProperties.PropertiesDictionary["ChatUser"] != null ? conn.ClientProperties.PropertiesDictionary["ChatUser"].ToString() : String.Empty;
                        if (!String.IsNullOrEmpty(chatUser))
                        {
                            chatUserSet.Add(chatUser);
                        }
                    
                    }
                }
            }

            return chatUserSet;            
        }


        public Boolean isNameAvailable(String name)
        {
            if (getChatUserNames() != null)
            {
                return !getChatUserNames().Contains(name);
            }
            return true;
        }

        



    }
}
