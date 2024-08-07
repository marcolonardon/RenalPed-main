using UnityEngine;
using UnityEngine.UI;

public class FocusInputField : MonoBehaviour
{
    public InputField inputField; 

    // Função para ser chamada ao clicar no botão
    public void FocusOnInputField()
    {
        inputField.Select();
    }
}
