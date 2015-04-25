using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



//From http://stackoverflow.com/questions/630803/associating-enums-with-strings-in-c-sharp  pattern of String-castable enum (But not working in Unity's Inspector)
public class eDisplayMode
{
	private eDisplayMode(string value) { Value = value; }
	
	public string Value { get; set; }
	
	public static eDisplayMode ShowFolders { get { return new eDisplayMode("ShowFolders"); } }
	public static eDisplayMode ShowFiles { get { return new eDisplayMode("ShowFiles"); } }
}



public class GUI_ChartViewOptions : MonoBehaviour 
{
	public eDisplayMode DisplayMode = eDisplayMode.ShowFolders;
	//public float DoNotDisplayPieUnderAngle = 5f;
	protected float DoNotDisplayPieUnderAngle = 5f;


}
