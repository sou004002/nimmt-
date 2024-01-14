using System;
using System.Text;
using Photon.Pun;
using TMPro;
using UnityEngine;
public class ProfileText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI label = default;
    private GameObject[] players;
    private StringBuilder builder;

    private void Start()
    {
        builder=new StringBuilder();
    }
    private void Update() {
        // まだルームに参加していない場合は更新しない
        if (!PhotonNetwork.InRoom) { return; }

        players=GameObject.FindGameObjectsWithTag("Player");
        UpdateLabel();

    }

    private void UpdateLabel()
    {
        var players=PhotonNetwork.PlayerList;
        // Array.Sort(
        //     players,
        //     (p1, p2) => {
        //         // スコアが多い順にソートする
        //         int diff = p2.GetScore() - p1.GetScore();
        //         if (diff != 0) {
        //             return diff;
        //         }
        //         // スコアが同じだった場合は、IDが小さい順にソートする
        //         return p1.ActorNumber - p2.ActorNumber;
        //     }
        // );
        builder.Clear();
        foreach(var player in players)
        {
            if(player.IsLocal)
            {
            builder.AppendLine($"* {player.NickName}({player.ActorNumber})   {player.GetScore()}");

            }
            else
            {
                builder.AppendLine($"  {player.NickName}({player.ActorNumber})   {player.GetScore()}");
            }
        }
        label.text=builder.ToString();
 
    }
}
