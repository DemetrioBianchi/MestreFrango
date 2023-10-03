using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coxinhas : MonoBehaviour
{
    [SerializeField] private float timer;
    // Update is called once per frame
    void Update()
    {
        if (!transform.GetChild(0).gameObject.activeSelf)
        {
            StartCoroutine("RestartRender");
        }
    }

    IEnumerator RestartRender()
    {
        yield return new WaitForSeconds(timer);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
}
