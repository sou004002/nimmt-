using System.Collections;
using System.Collections.Generic;
using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening; 
using System.Linq;
public class FieldManager : MonoBehaviourPunCallbacks,IPunObservable
{
    private int[] fieldCards01;
    private int[] fieldCards02;
    private int[] fieldCards03;
    private int[] fieldCards04;


    private const int ROWNUM=4;
    [SerializeField] TextMeshProUGUI FieldCardsText;
    [SerializeField] TextMeshProUGUI judgeCardsText;
    [SerializeField] TextMeshProUGUI judgePlayerText;

    private bool isGameStart=false;
    private const int MAX_PLAYERS=2;
    private GameObject deck;
    private PhotonView _deckPhoton;
    private Deck _deck;
    private GameObject[] players;
    private bool isResetButtonClick;
    private Player player;
    private List<(int,int)> JudgeList;
    GameObject moveCardObj;





    private Vector3[] CardLeftPositions=
    {
        new Vector3(-20,-24,7f),
        new Vector3(-20,-24,3f),
        new Vector3(-20,-24,-1f),
        new Vector3(-20,-24,-5f),
    };

    private const int CARD_OFFSET=3;

    void Update()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount==MAX_PLAYERS && !isGameStart)
        {
            if(photonView.IsMine)
            {
                Init();
            }
            isGameStart=true;
        }

        if(isGameStart)
        {
            if(!(fieldCards01==null||fieldCards02==null||fieldCards03==null||fieldCards04==null))
            {
                FieldCardsText.text=GetFieldCards();
            }
            players=GameObject.FindGameObjectsWithTag("Player");
            if(players!=null)
            {
                bool judgeFlag=true;
                foreach(GameObject p in players)
                {
                    if(p.GetComponent<PlayerDraw>().GetPlayedCard()==0)
                    {
                        judgeFlag=false;
                    }
                }
                if(judgeFlag)
                {
                    photonView.RPC("Judge",RpcTarget.All);

                }
            }
        }
    }

    public string GetFieldCards()
    {
        string str = "";
        for (int j = 0; j < fieldCards01.Length; j++)
        {
            str = str + fieldCards01[j] + " ";
        }
        str+=" | ";
        for (int j = 0; j < fieldCards02.Length; j++)
        {
            str = str + fieldCards02[j] + " ";
        }
        str+=" | ";
        for (int j = 0; j < fieldCards03.Length; j++)
        {
            str = str + fieldCards03[j] + " ";
        }
        str+=" | ";
        for (int j = 0; j < fieldCards04.Length; j++)
        {
            str = str + fieldCards04[j] + " ";
        }
        return str;
    }

    public Vector3[] GetCardLeftPositions()
    {
        return CardLeftPositions;
    }

    [PunRPC]
    public void IsResetButtonClickToFalse()
    {
        isResetButtonClick=false;
    }
    public void Init()
    {
        fieldCards01=new int[1];
        fieldCards02=new int[1];
        fieldCards03=new int[1];
        fieldCards04=new int[1];
        isResetButtonClick=false;

        deck=GameObject.FindWithTag("Deck");
        if(deck!=null)
        {
        _deckPhoton=deck.GetComponent<PhotonView>();
        _deck=deck.GetComponent<Deck>();
        _deck.generateDeckArray();
        Debug.Log(_deck.GetDecktop()==null);
        fieldCards01[0]=(_deck.GetDecktop());
        GameObject c1=PhotonNetwork.Instantiate("Card",CardLeftPositions[0],Quaternion.Euler(90,0,0));
        c1.GetComponent<PhotonView>().RPC("InitFieldCard",RpcTarget.AllBuffered,_deck.GetDecktop(),true);
        c1.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.AllBuffered,1);
        c1.GetComponent<PhotonView>().RPC("SetCardColumn",RpcTarget.AllBuffered,1);

        _deckPhoton.RPC("Draw",RpcTarget.All);

        fieldCards02[0]=(_deck.GetDecktop());
        GameObject c2=PhotonNetwork.Instantiate("Card",CardLeftPositions[1],Quaternion.Euler(90,0,0));
        c2.GetComponent<PhotonView>().RPC("InitFieldCard",RpcTarget.AllBuffered,_deck.GetDecktop(),true);
        c2.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.AllBuffered,2);
        c2.GetComponent<PhotonView>().RPC("SetCardColumn",RpcTarget.AllBuffered,1);


        _deckPhoton.RPC("Draw",RpcTarget.All);


        fieldCards03[0]=(_deck.GetDecktop());
        GameObject c3=PhotonNetwork.Instantiate("Card",CardLeftPositions[2],Quaternion.Euler(90,0,0));
        c3.GetComponent<PhotonView>().RPC("InitFieldCard",RpcTarget.AllBuffered,_deck.GetDecktop(),true);
        c3.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.AllBuffered,3);
        c3.GetComponent<PhotonView>().RPC("SetCardColumn",RpcTarget.AllBuffered,1);


        _deckPhoton.RPC("Draw",RpcTarget.All);


        fieldCards04[0]=(_deck.GetDecktop());
        GameObject c4=PhotonNetwork.Instantiate("Card",CardLeftPositions[3],Quaternion.Euler(90,0,0));
        c4.GetComponent<PhotonView>().RPC("InitFieldCard",RpcTarget.AllBuffered,_deck.GetDecktop(),true);
        c4.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.AllBuffered,4);
        c4.GetComponent<PhotonView>().RPC("SetCardColumn",RpcTarget.AllBuffered,1);


        _deckPhoton.RPC("Draw",RpcTarget.All);
        }
    }


    [PunRPC]
    public void Judge()
    {
        StartCoroutine(JudgeCoroutine());
    }
    IEnumerator JudgeCoroutine()
    {
        GameObject[] cards=GameObject.FindGameObjectsWithTag("Card");
        foreach(GameObject c in cards)
        {
            c.GetComponent<CardClick>().enabled=false;
        }
        JudgeList=new List<(int,int)>();　//tupleの第1要素：プレイヤーID　第2要素：プレイしたカード
        foreach(GameObject p in players)
        {
            PlayerDraw _player=p.GetComponent<PlayerDraw>();
            (int,int) t = (p.GetComponent<PhotonView>().OwnerActorNr,_player.GetPlayedCard());
            JudgeList.Add(t);
            _player.RemoveCard(_player.GetPlayedCard());
            _player.ResetPlayedCard();
        }
        JudgeList.Sort((tuple1,tuple2) => tuple1.Item2.CompareTo(tuple2.Item2));//第2要素でソート
        List<GameObject> moveCardObjects=new List<GameObject>();
        for(int i=0;i<players.Length;i++) 
        {
            var ps=PhotonNetwork.PlayerList;
            foreach(var p in ps)
            {
                if(p.ActorNumber==JudgeList[i].Item1)
                {
                    player=p;
                }
            }
            List<int> lastCards=new List<int>();
            lastCards.Add(fieldCards01[fieldCards01.Length-1]);
            lastCards.Add(fieldCards02[fieldCards02.Length-1]);
            lastCards.Add(fieldCards03[fieldCards03.Length-1]);
            lastCards.Add(fieldCards04[fieldCards04.Length-1]);

            int playRowNum=-1;
            int cardDiff=105;
            for(int j=0;j<ROWNUM;j++) //置く行を選択
            {
                if(JudgeList[i].Item2-lastCards[j]>0)
                {
                    if(cardDiff>=(JudgeList[i].Item2-lastCards[j]))
                    {
                        cardDiff=JudgeList[i].Item2-lastCards[j];
                        playRowNum=j;
                    }
                }
            }
            GameObject[] allCards=GameObject.FindGameObjectsWithTag("JudgeWaitCard");
            foreach(GameObject card in allCards)
            {
                if(card.GetComponent<Card>().GetCardNum()==JudgeList[i].Item2)
                {
                    moveCardObj=card;
                }
            }
            moveCardObjects.Add(moveCardObj);
            yield return new WaitForSeconds(0.7f);
            moveCardObj.GetComponent<Card>().Flip(true);

            if(playRowNum==-1)
            {
                StartCoroutine(JudgeCardMoveCoroutine());
                var pl=PhotonNetwork.PlayerList;
                
                foreach(GameObject p in players)
                {
                    if(p.GetComponent<PhotonView>().OwnerActorNr==player.ActorNumber)
                    {
                        p.GetComponent<GenerateResetButton>().InstantiateResetButton();
                    }
                }
                isResetButtonClick=true;
                yield return new WaitUntil(() =>!isResetButtonClick);
                GameObject[] ResetButtons=GameObject.FindGameObjectsWithTag("ResetButton");
                foreach(GameObject button in ResetButtons)
                {
                    if(button.GetComponent<ButtonInit>().GetIsClicked())
                    {
                        int buttonIndex=button.GetComponent<ButtonInit>().GetButtonNumber();
                        int damage=0;
                        switch(buttonIndex)
                        {
                            case 0:
                                for(int j=0;j<fieldCards01.Length;j++)
                                {
                                    damage+=CardNumToDamage(fieldCards01[j]);
                                }
                                player.Damage(damage);
                                photonView.RPC("ClearFieldCard",RpcTarget.All,1,JudgeList[i].Item2);
                                moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,1);   
                                break;
                            case 1:
                                for(int j=0;j<fieldCards02.Length;j++)
                                {
                                    damage+=CardNumToDamage(fieldCards02[j]);
                                }
                                player.Damage(damage);
                                photonView.RPC("ClearFieldCard",RpcTarget.All,2,JudgeList[i].Item2);
                                moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,2);   
                                break;
                            case 2:
                                for(int j=0;j<fieldCards03.Length;j++)
                                {
                                    damage+=CardNumToDamage(fieldCards03[j]);
                                }
                                player.Damage(damage);
                                photonView.RPC("ClearFieldCard",RpcTarget.All,3,JudgeList[i].Item2);
                                moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,3);   
                                break;
                            case 3:
                                for(int j=0;j<fieldCards04.Length;j++)
                                {
                                    damage+=CardNumToDamage(fieldCards04[j]);
                                }
                                player.Damage(damage);
                                photonView.RPC("ClearFieldCard",RpcTarget.All,4,JudgeList[i].Item2);
                                moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,4);   
                                break;
                        }
                    }
                    Destroy(button);
                }
                int toResetRow=moveCardObj.GetComponent<Card>().GetCardRow();
                if(toResetRow!=0)
                {
                    GameObject[] allFieldCards=GameObject.FindGameObjectsWithTag("FieldCard");
                    {
                        foreach(GameObject c in allFieldCards)
                        {
                            if(c.GetComponent<Card>().GetCardRow()==toResetRow&& c.GetComponent<Card>().GetCardColumn()!=0)
                            {
                                c.transform.DOMove(new Vector3(c.transform.position.x-30,c.transform.position.y,c.transform.position.z),1f);
                                c.tag="GabageCard";
                                yield return new WaitForSeconds(0.1f);
                            }
                        }
                    }
                }
                moveCardObj.GetComponent<PhotonView>().RPC("SetCardColumn",RpcTarget.All,1);
                moveCardObj.tag="JudgeEndCard";
                continue;
            }
            moveCardObj.tag="JudgeEndCard";

            switch(playRowNum)
            {
                case 0:
                    fieldCards01=AddFieldCard(fieldCards01,JudgeList[i].Item2);
                    moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,1);
                    break;
                case 1:
                    fieldCards02=AddFieldCard(fieldCards02,JudgeList[i].Item2);
                    moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,2);
                    break;
                case 2:
                    fieldCards03=AddFieldCard(fieldCards03,JudgeList[i].Item2);    
                    moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,3);               
                    break;
                case 3:
                    fieldCards04=AddFieldCard(fieldCards04,JudgeList[i].Item2);
                    moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,4);
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(0.5f);        
        }
        foreach(GameObject card in moveCardObjects)
        {
            int column=0;
            for(int j=0;j<fieldCards01.Length;j++)
            {
                if(card.GetComponent<Card>().GetCardNum()==fieldCards01[j])
                {
                    column=j+1;
                }
            }
            for(int j=0;j<fieldCards02.Length;j++)
            {
                if(card.GetComponent<Card>().GetCardNum()==fieldCards02[j])
                {
                    column=j+1;
                }
            }
            for(int j=0;j<fieldCards03.Length;j++)
            {
                if(card.GetComponent<Card>().GetCardNum()==fieldCards03[j])
                {
                    column=j+1;
                }
            }
            for(int j=0;j<fieldCards04.Length;j++)
            {
                if(card.GetComponent<Card>().GetCardNum()==fieldCards04[j])
                {
                    column=j+1;
                }
            }
            if(column>0 && card.GetComponent<Card>().GetCardColumn()!=6)
            {
                card.GetComponent<PhotonView>().RPC("SetCardColumn",RpcTarget.All,column);
            }
        }
        moveCardObjects.Clear();
        yield return StartCoroutine(JudgeCardMoveCoroutine());
        cards=GameObject.FindGameObjectsWithTag("Card");
        foreach(GameObject c in cards)
        {
            c.GetComponent<CardClick>().enabled=true;
        }
        yield return null;
    }

    IEnumerator JudgeCardMoveCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        List<GameObject> judgeEndCardObj=GameObject.FindGameObjectsWithTag("JudgeEndCard").ToList();
        judgeEndCardObj.Sort((card1,card2)=>card1.GetComponent<Card>().GetCardNum().CompareTo(card2.GetComponent<Card>().GetCardNum()));
        foreach(GameObject cardObj in judgeEndCardObj)
        {
            cardObj.tag="FieldCard";
            Card _card=cardObj.GetComponent<Card>();
            if(_card.GetCardColumn()==6)
            {
                foreach(Player p in PhotonNetwork.PlayerList)
                {
                    if(p.ActorNumber==cardObj.GetComponent<PhotonView>().OwnerActorNr)
                    {
                        int damage=0;
                        switch(_card.GetCardRow())
                        {
                            case 1:
                                for(int i=0;i<5;i++)
                                {
                                    damage+=CardNumToDamage(fieldCards01[i]);
                                }
                                p.Damage(damage);
                                fieldCards01=ResetFieldCard(fieldCards01);
                                break;
                            case 2:
                                for(int i=0;i<5;i++)
                                {
                                    damage+=CardNumToDamage(fieldCards02[i]);
                                }
                                p.Damage(damage);
                                fieldCards02=ResetFieldCard(fieldCards02);
                                break;
                            case 3:
                                for(int i=0;i<5;i++)
                                {
                                    damage+=CardNumToDamage(fieldCards03[i]);
                                }
                                p.Damage(damage);
                                fieldCards03=ResetFieldCard(fieldCards03);
                                break;
                            case 4:
                                for(int i=0;i<5;i++)
                                {
                                    damage+=CardNumToDamage(fieldCards04[i]);
                                }
                                p.Damage(damage);
                                fieldCards04=ResetFieldCard(fieldCards04);
                                break;
                            default:
                                break;
                        }


                    }
                }
                MoveFieldCard(cardObj,_card.GetCardRow(),_card.GetCardColumn());//6列に移動
                yield return new WaitForSeconds(1.0f);
                GameObject[] allFieldCards=GameObject.FindGameObjectsWithTag("FieldCard");
                {
                    foreach(GameObject c in allFieldCards)
                    {
                        if(c.GetComponent<Card>().GetCardRow()==_card.GetCardRow())
                        {
                            if(c.GetComponent<Card>().GetCardColumn()<6)
                            {
                                c.transform.DOMove(new Vector3(c.transform.position.x-30,c.transform.position.y,c.transform.position.z),1f);
                                c.tag="GabageCard";
                                yield return new WaitForSeconds(0.1f);
                            }
                        }
                    }
                }
                MoveFieldCard(cardObj,_card.GetCardRow(),1);

                cardObj.GetComponent<PhotonView>().RPC("SetCardColumn",RpcTarget.All,1);
            }
            else if(_card.GetCardColumn()>6)
            {
                int col=_card.GetCardColumn()-5;
                MoveFieldCard(cardObj,_card.GetCardRow(),_card.GetCardColumn()-5);
                cardObj.GetComponent<PhotonView>().RPC("SetCardColumn",RpcTarget.All,col);
            }
            else
            {
                MoveFieldCard(cardObj,_card.GetCardRow(),_card.GetCardColumn());
            }

            yield return new WaitForSeconds(0.7f);
        }
    }

    public void MoveFieldCard(GameObject card,int row,int column)
    {
        if(card.tag=="FieldCard")
        {
            card.transform.DOMove(new Vector3(CardLeftPositions[row-1].x+(column-1)*CARD_OFFSET,CardLeftPositions[row-1].y,
                                    CardLeftPositions[row-1].z),1f);
        }
    }

    public int[] AddFieldCard(int[] fieldcard,int card) //場の整数列にAddする
    {
        List<int> tmpList=new List<int>();
        foreach(int i in fieldcard)
        {
            tmpList.Add(i);
        }
        tmpList.Add(card);
        int[] newFieldCard=new int[tmpList.Count];
        for(int i=0;i<newFieldCard.Length;i++)
        {
            newFieldCard[i]=tmpList[i];
        }
        return newFieldCard;
    }

    public int[] ResetFieldCard(int[] fieldcard) //場の整数列を末項だけにする
    {
        //judgeCardsText.text+="Reset ";
        int[] newFieldCard=new int[1];
        newFieldCard[0]=fieldcard[fieldcard.Length-1];
        return newFieldCard;
    }

    [PunRPC]
    public void ClearFieldCard(int fieldcardindex,int card) //行をcardだけにする(ボタン押した用)
    {
        switch(fieldcardindex)
        {
            case 1:
                fieldCards01=new int[1]{card};
                break;
            case 2:
                fieldCards02=new int[1]{card};
                break;
            case 3:
                fieldCards03=new int[1]{card};
                break;
            case 4:
                fieldCards04=new int[1]{card};
                break;
            default:
                break;
        }
    }

    private int CardNumToDamage(int n)　//カード番号とダメージ数の変換
    {
        int d;
        if(n==55)
        {
            d=7;
        }
        else if(n%11==0)
        {
            d=5;
        }
        else if(n%10==0)
        {
            d=3;
        }
        else if(n%5==0)
        {
            d=2;
        }
        else
        {
            d=1;
        }
        return d;
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
    {
            if(stream.IsWriting)
            {
                stream.SendNext(fieldCards01);
                stream.SendNext(fieldCards02);
                stream.SendNext(fieldCards03);
                stream.SendNext(fieldCards04);
            } 
            else
            {
                fieldCards01=(int[])stream.ReceiveNext();
                fieldCards02=(int[])stream.ReceiveNext();
                fieldCards03=(int[])stream.ReceiveNext();
                fieldCards04=(int[])stream.ReceiveNext();
            }
    }
}
