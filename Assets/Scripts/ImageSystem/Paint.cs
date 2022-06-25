using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint : MonoBehaviour
{
    public ImageCreator imageCreator;
    public ImageSystem imageSystem;

    private Camera camera;
    void Start()
    {
        camera = this.GetComponent<Camera>();
    }


    public void SetImageCreator(ImageCreator IC)
    {
        imageCreator = IC;
    }

    // Update is called once per frame
    void Update()
    {
        Hit();
    }

    void Hit()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10000))
            {
                if (hit.transform.tag == "Paint")
                {
                    Vector2Int hitPoint = imageCreator.GetPositionOnWall(hit);
                    imageCreator.Paint(hitPoint.x, hitPoint.y);
                    imageSystem.UpdateUI();
                }
            }
        }
    }
}
