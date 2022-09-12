using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public GameObject dogPrefab;
    private bool dogSpawnReady;

    void Start()
    {
        dogSpawnReady = false;
    }

    // Update is called once per frame
    void Update()
    {
        // On spacebar press, send dog
        if (Input.GetKeyDown(KeyCode.Space) && dogSpawnReady == false)
        {
            StartCoroutine(makeDogs());
        }
        IEnumerator makeDogs()
        {
            dogSpawnReady = true;

            //spawndog
            Instantiate(dogPrefab, transform.position, dogPrefab.transform.rotation);

            //wait 1s
            yield return new WaitForSeconds(1);

            dogSpawnReady = false;
        }
    }
}
