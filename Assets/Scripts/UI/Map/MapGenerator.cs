using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 5;
    [SerializeField] private float xSpacing = 3f;
    [SerializeField] private float ySpacing = 3f;
    [SerializeField] private int minNodeCanSpawn = 2;

    [Header("References")]
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private Transform nodeContainer;
    [SerializeField] private UIDottedLineDrawer lineDrawer;
    [SerializeField] private Transform dotLineContainer;
    [SerializeField] PlayerNet playerNet;

    [SerializeField] private Button confirmButton;

    private MapNode currentNode;
    private MapNode currentSelectedNode;
    private List<List<MapNode>> layers = new();

    private SFXData ClickSFX => ResourceManager.Instance.GetAsset<SFXData>("UIButtonSFX");
    private SFXData WindSFX => ResourceManager.Instance.GetAsset<SFXData>("SelectNodeWindSFX");

    private void Start()
    {
        GenerateMap();
        confirmButton.onClick.AddListener(OnConfirmButtonPressed);
    }

    private void GenerateMap()
    {
        layers.Clear();

        for (int col = 0; col < width / 2; col++)
        {
            int nodeCount = Random.Range(minNodeCanSpawn, height + 1);
            if (col == 0 || col == (width / 2) - 1) nodeCount = 1;

            List<MapNode> layer = new List<MapNode>();

            for (int row = 0; row < nodeCount; row++)
            {
                MapNodeData data = new MapNodeData();
                data.gridPos = new Vector2Int(col, row);

                if (col == 0)
                    data.nodeType = ResourceManager.Instance.GetAsset<NodeType>("SeaNode");
                else if (col == (width / 2) - 1)
                    data.nodeType = ResourceManager.Instance.GetAsset<NodeType>("BossNode");
                else
                    data.nodeType = GetRandomMapNode();

                if (col == 1)
                {
                    while (data.nodeType.NodeTypes == NodeTypes.Shop)
                    {
                        data.nodeType = GetRandomMapNode();
                    }
                }   

                GameObject nodeObj = Instantiate(nodePrefab, nodeContainer);

                float totalHeight = (nodeCount - 1) * ySpacing;
                float yOffset = -totalHeight / 2f;
                nodeObj.transform.localPosition = new Vector2(col * xSpacing, row * ySpacing + yOffset);

                MapNode mapNode = nodeObj.GetComponent<MapNode>();
                mapNode.Init(data);
                mapNode.OnNodeClicked += OnNodeClicked;

                if (col == 0 && row == 0)
                {
                    mapNode.SetInteractable(false);
                    currentNode = mapNode;
                    currentNode.MarkAsCurrent(true);
                }
                else if (col == 1)
                {
                    mapNode.SetInteractable(true);
                }
                else
                {
                    mapNode.SetInteractable(false);
                    currentNode.MarkAsCurrent(true);
                }

                layer.Add(mapNode);
            }


            layers.Add(layer);
        }

        for (int col = 0; col < layers.Count - 1; col++)
        {
            var currentColumn = layers[col];
            var nextColumn = layers[col + 1];

            float maxYDistance = 0.75f * ySpacing;

            foreach (var startNode in currentColumn)
            {
                List<MapNode> validNext = new List<MapNode>();
                foreach (var endNode in nextColumn)
                {
                    float dy = Mathf.Abs(startNode.transform.localPosition.y - endNode.transform.localPosition.y);
                    if (dy <= maxYDistance)
                        validNext.Add(endNode);
                }

                if (validNext.Count == 0)
                {
                    MapNode closest = nextColumn[0];
                    float minDy = Mathf.Abs(startNode.transform.localPosition.y - closest.transform.localPosition.y);
                    foreach (var endNode in nextColumn)
                    {
                        float dy = Mathf.Abs(startNode.transform.localPosition.y - endNode.transform.localPosition.y);
                        if (dy < minDy)
                        {
                            minDy = dy;
                            closest = endNode;
                        }
                    }
                    validNext.Add(closest);
                }

                var shuffled = new List<MapNode>(validNext);
                Shuffle(shuffled);
                int connectionCount = Mathf.Min(Random.Range(1, 2), shuffled.Count);

                for (int i = 0; i < connectionCount; i++)
                {
                    var endNode = shuffled[i];
                    if (!startNode.linkedNode.Contains(endNode))
                    {
                        startNode.linkedNode.Add(endNode);
                        lineDrawer.DrawDottedLine(startNode.GetComponent<RectTransform>(),
                                                  endNode.GetComponent<RectTransform>());
                    }
                }
            }

            foreach (var endNode in nextColumn)
            {
                bool hasIncoming = false;
                foreach (var startNode in currentColumn)
                    if (startNode.linkedNode.Contains(endNode))
                        hasIncoming = true;

                if (!hasIncoming)
                {
                    MapNode closest = currentColumn[0];
                    float minDy = Mathf.Abs(currentColumn[0].transform.localPosition.y - endNode.transform.localPosition.y);
                    foreach (var startNode in currentColumn)
                    {
                        float dy = Mathf.Abs(startNode.transform.localPosition.y - endNode.transform.localPosition.y);
                        if (dy < minDy)
                        {
                            minDy = dy;
                            closest = startNode;
                        }
                    }

                    closest.linkedNode.Add(endNode);
                    lineDrawer.DrawDottedLine(closest.GetComponent<RectTransform>(),
                                              endNode.GetComponent<RectTransform>());
                }
            }
        }
    }

    private NodeType GetRandomMapNode()
    {
        NodeTypeList nodeTypeList = ResourceManager.Instance.GetAsset<NodeTypeList>("NodeTypeList");
        int index = Random.Range(0, nodeTypeList.NodeTypes.Count);
        NodeType node = nodeTypeList.NodeTypes[index];
        int Ran = Random.Range(0, 101);
        if (Ran <= node.rate)
        {
            return node;
        }
        else
        {
            node = GetRandomMapNode();
        }
        return node;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[rnd];
            list[rnd] = temp;
        }
    }

    private void OnNodeClicked(MapNode node)
    {
        if (!node.IsInteractable) return;

        if (currentSelectedNode != null)
            currentSelectedNode.MaskAsSelected(false);

        node.MaskAsSelected(true);
        currentSelectedNode = node;
        confirmButton.gameObject.SetActive(true);
    }

    private void OnConfirmButtonPressed()
    {
        if (currentSelectedNode != null)
        {
            OnNodeConfirm(currentSelectedNode);
            SendRequest(currentSelectedNode);
            currentSelectedNode = null;
            SFXManager.Instance.Play(ClickSFX, transform.position);
        }
    }

    private void SendRequest(MapNode mapNode)
    {
        playerNet.ChangeGameMapRequest(mapNode.nodeData.nodeType.NodeTypes);
    }

    private void OnNodeConfirm(MapNode node)
    {
        SFXManager.Instance.Play(ClickSFX, transform.position);

        confirmButton.gameObject.SetActive(false);

        currentNode = node;
        currentNode.MarkAsCurrent(true);

        int colIndex = node.nodeData.gridPos.x;

        foreach (var n in layers[colIndex])
            n.SetInteractable(false);

        if (colIndex + 1 < layers.Count)
        {
            foreach (var next in layers[colIndex + 1])
            {
                if (currentNode.linkedNode.Contains(next))
                    next.SetInteractable(true);
            }
        }

        gameObject.transform.parent.gameObject.SetActive(false);

        SFXManager.Instance.Play(WindSFX, transform.position);
    }
}
