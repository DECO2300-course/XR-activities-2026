using System;
using UnityEngine;

public class spawn : MonoBehaviour
{
    private Transform origin;
    private GameObject sp;
    public void Start()
    {
        sp = GameObject.Find("spawner");
        origin = sp.transform;
    }

    public void Spawn()
    {
        
        GameObject postit = Instantiate(
            this.gameObject,
            origin.position,
            origin.rotation
        );
      
        postit.transform.parent = sp.transform;
        
      
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Bin")
        {
            Destroy(this.gameObject);
        }
    }
}