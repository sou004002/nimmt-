using System.Collections;
using System.Collections.Generic;
using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerDraw : MonoBehaviourPunCallbacks
{
    private GameObject deck;
    private GameObject gameManager;
    private int HP;
    private const int DEFAULT_HP=66;

    private string nickName;
    [SerializeField] GameObject cardPrefab;

    // [SerializeField] TextMeshProUGUI intHandNumText;
    private List<int> intHandArray=new List<int>();
    private PhotonView _deckPhoton;
    private Deck _deck;
    private List<GameObject> HandCards=new List<GameObject>();
    [SerializeField] GameObject PlayCardWaitText;


    private int playedCard=0;
    private bool canDraw=true;


    // Start is called before the first frame update
    void Start()
    {
        intHandArray=new List<int>();
        //この後すぐにカードを引くとintHandArrayがnullになる
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1) && canDraw)
        {
            DrawHands();
        }
    }
    public void DrawHands()
    {
        if(photonView.IsMine && photonView!=null)
        {
            deck=GameObject.FindWithTag("Deck");
            if(deck!=null)
            {
                _deckPhoton=deck.GetComponent<PhotonView>();
                _deck=deck.GetComponent<Deck>();
                for(int i=0;i<10;i++)
                {
                    photonView.RPC(nameof(DrawCard),RpcTarget.All,_deck.GetDecktop());
                    GameObject card=Instantiate(cardPrefab,new Vector3(-7+intHandArray.Count*2,-15,-7.0f),Quaternion.Euler(90,0,0));
                    card.GetComponent<Card>().Init(_deck.GetDecktop());
                    HandCards.Add(card);
                    _deckPhoton.RPC("Draw",RpcTarget.All);//一番上を消す
                }
                canDraw=false;
                GameObject parent=GameObject.FindWithTag("Canvas");
                Instantiate(PlayCardWaitText,parent.transform);

            }
        }
    }
    // Update is called once per frame

    [PunRPC]
    public void CanDrawToTrue()
    {
        canDraw=true;
    }
    public List<int> GetIntHandArray()
    {
        return intHandArray;
    }
    public bool GetCanDraw()
    {
        return canDraw;
    }
    public string GetIntHandArrayToString()
    {
        string str="";
        foreach(int i in intHandArray)
        {
            str+=i+",";
        }
        return str;
    }

    public int GetPlayedCard()
    {
        return playedCard;
    }
    public void ResetPlayedCard()
    {
        playedCard=0;
    }
    public void RemoveCard(int card)
    {
        intHandArray.Remove(card);
    }

    [PunRPC]
    public void DrawCard(int card)
    {        
        intHandArray.Add(card);
    }
    [PunRPC]
    public void PlayCard(int card)
    {
        playedCard=card;
    }
}
