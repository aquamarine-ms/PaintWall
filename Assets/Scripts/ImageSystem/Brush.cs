using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Brush
{
    void Paint(int x, int y, TextureOutline tex);
    void SetSize(float size);
}
