# 概要
タイトル：「nimmt! - clone」<br>
バージョン：1.0.0<br>
ジャンル：オンラインカードゲーム<br>
プレイ時間：15分程度<br>
制作ソフト：unity,C#<br>
<br>
カードゲーム「二ムト」をオンライン上でプレイできるゲームです。<br>
現時点では2人プレイのみ対応しています。<br>
PUN2（Photon Unity Networking 2）を用いてオンラインプレイを実装しました。<br>


# 操作方法
右クリック：手札を引く<br>

# 遊び方

### ゲームの準備
プレイヤーは右クリックで10枚の手札を取得します。<br>
場には4枚のカードが縦に並べられます。場には4行のカードの列ができます。

### 手番
各プレイヤーは、手番ごとに手札から1枚のカードを選んで出します。

### 判定
各プレイヤーがカードを出したら、数の小さいカードからカードを並べていきます。<br>
出したカードと、場の行の右端のカードの数の差が一番小さい行に並べます。<br>
出したカードが行の6枚目のとき、プレイヤーは1～5枚目までのカードを取得し、カードの得点の合計のダメージを受けます。<br>
出したカードの数がどの行の右端のカードの数よりも小さいときは、任意の行を選び取得します。

### ゲームの終了
手札がなくなるまで手番を繰り返します。<br>
手札がなくなったら場をリセットし、ゲームの最初から繰り返し新たなラウンドとします。<br>
ラウンドの途中に誰かのHPが0以下になった場合、そのラウンドの終了時に得点が最も高いプレイヤーが勝利します。



# 起動方法
フォルダを解凍し、exeファイルを実行してください。<br>
2人目のプレイヤーがファイルを実行すると、ゲームが開始されます。<br>

# 仕様素材
・フォント：https://fonts.google.com/specimen/Zen+Maru+Gothic<br>
      
# お問い合わせ
* 作成者　Sou<br>
* 所属　法政大学情報科学部<br>
* E-mail　sou.suzuki.6x@stu.hosei.ac.jp<br>
