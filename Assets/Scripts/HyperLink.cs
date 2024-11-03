using UnityEngine;

public class HyperLInk : MonoBehaviour
{
    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }
}

