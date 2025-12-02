using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    public static DebugUI Instance { get; private set; }
    public GameObject debugUI;
    public PlayerCheat playerCheat;

    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;
    public Button button7;
    public Button button8;
    public Button button9;
    public Button button10;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
    }

    public void ToggleDebugUI()
    {
        debugUI.SetActive(!debugUI.activeSelf);
        playerCheat = PlayerNetManager.Instance.localCharacterRuntime.gameObject.GetComponent<PlayerCheat>();
        button1.onClick.AddListener(() => playerCheat.AddAttributePoint());
        button2.onClick.AddListener(() => playerCheat.AddExp());
        button3.onClick.AddListener(() => playerCheat.AddMoney());
        button4.onClick.AddListener(() => playerCheat.DealDamageToSelf());
        button5.onClick.AddListener(() => playerCheat.ClearEnemy());
        button6.onClick.AddListener(() => playerCheat.GoToCombatNode());
        button7.onClick.AddListener(() => playerCheat.GotoTreasureNode());
        button8.onClick.AddListener(() => playerCheat.GotoShopNode());
        button9.onClick.AddListener(() => playerCheat.GotoBossNode());
        button10.onClick.AddListener(() => playerCheat.GotoNextLevel());
    }
}
