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
        Head = GameObject.Find("Head").GetComponent<SkinnedMeshRenderer>();
        BodyChest = GameObject.Find("BodyChest").GetComponent<SkinnedMeshRenderer>();
        Arms = GameObject.Find("Arms").GetComponent<SkinnedMeshRenderer>();
        Hands = GameObject.Find("Hands").GetComponent<SkinnedMeshRenderer>();
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
        Neck.material.color = Color.red;
        Head.material.color = Color.red;
        BodyChest.material.color = Color.red;
        Arms.material.color = Color.red;
        Hands.material.color = Color.red;
        Legs.material.color = Color.red;
        Feet.material.color = Color.red;
    }
    public void SkinGreen()
    {
        Neck.material.color = Color.green;
        Head.material.color = Color.green;
        BodyChest.material.color = Color.green;
        Arms.material.color = Color.green;
        Hands.material.color = Color.green;
        Legs.material.color = Color.green;
        Feet.material.color = Color.green;
    }
    public void SkinGray()
    {
        Neck.material.color = Color.gray;
        Head.material.color = Color.gray;
        BodyChest.material.color = Color.gray;
        Arms.material.color = Color.gray;
        Hands.material.color = Color.gray;
        Legs.material.color = Color.gray;
        Feet.material.color = Color.gray;
    }
        public void SkinBlue()
    {
            Neck.material.color = Color.blue;
            Head.material.color = Color.blue;
            BodyChest.material.color = Color.blue;
            Arms.material.color = Color.blue;
            Hands.material.color = Color.blue;
            Legs.material.color = Color.blue;
            Feet.material.color = Color.blue;
        }
    public void SkinWhite()
    {
            Neck.material.color = Color.white;
            Head.material.color = Color.white;
            BodyChest.material.color = Color.white;
            Arms.material.color = Color.white;
            Hands.material.color = Color.white;
            Legs.material.color = Color.white;
            Feet.material.color = Color.white;
        }
    public void SkinBlack()
    {
            Neck.material.color = Color.black;
            Head.material.color = Color.black;
            BodyChest.material.color = Color.black;
            Arms.material.color = Color.black;
            Hands.material.color = Color.black;
            Legs.material.color = Color.black;
            Feet.material.color = Color.black;
        }
    public void SkinYellow()
    {
        Neck.material.color = Color.yellow;
        Head.material.color = Color.yellow;
        BodyChest.material.color = Color.yellow;
        Arms.material.color = Color.yellow;
        Hands.material.color = Color.yellow;
        Legs.material.color = Color.yellow;
        Feet.material.color = Color.yellow;
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
        Underwear.material.color = Color.red;
    }
    public void UnderGreen()
    {
        Underwear.material.color = Color.green;
    }
    public void UnderGray()
    {
        Underwear.material.color = Color.gray;
    }
    public void UnderBlue()
    {
        Underwear.material.color = Color.blue;
    }
    public void UnderBlack()
    {
        Underwear.material.color = Color.black;
    }
    public void UnderYellow()
    {
        Underwear.material.color = Color.yellow;
    }
    public void UnderWhite()
    {
        Underwear.material.color = Color.white;
    }
    public void UnderBasic()
    {
        Underwear.material.color = basic;
    }
}
