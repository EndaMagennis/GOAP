using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientSpawner : MonoBehaviour
{
    public GameObject patientPrefab;
    public int numPatients;


    private void Start()
    {
        for (int i = 0; i < numPatients; i++)
        {
            Instantiate(patientPrefab, this.transform.position, Quaternion.identity);
        }
        Invoke("SpawnPatient", 5);
    }
        

    void SpawnPatient()
    {
        Instantiate(patientPrefab, this.transform.position, Quaternion.identity);
        Invoke("SpawnPatient", Random.Range(6f, 10f));
    }
}
