using ExitGames.Client.Photon;
using Photon.Realtime;

public static class PlayerPropertiesExtensions
{
    private const string ScoreKey = "Score";
    private const int DEFAULT_HP=66;

    private static readonly Hashtable propsToSet = new Hashtable();

    // プレイヤーのスコアを取得する
    public static int GetScore(this Player player) {
        return (player.CustomProperties[ScoreKey] is int score) ? score : DEFAULT_HP;
    }

    // プレイヤーのスコアを加算する
    public static void Damage(this Player player, int value) {
        propsToSet[ScoreKey] = player.GetScore() - value;
        player.SetCustomProperties(propsToSet);
        propsToSet.Clear();
    }
}