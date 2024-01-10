using System.Collections;
using System.Collections.Generic;
using System;
using DG.Tweening;  //DOTweenを使うときはこのusingを入れる
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MoveTest : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI judgeCardsText;
    [SerializeField] TextMeshProUGUI judgeCardsText2;

    private int num,num2;
    void Start()
    {
        // 3秒かけて(5,0,0)へ移動する
        num=0;
        num2=0;
        StartCoru();
    }

    void Update()
    {
        judgeCardsText.text=num.ToString();
        judgeCardsText2.text=num2.ToString();

    }


    public void StartCoru()
    {
        StartCoroutine(CoroutineTest());
        
    }


    IEnumerator CoroutineTest()
    {
        for(int i = 0;i < 10; i++)
        {
            num++;
            yield return new WaitForSeconds(0.5f);
        }
        this.transform.DOMove(new Vector3(5f, 0f, 0f), 3f);
        StartCoroutine(CoroutineTest2());
    }
    IEnumerator CoroutineTest2()
    {
        for(int i = 0;i < 10; i++)
        {
            num2+=2;
            yield return new WaitForSeconds(0.5f);
        }
        //this.transform.DOMove(new Vector3(5f, 0f, 0f), 3f);
        
    }
}
