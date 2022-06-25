using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureBrush : Brush
{
    Texture2D texture;
    float _size = 1;
    public TextureBrush(Texture2D tex)
    {
        texture = tex;
    }

    public void Paint(int _x, int _y, TextureOutline tex)
    {
        Vector2Int size = new Vector2Int(tex.GetPaintingTexture().width, tex.GetPaintingTexture().height);
        Vector2Int brushSize = new Vector2Int(texture.width, texture.height);
        Vector2Int brushCenter = new Vector2Int((int)(brushSize.x/2), (int)(brushSize.y / 2));
        

        Vector2Int startPoint = new Vector2Int(_x - brushCenter.x, _y - brushCenter.y);
        Vector2Int endPoint = new Vector2Int(_x + brushCenter.x, _y + brushCenter.y);

        Texture2D paintTexture = tex.GetPaintingTexture();
        Texture2D textu = tex.GetTexture();

        Vector2Int point = new Vector2Int(0, 0);

        float validPixels = tex.GetPaintedValidPixels();
        float badPixels = tex.GetPaintedBadPixels();
        
        for (int y = startPoint.y; y <= endPoint.y; y++) {
            for (int x = startPoint.x; x <= endPoint.x; x++)
            {
                Color col = texture.GetPixel(point.x, point.y);
                Color colPaint = paintTexture.GetPixel(x, y);
                if (x < size.x && y < size.y && x >= 0 && y >= 0)
                {
                    Color colText = textu.GetPixel(x, y);
                    if (colText.a >= 0.1f)
                    {
                        if (colText.a > colPaint.a)
                        {
                            float alp = colPaint.a;
                            colPaint = colText;
                            colPaint.a = alp;
                            
                            
                            colPaint.a += col.a;

                            if (colText.a < colPaint.a)
                                colPaint.a = colText.a;

                            paintTexture.SetPixel(x, y, colPaint);

                            validPixels += colPaint.a - alp;
                        }
                    }
                    else
                    {
                        float alp = colPaint.a;
                        Color color = col;
                        color.a += colPaint.a;
                        

                        paintTexture.SetPixel(x, y, color);

                        if ((1.0f - colPaint.a) >= col.a)
                        {
                            badPixels += col.a;
                        }
                        else
                        {
                            badPixels += (1.0f - colPaint.a);
                        }
                        
                    }
                }
                point.x++;
            }
            point.x = 0;
            point.y++;
        }
        
        paintTexture.Apply();
        
        tex.SetPaintedValidPixels(validPixels);
        tex.SetPaintedBadPixels(badPixels);
    }
    public void SetSize(float size)
    {
        _size = size;
        texture.Resize((int)(texture.width * size), (int)(texture.height * size));
        texture.Apply();
    }
    
}
