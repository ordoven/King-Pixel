using UnityEngine;

/*
 * This Script is used for objects that want to display text on screen
 * 
 * This script depends on : TextBoxManager.cs 
 */
 
[RequireComponent(typeof(SpriteRenderer))]
public class ActivateTextAtLine : MonoBehaviour {
	
	public TextAsset TheText;
    public AudioClip VoiceSound;
	public int StartingPoint;
	public int EndPoint;
	private TextBoxManager _theTextManager;
	public bool ButtonPressReq;
	public bool DestroyWhenActivated;
	public GameObject Indicator;
	private bool _waitForPress;

	public void Start ()
    {
		_theTextManager = FindObjectOfType<TextBoxManager> ();
		if (Indicator != null) Indicator.SetActive (false);
	}
	
	public void Update ()
    {
	    if (!_waitForPress || !Input.GetKeyDown(KeyCode.E) || _theTextManager.Pressed) return;
	    
	    _theTextManager.Pressed = true;
	    _theTextManager.ReloadScript (TheText);
	    _theTextManager.CurrentLine = StartingPoint;
	    _theTextManager.EndLine = EndPoint;
	    _theTextManager.EnableTextBox (gameObject.GetComponent<SpriteRenderer>().sprite, VoiceSound);
	    
	    if (DestroyWhenActivated) Destroy (gameObject);
    }

	private void OnTriggerEnter2D(Collider2D other)
    {
	    if (!other.CompareTag("Player")) return;
	    
	    if (ButtonPressReq)
	    {
		    if (Indicator != null) Indicator.SetActive (true);
		    _waitForPress = true;
		    return;
	    }
	    
	    _theTextManager.ReloadScript (TheText);
	    _theTextManager.CurrentLine = StartingPoint;
	    _theTextManager.EndLine = EndPoint;
	    _theTextManager.EnableTextBox (gameObject.GetComponent<SpriteRenderer>().sprite, VoiceSound);
	    
	    if (DestroyWhenActivated) Destroy (gameObject);
    }

	private void OnTriggerExit2D(Collider2D other)
    {
	    if (!other.CompareTag("Player")) return;
	    
	    if (Indicator != null) Indicator.SetActive (false);
	    
	    _waitForPress = false;
    }
}
