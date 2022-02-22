using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class KillFeed : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] GameObject killFeedItemPrefab;

    Dictionary<Player, KillFeedItem> killFeedItems = new Dictionary<Player, KillFeedItem>();

    public static KillFeed instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void AddKillFeedItem(Player player, Player otherPlayer)
    {
        KillFeedItem item = Instantiate(killFeedItemPrefab, container).GetComponent<KillFeedItem>();
        item.GetKillFeed(player, otherPlayer);
        killFeedItems[player] = item;
        StartCoroutine(RemoveKillFeedItem(player));
    }

    IEnumerator RemoveKillFeedItem(Player player)
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(killFeedItems[player].gameObject);
        killFeedItems.Remove(player);
    }
}
