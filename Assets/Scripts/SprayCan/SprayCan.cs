using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SprayCan : MonoBehaviour
{
    public Camera cam;

    public ParticleSystem sprayEffect;

    private ImageCreator imageCreator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Spray();
    }

    public void SetImageCreator(ImageCreator creator)
    {
        imageCreator = creator;
    }

    void Move()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000))
        {

            transform.position = Vector3.Lerp(transform.position,
                new Vector3(hit.point.x, hit.point.y - 1, hit.point.z - 3), 10 * Time.deltaTime);

            transform.LookAt(hit.point);
        }
        else
        {
            Vector3 position = cam.transform.position;
            transform.position = Vector3.Lerp(transform.position,
                position - Vector3.Normalize(cam.ScreenToWorldPoint(Input.mousePosition)) * 5, 10 * Time.deltaTime);
        }
    }

    void Spray()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Color color = Color.white;
            if (Physics.Raycast(ray, out hit, 10000))
            {
                if (hit.transform.CompareTag("Paint"))
                {
                    var sprayEffectMain = sprayEffect.main;
                    sprayEffectMain.startColor = GetColor(hit);
                    sprayEffect.Play();
                }
            }
        }
    }

    Color GetColor(RaycastHit hit)
    {
        Color color = Color.white;

        Vector2Int hitPoint = imageCreator.GetPositionOnWall(hit);
        color = imageCreator.GetColor(hitPoint.x, hitPoint.y);
        if (color.a < 0.1f)
        {
            color = Color.white;
        }

        return color;
    }
}
