using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;
public class Card : MonoBehaviourPunCallbacks
{
    [SerializeField] private int cardNum;
    private Sprite[] images;
    private GameObject frontImage;
    private GameObject backImage;
    private bool isDrawing;

    [SerializeField]private int cardRow=0;
    [SerializeField]private int cardColumn=0;

    // private Vector3[] CardLeftPositions=
    // {
    //     new Vector3(-20,-24,7f),
    //     new Vector3(-20,-24,3f),
    //     new Vector3(-20,-24,-1f),
    //     new Vector3(-20,-24,-5f),
    // };

    // private const int CARD_OFFSET=3;

    void Start()
    {
        isDrawing=true;
    }
    void Update()
    {
        if(frontImage!=null && backImage!=null)
        {
            if(isDrawing)
            {
                frontImage.GetComponent<SpriteRenderer>().enabled=true;
                backImage.GetComponent<SpriteRenderer>().enabled=true;
            }
            else
            {
                frontImage.GetComponent<SpriteRenderer>().enabled=false;
                backImage.GetComponent<SpriteRenderer>().enabled=false;
            }
        }
    }
    
    public int GetCardNum()
    {
        return cardNum;
    }
    public void SetCardNum(int n)
    {
        cardNum=n;
    }
    public int GetCardRow()
    {
        return cardRow;
    }
    
    [PunRPC]
    public void SetCardRow(int n)
    {
        cardRow=n;
    }
        public int GetCardColumn()
    {
        return cardColumn;
    }

    [PunRPC]
    public void SetCardColumn(int n)
    {
        cardColumn=n;
    }
    public void SetIsdrawing(bool b)
    {
        isDrawing=b;
    }
    public void Init(int number)
    {
        if(images==null)
        {
            images = Resources.LoadAll<Sprite>("Sprites/");
        }
        if(frontImage==null)
        {
            frontImage=transform.Find("FrontImage").gameObject;
        }
        if(backImage==null)
        {
            backImage=transform.Find("BackImage").gameObject;
        }
        SetCardNum(number);
        frontImage.GetComponent<SpriteRenderer>().sprite=images[number];
        backImage.GetComponent<SpriteRenderer>().sprite=images[0];
        isDrawing=true;
        GetComponent<CardClick>().enabled=true;

    }

    public void Flip(bool isFront)
    {
        if(isFront)
        {
            this.gameObject.transform.DOLocalRotateQuaternion(Quaternion.Euler(90,0,0),1f);
        }
        else
        {
             this.gameObject.transform.DOLocalRotateQuaternion(Quaternion.Euler(-90,0,-180),1f);
        }
    }

    [PunRPC]
    public void InitFieldCard(int number,bool isFirstCard)
    {
        if(images==null)
        {
            images = Resources.LoadAll<Sprite>("Sprites/");
        }
        if(frontImage==null)
        {
            frontImage=transform.Find("FrontImage").gameObject;
        }
        if(backImage==null)
        {
            backImage=transform.Find("BackImage").gameObject;
        }
        SetCardNum(number);
        frontImage.GetComponent<SpriteRenderer>().sprite=images[number];
        backImage.GetComponent<SpriteRenderer>().sprite=images[0];
        if(isFirstCard)
        {
            this.gameObject.tag="FieldCard";
        }
        else
        {
            this.gameObject.tag="JudgeWaitCard";
        }
        //this.transform.DOMove(new Vector3(5f, 0f, 0f), 3f);
    }

    // void OnBecameInvisible()
    // {
    //     Destroy(this.gameObject);
    // }
    // [PunRPC]
    // public void AfterJudgeMove()//場の整数配列から出したいカードの場所を探して移動
    // {
    //     GameObject gameManager=GameObject.FindWithTag("GameManager");
    //     if(gameManager!=null)
    //     {
    //         int[] fieldCards01=gameManager.GetComponent<FieldManager>().GetFieldCards01();
    //         int[] fieldCards02=gameManager.GetComponent<FieldManager>().GetFieldCards02();
    //         int[] fieldCards03=gameManager.GetComponent<FieldManager>().GetFieldCards03();
    //         int[] fieldCards04=gameManager.GetComponent<FieldManager>().GetFieldCards04();
    //         int cardRow=-1,cardColumn=0;
    //         for(int i=0;i<fieldCards01.Length;i++)
    //         {
    //             if(fieldCards01[i]==cardNum)
    //             {
    //                 cardRow=0;
    //                 cardColumn=i+1;
    //                 break;
    //             }
    //         }
    //         for(int i=0;i<fieldCards02.Length;i++)
    //         {
    //             if(fieldCards02[i]==cardNum)
    //             {
    //                 cardRow=1;
    //                 cardColumn=i+1;
    //                 break;
    //             }
    //         }
    //         for(int i=0;i<fieldCards03.Length;i++)
    //         {
    //             if(fieldCards03[i]==cardNum)
    //             {
    //                 cardRow=2;
    //                 cardColumn=i+1;
    //                 break;
    //             }
    //         }
    //         for(int i=0;i<fieldCards04.Length;i++)
    //         {
    //             if(fieldCards04[i]==cardNum)
    //             {
    //                 cardRow=3;
    //                 cardColumn=i+1;
    //                 break;
    //             }
    //         }
    //         transform.DOMove(new Vector3(CardLeftPositions[cardRow].x+(cardColumn-1)*CARD_OFFSET,CardLeftPositions[cardRow].y,CardLeftPositions[cardRow].z),1f);
    //     }
    // }
}
