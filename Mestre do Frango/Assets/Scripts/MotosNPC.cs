using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotosNPC : MonoBehaviour
{
    [SerializeField] Transform[] entregas;
    [SerializeField] float speed, x, z;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("go");
    }

    IEnumerator go()
    {
        yield return new WaitForSeconds(0);
        transform.LookAt(entregas[1].position);
        transform.localPosition += new Vector3(x,0,z)* speed * Time.deltaTime;
        StartCoroutine("go");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Entregar"))
        {
            transform.localPosition = entregas[0].localPosition;
        }
    }
}
