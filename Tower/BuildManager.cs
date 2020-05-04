using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{

    bool isBuild = false, GetNumber = true;
    public Camera FollowCam;
    public GameObject Area;
    public GameObject ArcherTower;
    public GameObject CanonTower;
    public GameObject IceTower;
    public float AreaSize;
    public PlayerResource playerResource;
    public UIManager uiManager;

    Quaternion ArcherRotation = Quaternion.Euler(-90, 90, 90);
    Ray ray;
    Vector3 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isBuild)
        {
            isBuild = Input.GetKey(KeyCode.B);
            if(isBuild)
            {
                Instantiate(Area, new Vector3(0.0f, 0.1f, 0.0f), Quaternion.identity);
            }
        }
        else
        {
            if (!(Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1) ||
                  Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2)))
                GetNumber = true;

            if ((Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1)) && GetNumber)
            {
                ray = FollowCam.ScreenPointToRay(Input.mousePosition);
                mousePos.x = ray.origin.x + ray.direction.x * (-ray.origin.y / ray.direction.y);
                mousePos.z = ray.origin.z + ray.direction.z * (-ray.origin.y / ray.direction.y);
                mousePos.y = 0.0f;
                Debug.Log(mousePos);
                if (Mathf.Pow(mousePos.x, 2.0f) + Mathf.Pow(mousePos.z, 2.0f) < Mathf.Pow(AreaSize, 2.0f))
                {
                    if(playerResource.wood > 0 && playerResource.stone > 0)
                    {
                        playerResource.GetWood(-1);
                        playerResource.GetStone(-1);
                        uiManager.UpdateWood();
                        uiManager.UpdateStone();
                        Instantiate(ArcherTower, mousePos, ArcherRotation);
                    }
                }
                    
                GetNumber = false;
            }

            else if ((Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2)) && GetNumber)
            {
                ray = FollowCam.ScreenPointToRay(Input.mousePosition);
                mousePos.x = ray.origin.x + ray.direction.x * (-ray.origin.y / ray.direction.y);
                mousePos.z = ray.origin.z + ray.direction.z * (-ray.origin.y / ray.direction.y);
                mousePos.y = 0.0f;
                Debug.Log(mousePos);
                if (Mathf.Pow(mousePos.x, 2.0f) + Mathf.Pow(mousePos.z, 2.0f) < Mathf.Pow(AreaSize, 2.0f))
                {
                    if (playerResource.wood > 1 && playerResource.stone > 0)
                    {
                        playerResource.GetWood(-2);
                        playerResource.GetStone(-1);
                        uiManager.UpdateWood();
                        uiManager.UpdateStone();
                        Instantiate(CanonTower, mousePos, Quaternion.identity);
                    }
                }

                GetNumber = false;
            }
            
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            Destroy(GameObject.Find("Area(Clone)"));
            isBuild = false;
        }
    }
}
