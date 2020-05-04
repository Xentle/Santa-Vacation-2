using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class VRInput : BaseInput
{
    /*public Camera eventCamera = null;
    public OVRInput.Button clickButton = IVRInput.Button.PrimaryIndexTrigger;
    public OVRInput.Controller controller = IVRInput.Controller.All;
    protected override void Awake()
    {
        GetComponent<BaseInputModule>().inputOverride = this;
    }

    public override bool GetMouseButton(int button)
    {
        return OVRInput.Get(clickButton, controller);
    }

    public override bool GetMouseButtonDown(int button)
    {
        return OVRInput.GetDown(clickButton, controller);

    }

    public override bool GetMouseButtonUp(int button)
    {
        return OVRInput.GetUp(clickButton, controller);

    }

    public override Vector2 mousePosition
    {
        get
        {
            return new Vector2(eventCamera.pixelWidth/2,eventCamera.pixelHeight/2);
        }
    }*/
}
