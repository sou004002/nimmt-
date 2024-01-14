using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RegisterUnityAction : MonoBehaviour
{
    public UnityAction buttonAction; //UnityActionを用意する

    private void Start()
    {
        Button button = GetComponent<Button>(); //buttonコンポーネントを取得
        button.onClick.AddListener(buttonAction);　//UnityActionを登録
    }
}