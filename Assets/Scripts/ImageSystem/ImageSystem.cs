using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSystem : MonoBehaviour
{
    public Text goodValueText;
    public Text badValueText;
    public Text percent;
    public Slider slider;


    public Paint brush;
    public SprayCan can;
    public GameObject wall;
    public GameObject outlineWall;
    public Texture2D brushTexture;
    ImageCreator creator;

    void Start()
    {
        Material mat = wall.GetComponent<MeshRenderer>().materials[0];
        Material outMat = outlineWall.GetComponent<MeshRenderer>().materials[0];
        Texture2D tex = (mat.GetTexture("_MainTex") as Texture2D);

        CircleBrush _brush = new CircleBrush();
        _brush.SetSize(32);

        //TextureBrush _brush = new TextureBrush(brushTexture);
        creator = new ImageCreator(mat, outMat, _brush);
        creator.InitPicture(tex);
        can.SetImageCreator(creator);
        brush.SetImageCreator(creator);
               
    }
    
    public void UpdateUI()
    {
        TextureOutline outline = creator.GetTextureOuline();
        float percentValid = outline.GetPaintedValidPixels() / outline.GetValidPixels() * 100;
        goodValueText.text = percentValid.ToString() + "%";


        float percentBad = outline.GetPaintedBadPixels() / outline.GetBadPixels() * 100;
        badValueText.text = percentBad.ToString() + "%";

        float perc = percentValid / (percentValid + percentBad);
        slider.value = perc;
        percent.text = (perc * 100).ToString();
        percent.color = Color.Lerp(Color.red, Color.green, perc);
    }


}
