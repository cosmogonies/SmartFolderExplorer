using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GUI_List : MonoBehaviour 
{

    private const int MAXLINE = 22;

    private const float LINE_HEIGHT = 25;

    private const float LINE_TOP_POSITION = 540f; //TODO: Remove it and make it dynamic


    public GameObject panelHeader;

    public GameObject scrollbar;

    private List<GameObject> LinePanels;

    //private Dictionary<int, string> FromColumnOrderToPropertyName;
    private string[] PropertyNameArray;

	
	void Start () 
    {
        this.LinePanels = new List<GameObject>();

        PropertyNameArray = new string[4];
        PropertyNameArray[0] = "File Name";
        PropertyNameArray[1] = "Ext";
        PropertyNameArray[2] = "File Path";
        PropertyNameArray[3] = "Tags";

	}
	
	void Update () 
    {
	
	}


    //public void AddLine(string _FileName, string _FilePath, string _FileExtension, string[] _FileTags)
    public void AddLine(Dictionary<string, string> _InputDict)
    {
        //Input verification:
        // all PropertyName must be recognized:
        string[] currentItemValues = new string[_InputDict.Count];
        foreach (string PropertyName in _InputDict.Keys)
        {
            bool PropertyFound = false;
            for (int i = 0; i < PropertyNameArray.Length; i++)
			{
                if (PropertyNameArray[i] == PropertyName)
                {
                    currentItemValues[i] = _InputDict[PropertyName];
                    PropertyFound = true;
                }
			}
            if (! PropertyFound)
                Debug.LogWarning("Property named " + PropertyName+" is not found.");
        }



        // We create the linePanel
        GameObject newPanel = GameObject.Instantiate(panelHeader);

        newPanel.name = "Panel_Line_" + this.LinePanels.Count.ToString();


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

        //Offsetting by LINE_HEIGHT in vertical, from the previous (abonve) one:
        newLinePanelTransform.offsetMin = new Vector2(newLinePanelTransform.offsetMin.x, newLinePanelTransform.offsetMin.y - LINE_HEIGHT);
        newLinePanelTransform.offsetMax = new Vector2(newLinePanelTransform.offsetMax.x, newLinePanelTransform.offsetMax.y - LINE_HEIGHT);

        //Fill the columns:
        for (int i = 0; i < newLinePanelTransform.transform.childCount; i++)
        {
            string PropertyName = newLinePanelTransform.transform.GetChild(i).GetComponent<UnityEngine.UI.Text>().text;
            newLinePanelTransform.transform.GetChild(i).GetComponent<UnityEngine.UI.Text>().text = currentItemValues[i];
        }

        // Refresh visibilty (desactivate if off-screen)
        refreshVisibility(newLinePanelTransform);

        LinePanels.Add(newPanel);
    }

    
    private void offsetLines(float _ScrollBarValue)
    {
        // 0 : item[0] on top, item[n] on bottom  (where item[n].y <0 )
        // 1 : item[last] on bottom, item[m] on top (where item[m-1].y > Header.y ) 

        for (int i = 0; i < this.LinePanels.Count; i++)
        {
            GameObject currentLinePanel = this.LinePanels[i];

            RectTransform newLinePanelTransform = currentLinePanel.GetComponent<RectTransform>() as RectTransform;

            // Base position (at _ScrollBarValue ==0), item[0] on top:
            float newPositionYMinAbsolute = LINE_TOP_POSITION -  (i*LINE_HEIGHT);
            float newPositionYMaxRelative = -LINE_HEIGHT * (i + 1);

            // Applying _ScrollBarValue offset:
            newPositionYMinAbsolute += _ScrollBarValue * LINE_HEIGHT * (this.LinePanels.Count - MAXLINE);
            newPositionYMaxRelative += _ScrollBarValue * LINE_HEIGHT * (this.LinePanels.Count - MAXLINE);

            newLinePanelTransform.offsetMin = new Vector2(newLinePanelTransform.offsetMin.x, newPositionYMinAbsolute);
            newLinePanelTransform.offsetMax = new Vector2(newLinePanelTransform.offsetMax.x, newPositionYMaxRelative);

            refreshVisibility(newLinePanelTransform);

            //FYI
            // _ScrollBarValue ==0
            //Panel_Line_0 (0.0, 539.5) => (-30.0, -25.5)
            //Panel_Line_1 (0.0, 514.5) => (-30.0, -50.5)
            //Panel_Line_2 (0.0, 489.5) => (-30.0, -75.5) : (, 540 - i*25)=> (, -25*i)
        }

    }


    void refreshVisibility(RectTransform _ItemPosition)
    {
        bool isActivated = _ItemPosition.gameObject.activeSelf;

        if ((_ItemPosition.position.y < LINE_HEIGHT) || (_ItemPosition.position.y > LINE_TOP_POSITION))
        {
            if (isActivated)
            {
                _ItemPosition.gameObject.SetActive(false);
            }
        }       
        else
	    {
            if(! isActivated)
                _ItemPosition.gameObject.SetActive(true);
	    }
    }


    public void scrollBarCallback()
    {
        UnityEngine.UI.Scrollbar scrollBarComp = scrollbar.GetComponent<UnityEngine.UI.Scrollbar>();
        //Debug.Log("Value changed to =" + scrollBarComp.value);
        offsetLines(scrollBarComp.value);
    }

}
