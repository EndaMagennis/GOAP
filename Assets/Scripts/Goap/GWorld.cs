using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*holds the objects and states of the world.*/
public sealed class GWorld
{
    private static readonly GWorld instance = new GWorld();
    private static WorldStates world;
    private static Queue<GameObject> patients;
    private static Queue<GameObject> cubicles;

    
    static GWorld()
    {
        world = new WorldStates();
        patients = new Queue<GameObject>();
        cubicles = new Queue<GameObject>();

        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cubicle");
        foreach(GameObject c in cubes)
            cubicles.Enqueue(c);

        if(cubes.Length > 0)
            world.ModifyState("FreeCubicle" , cubes.Length);

        Time.timeScale = 5;
    }

    private GWorld()
    {

    }

    //adding the patient to a queue 
    public void AddPatient(GameObject patient)
    {
        patients.Enqueue(patient);
    }
    // Removing patient from queue
    public GameObject RemovePatient()
    {
        if (patients.Count == 0) return null;
        return patients.Dequeue();
    }

    //add a cubical to the list
    public void AddCubicle(GameObject cubicle)
    {
        cubicles.Enqueue(cubicle);
    }
    //remove a cubical from the queue
    public GameObject RemoveCubicle()
    {
        if(cubicles.Count == 0) return null ;
        return cubicles.Dequeue();
    }

    public static GWorld Instance
    {
        get { return instance; }
    }

    public WorldStates GetWorld()
    {
        return world;
    }
}
