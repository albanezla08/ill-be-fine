using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTargets : MonoBehaviour
{
    public GameObject targets;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn_a_Target());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator Spawn_a_Target()
    {

        yield return new WaitForSeconds(4f);
        GameObject spawn;
        spawn = Instantiate(targets, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);

        StartCoroutine(Spawn_a_Target());
        yield break;
    }

}
