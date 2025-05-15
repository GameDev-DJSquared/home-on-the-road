using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(ObjectInteractor))]
public class InventoryScript : MonoBehaviour
{

    public List<GameObject> prefabAssets;
    private Dictionary<string, GameObject> prefabDict;

    [SerializeField] Slider inventorySlider;

    [SerializeField]
    Image[] inventoryImages;

    [SerializeField] Light flashlightLight;
    [SerializeField]
    AudioClip[] flashlightSounds;
    [SerializeField] Animator flashlightAnimator;
    [SerializeField] GameObject flashlightModel;

    [SerializeField] int inventoryCapacity = 5;
    [SerializeField] float spawnDistance = 1f;
    [SerializeField] int healthHeal = 20;
    [SerializeField] float itemDamageAmount = 25;
    [SerializeField] float flashlightDamageRate = 0.005f;

    AudioSource audioSource;
    
    Item[] slots;
    Item.Type[] slotTypes;
    GameObject[] prefabs;

    int selectedSlot = 0;



    private void Awake()
    {
        slots = new Item[inventoryCapacity];
        slotTypes = new Item.Type[inventoryCapacity];
        prefabs = new GameObject[inventoryCapacity];
        slotTypes[0] = Item.Type.Other;
        slotTypes[1] = Item.Type.Other;

        audioSource = GetComponent<AudioSource>();
        GetComponent<ObjectInteractor>().OnObjectGrabbed += OnObjectGrabbed;

        prefabDict = new Dictionary<string, GameObject>();
        foreach (GameObject prefab in prefabAssets)
        {
            if (prefab != null)
            {
                prefabDict[prefab.name] = prefab;
            }
        }
    }


    private void Update()
    {
        if(GameManager.instance.paused)
        {
            return;
        }

        

        if(InputManager.instance.GetDropPressed())
        {
            DropItem();
        }

        if(slots[selectedSlot] != null)
        {
            


            if(slots[selectedSlot].type == Item.Type.Food && InputManager.instance.GetInteractPressed() && GetComponent<PlayerHealth>().health != GetComponent<PlayerHealth>().healthI)
            {
                GetComponent<PlayerHealth>().health += healthHeal;
                slots[selectedSlot] = null;
            } else if (slots[selectedSlot].type == Item.Type.Other && slots[selectedSlot].name == "flashlight" && InputManager.instance.GetInteractPressed())
            {
                if(flashlightLight.enabled)
                {
                    audioSource.clip = flashlightSounds[0];
                    audioSource.Play();
                } else
                {
                    audioSource.clip = flashlightSounds[1];
                    audioSource.Play();
                }
                flashlightLight.enabled = !flashlightLight.enabled;
            } else if (slots[selectedSlot].name == "flashlight" && InputManager.instance.GetFirePressed())
            {
                flashlightAnimator.Play("flashlightStrike", -1, 0f);
                //Debug.Log("FIRED");
            }

            
        } else
        {
            

        }



        // UI
        inventorySlider.value = WeightCapacity();

        for(int i = 0; i < inventoryImages.Length; i++)
        {
            if(selectedSlot == i)
            {
                inventoryImages[i].color = Color.yellow;

            } else
            {
                inventoryImages[i].color = Color.white;
            }

            if(slots.Length > i)
            {
                if (slots[i] != null)
                {
                    inventoryImages[i].transform.GetChild(1).gameObject.SetActive(true);
                    inventoryImages[i].transform.GetChild(1).GetComponent<Image>().sprite = slots[i].image;

                    if (slots[i].name == "flashlight")
                    {
                        inventoryImages[i].transform.GetChild(0).gameObject.SetActive(true);
                        float x = Mathf.Clamp(slots[i].durability / 100f, 0, 1);
                        inventoryImages[i].transform.GetChild(0).GetComponent<Image>().fillAmount = x;
                        inventoryImages[i].transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(Color.red, Color.white, x);
                        
                        if(flashlightLight.enabled && i == selectedSlot)
                        {
                            slots[i].durability -= flashlightDamageRate;

                        }

                        if (slots[i].durability <= 0)
                        {
                            slots[i] = null;
                            UpdateModel();
                        }
                    }
                    else
                    {
                        inventoryImages[i].transform.GetChild(0).gameObject.SetActive(false);

                    }
                } else
                {
                    inventoryImages[i].transform.GetChild(1).gameObject.SetActive(false);
                    inventoryImages[i].transform.GetChild(1).GetComponent<Image>().sprite = null;
                    inventoryImages[i].transform.GetChild(0).gameObject.SetActive(false);

                }
            }
        }

        if (InputManager.instance.GetSlot1Pressed())
        {
            selectedSlot = 0;
            UpdateModel();
            //Debug.Log("new slot: " + selectedSlot);
        }

        if (InputManager.instance.GetSlot2Pressed())
        {
            selectedSlot = 1;
            UpdateModel();
            //Debug.Log("new slot: " + selectedSlot);
        }
        if (InputManager.instance.GetSlot3Pressed())
        {
            selectedSlot = 2;
            UpdateModel();
            //Debug.Log("new slot: " + selectedSlot);
        }
        if (InputManager.instance.GetSlot4Pressed())
        {
            selectedSlot = 3;
            UpdateModel();
            //Debug.Log("new slot: " + selectedSlot);
        }
        if (InputManager.instance.GetSlot5Pressed())
        {
            selectedSlot = 4;
            UpdateModel();
            //Debug.Log("new slot: " + selectedSlot);
        }



        Vector2 scroll = InputManager.instance.GetScrollDir();
        if(scroll != Vector2.zero)
        {
            if (scroll.y < 0)
                selectedSlot = (selectedSlot + 1) % inventoryCapacity;
            else if (scroll.y > 0)
                selectedSlot = (selectedSlot - 1 + inventoryCapacity) % inventoryCapacity;


            UpdateModel();
            //Debug.Log("new slot: " + selectedSlot);
        }
    }


