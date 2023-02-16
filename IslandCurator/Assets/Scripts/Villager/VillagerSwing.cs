using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerSwing : MonoBehaviour
{
    [SerializeField] GrabbableItem _grabbableBody;
    [SerializeField] float _maxSwing = 30f;
    [SerializeField] float _maxFall = 10f;

    void OnEnable()
    {
        transform.parent.GetComponent<GrabVillager>().OnLand += Land;
    }

    void OnDisable()
    {
        transform.parent.GetComponent<GrabVillager>().OnLand -= Land;
    }

    void Update()
    {
        if (_grabbableBody.Held)
        {
            Vector3 mouseMovement = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);

            if (mouseMovement != Vector3.zero)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(transform.up, mouseMovement.normalized), _maxSwing * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Vector3.zero), _maxFall * Time.deltaTime);
            }
        }
    }

    void Land()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
