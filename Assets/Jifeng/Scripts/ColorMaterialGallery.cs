using System;
using System.Collections;
using UnityEngine;

namespace Jifeng.DemoSoftVr
{
    // while editing to many dices in same level,color one make it easy to find
    //      game view will not use it.
    public class ColorMaterialGallery :MonoBehaviour
    {
        [Tooltip("color the position helper,play mode does not use it")]
        public Material DefaultMat;
        public Material RedMat;
        public Material OrangeMat;
        public Material YellowMat;
        public Material GreenMat;
        public Material BlueMat;
        public Material PurpleMat;
        public Material TanMat;
        public Material BlackMat;
        public Material GrayMat;
        public Material PinkMat;

        [Tooltip("auto load from Resources/ColorMaterialGallery")]
        public bool LoadFromMaterialGallery = false;

        public enum EditTagType :int
        {
            eDefault = 0 ,
            eRed ,
            eOrange ,
            eYellow ,
            eGreen ,
            eBlue ,
            ePurple ,
            eTan ,
            eBlack ,
            eGray ,
            ePink ,
        }

        void    Start()
        {
            tryLoaded = false;
            if(LoadFromMaterialGallery)
            {
                LoadColorMaterials();
            }
        }

        // get a random material
        public EditTagType GetSchemaMaterial()
        {           
            int v = UnityEngine.Random.Range(0, (int)EditTagType.ePink);
            return (EditTagType)v;
        }

        // get material with special color
        public Material GetSchemaMaterial(EditTagType ett)
        {
            var v = this;
            switch (ett)
            {
                case ColorMaterialGallery.EditTagType.eRed:
                    return v.RedMat;
                case ColorMaterialGallery.EditTagType.eOrange:
                    return v.OrangeMat;
                case ColorMaterialGallery.EditTagType.eYellow:
                    return v.YellowMat;
                case ColorMaterialGallery.EditTagType.eGreen:
                    return v.GreenMat;
                case ColorMaterialGallery.EditTagType.eBlue:
                    return v.BlueMat;
                case ColorMaterialGallery.EditTagType.ePurple:
                    return v.PurpleMat;
                case ColorMaterialGallery.EditTagType.eTan:
                    return v.TanMat;
                case ColorMaterialGallery.EditTagType.eBlack:
                    return v.BlackMat;
                case ColorMaterialGallery.EditTagType.eGray:
                    return v.GrayMat;
                case ColorMaterialGallery.EditTagType.ePink:
                    return v.PinkMat;
                default:
                    return v.DefaultMat;
            }
        }

        private bool tryLoaded = false;
        public  void    LoadColorMaterials()
        {
            if(tryLoaded)
            {
                return;
            }
            tryLoaded = true;
            RedMat = LoadMaterialHelp("red");
            OrangeMat = LoadMaterialHelp("orange");
            YellowMat = LoadMaterialHelp("yellow");
            GreenMat = LoadMaterialHelp("green");
            BlueMat = LoadMaterialHelp("blue");
            PurpleMat = LoadMaterialHelp("purple");
            TanMat = LoadMaterialHelp("tan");
            BlackMat = LoadMaterialHelp("black");
            GrayMat = LoadMaterialHelp("gray");
            PinkMat = LoadMaterialHelp("pink");
            DefaultMat = LoadMaterialHelp("default");
        }

        private readonly string COLOR_PATH =
                            @"Assets/Jifeng/Materials/ColorMaterialGallery/";
        private Material    LoadMaterialHelp(string name)
        {
           
            var p = UnityEditor.AssetDatabase.LoadAssetAtPath(
                                        COLOR_PATH + name + ".mat"
                                        ,typeof(Material));
            return p as Material;
        }
    }    
}
