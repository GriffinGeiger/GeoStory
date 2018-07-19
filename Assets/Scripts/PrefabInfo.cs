using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabInfo : MonoBehaviour
{
    public enum PrefabType { ScrollArea, Button, BackgroundImage,ElementNode, NodeHeader,
        NodeBody, NodeBodyImage, NodeFooter, NodeConnector, NodeConnectorReceiver, PageNodeConnectorReceiver  };
    public PrefabType prefabType;
}
