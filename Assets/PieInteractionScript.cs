using UnityEngine;
using System.Collections;

public class PieInteractionScript : MonoBehaviour 
{
	public string toolTipText;

	public TaggedFolder Folder;

	string currentToolTipText;
	GUIStyle guiStyleFore;
	GUIStyle guiStyleBack;
	

	void Start () 
	{
		this.guiStyleFore = new GUIStyle();			
		this.guiStyleFore.normal.textColor = Color.white;
		this.guiStyleFore.alignment = TextAnchor.UpperCenter;
		this.guiStyleFore.wordWrap = true;

		this.guiStyleBack = new GUIStyle();			
		this.guiStyleBack.normal.textColor = Color.white; //Color.black;
		this.guiStyleBack.alignment = TextAnchor.UpperCenter;
		this.guiStyleBack.wordWrap = true;

	}

	void OnMouseEnter()
	{
		this.currentToolTipText = toolTipText;
	}
	
	void OnMouseExit()
	{
		this.currentToolTipText = "";
	}



	void OnGUI()
	{
		if( this.currentToolTipText!="" )
		{
			float x = Event.current.mousePosition.x;
			float y = Event.current.mousePosition.y;
			GUI.Label( new Rect (x-149,y+21,300,60), currentToolTipText, guiStyleBack);
			GUI.Label( new Rect (x-149,y+21,300,60), currentToolTipText, guiStyleBack);
		}
	}



	//void Update () 
	void OnMouseUp()
	{

		if( this.currentToolTipText!="" )
		{
			//Event.current.GetTypeForControl(
			Debug.Log ( this.currentToolTipText );


			TagScan comp = Camera.main.GetComponent<TagScan>() as TagScan;
			comp.refresh(this.Folder);
		}
		
	}



}


