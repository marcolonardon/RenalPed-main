using UnityEngine;
using UnityEngine.UI;

public class FocusInputField : MonoBehaviour
{
    public InputField inputField; 

    // Fun��o para ser chamada ao clicar no bot�o
    public void FocusOnInputField()
    {
        inputField.Select();
    }
}
