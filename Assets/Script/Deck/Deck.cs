using System.Collections;
using System.Collections.Generic;
using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Deck : MonoBehaviourPunCallbacks,IPunObservable
{
    // private List<int> deckArray;
    private int[] intDeckArray;
    [SerializeField] TextMeshProUGUI intDeckNumText;
    [SerializeField] private GameObject cardPrefab;
    private GameObject[] playerPrefabs;


    public void generateDeckArray()//デッキ作成
    {
        List<int> deckArray=new List<int>();
        for (int i=1;i<105;i++)
        {
            deckArray.Add(i);
        }
        int n=deckArray.Count;
        while(n>1)
        {
            n--;
            int k=UnityEngine.Random.Range(0,n+1);
            int temp=deckArray[k];
            deckArray[k]=deckArray[n];
            deckArray[n]=temp;
        }
        CopyIntDeckArray(deckArray);
    }
    public int[] GetIntDeckArray()
    {
        return intDeckArray;
    }
    public void CopyIntDeckArray(List<int> deckarray)
    {
        intDeckArray=new int[deckarray.Count];
        for(int i=0;i<intDeckArray.Length;i++)
        {
            intDeckArray[i]=deckarray[i];
        }
    }

    public int GetDecktop()
    {
        return intDeckArray[0];
    }
    private void Update()
    {
        if(intDeckArray!=null)
        {
            intDeckNumText.text=String.Join(",",GetIntDeckArray());
        }
    }



    [PunRPC]
    public void Draw(PhotonMessageInfo info)
    {
        List<int> deckArray=new List<int>();
        foreach(int i in intDeckArray)
        {
            deckArray.Add(i);
        }
        int removedCard=deckArray[0];
        deckArray.RemoveAt(0);
        CopyIntDeckArray(deckArray);
    }

    // public void Draw()
    // {
    //     List<int> deckArray=new List<int>();
    //     foreach(int i in intDeckArray)
    //     {
    //         deckArray.Add(i);
    //     }
    //     int removedCard=deckArray[0];
    //     deckArray.RemoveAt(0);
    //     CopyIntDeckArray(deckArray);
    // }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
    {
            if(stream.IsWriting)
            {
                stream.SendNext(intDeckArray);
            } else
            {
                intDeckArray = (int[])stream.ReceiveNext();
            }
    }
}
