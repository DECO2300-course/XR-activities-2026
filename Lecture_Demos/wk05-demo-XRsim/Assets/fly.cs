using UnityEngine; 
using UnityEngine.InputSystem;
    public class fly : MonoBehaviour
    	{
    	    public InputActionReference flyAction;
        	    public float speed = 2f;
            void Update()
        	    {
        	        float input = flyAction.action.ReadValue<float>();
            	        transform.position += Vector3.up * input * speed * Time.deltaTime;
                }
    	}