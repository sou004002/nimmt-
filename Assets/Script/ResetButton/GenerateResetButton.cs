using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class GenerateResetButton : MonoBehaviourPunCallbacks
{
    [SerializeField] RegisterUnityAction button;
    [SerializeField] GameObject buttonText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [PunRPC]
    public void InstantiateResetButton()
    {
        GameObject gameManager=GameObject.FindWithTag("GameManager");
        GameObject parent=GameObject.FindWithTag("Canvas");
        Vector3[] cardLeftPositions=gameManager.GetComponent<FieldManager>().GetCardLeftPositions();
        if(photonView.IsMine)
        {
            for(int i=0;i < cardLeftPositions.Length;i++)
            {
                RegisterUnityAction b=Instantiate(button,parent.transform);
                b.GetComponent<ButtonInit>().SetButtonNumber(i);
                RectTransform rectTransform=b.GetComponent<RectTransform>();
                Vector2 position=rectTransform.anchoredPosition;
                position.y = 110-i*60;
                rectTransform.anchoredPosition = position;
                b.buttonAction=ButtonScript;
            }
            GameObject buttonTextObj=Instantiate(buttonText,parent.transform);
            buttonTextObj.GetComponent<TextMeshProUGUI>().text="取る行を選んでください";

        }
    }

    public void ButtonScript()
    {
        GameObject gameManager=GameObject.FindWithTag("GameManager");
        gameManager.GetComponent<PhotonView>().RPC("IsResetButtonClickToFalse",RpcTarget.All);
    }
}
