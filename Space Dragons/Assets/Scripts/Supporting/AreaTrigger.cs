using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaTrigger : MonoBehaviour
{
    [SerializeField] GameObject triggerActive = null;
    [SerializeField] UnityEvent triggerFunctionOn = null;
    [SerializeField] UnityEvent triggerFunctionOff = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerActive)  triggerActive.SetActive(true);
        if (triggerFunctionOn != null) triggerFunctionOn.Invoke();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        triggerActive.SetActive(false);
        if (triggerFunctionOff != null) triggerFunctionOff.Invoke();
    }

}