    public void UpdateModel()
    {
        if (slots[selectedSlot] == null)
        {
            flashlightModel.SetActive(false);


        }
        else if (slots[selectedSlot].name == "flashlight")
        {
            flashlightModel.SetActive(true);
        }
        else
        {
            flashlightModel.SetActive(false);
            flashlightLight.enabled = false;
        }
    }


    public void DropItem()
    {

        if (slots[selectedSlot] != null)
        {
            


            Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;
            Quaternion spawnRotation = transform.rotation;

            prefabs[selectedSlot].transform.position = spawnPosition;
            prefabs[selectedSlot].transform.rotation = spawnRotation;
            prefabs[selectedSlot].SetActive(true);
            if (slots[selectedSlot].name == "flashlight")
            {
                flashlightLight.enabled = false;
                flashlightModel.SetActive(false);

            }

            //GameObject go = Instantiate(prefabs[selectedSlot], spawnPosition, spawnRotation);
            // go.SetActive(true);
            slots[selectedSlot] = null;
        }

        UpdateModel();


    }


    void OnObjectGrabbed(Item item, GameObject go)
    {
        int remainingCapacity = inventoryCapacity - TotalWeight();
        if(remainingCapacity >= item.inventorySize && OpenSlot(item.type))
        {


            // ADD THAT SUCKA

            if (slots[selectedSlot] == null && slotTypes[selectedSlot] == item.type)
            {
                slots[selectedSlot] = item;
                prefabs[selectedSlot] = go;
            } else
            {
                int slotIndex = FindSlot(item.type);
                slots[slotIndex] = item;
                prefabs[slotIndex] = go;
            }

            if(DropOffZone.items.Contains(item))
            {
                DropOffZone.items.Remove(item);
                FindObjectOfType<DropOffZone>().UpdateText();
            }
            go.SetActive(false);
            //Destroy(go);

            UpdateModel();
        }
    }


    public void DamageWeapon()
    {
        if (slots[selectedSlot] == null || slots[selectedSlot].type == Item.Type.Food)
        {
            Debug.LogWarning("No weapon to damage");
            return;
        }

        slots[selectedSlot].durability -= itemDamageAmount;
        if (slots[selectedSlot].durability <= 0)
        {
            slots[selectedSlot] = null;
            UpdateModel();
        }
    }

    public GameObject GetPrefabByName(string name)
    {
        if (prefabDict.TryGetValue(name, out GameObject prefab))
        {
            return prefab;
        }
        else
        {
            Debug.LogWarning($"Prefab with name '{name}' not found.");
            return null;
        }
    }

    
    public bool FlashlightOn()
    {
        return flashlightLight.enabled;
    }
    
    
    //int FindSlot()
    //{
    //    for(int i = 0; i < slots.Length; i++)
    //    {
    //        if(slots[i] == null) return i;
    //    }
    //    return -1;
    //}


    //public bool OpenSlot()
    //{
    //    foreach(Item i in slots)
    //    {
    //        if (i == null) return true;
    //    }
    //    return false;
    //}

    int FindSlot(Item.Type type)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null && slotTypes[i] == type) return i;
        }
        return -1;
    }

    public bool OpenSlot(Item.Type type)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null && slotTypes[i] == type) return true;
        }
        return false;
    }

    public float WeightCapacity()
    {
        return Mathf.Clamp((float)TotalWeight() / inventoryCapacity, 0, 1);
    }

    public int TotalWeight()
    {
        int total = 0;
        foreach(Item i in slots)
        {
            if(i != null)
                total += i.inventorySize;
        }
        return total;
    }


}
