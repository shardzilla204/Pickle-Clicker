using PickleClicker.Pickle;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PickleClicker 
{  
    public class SpriteMerger : MonoBehaviour
    {
        [SerializeField] private List<Sprite> topper = new List<Sprite>();
        [SerializeField] private List<Sprite> body = new List<Sprite>();
        [SerializeField] private List<Sprite> accessories = new List<Sprite>();
        [SerializeField] private List<Sprite> skin = new List<Sprite>();

        [SerializeField] private List<EquipSlot> equipSlots = new List<EquipSlot>();
        [SerializeField] private Image image;
        [SerializeField] private SpriteRenderer result;
        [SerializeField] private SpriteRenderer pickleButton;

        [SerializeField] private Sprite defaultSkin;

        private void Awake() 
        {
            equipSlots = GameObject.FindObjectsOfType<EquipSlot>().ToList();
            pickleButton = GameObject.FindObjectOfType<PickleButton>().GetComponent<SpriteRenderer>();
            result = gameObject.GetComponent<SpriteRenderer>();
            image = gameObject.GetComponent<Image>();
            result.sprite = image.sprite;
            skin.Add(defaultSkin);
        }

        public void AppendToList(GameObject itemToAdd)
        {
            CosmeticScriptableObject cosmeticObject = itemToAdd.GetComponent<Cosmetic>().cosmeticScriptableObject;
            itemToAdd.GetComponent<Cosmetic>().typePanel.SetActive(false);

            Sprite cosmeticSprite = cosmeticObject.cosmetic;

            if (topper.Contains(cosmeticSprite)) return;
            if (body.Contains(cosmeticSprite)) return;
            if (accessories.Contains(cosmeticSprite)) return;
            if (skin.Contains(cosmeticSprite)) return;

            if (cosmeticObject.cosmeticType == CosmeticType.Topper) topper.Add(cosmeticSprite);
            if (cosmeticObject.cosmeticType == CosmeticType.Body) body.Add(cosmeticSprite);
            if (cosmeticObject.cosmeticType == CosmeticType.Accessory) accessories.Add(cosmeticSprite);
            if (cosmeticObject.cosmeticType == CosmeticType.Skin) skin.Add(cosmeticSprite);   

            if (skin.Count == 0 && !skin.Contains(defaultSkin))
            {
                skin.Add(defaultSkin);
            }    
            else if (skin.Count > 1)
            {
                skin.Remove(defaultSkin);
            }
        
            Merge();    
        }

        public void RemoveFromList(GameObject itemToRemove)
        {   
            CosmeticScriptableObject cosmeticObject = itemToRemove.GetComponent<Cosmetic>().cosmeticScriptableObject;
            itemToRemove.GetComponent<Cosmetic>().typePanel.SetActive(true);

            Sprite cosmeticSprite = cosmeticObject.cosmetic;

            // Debug.Log(cosmeticSprite);

            if (cosmeticObject.cosmeticType == CosmeticType.Topper) topper.Remove(cosmeticSprite);
            if (cosmeticObject.cosmeticType == CosmeticType.Body) body.Remove(cosmeticSprite);
            if (cosmeticObject.cosmeticType == CosmeticType.Accessory) accessories.Remove(cosmeticSprite);
            if (cosmeticObject.cosmeticType == CosmeticType.Skin) skin.Remove(cosmeticSprite);   
            
            if (skin.Count == 0) skin.Add(defaultSkin);

            // Debug.Log("Removed Object!");
            Merge();
        }

        public void ClearSprites()
        {
            topper.Clear();
            body.Clear();
            accessories.Clear();
            skin.Clear();

            foreach(EquipSlot equipSlot in equipSlots)
            {
                if (equipSlot.transform.childCount != 1)
                {
                    equipSlot.GetComponentInChildren<Cosmetic>().typePanel.SetActive(true);
                    equipSlot.OnClear(); 
                }
            }

            if (skin.Count <= 0) skin.Add(defaultSkin);

            Merge();
        }

        public void Merge()
        {
            Texture2D texture = new Texture2D(image.sprite.texture.width, image.sprite.texture.height);
            Color[] colorArray = new Color[texture.width * texture.height];
            Color[][] toppersArray = new Color[topper.Count][];
            Color[][] bodyArray = new Color[body.Count][];
            Color[][] accessoriesArray = new Color[accessories.Count][];
            Color[][] skinArray = new Color[skin.Count][];

            if (skin.Count > 0) 
            {
                skinArray[0] = skin[0].texture.GetPixels();
                colorArray[0] = ApplySkin(texture, colorArray, skinArray)[0];
            }

            if (body.Count > 0) 
            {
                bodyArray[0] = body[0].texture.GetPixels();
                colorArray[1] = ApplyBody(texture, colorArray, bodyArray)[1];
            }

            if (topper.Count > 0)
            {
                for (int index = 0; index < topper.Count; index++)
                {
                    toppersArray[index] = topper[index].texture.GetPixels();
                    colorArray[index + 2] = ApplyTopper(texture, colorArray, toppersArray)[index];
                }
            }
            if (accessories.Count > 0)
            {
                for (int index = 0; index < accessories.Count; index++)
                {
                    accessoriesArray[index] = accessories[index].texture.GetPixels();
                    colorArray[index + 4] = ApplyAccessories(texture, colorArray, accessoriesArray)[index];
                }
            }
        

            texture.SetPixels(colorArray);
            texture.Apply();

            Sprite finalSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 3);
            finalSprite.name = "NewSprite";
            result.sprite = finalSprite;
            Debug.Log(pickleButton);
            Debug.Log(finalSprite);
            pickleButton.sprite = finalSprite;
        }

        private Color[] ApplySkin(Texture2D texture, Color[] colors, Color[][] skinArray)
        {
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    int pixelIndex = x + (y * texture.width);

                    Color skinPixel = skinArray[0][pixelIndex];
                    if (skinPixel.a == 1)
                    {
                        colors[pixelIndex] = skinPixel;
                    }
                    else if (skinPixel.a > 0)
                    {
                        colors[pixelIndex] = NormalBlend(colors[pixelIndex], skinPixel);
                    }
                }
            }
            return colors;
        }

        private Color[] ApplyBody(Texture2D texture, Color[] colors, Color[][] bodyArray)
        {
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    int pixelIndex = x + (y * texture.width);

                    Color bodyPixel = bodyArray[0][pixelIndex];
                    if (bodyPixel.a == 1)
                    {
                        colors[pixelIndex] = bodyPixel;
                    }
                    else if (bodyPixel.a > 0)
                    {
                        colors[pixelIndex] = NormalBlend(colors[pixelIndex], bodyPixel);
                    }
                }
            }
            return colors;
        }

        private Color[] ApplyTopper(Texture2D texture, Color[] colors, Color[][] toppersArray)
        {
            try 
            {
                for (int x = 0; x < texture.width; x++)
                {
                    for (int y = 0; y < texture.height; y++)
                    {
                        int pixelIndex = x + (y * texture.width);

                        for (int index = 0; index < topper.Count; index++)
                        {
                            Color topperPixel = toppersArray[index][pixelIndex];
                            if (topperPixel.a == 1)
                            {
                                colors[pixelIndex] = topperPixel;
                            }
                            else if (topperPixel.a > 0)
                            {
                                colors[pixelIndex] = NormalBlend(colors[pixelIndex], topperPixel);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            return colors;
        }

        private Color[] ApplyAccessories(Texture2D texture, Color[] colors, Color[][] accessoriesArray)
        {
            try 
            {
                for (int x = 0; x < texture.width; x++)
                {
                    for (int y = 0; y < texture.height; y++)
                    {
                        int pixelIndex = x + (y * texture.width);

                        for (int index = 0; index < accessories.Count; index++)
                        {
                            Color accessoryPixel = accessoriesArray[index][pixelIndex];
                            if (accessoryPixel.a == 1)
                            {
                                colors[pixelIndex] = accessoryPixel;
                            }
                            else if (accessoryPixel.a > 0)
                            {
                                colors[pixelIndex] = NormalBlend(colors[pixelIndex], accessoryPixel);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            return colors;
        }

        private Color NormalBlend(Color destination, Color sprite)
        {
            float spriteAlpha = sprite.a;
            float destinationAlpha = (1 - spriteAlpha) * destination.a;
            Color destinationLayer = destination * destinationAlpha;
            Color spriteLayer = sprite * spriteAlpha;
            return destinationLayer + spriteLayer;
        }
    }
}