using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    [Header("Detetion Parameters")]
    public Transform detectionPoint;

    private const float detectionRadius = 0.2f;

    public LayerMask detectionLayer;

    public GameObject detectedObject;
    [Header("Examine Fiels")]
    public GameObject ExamineWindown;
    public GameObject GrabObject;
    public float GrabObjectValue;
    public Transform GrabPoint;
    public Image ExamineImage;
    public Text ExamineText;
    public bool isExamining;
    public bool isGrabbing;
    

 

    void Update()
    {
        if (DetectObject())
        {
            if (InteractInput())
            {
                if (isGrabbing)
                {
                    GrabDrop();
                    return;
                }
                detectedObject.GetComponent<Item>().Interact();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position, detectionRadius);
    }

    private bool InteractInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
    private bool DetectObject()
    {
       
            

        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);
        if(obj == null)
        {
            detectedObject = null;
            return false;
        }
        else
        {
            detectedObject = obj.gameObject;
            return true;
        }
    }

   
   
    public void ExamineItem(Item item)
    {
        if(isExamining)
        {
            ExamineWindown.SetActive(false);
            isExamining = false;

        }
        else
        {
            ExamineImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            ExamineText.text = item.descriptionText;
            ExamineWindown.SetActive(true);
            isExamining = true;
        }

       
    }
    public void GrabDrop()
    {
        if(isGrabbing)
        {
            isGrabbing = false;

            GrabObject.transform.parent = null;

            GrabObject.transform.position =
                new Vector3(GrabObject.transform.position.x, GrabObjectValue, GrabObject.transform.position.z);

            GrabObject = null;
        }
        else
        {
            isGrabbing = true;
            GrabObject = detectedObject;
            GrabObject.transform.parent = transform;
            GrabObjectValue = GrabObject.transform.position.y;
            GrabObject.transform.localPosition = GrabPoint.localPosition;
        }
    }

  
}
