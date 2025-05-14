using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject skeleton1;
    public GameObject skeleton2;
    public GameObject skeleton3;
    public GameObject skeleton4;
    public GameObject skeleton5;
    public GameObject skeleton6;
    public GameObject bigBoss;

    private HashSet<GameObject> defeatedEnemies = new HashSet<GameObject>();

    public void EnemyDefeated(GameObject enemy) {
        defeatedEnemies.Add(enemy);

        if (!skeleton2.activeInHierarchy &&
            defeatedEnemies.Contains(skeleton1) &&
            defeatedEnemies.Contains(skeleton3))
        {
            skeleton2.SetActive(true);
            Debug.Log("Skeleton2 spawned.");
        }

        if (!bigBoss.activeInHierarchy &&
            defeatedEnemies.Contains(skeleton4) &&
            defeatedEnemies.Contains(skeleton5) &&
            defeatedEnemies.Contains(skeleton6))
        {
            bigBoss.SetActive(true);
            Debug.Log("Big Boss spawned.");
        }
    }
}