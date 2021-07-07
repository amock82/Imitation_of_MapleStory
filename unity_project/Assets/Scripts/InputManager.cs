using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    float pickUpDelay = 0.1f;
    float pickUpTimer = 0;

    bool isPickable = true;

    public static InputManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(pickUpTimer > 0)
        {
            pickUpTimer -= Time.deltaTime;

            isPickable = false;
        }
        else
        {
            if (Input.GetButton("PickUp"))
            {
                isPickable = true;

                pickUpTimer = pickUpDelay;
            }
        }

    }

    public bool GetIsPickable()
    {
        return isPickable;
    }

    public void SetIsPickable(bool value)
    {
        isPickable = value;
    }

    //public void PickUpInput()
    //{
    //    isPickable = false;
    //}
}
