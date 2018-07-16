using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabInfo : MonoBehaviour
{
    public enum PrefabType { ScrollArea, Button, BackgroundImage, NodeHeader,
        NodeBody, NodeBodyImage, NodeFooter, NodeConnector, NodeConnectorReceiver, PageNodeConnectorReceiver  };
    public PrefabType prefabType;
    public PageElementEventTrigger.Action buttonAction; //Check to see if this is necessary, I think button action is thoroughly handled in pageElementEventTrigger
}
