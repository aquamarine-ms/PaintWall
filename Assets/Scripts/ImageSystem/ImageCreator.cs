using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class ImageCreator
{
    private Material material;
    private Material outlineMaterial;
    private TextureOutline texture;
    private Color colorOutline = new Color(1,0,0,1);
    private Brush brush;


    public ImageCreator(Material mat, Material outlineMat, Brush br)
    {
        material = mat;
        outlineMaterial = outlineMat;
        brush = br;
        texture = new TextureOutline();
    }


    public void InitPicture(Texture2D tex)
    {
        texture.SetTexture(tex);
        texture.SetOutlineTexture(GetOutlineTexture(tex));
        texture.SetPaintingTexture(GetPaintingTexture(tex));
        CountingPixels();
        material.SetTexture("_MainTex", texture.GetPaintingTexture());
        outlineMaterial.SetTexture("_MainTex", texture.GetOutlineTexture());
    }

    public void SetColorOutline(Color32 col)
    {

    }

    public TextureOutline GetTextureOuline()
    {
        return texture;
    }

    public int RemapX(float num, float fromMin, float fromMax)
    {
        Vector2Int size = GetSize();
        int toMin = 0;
        int toMax = size.x;


        return (int)((toMax - toMin)*((num - fromMin) / (fromMax - fromMin)) + toMin);
    }

    public int RemapY(float num, float fromMin, float fromMax)
    {
        Vector2Int size = GetSize();
        int toMin = 0;
        int toMax = size.y;

        return (int)((toMax - toMin) * (((num - fromMin) / (fromMax - fromMin))) + toMin);
    }

    public void Paint(int x, int y)
    {
        brush.Paint(x, y, texture);
        //CountingPaintedPixels();
    }

    public Vector2Int GetSize()
    {
        return new Vector2Int(texture.GetPaintingTexture().width, texture.GetPaintingTexture().height);
    }

    public Color GetColor(int x, int y)
    {
        return texture.GetTexture().GetPixel(x, y);
    }
    void CountingPixels()
    {
        float validPixels = 0;
        float badPixels = 0;

        Texture2D tex = texture.GetTexture();

        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                Color col = tex.GetPixel(x, y);

                if(col.a >= 0.1f)
                {
                    validPixels+=col.a;
                }
                else
                {
                    badPixels+=(1 - col.a);
                }
            }
        }

        texture.SetValidPixels(validPixels);
        texture.SetBadPixels(badPixels);
    }

    void CountingPaintedPixels()
    {
        float validPixels = 0;
        float badPixels = 0;

        Texture2D texPainted = texture.GetPaintingTexture();
        Texture2D tex = texture.GetTexture();

        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                Color col = tex.GetPixel(x, y);
                Color colPainted = texPainted.GetPixel(x, y);

                if (col.a >= 0.1f)
                {

                    validPixels+=colPainted.a/col.a;
                }
                else
                {

                    badPixels += colPainted.a;
                }
            }
        }

        texture.SetPaintedValidPixels(validPixels);
        texture.SetPaintedBadPixels(badPixels);
    }

    bool CheckPixels(Texture2D tex, int x, int y)
    {
        int w = tex.width;
        int h = tex.height;

        if ((x - 1) >= 0 && (y - 1) >= 0 && tex.GetPixel(x - 1, y - 1).a < 0.3f) return true;
        if ((y - 1) >= 0 && tex.GetPixel(x, y - 1).a < 0.3f) return true;
        if ((x + 1) < w && (y - 1) >= 0 && tex.GetPixel(x + 1, y - 1).a < 0.3f) return true;
        if ((x - 1) >= 0 && tex.GetPixel(x - 1, y).a < 0.3f) return true;
        if ((x + 1) < w && tex.GetPixel(x + 1, y).a < 0.3f) return true;
        if ((x - 1) >= 0 && (y + 1) < h && tex.GetPixel(x - 1, y + 1).a < 0.3f) return true;
        if ((y + 1) < h && tex.GetPixel(x, y + 1).a < 0.3f) return true;
        if ((x + 1) < w && (y + 1) < h && tex.GetPixel(x + 1, y + 1).a < 0.3f) return true;

        if ((x - 1) < 0 || (y - 1) < 0 || (y + 1) > h || (x + 1) > w) return true;

        return false;
    }

    Texture2D GetPaintingTexture(Texture2D tex)
    {
        Texture2D paintingTex = new Texture2D(tex.width, tex.height, tex.format, false);
        paintingTex.filterMode = FilterMode.Point;
        paintingTex.name = tex.name;

        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                paintingTex.SetPixel(x, y, new Color(1, 1, 1, 0));
            }
        }
        paintingTex.Apply();
        return paintingTex;
    }
    Texture2D GetOutlineTexture(Texture2D tex)
    {
        
        Texture2D outlineTex = new Texture2D(tex.width,tex.height,tex.format,false);
        outlineTex.filterMode = FilterMode.Bilinear;
        outlineTex.name = tex.name;

        /*int maxSize = (int)Mathf.Sqrt(tex.width * tex.height / 16384);
        Debug.Log(maxSize);
        if (maxSize < 1)
            maxSize = 1;
        */

        for (int y = 0; y < tex.height; y++)
        {
            for(int x = 0; x < tex.width; x++)
            {
                Color color = tex.GetPixel(x, y);
                if(color.a < 0.3f)
                {
                    outlineTex.SetPixel(x, y, new Color(1, 1, 1, 0));
                }
                else
                {
                    if(CheckPixels(tex, x, y))
                    {
                        outlineTex.SetPixel(x, y, colorOutline);
                    }
                    else
                        outlineTex.SetPixel(x, y,new Color(1, 1, 1, 0));

                }
            }
        }
        
        outlineTex.Apply();
        return outlineTex;
    }

    public void SetSizeBrush(int size)
    {
        brush.SetSize(size);
    }
    public Vector2Int GetPositionOnWall(RaycastHit hit)
    {
        Vector3 size = hit.transform.GetComponent<BoxCollider>().size;
        Vector3 pos = hit.transform.transform.position;
        Vector3 scale = hit.transform.localScale;
        //Debug.Log("StartPos:" + pos + "Size: " + imageCreator.GetSize() + " Position: " + hit.point + "Size2:" + size);
        float x = (size.x / 2 * scale.x);
        float y = (size.z / 2 * scale.z);
        return new Vector2Int(RemapX(hit.point.x, pos.x - x, pos.x + x), RemapY(hit.point.y, pos.y - y, pos.y + y));
    }
    
}
