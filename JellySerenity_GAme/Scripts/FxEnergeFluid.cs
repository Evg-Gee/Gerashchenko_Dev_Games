using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxEnergeFluid : MonoBehaviour
{
    [SerializeField] private ParticleSystem _fxEnergiBlue;
    [SerializeField] private ParticleSystem _fxEnergiPnck;
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
			{ "Blue", _fxEnergiBlue },
			{ "Blue (Instance)", _fxEnergiBlue },
			{ "Red", _fxEnergiPnck },
			{ "Red (Instance)", _fxEnergiPnck }
		};
	}

    public IEnumerator FXRotation(JellySystem otherJelly)
	{
	    // Вычисляем направление
        Vector3 direction = otherJelly.transform.position - this.transform.position;
        direction.y = 0; // Игнорируем ось Y
        direction.Normalize();

        // Создаем целевую ротацию
        Quaternion targetRotation = Quaternion.LookRotation(direction);

		currentEffect.transform.rotation = targetRotation;

		currentEffect.Play();

		yield return new WaitForSeconds(10f);

		currentEffect.Stop();
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
