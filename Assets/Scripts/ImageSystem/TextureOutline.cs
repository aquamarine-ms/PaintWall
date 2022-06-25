using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureOutline
{
    private Texture2D normalTexture;
    private Texture2D outlineTexture;
    private Texture2D paintingTexture;
    private float countValidPixels;
    private float countBadPixels;

    private float countPaintedValidPixels;
    private float countPaintedBadPixels;


    public void SetValidPixels(float pix)
    {
        countValidPixels = pix;
    }

    public void SetBadPixels(float pix)
    {
        countBadPixels = pix;
    }

    public float GetValidPixels()
    {
        return countValidPixels;
    }

    public float GetBadPixels()
    {
        return countBadPixels;
    }

    public void SetPaintedValidPixels(float pix)
    {
        countPaintedValidPixels = pix;
    }

    public void SetPaintedBadPixels(float pix)
    {
        countPaintedBadPixels = pix;
    }

    public float GetPaintedValidPixels()
    {
        return countPaintedValidPixels;
    }

    public float GetPaintedBadPixels()
    {
        return countPaintedBadPixels;
    }

    public void SetTexture(Texture2D tex)
    {
        normalTexture = tex;
    }

    public void SetOutlineTexture(Texture2D tex)
    {
        outlineTexture = tex;
    }

    public void SetPaintingTexture(Texture2D tex)
    {
        paintingTexture = tex;
    }

    public Texture2D GetTexture()
    {
        return normalTexture;
    }

    public Texture2D GetOutlineTexture()
    {
        return outlineTexture;
    }

    public Texture2D GetPaintingTexture()
    {
        return paintingTexture;
    }
}
