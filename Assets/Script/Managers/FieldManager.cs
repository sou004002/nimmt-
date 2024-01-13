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


    [SerializeField]private List<GameObject>[] fieldCardObjects=new List<GameObject>[]{new List<GameObject>(),new List<GameObject>(),
                                                    new List<GameObject>(),new List<GameObject>()};

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
        //judgeCardsText.text=isResetButtonClick.ToString();
        // //  FieldCardsText.text=judgeCount;
        if(isGameStart)
        {
            if(!(fieldCards01==null||fieldCards02==null||fieldCards03==null||fieldCards04==null))
            {
                FieldCardsText.text=GetFieldCards();
            }
            if(fieldCardObjects!=null)
            {
                judgeCardsText.text=GetFieldCardsObj();
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
    public string GetFieldCardsObj()
    {
        string str = "";
        for(int i=0;i<fieldCardObjects.Length;i++)
        {
            for (int j = 0; j < fieldCardObjects[i].Count; j++)
            {
                str = str + fieldCardObjects[i][j].GetComponent<Card>().GetCardNum() + " ";
            }
            str+=" | ";
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
        GameObject[] ResetButtons=GameObject.FindGameObjectsWithTag("ResetButton");
        foreach(GameObject button in ResetButtons)
        {
            if(button.GetComponent<ButtonInit>().GetIsClicked())
            {
                int buttonIndex=button.GetComponent<ButtonInit>().GetButtonNumber();
                if(player==null)
                {
                    Debug.Log("player is null");
                }
                int newCard=0;
                foreach((int,int) t in JudgeList)
                {
                    if(t.Item1==player.ActorNumber)
                    {
                        newCard=t.Item2;
                    }
                }
                switch(buttonIndex)
                {
                    case 0:
                        player.Damage(SumDamage(fieldCards01));
                        fieldCards01=new int[]{newCard};
                        moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,1);   
                        break;
                    case 1:
                        player.Damage(SumDamage(fieldCards02));
                        fieldCards02=new int[]{newCard};
                        moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,2);   
                        break;
                    case 2:
                        player.Damage(SumDamage(fieldCards03));
                        fieldCards03=new int[]{newCard};
                        moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,3);   
                        break;
                    case 3:
                        player.Damage(SumDamage(fieldCards04));
                        fieldCards04=new int[]{newCard};
                        moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,4);   
                        break;
                }
                // foreach(GameObject c in fieldCardObjects[button.GetComponent<ButtonInit>().GetButtonNumber()])//連続でカード出すとエラー
                // {
                //     c.transform.DOMove(new Vector3(c.transform.position.x-30,c.transform.position.y,c.transform.position.z),1f);
                //     yield return new WaitForSeconds(0.1f);
                // }
                // fieldCardObjects[button.GetComponent<ButtonInit>().GetButtonNumber()].Clear();
            }
            Destroy(button);
        }
        
        // swtich(buttonIndex)
        // {
        //     case 0:
        //         player.Damage(SumDamage(fieldCards01));
        //         fieldCards01=new int[]{newCard};
        //         moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,1);   
        //         break;
        //     case 1:
        //         player.Damage(SumDamage(fieldCards02));
        //         fieldCards02=new int[]{newCard};
        //         moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,2);
        //         break;
        //     case 2:
        //         player.Damage(SumDamage(fieldCards03));
        //         fieldCards03=new int[]{newCard};
        //         moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,3);

        //         break;
        //     case 3:
        //         player.Damage(SumDamage(fieldCards04));
        //         fieldCards04=new int[]{newCard};
        //         moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,4);
        //         break;
        //     default:
        //         break;
        // }
        // for(int j=0;j<fieldCardObjects.Length;j++)
        // {
        //     if(fieldCardObjects[j].Count==0)
        //     {
        //         Debug.Log("j: "+j);
        //         switch(j)
        //         {
        //             case 0:
        //                 player.Damage(SumDamage(fieldCards01));
        //                 fieldCards01=new int[]{JudgeList[i].Item2};
        //                 moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,1);   
        //                 break;
        //             case 1:
        //                 player.Damage(SumDamage(fieldCards02));
        //                 fieldCards02=new int[]{JudgeList[i].Item2};
        //                 moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,2);
        //                 break;
        //             case 2:
        //                 player.Damage(SumDamage(fieldCards03));
        //                 fieldCards03=new int[]{JudgeList[i].Item2};
        //                 moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,3);

        //                 break;
        //             case 3:
        //                 player.Damage(SumDamage(fieldCards04));
        //                 fieldCards04=new int[]{JudgeList[i].Item2};
        //                 moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,4);
        //                 break;
        //             default:
        //                 break;
        //         }
        //     }
        // }
        isResetButtonClick=false;
    }
    public void Init()
    {
        //judgeCardsText.text+="Ini";
        fieldCards01=new int[1];
        fieldCards02=new int[1];
        fieldCards03=new int[1];
        fieldCards04=new int[1];
        isResetButtonClick=false;
        for(int i=0;i<fieldCardObjects.Length;i++)
        {
            fieldCardObjects[i]=new List<GameObject>();
        }
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
        fieldCardObjects[0].Add(c1);
        _deckPhoton.RPC("Draw",RpcTarget.All);

        fieldCards02[0]=(_deck.GetDecktop());
        GameObject c2=PhotonNetwork.Instantiate("Card",CardLeftPositions[1],Quaternion.Euler(90,0,0));
        c2.GetComponent<PhotonView>().RPC("InitFieldCard",RpcTarget.AllBuffered,_deck.GetDecktop(),true);
        c2.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.AllBuffered,2);

        fieldCardObjects[1].Add(c2);
        _deckPhoton.RPC("Draw",RpcTarget.All);


        fieldCards03[0]=(_deck.GetDecktop());
        GameObject c3=PhotonNetwork.Instantiate("Card",CardLeftPositions[2],Quaternion.Euler(90,0,0));
        c3.GetComponent<PhotonView>().RPC("InitFieldCard",RpcTarget.AllBuffered,_deck.GetDecktop(),true);
        c3.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.AllBuffered,3);
        fieldCardObjects[2].Add(c3);
        _deckPhoton.RPC("Draw",RpcTarget.All);


        fieldCards04[0]=(_deck.GetDecktop());
        GameObject c4=PhotonNetwork.Instantiate("Card",CardLeftPositions[3],Quaternion.Euler(90,0,0));
        c4.GetComponent<PhotonView>().RPC("InitFieldCard",RpcTarget.AllBuffered,_deck.GetDecktop(),true);
        c4.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.AllBuffered,4);
        fieldCardObjects[3].Add(c4);
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
            // GameObject moveCardObj=null;
            foreach(GameObject card in allCards)
            {
                if(card.GetComponent<Card>().GetCardNum()==JudgeList[i].Item2)
                {
                    moveCardObj=card;
                }
            }
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
                        //judgeCardsText.text+=p.GetComponent<PhotonView>().OwnerActorNr.ToString()+"true";

                    }
                    else
                    {
                        //judgeCardsText.text+=p.GetComponent<PhotonView>().OwnerActorNr.ToString()+"false";
                    }
                }
                isResetButtonClick=true;
                yield return new WaitUntil(() =>!isResetButtonClick);
                int toResetRow=0;
                if(fieldCards01[0]==JudgeList[i].Item2)
                {
                    toResetRow=1;
                }
                else if(fieldCards02[0]==JudgeList[i].Item2)
                {
                    toResetRow=2;
                }
                else if(fieldCards03[0]==JudgeList[i].Item2)
                {
                    toResetRow=3;
                }
                else if(fieldCards04[0]==JudgeList[i].Item2)
                {
                    toResetRow=4;
                }
                Debug.Log("toResetRow"+toResetRow);
                if(toResetRow!=0)
                {
                    foreach(GameObject c in fieldCardObjects[toResetRow-1])//連続でカード出すとエラー
                    {
                        c.transform.DOMove(new Vector3(c.transform.position.x-30,c.transform.position.y,c.transform.position.z),1f);
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                moveCardObj.GetComponent<PhotonView>().RPC("SetCardColumn",RpcTarget.All,1);
                moveCardObj.tag="JudgeEndCard";
                //movecardObjを1列目に移動
                // Debug.Log("a");
                continue;
            }
            moveCardObj.tag="JudgeEndCard";

            switch(playRowNum)
            {
                case 0:
                    fieldCards01=AddFieldCard(fieldCards01,JudgeList[i].Item2);
                    moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,1);
                    moveCardObj.GetComponent<PhotonView>().RPC("SetCardColumn",RpcTarget.All,fieldCards01.Length);
                    if(fieldCards01.Length==6)
                    {
                        player.Damage(SumDamage(fieldCards01));
                        fieldCards01=ResetFieldCard(fieldCards01);
                    }
                    break;
                case 1:
                    fieldCards02=AddFieldCard(fieldCards02,JudgeList[i].Item2);
                    moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,2);
                    moveCardObj.GetComponent<PhotonView>().RPC("SetCardColumn",RpcTarget.All,fieldCards02.Length);
                    if(fieldCards02.Length==6)
                    {
                        player.Damage(SumDamage(fieldCards02));
                        fieldCards02=ResetFieldCard(fieldCards02);
                    }
                    break;
                case 2:
                    fieldCards03=AddFieldCard(fieldCards03,JudgeList[i].Item2);    
                    moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,3);
                    moveCardObj.GetComponent<PhotonView>().RPC("SetCardColumn",RpcTarget.All,fieldCards03.Length);               
                    if(fieldCards03.Length==6)
                    {
                        player.Damage(SumDamage(fieldCards03));
                        fieldCards03=ResetFieldCard(fieldCards03);
                    }
                    break;
                case 3:
                    fieldCards04=AddFieldCard(fieldCards04,JudgeList[i].Item2);
                    moveCardObj.GetComponent<PhotonView>().RPC("SetCardRow",RpcTarget.All,4);
                    moveCardObj.GetComponent<PhotonView>().RPC("SetCardColumn",RpcTarget.All,fieldCards04.Length);
                    if(fieldCards04.Length==6)
                    {
                        player.Damage(SumDamage(fieldCards04));
                        fieldCards04=ResetFieldCard(fieldCards04);
                    }
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(0.5f);        
        }
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
                MoveFieldCard(cardObj,_card.GetCardRow(),_card.GetCardColumn());//6列に移動
                yield return new WaitForSeconds(1.0f);
                foreach(GameObject c in fieldCardObjects[_card.GetCardRow()-1])//連続でカード出すとエラー
                {
                    c.transform.DOMove(new Vector3(c.transform.position.x-30,c.transform.position.y,c.transform.position.z),1f);
                    yield return new WaitForSeconds(0.1f);
                }
                MoveFieldCard(cardObj,_card.GetCardRow(),1);
                fieldCardObjects[_card.GetCardRow()-1].Clear();
                cardObj.GetComponent<PhotonView>().RPC("SetCardColumn",RpcTarget.All,1);
            }
            else
            {
                MoveFieldCard(cardObj,_card.GetCardRow(),_card.GetCardColumn());
            }
            fieldCardObjects[_card.GetCardRow()-1].Add(cardObj);
            yield return new WaitForSeconds(0.7f);
        }
    }

    IEnumerator ResetCardCoroutine(int i)
    {
        foreach(GameObject c in fieldCardObjects[i])//連続でカード出すとエラー
        {
            c.transform.DOMove(new Vector3(c.transform.position.x-30,c.transform.position.y,c.transform.position.z),1f);
            yield return new WaitForSeconds(0.1f);
        }
       fieldCardObjects[i].Clear(); 
    }
    

    public void MoveFieldCard(GameObject card,int row,int column)
    {
        Debug.Log("Card: "+card.GetComponent<Card>().GetCardNum());
        Debug.Log("Row: "+card.GetComponent<Card>().GetCardRow());
        Debug.Log("Column: "+card.GetComponent<Card>().GetCardColumn());

        if(card.tag=="FieldCard")
        {
            //FieldCardsText.text+=card.GetComponent<Card>().GetCardNum().ToString()+row.ToString()+column.ToString()+". ";
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
        int[] newFieldCard=new int[1];
        newFieldCard[0]=fieldcard[fieldcard.Length-1];
        return newFieldCard;
    }

    public int SumDamage(int[] fieldcard) //ダメージ量の計算
    {
        int allDamage=0;
        for(int i=0; i<fieldcard.Length-1;i++)
        {
            allDamage+=CardNumToDamage(fieldcard[i]);
        }
        return allDamage;
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
