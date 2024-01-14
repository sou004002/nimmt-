using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using DG.Tweening; 

public class CardClick : MonoBehaviourPunCallbacks,IPointerClickHandler
{
    private GameObject[] players;
    private GameObject[] cards;

    // [SerializeField] private GameObject player;
    private GameObject gameManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject playCardWaitText=GameObject.FindWithTag("PlayCardText");
        Destroy(playCardWaitText);
        int card=GetComponent<Card>().GetCardNum();
        players=GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject obj in players)
        {
            if(obj.GetComponent<PhotonView>().IsMine)
            {
                obj.GetComponent<PhotonView>().RPC("PlayCard",RpcTarget.All,card);
            }
        }      
        cards=GameObject.FindGameObjectsWithTag("Card");
        foreach(GameObject c in cards)
        {
            if(c.tag=="Card")
            {
                c.GetComponent<CardClick>().enabled=false;
            }
        }
        GameObject toFieldCard=PhotonNetwork.Instantiate("Card",this.transform.position,Quaternion.Euler(-90,0,-180));
        toFieldCard.GetComponent<PhotonView>().RPC("InitFieldCard",RpcTarget.AllBuffered,card,false);
        Destroy(this.gameObject);
        toFieldCard.transform.DOMove(new Vector3(toFieldCard.GetComponent<PhotonView>().OwnerActorNr*3,-24,0),1f);
        // gameManager=GameObject.FindWithTag("GameManager");
        // if(gameManager!=null)
        // {
        //     gameManager.GetComponent<FieldManager>().AddJudgeWaitCards(toFieldCard);
        // }
        //見た目を消す
        // if(photonView!=null && photonView.IsMine)
        // {
        //     int card=GetComponent<Card>().GetCardNum();
        //     players=GameObject.FindGameObjectsWithTag("Player");
        //     foreach(GameObject obj in players)
        //     {
        //         if(obj.GetComponent<PhotonView>().OwnerActorNr==photonView.OwnerActorNr)
        //         {
        //             Debug.Log(photonView.OwnerActorNr);
        //             obj.GetComponent<PhotonView>().RPC("PlayCard",RpcTarget.All,card);
        //         }
        //     }
        //     cards=GameObject.FindGameObjectsWithTag("Card");
        //     foreach(GameObject c in cards)
        //     {
        //         if(c.GetComponent<PhotonView>().IsMine)
        //         {
        //             if(c.tag=="Card")
        //             {
        //                 c.GetComponent<CardClick>().enabled=false;
        //             }
        //         }
        //     }
        //     GameObject toFieldCard=PhotonNetwork.Instantiate("Card",this.transform.position,Quaternion.Euler(90,0,0));
        //     toFieldCard.GetComponent<PhotonView>().RPC("InitFieldCard",RpcTarget.AllBuffered,card);
        //     // GetComponent<Card>().SetIsdrawing(false);
        //     Destroy(this.gameObject);
        //     toFieldCard.transform.DOMove(new Vector3(toFieldCard.GetComponent<PhotonView>().OwnerActorNr*3,-24,0),1f);
        //     gameManager=GameObject.FindWithTag("GameManager");
        //     if(gameManager!=null)
        //     {
        //         gameManager.GetComponent<FieldManager>().AddJudgeWaitCards(toFieldCard);
        //     }

        // }
    }
}
