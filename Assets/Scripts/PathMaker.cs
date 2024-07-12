using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMaker : MonoBehaviour
{
    [System.Serializable]
    public class ActivationPoint
    {
        public GameObject objectToActivate;
        public int activationIndex;
    }

    [SerializeField] Transform[] Points;
    [SerializeField] private float moveSpeed;
    [SerializeField] private ActivationPoint[] activationPoints; // Array de pontos de ativa��o

    private int pointsIndex;
    private bool pathCompleted = false; // Indica se o caminho foi completado
    private int maxIndex; // �ndice m�ximo recebido do PlayerPrefs ou padr�o

    // Start is called before the first frame update
    void Start()
    {
        // L� o valor do �ndice m�ximo a partir do PlayerPrefs ou define como 2 se n�o existir
        maxIndex = PlayerPrefs.GetInt("MaxIndex", 2);

        transform.position = Points[pointsIndex].transform.position;

        // Desativa todos os GameObjects no in�cio
        foreach (ActivationPoint activationPoint in activationPoints)
        {
            if (activationPoint.objectToActivate != null)
            {
                activationPoint.objectToActivate.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!pathCompleted && pointsIndex <= maxIndex)
        {
            transform.position = Vector2.MoveTowards(transform.position, Points[pointsIndex].transform.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, Points[pointsIndex].transform.position) < 0.1f)
            {
                // Ativa o GameObject correspondente ao ponto atual, se existir
                foreach (ActivationPoint activationPoint in activationPoints)
                {
                    if (pointsIndex == activationPoint.activationIndex && activationPoint.objectToActivate != null)
                    {
                        activationPoint.objectToActivate.SetActive(true);
                    }
                }

                pointsIndex++;

                if (pointsIndex > maxIndex)
                {
                    pathCompleted = true;
                    pointsIndex--; // Permanece no �ltimo �ndice permitido
                }
            }
        }
    }
}
