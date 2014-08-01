﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


/*
public enum eDisplayMode
{
	//kShowFolders="ShowFolders",
	//kShowFiles="ShowFiles"
	ShowFolders=0,
	ShowFiles=1
}
*/

//From http://stackoverflow.com/questions/630803/associating-enums-with-strings-in-c-sharp  pattern of String-castable enum (But not working in Unity's Inspector)
public class eDisplayMode
{
	private eDisplayMode(string value) { Value = value; }
	
	public string Value { get; set; }
	
	public static eDisplayMode ShowFolders { get { return new eDisplayMode("ShowFolders"); } }
	public static eDisplayMode ShowFiles { get { return new eDisplayMode("ShowFiles"); } }
}



public class GUI_LeftSidePanel : MonoBehaviour 
{
	public eDisplayMode DisplayMode = eDisplayMode.ShowFolders;
	//public float DoNotDisplayPieUnderAngle = 5f;
	protected float DoNotDisplayPieUnderAngle = 5f;

	void Start ()
	{


	}
	void Update (){}

	void OnGUI()
	{
		float HEIGHT_START = Screen.height * 0.33f;

		float WIDTH = Screen.width * 0.33f;
		GUI.Label( new Rect(0f,HEIGHT_START,WIDTH, 32f), "* DISPLAY *" );
		this.DoNotDisplayPieUnderAngle = GUI.HorizontalSlider( new Rect(0f,HEIGHT_START+32f,WIDTH, 32f), this.DoNotDisplayPieUnderAngle, 0f,360f);
		if( GUI.Button( new Rect(0f,HEIGHT_START+64f,WIDTH, 32f), this.DisplayMode.Value ) )
		{
		}



	}




}
