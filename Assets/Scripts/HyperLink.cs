using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Hyperlink : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Camera _mainCamera;
    private int linkIndex;

    public void OnPointerClick(PointerEventData eventData)
    {
        linkIndex = TMP_TextUtilities.FindIntersectingLink(_text, Input.mousePosition, _mainCamera);

        if (linkIndex != -1)
        {
            Application.OpenURL(_text.textInfo.linkInfo[linkIndex].GetLinkID());
        }
    }
}