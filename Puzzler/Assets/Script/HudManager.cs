using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
	[Header ("Game Objects")]
	[SerializeField] private GameObject PauseMenu;
	[SerializeField] private GameObject Reticle;

	private CanvasGroup Reticle_Canvas;
	private Transform Reticle_Trans;
	private Image Reticle_Color;

	[Header ("Reticle Options")]
	[SerializeField]private Color HighlightColor = Color.white;
	[SerializeField]private Color normalColor = Color.grey;

	private readonly Vector3 ReticleMaxSize = new Vector3(.3f,.3f,.3f);
	private readonly Vector3 ReticleMinSize = new Vector3(.15f,.15f,.15f);

	[Header ("Black Screen")]
	[SerializeField] private CanvasGroup BlackScreen;

	void Start(){


		Reticle_Canvas = Reticle.GetComponent<CanvasGroup>();	
		Reticle_Trans = Reticle.GetComponent<Transform>();	
		Reticle_Color = Reticle.GetComponent<Image>();	

		Fade(true);
	}

	public void Pause(){


		BlackScreen.alpha = .5f;
		Reticle_Canvas.alpha = 0f;
		PauseMenu.SetActive(true);
		

	}

	public void UnPause(){


		BlackScreen.alpha = 0;
		Reticle_Canvas.alpha = 1f;
		PauseMenu.SetActive(false);


	}

	public void ReticleSize(bool enlarge){


		if (enlarge){

			if (Reticle_Trans.localScale.x != ReticleMaxSize.x)
			{
				Reticle_Trans.localScale = ReticleMaxSize;
				Reticle_Color.color = HighlightColor;
			}


		}
		else {


			if (Reticle_Trans.localScale.x != ReticleMinSize.x)
			{
				Reticle_Trans.localScale = ReticleMinSize;
				Reticle_Color.color = HighlightColor;
			}

		}


	}

	public void Fade(bool fadeIn){

		StartCoroutine(FadeScreen(fadeIn));
	}


	public IEnumerator FadeScreen(bool fadeIn){

		if(fadeIn){

			for(float i = 1; i >= 0; i -= Time.deltaTime){

				BlackScreen.alpha = i;
				yield return null;   

			}


		}
		else{

			for(float i = 0; i <= 1; i += Time.deltaTime){

				BlackScreen.alpha = i;
				yield return null;   

			}

		}

	}


}
