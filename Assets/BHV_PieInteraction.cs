using UnityEngine;
using System.Collections;

public class BHV_PieInteraction : MonoBehaviour 
{
	public string toolTipText;

	public SmartFolder.TaggedFolder Folder;

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


	/*
	//void Update () 
	void OnMouseUp()
	{

		if( this.currentToolTipText!="" )
		{
			//Event.current.GetTypeForControl(
			Debug.Log ( this.currentToolTipText );


			//TagScan comp = Camera.main.GetComponent<TagScan>() as TagScan;
			TagScan comp = GameObject.FindGameObjectWithTag("Core").GetComponent<TagScan>() as TagScan;

			Debug.Log ( comp.name );

			comp.updateFolderCharts(this.Folder);
		}
		
	}
	*/

	void OnMouseOver () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			if( this.currentToolTipText!="" )
			{
				//Event.current.GetTypeForControl(
				Debug.Log ( this.currentToolTipText );
				


				if(this.Folder.Folders.Count==0)
				{
					Debug.LogWarning ( "No SubFolders for current Hovered Folder, Aborting" );

				}
				else
				{

					//TagScan comp = Camera.main.GetComponent<TagScan>() as TagScan;
					TagScan comp = GameObject.FindGameObjectWithTag("Core").GetComponent<TagScan>() as TagScan;
					
					Debug.Log ( comp.name );
					
					comp.updateFolderCharts(this.Folder);

				}

			}


		}


			//Debug.Log("Left click on this object");
		if(Input.GetMouseButtonDown(1))
		{
			if( this.currentToolTipText!="" )
			{

				if(this.Folder.Parent == null )
				{
					Debug.LogWarning ( "We are already in ROOT, Aborting" );
				}
				else
				{

					//Debug.Log("Left click on this object");
					TagScan comp = GameObject.FindGameObjectWithTag("Core").GetComponent<TagScan>() as TagScan;
					Debug.Log("Returning to folder = "+this.Folder.Parent.Parent.FolderName);
					comp.updateFolderCharts(this.Folder.Parent.Parent);
				}
			}
		}

			//Debug.Log("Right click on this object");
		//if(Input.GetMouseButtonDown(2))
			//Debug.Log("Middle click on this object");
	}



}


