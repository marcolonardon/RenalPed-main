using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WashHandDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image soap;
    public Image clickHand;
    public Image clickCircle;
    public Image[] bacterium;
    public RectTransform needleParent;
    public Button rotateButton;// Arraste o botão aqui no Inspector
    private float currentAngle = 0f; // Ângulo atual da agulha
    private const float maxAngle = 180f; // Ângulo máximo da agulha
    private float targetAngle = 0f; // Ângulo alvo para interpolação
    public float smoothSpeed = 0.95f; // Velocidade da interpolação
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Transform initialParent;

    private const int damage = 25;
    private float damageCooldown = 0.5f; // Tempo de cooldown em segundos
    private Dictionary<Bacteria, float> bacteriaCooldowns = new Dictionary<Bacteria, float>();
    private int bacteriaCount;

    private void Start()
    {
        if (PlayerPrefs.GetInt("levelAt") < 3)
            PlayerPrefs.SetInt("levelAt", 3);

        PlayerPrefs.SetInt("MaxIndex", 13);
        PlayerPrefs.SetInt("MinIndex", 9);
        rotateButton.onClick.AddListener(RotateNeedle);
        bacteriaCount = bacterium.Length;
    }
    void Update()
    {
        // Interpola suavemente o ângulo atual para o ângulo alvo
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, smoothSpeed * Time.deltaTime);

        // Aplica a rotação ao GameObject pai da agulha
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

        // Não mudar de cena aqui; aguardar o OnEndDrag para verificar o estado do sabonete.
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
        transform.SetAsLastSibling(); //sempre na frente das outras imagens quando arrasta
        soap.raycastTarget = false; //para dropar na camada de baixo, ignora o que está arrastando
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

        if (bacteriaCount == 0) //Verifica se todas as bacterias foram mortas ou nao
        {
            SceneManager.LoadScene("HandSpeechBubblePage2");

            if (PlayerPrefs.GetInt("levelAt") < 3) // Desbloqueia a proxima missao
                PlayerPrefs.SetInt("levelAt", 3);
            ScoreManager.Instance.AddWashHandsScore(1, 1); ///////////////////////////////////////////////////////////////////////
        }
        else
        {
            SceneManager.LoadScene("HandSpeechBubblePage1");
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
        // Incrementa o ângulo alvo da agulha
        targetAngle += 10.3f;

        // Limita o ângulo alvo ao máximo de 180 graus
        if (targetAngle > maxAngle)
        {
            targetAngle = 0f; // Reseta o ângulo alvo para 0 se ultrapassar 180 graus
        }
    }

    private bool IsDragging { get; set; }
}
