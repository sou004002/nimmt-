using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ResetButtonClick : MonoBehaviour,IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("button click");
        GameObject GameManager=GameObject.FindWithTag("GameManager");
        GameManager.GetComponent<FieldManager>().IsResetButtonClickToFalse();
    }
}
