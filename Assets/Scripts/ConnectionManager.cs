using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public List<Connection> connections;

    public struct Connection
    {
        string originPageName;
        int originElementIndex;
        int selectionConnectorIndex;
        string connectedPageName;  //if empty then no connection
        int connectedElementIndex; //if -1 then its connected to a pageConnector

        //Creates connections from the SelectionConnectorManagers
     /*   public Connection(SelectionConnectorManager originConnector, SelectionConnectorManager receivingConnector)
        {
            originElementIndex = originConnector.connectionKey;
        }
        */
    }

    public void makeConnection()
    {

    }
}
