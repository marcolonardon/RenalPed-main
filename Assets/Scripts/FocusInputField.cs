using UnityEngine;
using UnityEngine.UI;

public class FocusInputField : MonoBehaviour
{
    public InputField inputField; // Arraste o InputField para esse campo no Inspector

    // Fun��o para ser chamada ao clicar no bot�o
    public void FocusOnInputField()
    {
        inputField.Select();
    }
}
