using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardButtonInit : MonoBehaviour
{
    private int cardNum;
    private Sprite[] images;

    private Image cardImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   public int GetSetCardNum
   {
       get { return cardNum; }
       set { cardNum = value; }
   }
    public void Init(int number)
    {
        if(images==null)
        {
            images = Resources.LoadAll<Sprite>("Sprites/");
        }
        GetSetCardNum=number;
        GetComponent<Image>().sprite=images[number];
    }
}
