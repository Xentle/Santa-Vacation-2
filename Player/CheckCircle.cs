using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCircle : MonoBehaviour
{
    struct myLine
    {
        public Vector3 S;
        public Vector3 E;
    };

    Transform transform = null;
    TrailRenderer trailRenderer = null;
    public Vector3 Centor;
    public int CircleStart;
    public double dot;
    public double l;
    public double minX, minZ, maxX, maxZ, area;
    public int numPositions, numLines;
    public Vector3[] TrailRecord = new Vector3[1500];
    GameObject[] Enemylist;
    GameObject[] Woodlist;
    GameObject[] Stonelist;
    myLine[] lines = new myLine[300];
    bool check = true;

    public int resourceNumWood;
    public int resourceNumStone;
    public PlayerResource playerResource;
    public UIManager uiManager;

    private AudioSource audioPlayer;
    public AudioClip useClip;

    public ParticleSystem SplashEffect;
    private void Awake()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    void Update()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        numPositions = trailRenderer.GetPositions(TrailRecord);
        numLines = numPositions - 1;
        for (int i = 0; i < numPositions; i++)
            TrailRecord[i].y = 0.0f;

        if (isLineCollide())
        {
            Enemylist = GameObject.FindGameObjectsWithTag("Enemy");
            Woodlist = GameObject.FindGameObjectsWithTag("Wood");
            Stonelist = GameObject.FindGameObjectsWithTag("Stone");

            foreach (GameObject Enemy in Enemylist)
            {
                if (Enemy.name == "micro_dragon_fino(Clone)")
                {
                    //Debug.Log("I dont want dino to die!");
                    continue;
                }

                check = true;
                transform = Enemy.GetComponent<Transform>();
                myLine currentLine;
                currentLine.S = transform.position;
                currentLine.E = Centor;
                for (int i = CircleStart + 1; i < numLines - 1; i++)
                {
                    if (isLinesIntersect(lines[i], currentLine))
                    {
                        check = false;
                        break;
                    }
                }

                if (check)
                {
                    //Debug.Log("Destroy Enemy");
                    //EffectManager.Instance.SplashEffect(Enemy.transform.position, Vector3.up, null,
                    //    EffectManager.EffectType.Splash);
                    Instantiate(SplashEffect, Enemy.transform.position, Quaternion.LookRotation(Vector3.up));
                    audioPlayer.PlayOneShot(useClip);
                    
                    Destroy(Enemy);
                }
            }

            foreach (GameObject Wood in Woodlist)
            {
                check = true;
                transform = Wood.GetComponent<Transform>();
                myLine currentLine;
                currentLine.S = transform.position;
                currentLine.E = Centor;
                for (int i = CircleStart + 1; i < numLines - 1; i++)
                {
                    if (isLinesIntersect(lines[i], currentLine))
                    {
                        check = false;
                        break;
                    }
                }
                if (check)
                {
                    playerResource.GetWood(resourceNumWood);
                    uiManager.UpdateWood();

                    Destroy(Wood);
                }
            }

            foreach (GameObject Stone in Stonelist)
            {
                check = true;
                transform = Stone.GetComponent<Transform>();
                myLine currentLine;
                currentLine.S = transform.position;
                currentLine.E = Centor;
                for (int i = CircleStart + 1; i < numLines - 1; i++)
                {
                    if (isLinesIntersect(lines[i], currentLine))
                    {
                        check = false;
                        break;
                    }
                }
                if (check)
                {
                    playerResource.GetStone(resourceNumStone);
                    uiManager.UpdateStone();

                    Destroy(Stone);
                }
            }


        }
    }

    public bool isLineCollide()
    {
        for (int i = 0; i < numLines; i++)
        {
            lines[i].S = TrailRecord[i];
            lines[i].E = TrailRecord[i + 1];
        }
        myLine currentLine;
        currentLine.S = TrailRecord[numPositions - 2];
        currentLine.E = TrailRecord[numPositions - 1];

        for (int i = 0; i < numLines - 10; i++)
        {
            if (isLinesIntersect(lines[i], currentLine))
            {
                CircleStart = i;
                Centor = Vector3.zero;
                for (int j = CircleStart + 1; j <= numPositions - 2; j++)
                    Centor += (TrailRecord[j] / (numPositions - CircleStart - 2));
                for (int j = CircleStart + 1; j <= numPositions - 2; j++)
                {
                    if ((TrailRecord[j] - Centor).magnitude < 0.5)
                        return false;
                }
                return true;
            }
        }
        return false;
    }

    private bool checkPoints(Vector3 pointA, Vector3 pointB)
    {
        return (pointA.x == pointB.x && pointA.z == pointB.z);
    }

    bool isLinesIntersect(myLine L1, myLine L2)
    {
        dot = Vector3.Dot(L1.S - L1.E, L2.S - L2.E);
        l = (L1.S - L1.E).magnitude * (L2.S - L2.E).magnitude;
        minX = maxX = TrailRecord[CircleStart + 1].x;
        minZ = maxZ = TrailRecord[CircleStart + 1].z;


        if (checkPoints(L1.S, L2.S) ||
            checkPoints(L1.S, L2.E) ||
            checkPoints(L1.E, L2.S) ||
            checkPoints(L1.E, L2.E))
            return false;

        if (dot == l)
            return false;
        if (dot * (-1) == l)
            return false;

        if ((Mathf.Max(L1.S.x, L1.E.x) >= Mathf.Min(L2.S.x, L2.E.x)) &&
            (Mathf.Max(L2.S.x, L2.E.x) >= Mathf.Min(L1.S.x, L1.E.x)) &&
            (Mathf.Max(L1.S.z, L1.E.z) >= Mathf.Min(L2.S.z, L2.E.z)) &&
            (Mathf.Max(L2.S.z, L2.E.z) >= Mathf.Min(L1.S.z, L1.E.z)))
        {
            for (int i = CircleStart + 1; i < numPositions - 1; i++)
            {
                if (TrailRecord[i].x < minX)
                    minX = TrailRecord[i].x;
                if (TrailRecord[i].x > maxX)
                    maxX = TrailRecord[i].x;
                if (TrailRecord[i].z < minZ)
                    minZ = TrailRecord[i].z;
                if (TrailRecord[i].z > maxZ)
                    maxZ = TrailRecord[i].z;
            }
            area = (maxX - minX) * (maxZ - minZ);
            if ((maxX - minX) < 1.0f)
                area /= 10.0f;
            if ((maxZ - minZ) < 1.0f)
                area /= 10.0f;

            if (area > 10)
            {
                return true;
            }
        }

        return false;
    }
    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }

}
/*

*/
