using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Serialization;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Object = UnityEngine.Object;
using UnityEngine.SceneManagement;

public class Vr_pointer : MonoBehaviour
{
     public SteamVR_Behaviour_Pose pose;

        //public SteamVR_Action_Boolean interactWithUI = SteamVR_Input.__actions_default_in_InteractUI;
        public SteamVR_Action_Boolean m_GrabAction = null;
        
        public bool active = true;
        public Color color;
        public float thickness = 0.002f;
        public Color clickColor = Color.green;
        public GameObject holder;
        public GameObject pointer;
        bool isActive = false;
        public bool addRigidBody = false;
        public Transform reference;
        public event PointerEventHandler PointerIn;
        public event PointerEventHandler PointerOut;
        public event PointerEventHandler PointerClick;

        Transform previousContact = null;

        /*wj*/
        private bool isGrab = false;

        public float m_Force = 1.0f;
        //public GameObject bombObj;
        
        private Animator m_Animator = null;


        private FixedJoint m_Joint = null;
        private Takeable m_CurrentTakeable = null;
        public List<Takeable> m_ContactTakeable = new List<Takeable>();
        public float m_Lifetime = 5.0f; //lifetime of projectile, destroy when not hit at some point

        

        private void Start()
        {
            m_Joint = GetComponent<FixedJoint>();
            if (pose == null)
                pose = this.GetComponent<SteamVR_Behaviour_Pose>();
            if (pose == null)
                Debug.LogError("No SteamVR_Behaviour_Pose component found on this object");

            if (m_GrabAction == null)
                Debug.LogError("No ui interaction action has been set on this component.");

            m_Animator = GetComponent<Animator>();

            holder = new GameObject();
            holder.transform.parent = this.transform;
            holder.transform.localPosition = Vector3.zero;
            holder.transform.localRotation = Quaternion.identity;

            pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
//            pointer.transform.parent = holder.transform;
            pointer.transform.parent = reference;
          
            pointer.transform.localScale = new Vector3(thickness, thickness, 100f);
            pointer.transform.localPosition = new Vector3(0f, 0f, 50f);
            pointer.transform.localRotation = Quaternion.identity;
            BoxCollider collider = pointer.GetComponent<BoxCollider>();
            if (addRigidBody)
            {
                if (collider)
                {
                    collider.isTrigger = true;
                }
                Rigidbody rigidBody = pointer.AddComponent<Rigidbody>();
                rigidBody.isKinematic = true;
            }
            else
            {
                if (collider)
                {
                    Object.Destroy(collider);
                }
            }
            Material newMaterial = new Material(Shader.Find("Unlit/Color"));
            newMaterial.SetColor("_Color", color);
            pointer.GetComponent<MeshRenderer>().material = newMaterial;
        }

        public virtual void OnPointerIn(PointerEventArgs e)
        {
            if (PointerIn != null)
                PointerIn(this, e);
            Debug.Log("Enter"+e.target.name);
            
            //if (!e.target.CompareTag("Bomb"))
            //    return;
            
            //m_ContactTakeable.Add(e.target.gameObject.GetComponent<Takeable>());
        }

        public virtual void OnPointerClick(PointerEventArgs e)
        {
            if (PointerClick != null)
                PointerClick(this, e);
            Debug.Log("Click");

            
            //canvas
            if (e.target.name == "Button")
            {
                SceneManager.LoadScene("MainScene");
            }
            
            //bomb interaction
            Takeable takeable = e.target.GetComponent<Takeable>();
            if (!isGrab)
            {
                Debug.Log(pose.inputSource+" PointerGrab");
                PickUp(takeable);
                isGrab = true;
            }
            else
            {
                Debug.Log(pose.inputSource+" PointerFire");
                Drop();
                isGrab = false;
            }

            
           

        }
        

        public virtual void OnPointerOut(PointerEventArgs e)
        {
            if (PointerOut != null)
                PointerOut(this, e);
            Debug.Log("Exit");
            //if (!e.target.CompareTag("Bomb"))
            //    return;
            
            //m_ContactTakeable.Remove(e.target.gameObject.GetComponent<Takeable>());
        }

