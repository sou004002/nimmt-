using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
[RequireComponent(typeof(PhotonView))]
public class CubeCreator : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Material CubeMaterial;

    int Timer = 0;
    bool isJoinedRoom = false;
    PhotonView PhotonView;

    void Start()
    {
        //RPC のために PhotonView 取得
        PhotonView = GetComponent<PhotonView>();

        //Photon に接続
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster() {
        // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
        PhotonNetwork.JoinOrCreateRoom("CubeCreator", new RoomOptions(), TypedLobby.Default);
        Debug.Log("master");
    }
    // // ロビーに入室すると呼ばれるイベント
    // void OnJoinedLobby()
    // {
    //     Debug.Log("ロビーに入室成功");

    //     //ルームに入室するか作成する
    //     PhotonNetwork.JoinOrCreateRoom("CubeCreator", null, null);
    // }

    //ルームに入室すると呼ばれる
    void OnJoinedRoom()
    {
        Debug.Log("ルームに入室成功");
        isJoinedRoom = true;
    }

    void Update()
    {
        Timer++;
        //100フレーム経過していない、又はルームに入室していないならスキップ
        if (Timer < 100 || !isJoinedRoom)
        {
            return;
        }
        //Cube を作成する関数を全クライアントで実行
        PhotonView.RPC("CreateCube", RpcTarget.AllBuffered);
        Timer = 0;
    }

    [PunRPC]
    void CreateCube(int viewID)
    {
        //Cube を生成
        var _cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //わかりやすいように上空に移動
        _cube.transform.position = new Vector3(0, 10, 0);

        //マテリアルを設定
        _cube.GetComponent<Renderer>().material = CubeMaterial;

        //Rigidbody を追加
        var _rigidbody = _cube.AddComponent<Rigidbody>();

        //PUN を追加
        var _photonView = _cube.gameObject.AddComponent<PhotonView>();
        var _photonTransformView = _cube.gameObject.AddComponent<PhotonTransformView>();
        var _photonRigidbodyView = _cube.gameObject.AddComponent<PhotonRigidbodyView>();

        //PhotonView の ObservedComponents リストを初期化
        _photonView.ObservedComponents = new List<Component>();

        //PhotonView に ViewID を設定
        _photonView.ViewID = viewID;

        //到達保証の設定
        //詳しくは https://support.photonengine.jp/hc/ja/articles/224763767-PUN%E3%81%A7%E5%88%B0%E9%81%94%E4%BF%9D%E8%A8%BC%E3%81%AE%E8%A8%AD%E5%AE%9A%E3%82%92%E8%A1%8C%E3%81%86
        //_photonView.synchronization = ViewSynchronization.ReliableDeltaCompressed;

        //PhotonTransformView の設定
        //位置の同期を有効にする
        _photonTransformView.m_SynchronizePosition = true;
        _photonTransformView.m_SynchronizeRotation = true;

        //リストに追加して同期対象に加える
        _photonView.ObservedComponents.Add(_photonTransformView);
        _photonView.ObservedComponents.Add(_photonRigidbodyView);

        Debug.Log("Create!");
    }
}

