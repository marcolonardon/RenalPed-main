using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading;

public class CircleSlot : MonoBehaviour, IDropHandler
{
    public string[] expectedItemTags; // Array de tags esperadas dos objetos para este slot
    public string slotTag; // Tag única do slot
    public bool keepItemActivated;

    private string currentItemTag; // Tag atual do item no slot

    public void OnItemDropped(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        DragDrop draggableItem = droppedObject.GetComponent<DragDrop>();

        // Verifica se o objeto tem uma das tags esperadas para este slot
        if (draggableItem != null)
        {
            foreach (string tag in expectedItemTags)
            {
                if (droppedObject.CompareTag(tag)) // Verifica se a tag é correta para este slot
                {
                    draggableItem.parentAfterDrag = transform;
                    // Mantém ou não o objeto ativado conforme a configuração
                    droppedObject.SetActive(keepItemActivated);
                    currentItemTag = tag; // Atualiza a tag atual do item no slot
                    
                    //Debug.Log("Objeto dropado com a tag correta: " + tag);

                    // Verifica se o slot correspondente ao objeto dropado é este slot
                    if (gameObject.CompareTag(droppedObject.GetComponent<DragDrop>().initialParent.tag))
                    {
                        setAnswer(slotTag);
                        Debug.Log("Objeto dropado neste slot: " + slotTag);
                    }
                }
            }

            
        }
    }

    private void setAnswer(string slotTag)
    {
        switch (slotTag)
        {
            case "Slot1":
                PlayerPrefs.SetString("CircleSlot1", currentItemTag);
                break;
            case "Slot2":
                PlayerPrefs.SetString("CircleSlot2", currentItemTag);
                break;
            case "Slot3":
                PlayerPrefs.SetString("CircleSlot3", currentItemTag);
                break;
            case "Slot4":
                PlayerPrefs.SetString("CircleSlot4", currentItemTag);
                break;
            case "Slot5":
                PlayerPrefs.SetString("CircleSlot5", currentItemTag);
                break;
            case "Slot6":
                PlayerPrefs.SetString("CircleSlot6", currentItemTag);
                break;
            default:
                Debug.Log("Switch Error- Tag: " + currentItemTag);
                break;
        }
    }


    // Método para obter a tag atual do item no slot
    public string GetCurrentItemTag()
    {
        return currentItemTag;
    }

    // Chamado quando um objeto é dropado neste slot
    public void OnDrop(PointerEventData eventData)
    {
        OnItemDropped(eventData);
    }


}
