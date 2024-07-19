using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WashHandDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private const int MAXSCORE = 1000;

    public Image soap;
    public Image clickHand;
    public Image clickCircle;
    public Image[] bacterium;
    public RectTransform needleParent;
    public Button rotateButton;
    private float currentAngle = 0f;
    private const float maxAngle = 180f;
    private float targetAngle = 0f;
    public float smoothSpeed = 0.95f;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Transform initialParent;

    private const int damage = 25;
    private float damageCooldown = 0.5f;
    private Dictionary<Bacteria, float> bacteriaCooldowns = new Dictionary<Bacteria, float>();
    private int bacteriaCount;

    private int AddScore;

    private void Start()
    {
        if (PlayerPrefs.GetInt("levelAt") < 3)
            PlayerPrefs.SetInt("levelAt", 3);

        PlayerPrefs.SetInt("MaxIndex", 13);
        PlayerPrefs.SetInt("MinIndex", 9);
        rotateButton.onClick.AddListener(RotateNeedle);
        bacteriaCount = bacterium.Length;
        AddScore = PlayerPrefs.GetInt("WashScore", 0); 
    }

    void Update()
    {
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, smoothSpeed * Time.deltaTime);
        needleParent.localRotation = Quaternion.Euler(0, 0, -currentAngle);
    }

    private void OnEnable()
    {
        Bacteria.OnBacteriaDestroyed += HandleBacteriaDestroyed;
    }

    private void OnDisable()
    {
        Bacteria.OnBacteriaDestroyed -= HandleBacteriaDestroyed;
    }

    private void HandleBacteriaDestroyed(Bacteria bacteria)
    {
        bacterium = RemoveBacteriaFromArray(bacterium, bacteria.GetComponent<Image>());
        bacteriaCount--;
    }

    private Image[] RemoveBacteriaFromArray(Image[] array, Image element)
    {
        List<Image> list = new List<Image>(array);
        list.Remove(element);
        return list.ToArray();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        clickHand.gameObject.SetActive(false);
        clickCircle.gameObject.SetActive(false);
        parentAfterDrag = transform.parent;
        initialParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        soap.raycastTarget = false;
        IsDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        CheckForBacteriaCollision();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        transform.SetParent(initialParent);
        soap.raycastTarget = true;
        IsDragging = false;

        if (bacteriaCount == 0)
        {
            
            if (PlayerPrefs.GetInt("levelAt") < 3)
                PlayerPrefs.SetInt("levelAt", 3);
            PlayerPrefs.SetInt("WashScore", AddScore);
            SceneManager.LoadScene("HandSpeechBubblePage2");
        }
        else
        {
            SceneManager.LoadScene("HandSpeechBubblePage1");
            AddScore -= 100;
            if (AddScore < 0)
            {
                AddScore = 0; // Garante que a pontuação não fique negativa
            }
            PlayerPrefs.SetInt("WashScore", AddScore);
            Debug.Log("Errou. Score --> " + AddScore);
        }
    }

    private void CheckForBacteriaCollision()
    {
        foreach (Image bacteriaImage in bacterium)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(bacteriaImage.rectTransform, Input.mousePosition, null))
            {
                Bacteria bacteria = bacteriaImage.GetComponent<Bacteria>();
                if (bacteria != null)
                {
                    if (CanTakeDamage(bacteria))
                    {
                        bacteria.TakeDamage(damage);
                        RotateNeedle();
                        bacteriaCooldowns[bacteria] = Time.time + damageCooldown;
                    }
                }
            }
        }
    }

    private bool CanTakeDamage(Bacteria bacteria)
    {
        if (!bacteriaCooldowns.ContainsKey(bacteria))
        {
            bacteriaCooldowns[bacteria] = 0;
        }
        return Time.time >= bacteriaCooldowns[bacteria];
    }

    void RotateNeedle()
    {
        targetAngle += 10.3f;
        if (targetAngle > maxAngle)
        {
            targetAngle = 0f;
        }
    }

    private bool IsDragging { get; set; }
}
