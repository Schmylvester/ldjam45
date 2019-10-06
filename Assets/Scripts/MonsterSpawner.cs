using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            SpawnLocation sl = transform.GetChild(i).GetComponent<SpawnLocation>();

            float sumWeight = 0;
            foreach (SpawnLocation.Spawn spawn in sl.spawnList)
            {
                sumWeight += spawn.weight;
            }

            foreach (SpawnLocation.Spawn spawn in sl.spawnList)
            {
                if (Random.Range(0.0f, 1.0f) < spawn.weight / sumWeight)
                {
                    if (spawn.prefab == null) break; //chance of no spawn

                    spawn.prefab.SetActive(false);
                    GameObject go = Instantiate(spawn.prefab);
                    go.SetActive(true);
                    spawn.prefab.SetActive(true);

                    Vector2 randOffset = new Vector2(Random.Range(-1, 1) * 0.1f, Random.Range(-1, 1) * 0.1f);
                    go.transform.position = new Vector2(sl.transform.position.x, sl.transform.position.y) + randOffset;
                    break; //only one per spawn location
                }
            }
        }
    }
}
