using UnityEngine;
using System.Collections;

public delegate void OnCardSelectedDelegate (Card card);

public class Card : MonoBehaviour {

	public event OnCardSelectedDelegate OnCardSelected;
	private void OnCardSelectedDelegateHandler()
	{
		if(OnCardSelected !=null) OnCardSelected(this);
	}

	[SerializeField]
	private SpriteRenderer _faceSideRenderer;

	private Collider2D _col;
	public bool IsClickable
	{
		set
		{ 
			_col = _col ?? GetComponent<Collider2D> ();
			if(_col!=null) _col.enabled = value;
		}
	}

	public Sprite Sprite
	{
		get
		{
			if (_faceSideRenderer == null)
				return null;
			return _faceSideRenderer.sprite;
		}
		set
		{
			if (_faceSideRenderer == null)
				return;
			_faceSideRenderer.sprite = value;
		}
	}

	private Vector3 _closeCond = Vector3.zero;
	private Vector3 _openCond = new Vector3 (0, 180, 0);

	private bool _isCardSelected;

	private RotationComponent _rotator;
	private RotationComponent Rotator
	{
		get
		{ 
			_rotator = _rotator ?? GetComponent<RotationComponent> ();
			//if (_rotator == null) GetComponent<RotationComponent> ();
			return _rotator;
		}
	}

	public void Open () 
	{
		Rotator.StartRotation (_openCond);

	}

	public void Close ()
	{
		Rotator.StartRotation (_closeCond);
		_isCardSelected = false;
	}

	public override bool Equals (object o)
	{
		if (o == null)
			return false;

		Card temp = o as Card;

		if (temp == null)
			return false;

		return temp.Sprite == this.Sprite;
	}

//	private void OnGUI()
//	{
//		if (GUI.Button (new Rect (10, 10, 100, 50), "Open")) {
//			Open ();
//		}
//		if (GUI.Button (new Rect (10, 60, 100, 50), "Close")) {
//			Close ();
//		}
//	}

	void OnMouseDown()
	{
		Open ();
		if (!_isCardSelected) {
			OnCardSelectedDelegateHandler ();
			_isCardSelected = true;
		}
	}
}
