using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralLevel : MonoBehaviour
{
    [SerializeField] List<GameObject> blocLvl;
    [SerializeField] float distancia;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //sistema de randomização de BlocLvl
            blocLvl[0].transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + distancia);
        }
    }
}
