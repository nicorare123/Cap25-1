using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject targetPanel;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetPanel != null)
            targetPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetPanel != null)
            targetPanel.SetActive(false);
    }
}
