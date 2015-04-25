using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GUI_List : MonoBehaviour 
{

    private const int MAXLINE = 20;


    public GameObject panelHeader;

    public GameObject scrollbar;

    private List<GameObject> LinePanels;

   //private Dictionary<int, string> FromColumnOrderToPropertyName;

	
	void Start () 
    {
        this.LinePanels = new List<GameObject>();
	}
	
	void Update () 
    {
	
	}

    /*
    private void AnalyseHeader()
    {
        //foreach (Transform child in this.panelHeader.transform)
        for (int i = 0; i < this.panelHeader.transform.GetChildCount(); i++)
        {
            FromColumnOrderToPropertyName[i] = this.panelHeader.transform.GetChild(i).GetComponent<UnityEngine.UI.Text>().text;       
        }
       
    }
     */



    public void AddLine(string _FileName, string _FilePath, string _FileExtension, string[] _FileTags)
    {
        // We create the linePanel
        GameObject newPanel = GameObject.Instantiate(panelHeader);

        // Pruning off screen lines
        if (this.LinePanels.Count > 50)
            newPanel.SetActive(false);

        newPanel.name = "Panel_Line_" + this.LinePanels.Count.ToString();

        // We place it to the right place :

        // We search the previous one:
        RectTransform newLinePanelTransform = newPanel.GetComponent<RectTransform>() as RectTransform;

        GameObject aboveLinePanel;
        if (this.LinePanels.Count == 0)
        {
            aboveLinePanel = this.panelHeader;
        }
        else
        {
            aboveLinePanel = this.LinePanels[this.LinePanels.Count-1];
        }

        RectTransform aboveLinePanelTransform = aboveLinePanel.GetComponent<RectTransform>() as RectTransform;

        newLinePanelTransform.SetParent(this.transform, false);

        newLinePanelTransform.anchoredPosition = aboveLinePanelTransform.anchoredPosition;


        //Matching the header :
        newLinePanelTransform.offsetMin = aboveLinePanelTransform.offsetMin;
        newLinePanelTransform.offsetMax = aboveLinePanelTransform.offsetMax;

        //Offsetting by 50 in vertical:
        newLinePanelTransform.offsetMin = new Vector2(newLinePanelTransform.offsetMin.x, newLinePanelTransform.offsetMin.y - 25f);
        newLinePanelTransform.offsetMax = new Vector2(newLinePanelTransform.offsetMax.x, newLinePanelTransform.offsetMax.y - 25f);




        for (int i = 0; i < newLinePanelTransform.transform.childCount; i++)
        {
            //FromColumnOrderToPropertyName[i] = this.panelHeader.transform.GetChild(i).GetComponent<UnityEngine.UI.Text>().text;
            string PropertyName = newLinePanelTransform.transform.GetChild(i).GetComponent<UnityEngine.UI.Text>().text;

            switch (PropertyName)
            {
                case "File Name":
                    newLinePanelTransform.transform.GetChild(i).GetComponent<UnityEngine.UI.Text>().text = _FileName;
                    break;
                case "Ext":
                    newLinePanelTransform.transform.GetChild(i).GetComponent<UnityEngine.UI.Text>().text = _FileExtension;
                    break;
                case "Path":
                    newLinePanelTransform.transform.GetChild(i).GetComponent<UnityEngine.UI.Text>().text = _FilePath;
                    break;
                case "Tags":
                    newLinePanelTransform.transform.GetChild(i).GetComponent<UnityEngine.UI.Text>().text =  string.Join(",", _FileTags); 
                    break;

                default:
                    newLinePanelTransform.transform.GetChild(i).GetComponent<UnityEngine.UI.Text>().text = "NOT FOUND";
                    break;

            }
            
                


        }


        LinePanels.Add(newPanel);


    }
    
    


}
