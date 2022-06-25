using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallImage : MonoBehaviour
{
    public ImageCreator imageCreator;
    public int scaleBrush;
    public void SetImageCreator(ImageCreator IC)
    {
        imageCreator = IC;
    }

    private void OnCollisionStay(Collision collision)
    {

        Vector3 size = this.GetComponent<BoxCollider>().size;
        Vector3 pos = this.transform.position;
        Vector3 scale = this.transform.localScale;
        if (collision.transform.tag == "Paint")
        {  
            foreach (ContactPoint hit in collision.contacts)
            {
                Debug.Log("StartPos:"+ pos +"Size: "+ imageCreator.GetSize() + " Position: "+hit.point + "Size2:" + size);
                float x = (size.x / 2 * scale.x);
                float y = (size.z / 2 * scale.z);
                Vector2Int hitPoint = new Vector2Int(imageCreator.RemapX(hit.point.x, pos.x - x, pos.x + x),imageCreator.RemapY(hit.point.y, pos.y - y, pos.y + y));
                Debug.Log(hitPoint);
                
                imageCreator.Paint(hitPoint.x, hitPoint.y);

            }
        }
    }
}
