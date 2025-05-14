using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeScript : MonoBehaviour
{
    public Volume volume;
    public float speed = 0.02f;
    VolumeProfile profile;
    public static VolumeScript instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Volume Manager!");
        }
        instance = this;
        profile = volume.sharedProfile;
        //ChangeBloomImmediately(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //Outside Scripts access this function to change bloom
    public void ChangeBloom(bool raisingBloom = false)
    {
        
        if (raisingBloom)
            ChangeBloomImmediately(5);
        else
            StartCoroutine(BloomChanger(0, speed));
    }

    //Raising bloom doesn't look good, so just change it immediately
    public void ChangeBloomImmediately(float newValue)
    {
        if (profile.TryGet<Bloom>(out Bloom bloom))
        {
            MinFloatParameter bloomI = bloom.intensity;
            bloomI.value = newValue;
            bloom.intensity = bloomI;
        } else
        {
            Debug.Log("Bloom Not Found");
        }
            
    }

    
    private IEnumerator BloomChanger(float targetBloom, float speed)
    {

        
        
        //First make sure theres a Bloom Component
        if(profile.TryGet<Bloom>(out Bloom bloom)) {
            
            //Grab the intensity
            MinFloatParameter bloomI = bloom.intensity;

            //While the bloom is offtarget, incrementally change it until it is on target
            while (bloomI.value != targetBloom)
            {
                
                if((float)bloomI > targetBloom)
                {
                    bloomI.value -= speed / 10;
                    
                } else
                {
                    bloomI.value += speed / 10;
                }
                if(Mathf.Abs(bloomI.value - targetBloom) < speed)
                {
                    bloomI.value = targetBloom;
                }
                bloom.intensity = bloomI;
                //Wait a frame to run again
                yield return new WaitForEndOfFrame();
            }
        } else
        {
            //If no bloom
            Debug.Log("Bloom Not Found");

        }
    }

    public void ChangeVignetteIntensity(float intensity)
    {
        if (profile.TryGet<Vignette>(out Vignette vig))
        {
            vig.intensity.Override(intensity);
        }
        else
        {
            Debug.Log("Vignette Not Found");
        }
    }



    public void Vignette(bool turnOn)
    {
        if(profile.TryGet<Vignette>(out Vignette vig))
        {
            if(turnOn)
            {
                vig.active = true;
            } else
            {
                vig.active = false;
            }
        } else
        {
            Debug.Log("Vignette Not Found");
        }
    }

    public void VignetteColor(Color color)
    {
        if (profile.TryGet<Vignette>(out Vignette vig))
        {


            //vig.color = new ColorParameter(color, true);
            vig.color.Override(color);
        }
        else
        {
            Debug.Log("Vignette Not Found");
        }
    }

    public bool VignetteIsColor(Color co)
    {
        if (profile.TryGet<Vignette>(out Vignette vig))
        {

            ////ColorParameter co = new ColorParameter();
            ColorParameter cp = vig.color;
            if(cp.value == co)
            {
                return true;
            }

        }
        else
        {
            Debug.Log("Vignette Not Found");
        }
        return false;
    }

    public void Bloom(bool turnOn)
    {
        if (profile.TryGet<Bloom>(out Bloom bloom))
        {
            if (turnOn)
            {
                bloom.active = true;
            }
            else
            {
                bloom.active = false;
            }
        }
        else
        {
            Debug.Log("Bloom Not Found");
        }
    }

}
