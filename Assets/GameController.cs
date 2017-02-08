using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{

	[SerializeField]
	private GameObject _restartButton;

	[SerializeField]
	private Transform _startPointTransform;

	[SerializeField]
	private Card _cardPrefab;

	[SerializeField]
	private Sprite[] _sprites;

	[SerializeField, Range(.1f,4f)]
	private float _showDelay = 2;

	private int _width =4;
	private int _height =3;

	private int _widthCard = 5;
	private int _heightCard= 5;

	private List<Card> _cards;
	private List<Card> _selectedCards = new List<Card>();

	private int _randomStartIndex;

	private void Start()
	{
		_randomStartIndex = Random.Range (0, _sprites.Length);
		Debug.Log ("0"+ _randomStartIndex);

		_cards=new List<Card>();

		GenerateField ();
		SetRandomPicture ();
		ShowCards (true);
		SetCardClickable (false);
		Invoke ("CloseAllCards", _showDelay);
	}

	void OnDestroy()
	{
		CancelInvoke ();
	}

	private Sprite GetNextSprite()
	{
		_randomStartIndex = _randomStartIndex>=_sprites.Length ? 0 : _randomStartIndex;
		var temp = _sprites [_randomStartIndex];
		_randomStartIndex++;
		return temp;
	}

	void GenerateField ()
	{
		for (int i = 0; i < _height; i++) 
		{
			for (int j = 0; j < _width; j++) 
			{
				var temp = Instantiate (_cardPrefab) as Card;
				if (temp == null)
				{
					Debug.Log ("card is nul");
					continue;
				}
				temp.gameObject.transform.position = new Vector3 (_startPointTransform.position.x + j * _widthCard,
																_startPointTransform.position.y - i * _widthCard,0);
				temp.Sprite = null;
				_cards.Add (temp);
				temp.OnCardSelected += OnCardSelected;
			}
		}
	}

	public  void OnCardSelected (Card card)
	{
	//Debug.Log("Selected");
		if(_selectedCards.Count==2)
		{
			Debug.Log ("all cards selected");
			return;
		}
		_selectedCards.Add (card);

		if (_selectedCards.Count == 2)
		{
			
			Debug.Log ("chek it");
			//SetCardClickable (false);

			if (_selectedCards [0] != null && _selectedCards [1] != null)
			{
				if (_selectedCards [0].Equals (_selectedCards [1])) 
				{
					Debug.Log ("guessed");

					Behaviour h = (Behaviour)_selectedCards [0].gameObject.GetComponent ("Halo");
					h.enabled = true;

					h = (Behaviour)_selectedCards [1].gameObject.GetComponent ("Halo");
					h.enabled = true;
					_cards.Remove(_selectedCards[0]);
					_cards.Remove(_selectedCards[1]);
					_selectedCards.Clear ();
					SetCardClickable (true);
				}
				else
				{
					Invoke ("CloseSelectedCards", .5f);
				}
			}
		}
	}

	void CloseSelectedCards()
	{
		_selectedCards [0].Close ();
		_selectedCards [1].Close ();
		_selectedCards.Clear ();
		SetCardClickable (true);
	}

	void SetCardClickable(bool flag)
	{
		if (_cards == null || _cards.Count == 0) 
		{
			Debug.Log ("empty");
			return;
		}

		foreach (var item in _selectedCards)
		{
			if (item == null)
				continue;
			item.IsClickable = flag;
		}
	}

	void SetRandomPicture () 
	{
		if (_cards == null || _cards.Count == 0)
		{
			Debug.Log ("empty");
			return;
		}
		for (int i = 0; i < _cards.Count; i++) 
		{
			if (_cards [i] == null)
				continue;
			if (_cards [i].Sprite != null)
				continue;

			var tempSprite = GetNextSprite ();
			_cards [i].Sprite = tempSprite;

			int nextIndex = 0;

			do
			{
				nextIndex = Random.Range (0, _cards.Count);
			} 
			while (_cards [nextIndex].Sprite != null);

			_cards [nextIndex].Sprite = tempSprite;
		}
	}

	void CloseAllCards()
	{
		ShowCards (false);
		SetCardClickable (true);
	}

	void ShowCards(bool flag)
	{
		if (_cards == null || _cards.Count == 0)
		{
			Debug.Log ("empty");
			return;
		}

		foreach (var item in _cards)
		{
			if (item == null)
				continue;
			if (flag)
				item.Open ();
			else
			{
				item.Close ();
			}
		}
	}

	void Update()
	{
		if (_cards.Count!=0)
		{
			_restartButton.SetActive (false);
		}
		else
		{
			_restartButton.SetActive(true);
		}
	}

	public void RestartLevel()
	{
		Application.LoadLevel (0);
	}
}
