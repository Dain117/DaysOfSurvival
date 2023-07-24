using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    SkinnedMeshRenderer Neck;
    SkinnedMeshRenderer Head;
    SkinnedMeshRenderer BodyChest;
    SkinnedMeshRenderer Arms;
    SkinnedMeshRenderer Hands;
    SkinnedMeshRenderer Legs;
    SkinnedMeshRenderer Feet;
    SkinnedMeshRenderer Underwear;

    Color basic;

    // Start is called before the first frame update
    void Start()
    {
        Neck = GameObject.Find("Neck").GetComponent<SkinnedMeshRenderer>();
        Head = GameObject.Find("PHead").GetComponent<SkinnedMeshRenderer>();
        BodyChest = GameObject.Find("BodyChest").GetComponent<SkinnedMeshRenderer>();
        Arms = GameObject.Find("Arms").GetComponent<SkinnedMeshRenderer>();
        Hands = GameObject.Find("PHands").GetComponent<SkinnedMeshRenderer>();
        Legs = GameObject.Find("Legs").GetComponent<SkinnedMeshRenderer>();
        Feet = GameObject.Find("Feet").GetComponent<SkinnedMeshRenderer>();
        Underwear = GameObject.Find("Underwear").GetComponent<SkinnedMeshRenderer>();

        basic = Neck.material.color;

    }
    // Update is called once per frame
    void Update()
    {
    
    }

    public void SkinRed()
    {
        Neck.material.color = new Color(0.7f, 0.2f, 0.2f);
        Head.material.color = new Color(0.7f, 0.2f, 0.2f);
        BodyChest.material.color = new Color(0.7f, 0.2f, 0.2f);
        Arms.material.color = new Color(0.7f, 0.2f, 0.2f);
        Hands.material.color = new Color(0.7f, 0.2f, 0.2f);
        Legs.material.color = new Color(0.7f, 0.2f, 0.2f);
        Feet.material.color = new Color(0.7f, 0.2f, 0.2f);
    }
    public void SkinGreen()
    {
        Neck.material.color = new Color(0.3f, 0.6f, 0.3f);
        Head.material.color = new Color(0.3f, 0.6f, 0.3f);
        BodyChest.material.color = new Color(0.3f, 0.6f, 0.3f);
        Arms.material.color = new Color(0.3f, 0.6f, 0.3f);
        Hands.material.color = new Color(0.3f, 0.6f, 0.3f);
        Legs.material.color = new Color(0.3f, 0.6f, 0.3f);
        Feet.material.color = new Color(0.3f, 0.6f, 0.3f);
    }
    public void SkinGray()
    {
        Neck.material.color = new Color(0.5f, 0.5f, 0.5f);
        Head.material.color = new Color(0.5f, 0.5f, 0.5f);
        BodyChest.material.color = new Color(0.5f, 0.5f, 0.5f);
        Arms.material.color = new Color(0.5f, 0.5f, 0.5f);
        Hands.material.color = new Color(0.5f, 0.5f, 0.5f);
        Legs.material.color = new Color(0.5f, 0.5f, 0.5f);
        Feet.material.color = new Color(0.5f, 0.5f, 0.5f);
    }
        public void SkinBlue()
    {
            Neck.material.color = new Color(0.4f, 0.4f, 0.6f);
        Head.material.color = new Color(0.4f, 0.4f, 0.6f);
        BodyChest.material.color = new Color(0.4f, 0.4f, 0.6f);
        Arms.material.color = new Color(0.4f, 0.4f, 0.6f);
        Hands.material.color = new Color(0.4f, 0.4f, 0.6f);
        Legs.material.color = new Color(0.4f, 0.4f, 0.6f);
        Feet.material.color = new Color(0.4f, 0.4f, 0.6f);
    }
    public void SkinWhite()
    {
            Neck.material.color = new Color(0.8f, 0.8f, 0.7f);
        Head.material.color = new Color(0.8f, 0.8f, 0.7f);
        BodyChest.material.color = new Color(0.8f, 0.8f, 0.7f);
        Arms.material.color = new Color(0.8f, 0.8f, 0.7f);
        Hands.material.color = new Color(0.8f, 0.8f, 0.7f);
        Legs.material.color = new Color(0.8f, 0.8f, 0.7f);
        Feet.material.color = new Color(0.8f, 0.8f, 0.7f);
    }
    public void SkinBlack()
    {
            Neck.material.color = new Color(0.2f, 0.1f, 0.1f);
        Head.material.color = new Color(0.2f, 0.1f, 0.1f);
        BodyChest.material.color = new Color(0.2f, 0.1f, 0.1f);
        Arms.material.color = new Color(0.2f, 0.1f, 0.1f);
        Hands.material.color = new Color(0.2f, 0.1f, 0.1f);
        Legs.material.color = new Color(0.2f, 0.1f, 0.1f);
        Feet.material.color = new Color(0.2f, 0.1f, 0.1f);
    }
    public void SkinYellow()
    {
        Neck.material.color = new Color(0.6f, 0.5f, 0.3f);
        Head.material.color = new Color(0.6f, 0.5f, 0.3f);
        BodyChest.material.color = new Color(0.6f, 0.5f, 0.3f);
        Arms.material.color = new Color(0.6f, 0.5f, 0.3f);
        Hands.material.color = new Color(0.6f, 0.5f, 0.3f);
        Legs.material.color = new Color(0.6f, 0.5f, 0.3f);
        Feet.material.color = new Color(0.6f, 0.5f, 0.3f);
    }
    public void SkinBasic()
    {
        Neck.material.color = basic;
        Head.material.color = basic;
        BodyChest.material.color = basic;
        Arms.material.color = basic;
        Hands.material.color = basic;
        Legs.material.color = basic;
        Feet.material.color = basic;
    }

    public void UnderRed()
    {
        Underwear.material.color = new Color(0.7f, 0.2f, 0.2f);
    }
    public void UnderGreen()
    {
        Underwear.material.color = new Color(0.3f, 0.6f, 0.3f);
    }
    public void UnderGray()
    {
        Underwear.material.color = new Color(0.5f, 0.5f, 0.5f);
    }
    public void UnderBlue()
    {
        Underwear.material.color = new Color(0.4f, 0.4f, 0.6f);
    }
    public void UnderBlack()
    {
        Underwear.material.color = new Color(0.2f, 0.1f, 0.1f);
    }
    public void UnderYellow()
    {
        Underwear.material.color = new Color(0.6f, 0.5f, 0.3f);
    }
    public void UnderWhite()
    {
        Underwear.material.color = new Color(0.8f, 0.8f, 0.7f); 
    }
        public void UnderBasic()
    {
        Underwear.material.color = basic;
    }
}
