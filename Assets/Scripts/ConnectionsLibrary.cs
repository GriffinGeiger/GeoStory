using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ConnectionsLibrary
    {
        //removes connections to this Page or pageElement;
        public static void removeConnectionsTo(Story currentStory,Page removePage)
        {
            foreach(Page page in currentStory.getPages())
            {
                foreach(GameObject element in page.getElements())
                {
                    PageElementEventTrigger peet = element.GetComponent<PageElementEventTrigger>();
                    foreach(KeyValuePair<int,ConnectionInfo> connection in peet.connections)
                    {
                        if (connection.Value.connectedPage.Equals(removePage))
                            peet.connections.Remove(connection.Key);
                    }
                }
            }
        }
        public static void removeConnectionsTo(Story currentStory, GameObject removeElement)
        {
            
            foreach (Page page in currentStory.getPages())
            {
                foreach (GameObject element in page.getElements())
                {
                    PageElementEventTrigger peet = element.GetComponent<PageElementEventTrigger>();
                    foreach (KeyValuePair<int, ConnectionInfo> connection in peet.connections)
                    {
                        if(connection.Value.connectedElement != null)
                            if (connection.Value.connectedElement.Equals(removeElement))
                                peet.connections.Remove(connection.Key);
                    }
                }
            }
        }
    }
}
