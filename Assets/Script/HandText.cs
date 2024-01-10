using System;
using System.Text;
using Photon.Pun;
using TMPro;
using UnityEngine;
public class HandText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI label = default;
    private GameObject[] players;
    private StringBuilder builder;
    // Start is called before the first frame update
    void Start()
    {
        builder=new StringBuilder(); 
    }

    // Update is called once per frame
    void Update()
    {
        players=GameObject.FindGameObjectsWithTag("Player");
        if(players!=null)
        {
            builder.Clear();
            foreach(GameObject p in players)
            {
                builder.AppendLine(p.GetComponent<PlayerDraw>().GetIntHandArrayToString());
                // Debug.Log(p.GetComponent<PlayerDraw>().GetIntHandArrayToString());
            }
        }
        label.text=builder.ToString();
        
    }
}
