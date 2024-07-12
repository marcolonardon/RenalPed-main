using UnityEngine;
using UnityEngine.UI;
using System;

public class Bacteria : MonoBehaviour
{
    public int health = 100;
    private Image bacteriaImage;
    private Color originalColor;
    public static event Action<Bacteria> OnBacteriaDestroyed;

    private void Awake()
    {
        bacteriaImage = GetComponent<Image>();
        originalColor = bacteriaImage.color;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateOpacity();
        if (health <= 0)
        {
            OnBacteriaDestroyed?.Invoke(this);
            Destroy(gameObject);
            // Você pode adicionar efeitos visuais ou sons aqui, se quiser
        }
    }

    private void UpdateOpacity()
    {
        Color color = bacteriaImage.color;
        color.a = Mathf.Max(color.a - 0.2f, 0f); // Diminui a opacidade em 20% (0.2)
        bacteriaImage.color = color;
    }
}
