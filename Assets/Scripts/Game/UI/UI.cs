using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
	
	//Reference to Menu
	public GameObject Menu;
	
	public GameObject Unit_Menu;
	
	public State_Handler state;
	
	public GameObject Stam;
	
	private bool Menu_Open;
	private bool Unit_Menu_Open;
	
	public GameObject GameOver;
	public Text GameOver_text;
	
	public float TimeToType = 3.0f;
	public float TimeToTypeFinale = 25.0f;

	private float textPercentage = 0;
	
	private float textPercentage2 = 0;
	
	private string howard = "Do not fear my Child, even though you have lost, another shall take your place and attempt again. Rise Up Gamers.";
	private string howard_finale = "Congratulations. You actually made it. You are without a doubt a true Bethesda fan. Even after selling you buggy games for more than 10 years. And attempting to get away with sending cheap Nylon Bags with your collectors edition of Fallout 76. You still stood by us. That is why I now shall grant you entry here into your new home. Welcome.";
	
    //Attach an Image you want to fade in the GameObject's Inspector
    public Image m_Image;
    //Use this to tell if the toggle returns true or false
    bool m_Fading;

	public GameObject Finale;
	public Text Finale_Text;
	
	public GameObject Restart;
	
	// Use this for initialization
	void Start () {
		
		Menu_Open = false;
		Menu.SetActive(false);
		
	}
	
	public void Fade(bool x){
		m_Fading = x;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (state.State == 6 & !Menu_Open){
			Menu.SetActive(true);
			Menu_Open = true;
		}
		
		else if (!(state.State == 6) & Menu_Open){
			Menu.SetActive(false);
			Menu_Open = false;
		}
		
		        //If the toggle returns true, fade in the Image
        if (m_Fading == true)
        {
            //Fully fade in Image (1) with the duration of 2
            m_Image.CrossFadeAlpha(1, 2.0f, false);
        }
        //If the toggle is false, fade out to nothing (0) the Image with a duration of 2
        if (m_Fading == false)
        {
            m_Image.CrossFadeAlpha(0, 2.0f, false);
        }
		
		if (state.State == 9){
			GameOver.gameObject.SetActive(true);
			int numberOfLettersToShow = (int)(howard.Length * textPercentage);
			GameOver_text.text = howard.Substring(0, numberOfLettersToShow);
			textPercentage += Time.deltaTime / TimeToType;
			textPercentage = Mathf.Min(1.0f, textPercentage);
		}
		
		if (state.State == 10){
			Restart.gameObject.SetActive(true);
		}
		
		if (state.State == 14){
			Stam.gameObject.SetActive(false);
			Finale.gameObject.SetActive(true);
			int numberOfLettersToShow = (int)(howard_finale.Length * textPercentage2);
			Finale_Text.text = howard_finale.Substring(0, numberOfLettersToShow);
			textPercentage2 += Time.deltaTime / TimeToTypeFinale;
			textPercentage2 = Mathf.Min(1.0f, textPercentage2);
		}
	}
	
	public void OnGUI()
	{
		//m_Fading = GUI.Toggle(new Rect(0, 0, 100, 30), m_Fading, "Fade In/Out");
	}
}



