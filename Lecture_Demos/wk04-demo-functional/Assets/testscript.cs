using UnityEngine;

public class testscript : MonoBehaviour
{

    public int number = 2;
    public string myName = "somename";
    public GameObject other;
    
    void Start()
    {
        myName = other.name;
        
       
    }

   
    void Update()
    {
       transform.position = Vector3.MoveTowards(transform.position, other.transform.position, 0.001f);
    }
}
