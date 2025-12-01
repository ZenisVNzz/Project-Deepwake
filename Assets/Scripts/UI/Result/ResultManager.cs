using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI goldObtained;
    public TextMeshProUGUI itemObtained;
    public TextMeshProUGUI enemyDefeated;
    public TextMeshProUGUI totalDamageDealt;
    public TextMeshProUGUI totalDamageTaken;

    public List<Node> nodes = new();

    public Button confirmButton;

    public PlayerArchiveData data;

    private void Awake()
    {
        data = PlayerNetManager.Instance.localCharacterRuntime.gameObject.GetComponent<PlayerArchiveData>();
        confirmButton.onClick.AddListener(OnConfirmButtonPressed);
        Init();
    }

    private void Init()
    {
        goldObtained.text = $"{data.goldObtained}";
        itemObtained.text = $"{data.itemObtained}";
        enemyDefeated.text = $"{data.enemyDefeated}";
        totalDamageDealt.text = $"{data.totalDamageDealt.ToString("F1")}";
        totalDamageTaken.text = $"{data.totalDamageTaken.ToString("F1")}";

        int currentLevel = GameController.Instance.CurrentLevel;
        int currentNode = GameController.Instance.CurrentNode;
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].index.text = $"{currentLevel} - {i + 1}";
            if (i + 1 == currentNode)
            {
                nodes[i].mask.SetActive(true);
            }
        }

        data.gameObject.GetComponent<CharacterUIManager>().ToggleUICanvas();
    }

    public void OnConfirmButtonPressed()
    {
        _ = SceneLoader.Instance.LoadScene("Title", false);
    }
}