        public void PickUp(Takeable takeable)
        {
            //get obj
            m_CurrentTakeable = takeable;
            
            //null check
            if (!m_CurrentTakeable) return;
            
            //already held check
            if (m_CurrentTakeable.m_ActivePointer)
                m_CurrentTakeable.m_ActivePointer.Drop();
            
            //pleassssssseeeeee
            // Enemy_ranged script = m_CurrentTakeable.gameObject.GetComponent<Enemy_ranged>();
            // StopCoroutine(script.FireObject());
            
            //position
            m_CurrentTakeable.transform.position = reference.position;
            m_CurrentTakeable.transform.rotation = reference.rotation;
            
            //Attach
            Rigidbody targetBody = m_CurrentTakeable.GetComponent<Rigidbody>();
            targetBody.isKinematic = false;
            m_Joint.connectedBody = targetBody;

            takeable.StopMove = true;
            
            //set active pointer
            m_CurrentTakeable.m_ActivePointer = this;


        }

        public void Drop()
        {
            //Null check
            if (!m_CurrentTakeable) return;
            
            //m_CurrentTakeable.transform.position = reference.position;
            //m_CurrentTakeable.transform.rotation = reference.rotation;
            
            //fire
            Rigidbody targetBody = m_CurrentTakeable.gameObject.GetComponent<Rigidbody>();
            targetBody.AddRelativeForce( Vector3.forward * m_Force, ForceMode.Impulse);
            //StartCoroutine(TrackLifetime());
            //Detach
            m_Joint.connectedBody = null;
            
            //clear
            m_CurrentTakeable.m_ActivePointer = null;
            m_CurrentTakeable = null;
        }
        /*private IEnumerator TrackLifetime()
        {
            yield return new WaitForSeconds(m_Lifetime);
            SetInnactive();
        }
        public void SetInnactive()
        {
            Rigidbody targetBody = m_CurrentTakeable.GetComponent<Rigidbody>();
            targetBody.velocity = Vector3.zero;
            targetBody.angularVelocity = Vector3.zero;
       
            gameObject.SetActive(false);
        }*/
        


        private void Update()
        {
            if (!isActive)
            {
                isActive = true;
                this.transform.GetChild(0).gameObject.SetActive(true);
            }

            float dist = 100f;

            //Ray raycast = new Ray(transform.position, transform.forward);
            Ray raycast = new Ray(reference.position, reference.forward);
            
            RaycastHit hit;
            bool bHit = Physics.Raycast(raycast, out hit);

            if (previousContact && previousContact != hit.transform)
            {
                PointerEventArgs args = new PointerEventArgs();
                args.fromInputSource = pose.inputSource;
                args.distance = 0f;
                args.flags = 0;
                args.target = previousContact;
                OnPointerOut(args);
                previousContact = null;
            }
            if (bHit && previousContact != hit.transform)
            {
                PointerEventArgs argsIn = new PointerEventArgs();
                argsIn.fromInputSource = pose.inputSource;
                argsIn.distance = hit.distance;
                argsIn.flags = 0;
                argsIn.target = hit.transform;
                OnPointerIn(argsIn);
                previousContact = hit.transform;
            }
            if (!bHit)
            {
                previousContact = null;
            }
            if (bHit && hit.distance < 100f)
            {
                dist = hit.distance;
            }

            if (bHit && m_GrabAction.GetStateUp(pose.inputSource))
            {
                PointerEventArgs argsClick = new PointerEventArgs();
                argsClick.fromInputSource = pose.inputSource;
                argsClick.distance = hit.distance;
                argsClick.flags = 0;
                argsClick.target = hit.transform;
                OnPointerClick(argsClick);
            }

            if (m_GrabAction != null && m_GrabAction.GetState(pose.inputSource))
            {
                pointer.transform.localScale = new Vector3(thickness * 5f, thickness * 5f, dist);
                pointer.GetComponent<MeshRenderer>().material.color = clickColor;
            }
            else
            {
                pointer.transform.localScale = new Vector3(thickness, thickness, dist);
                pointer.GetComponent<MeshRenderer>().material.color = color;
            }
            pointer.transform.localPosition = new Vector3(0f, 0f, dist / 2f);
        }
    }

    public struct PointerEventArgs
    {
        public SteamVR_Input_Sources fromInputSource;
        public uint flags;
        public float distance;
        public Transform target;
    }

    public delegate void PointerEventHandler(object sender, PointerEventArgs e);

