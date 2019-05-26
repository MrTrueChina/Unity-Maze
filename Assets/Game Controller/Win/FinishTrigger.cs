using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //TODO：玩家接触到时发出通关消息
        if (other.tag == "Player")
            Debug.Log("WIN");
    }
}
