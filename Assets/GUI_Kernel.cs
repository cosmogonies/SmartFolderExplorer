using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;


public class GUI_Kernel : MonoBehaviour 
{
	//This behaviour hold the several GUI trigger into the system

	public GameObject addressInput;

	public GameObject isRecursiveToggleButton;


    public GameObject ListPanel;

	void Start () 
	{
	
	}

	void Update () 
	{

	}


	public void scanFolder()
	{

		Debug.Log("scanFolder");

		bool isRecursive = isRecursiveToggleButton.GetComponent<UnityEngine.UI.Toggle>().isOn;

		string address = addressInput.GetComponent<UnityEngine.UI.InputField>().text;

		Debug.Log("isRecursive="+isRecursive);
		Debug.Log("address="+address);


		CLS_FolderTool gugu = new CLS_FolderTool();
		List<string> result = gugu.scanDirectory(address);


        foreach( string currentFile in result )
        {

            GUI_List comp = ListPanel.GetComponent<GUI_List>() as GUI_List;

            comp.AddLine( "bob", @"C:\caca\lolo", ".txt", new string[]{"home","nemo","toto"} );
        }


	}



















}
