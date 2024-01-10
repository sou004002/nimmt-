// using System.Collections.Generic;
// using ExitGames.Client.Photon;
// using Photon.Realtime;
// using UnityEngine;


// public static class DeckProperty
// {
//     private const string KeyDeckArray="DeckArray";
//     private static readonly Hashtable propsToSet=new Hashtable();

//     public static int[] TryGetDeckArray(this Room room)
//     {
//         return(room.CustomProperties[KeyDeckArray] is int[] value)? value:new int[1];
//     }
//     public static void SetDeckArray(this Room room,out int[] intDeckArray)
//     {
//         propsToSet[KeyDeckArray]=intDeckArray;

//         room.SetCustomProperties(propsToSet);

//         propsToSet.Clear();
//     }
// }