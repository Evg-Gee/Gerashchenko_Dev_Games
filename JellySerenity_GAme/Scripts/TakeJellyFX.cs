using System.Collections.Generic;
using UnityEngine;

public class TakeJellyFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem takeJellyFX_Blue;
    [SerializeField] private ParticleSystem takeJellyFX_Pinck;
    private ParticleSystem currentEffect;
	private Dictionary<string, ParticleSystem> FxmaterialMap;
	
    void Awake()
        {
            SetMaterialDictionarys();
        }

    private void SetMaterialDictionarys()
	{
		FxmaterialMap = new Dictionary<string, ParticleSystem>
		{
			{ "Blue", takeJellyFX_Blue },
			{ "Blue (Instance)", takeJellyFX_Blue },
			{ "Red", takeJellyFX_Pinck },
			{ "Red (Instance)", takeJellyFX_Pinck }
		};
	}

     public void PlayTakeJellyFX()
    {
        if (currentEffect != null)
        {
            currentEffect.Stop();
        }

        currentEffect.Play();
    }

    public void SetNewColor(string jellyColor)
    {
        currentEffect = FxmaterialMap[jellyColor];
    }

    public void SetStartColor(Material meshRendererGivers)
    {
        if (meshRendererGivers == null) return;

        currentEffect = FxmaterialMap[meshRendererGivers.name];
    }
    
    public void StopFX()
   {
        if (currentEffect != null)
        {
            currentEffect.Stop();
            currentEffect = null;
        }
    }


}
