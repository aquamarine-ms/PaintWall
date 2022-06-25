using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBrush : Brush
{

    private int _size = 0;

    public void SetSize(float size)
    {
        _size = (int)size;
    }

    public void Paint(int x, int y, TextureOutline tex)
    {
        Vector2 size = new Vector2Int(tex.GetPaintingTexture().width, tex.GetPaintingTexture().height);
        Texture2D paintTexture = tex.GetPaintingTexture();
        Texture2D texture = tex.GetTexture();
        
        float validPixels = tex.GetPaintedValidPixels();
        float badPixels = tex.GetPaintedBadPixels();

        for (int i = x - _size; i <= x + _size; i++)
            for (int j = y - _size; j <= y + _size; j++)
            {
                float distance = Vector2.Distance(new Vector2(i, j), new Vector2(x, y));
                if (distance <= _size && i < size.x && j < size.y && i >= 0 && j >= 0)
                {
                    Color color = texture.GetPixel(i, j);
                    float alpha = 1;
                    float fromMin = 1;
                    if (_size > 10)
                    {
                        fromMin = _size / 10;
                    }

                    if (distance <= fromMin)
                    {
                        alpha = 1;
                    }
                    else
                    {
                        alpha = Remap(distance, fromMin, _size, 1, 0);
                    }
                    
                    Color colPaint = paintTexture.GetPixel(i, j);
                    

                    

                    if (color.a >= 0.1f)
                    {
                        if (colPaint.a + alpha < color.a)
                        {
                            alpha = colPaint.a + alpha;
                        }
                        else
                        {
                            alpha = color.a;
                        }
                        
                        validPixels += alpha - colPaint.a;
                    }
                    else
                    {
                        if (colPaint.a + alpha < 1)
                        {
                            alpha = colPaint.a + alpha;
                        }
                        else
                        {
                            alpha = 1;
                        }
                        
                        badPixels += alpha - colPaint.a;

                        color = Color.white;
                    }

                    color.a = alpha;
                    paintTexture.SetPixel(i, j, color);
                }
            }
        Debug.Log(validPixels);
        paintTexture.Apply();
        tex.SetPaintedValidPixels(validPixels);
        tex.SetPaintedBadPixels(badPixels);
    }
    float Remap(float num, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (toMax - toMin)*((num - fromMin) / (fromMax - fromMin)) + toMin;
    }
}
