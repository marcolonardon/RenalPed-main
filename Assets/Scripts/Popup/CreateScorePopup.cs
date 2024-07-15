using UnityEngine;
using UnityEngine.UI;

public class CreateScorePopup : MonoBehaviour
{
    public static CreateScorePopup Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OpenScorePopup);
        }
    }


    //Para chamar em outro script  CreateScorePopup.Instance.OpenScorePopup();
    public void OpenScorePopup()
    {
        Debug.Log("Opening Score Popup");
        ScorePopup popup = UIController.Instance.CreateScorePopup();
        popup.Init(UIController.Instance.MainCanvas);
    }
}
