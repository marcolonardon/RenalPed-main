using UnityEngine;

public class RandomColorLegacyButton_OnGUI : MonoBehaviour
{
    private Color buttonColor = Color.white;  // Cor inicial do botão
    private Rect buttonRect = new Rect(100, 100, 200, 50);  // Posição e tamanho do botão

    void OnGUI()
    {
        // Define a cor do botão
        GUI.backgroundColor = buttonColor;

        // Cria o botão e verifica se foi clicado
        if (GUI.Button(buttonRect, "Clique em mim!"))
        {
            // Gera uma cor aleatória
            buttonColor = new Color(Random.value, Random.value, Random.value);
        }
    }
}
