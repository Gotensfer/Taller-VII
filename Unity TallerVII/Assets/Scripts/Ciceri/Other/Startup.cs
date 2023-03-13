using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
   
    public static void InstantiatePrefabs()
    {
        Debug.Log("inicia el objeto game manager");
        GameObject[] prefabInstantiate = Resources.LoadAll<GameObject>("InstantiateOnLoad/");
        foreach (GameObject prefab in prefabInstantiate)
        {
            Debug.Log("creacion prefab "+ prefab.name);
            GameObject.Instantiate(prefab);
        }
    }

   
}
