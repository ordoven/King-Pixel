using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour {

	public GameObject TextBox;
	public Text TheText;
	public TextAsset TextFile;
	public string[] Sentences;
	public int CurrentLine;
	public int EndLine;
	public Rigidbody2D Player;
	public bool IsActive;
	private bool _isTyping, _cancelTyping;
	public float TypingSpeed;
	private AudioSource _audioSound;
	public bool Pressed;

	private void Start ()
	{
		_audioSound = GetComponent<AudioSource> ();
		if (GameObject.FindGameObjectWithTag("Player") != null)
			Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
		Pressed = false;
		if (TextFile != null) Sentences = (TextFile.text.Split ('\n'));
		if (EndLine == 0) EndLine = Sentences.Length - 1;
		DisableTextBox ();
	}

	public void Update()
	{			
		if (TextBox == null)
		{
			try { TextBox = GameObject.Find("TextBox"); TheText = TextBox.GetComponentInChildren<Text>(false); DisableTextBox(); }
			catch { return; }

		}
		if (Player == null)
		{
			try { Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>(); }
			catch { return; }
		}

		if (!IsActive || !Input.GetKeyDown(KeyCode.Return)) return;
		
		if (!_isTyping)
		{
			CurrentLine += 1;
			if (CurrentLine > EndLine)
				DisableTextBox ();
			else
				StartCoroutine (TextScroll(Sentences[CurrentLine]));
		}
		else if (_isTyping && !_cancelTyping)
		{
			_cancelTyping = true;
		}
	}

	// co-routine
	private IEnumerator TextScroll (string lineOfText)
	{
		var letter = 0;
		TheText.text = "";
		_isTyping = true;
		_cancelTyping = false;

		while (_isTyping && !_cancelTyping && (letter < lineOfText.Length - 1))
		{
			TheText.text += lineOfText [letter];
			_audioSound.Play ();
			letter++;
			yield return new WaitForSeconds (TypingSpeed);
		}

		TheText.text = lineOfText;
		_isTyping = false;
		_cancelTyping = false;
	}

	public void EnableTextBox(Sprite fatBoy, AudioClip voice)
	{
		if (TextBox == null) return;
		TextBox.transform.Find("Image").GetComponent<Image>().sprite = fatBoy;
		GetComponent<AudioSource>().clip = voice;
		TextBox.SetActive(true);
		IsActive = true;
		Player.bodyType = RigidbodyType2D.Static;
		StartCoroutine(TextScroll(Sentences[CurrentLine]));
	}

	public void DisableTextBox()
	{
		if (TextBox == null) return;
		TextBox.transform.Find("Image").GetComponent<Image>().sprite = null;
		GetComponent<AudioSource>().clip = null;
		TextBox.SetActive(false);
		IsActive = false;
		Pressed = false;
		Player.bodyType = RigidbodyType2D.Dynamic;
	}

	public void ReloadScript(TextAsset theText)
	{
		if (theText == null) return;
		Sentences = new string[1];
		Sentences = theText.text.Split ('\n');
	}
}
