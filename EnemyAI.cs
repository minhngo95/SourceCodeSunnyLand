
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyAI : MonoBehaviour
{
    public List<Transform> points;

    public int nextID =0;

    int IdChangeValue = 1;

    public float speed = 2;

    private void Reset()
    {
        Init();
    }

    private void Init()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;

        GameObject root = new GameObject(name + "Root");

        root.transform.position = transform.position;

        transform.SetParent(root.transform);

        GameObject waypoints = new GameObject("Waypoint");

        waypoints.transform.SetParent(root.transform);
        waypoints.transform.position = root.transform.position;

        GameObject p1 = new GameObject("Point1"); p1.transform.SetParent(waypoints.transform);p1.transform.position = root.transform.position;
        GameObject p2 = new GameObject("Point2"); p2.transform.SetParent(waypoints.transform);p2.transform.position = root.transform.position;

        points = new List<Transform>();
        points.Add(p1.transform);
        points.Add(p2.transform);
    }

    private void Update()
    {
        MoveToNextPoint();
    }
    void MoveToNextPoint()
    {
        Transform goalPoint = points[nextID];

        if (goalPoint.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

        transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, speed * Time.deltaTime) ;

        if(Vector2.Distance(transform.position, goalPoint.position)<1f)
        {
            if (nextID == points.Count - 1)
                IdChangeValue = -1;
            if (nextID == 0)
                IdChangeValue = 1;

            nextID += IdChangeValue;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log($"{name} Triggered");
            //FindObjectOfType<LifeCount>().LoseLife();
            
        }
    }



}
