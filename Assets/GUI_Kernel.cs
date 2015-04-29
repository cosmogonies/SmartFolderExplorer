using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;


public class GUI_Kernel : MonoBehaviour 
{
	//This behaviour hold the several GUI trigger into the system

	public GameObject addressInput;

	public GameObject isRecursiveToggleButton;

    public GameObject TagOverviewCanvas;

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

        GUI_List comp = ListPanel.GetComponent<GUI_List>() as GUI_List;


        Dictionary<string, int> TagPopularity = new Dictionary<string, int>(); //tag => #occurence

        foreach( string currentFile in result )
        {

            System.IO.FileInfo currentFileInfo = new FileInfo(currentFile);

            Dictionary<string, string> input = new Dictionary<string, string>();
            input["File Name"] = currentFileInfo.Name;
            input["Ext"] = currentFileInfo.Extension;
            input["File Path"] = currentFileInfo.DirectoryName;


            string[] tags = gugu.findTagInFilePath(currentFile);
        
            for (int i = 0; i < tags.Length; i++)
			{
                string currentTag = tags[i];
			    if( TagPopularity.ContainsKey(currentTag) )
                {
                    TagPopularity[currentTag]++;
                }
                else
                    TagPopularity[currentTag]=1;
			}


            input["Tags"] = System.String.Join(".", tags );




            //comp.AddLine(input);
        }


        // TESTING TAGIFY BY PATH:
        UnityEngine.Debug.Log(TagPopularity.Keys.Count+" found");
        foreach (string key in TagPopularity.Keys)
        {
            int occ = TagPopularity[key];
            if (occ > 1)
                Debug.Log(key+"("+occ+")");
        }



	}


    public void OpenCloseTagOverview()
    {
        this.TagOverviewCanvas.SetActive(!this.TagOverviewCanvas.activeSelf);
    }
















}
