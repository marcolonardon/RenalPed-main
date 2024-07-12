using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AwardManager : MonoBehaviour
{
    public float rayLength;
    public LayerMask layerMask;
    
    // Start is called before the first frame update
    void Start()
    {
       //StartCoroutine(WaitAndLoadMenu());
    }

    private IEnumerator WaitAndLoadMenu()
    {
        yield return new WaitForSeconds(3); // Espera por 3 segundos
        SceneManager.LoadScene("menu"); // Carrega a cena "menu"
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, rayLength, layerMask))
            {
                Debug.Log(hit.collider.name);
            }
        }
    }
}
