﻿using UnityEngine;
using System.Collections;

using System.Collections.Generic;

using System.Diagnostics;
using System.IO;

using UnityEditor;
using System.Reflection;

/*
public class TaggedItem
{

}
*/

public class TaggedFile
{
	public long Size = 0; //in Bytes

	public string Extension = ""; //with .  (as in .png, .mov)

	string FileName="";
	public string FilePath="";

	public TaggedFolder Folder;

	//int Depth;

	public TaggedFile(string _FullPath, string _Extension, long _Size, TaggedFolder _Parent)
	{
		this.FilePath = _FullPath;
		this.Extension = _Extension;
		this.Size = _Size;
		this.Folder = _Parent;
	}
}

public class TaggedFolder
{
	public int Depth;
	public long LocalSize = 0; //in Bytes , sum of all direct files.
	public long TotalSize = 0; //in Bytes , sum of all inherited files.

	public string FolderName="";
	public string FolderPath="";

	public List<TaggedFile> Files;

	public List<TaggedFolder> Folders;	//Containing folders.

	public TaggedFolder(string _FullPath)
	{
		string[] tokenized = _FullPath.Split( '\\');

		this.FolderName = tokenized[tokenized.Length-1];
		this.FolderPath = _FullPath;

		this.Depth = tokenized.Length;

		this.Files = new List<TaggedFile>();
		this.Folders = new List<TaggedFolder>();
	}

	public void calcSize()
	{
		foreach( TaggedFile cur in Files 	)
		{
			this.LocalSize+= cur.Size;
		}

	}

}


public class TagScan : MonoBehaviour
{

	//Dictionary<string, List<TaggedFile> > CloudData = new Dictionary<string, List<TaggedFile>> ();
	//Dictionary<string, List<System.IO.FileInfo> > CloudData = new Dictionary<string, List<TaggedFile>> ();

	//List<System.IO.FileInfo> Data = new List<System.IO.FileInfo> ();

	Dictionary<string, long> Statistic_Extension = new Dictionary<string, long> ();	

	
	List<TaggedFolder> Data = new List<TaggedFolder> ();
	List<TaggedFile> DataFiles = new List<TaggedFile> ();

	public int MinDepth=int.MaxValue;
	public int MaxDepth=0;
	Dictionary<int, List<TaggedFolder> > DepthDict;
	public TaggedFolder CurrentFolder;

	public string rootFolder = @"D:\I_DRIVE\cPaulhiac\MAYA";
	public float progression =0.0f;
	int MAXFOLDER=0;
	private int currentFolderCount=0;

	// Use this for initialization
	void Start ()
	{
		DepthDict = new Dictionary<int, List<TaggedFolder>>();
	}

	
	public void scanFolder()
	//public IEnumerator scanFolder()
	{

		//rootFolder = @"D:\I_DRIVE\cPaulhiac\MAYA";
		string[] tokenized = rootFolder.Split( '\\');
		this.MinDepth = tokenized.Length;
		UnityEngine.Debug.Log ("Start depth found are "+this.MinDepth);

		//string rootFolder = @"D:\I_DRIVE\cPaulhiac";
		//string rootFolder = @"W:\PJPMMP";
	
		this.MAXFOLDER = System.IO.Directory.GetDirectories(rootFolder, "*", System.IO.SearchOption.AllDirectories).Length;

		Stopwatch currentWatch2 = System.Diagnostics.Stopwatch.StartNew();
		string searchPattern = "*";
		string[] files = System.IO.Directory.GetFiles(rootFolder, searchPattern, System.IO.SearchOption.AllDirectories);
		currentWatch2.Stop();
		UnityEngine.Debug.Log ("Found "+files.Length+" #files in "+(currentWatch2.ElapsedMilliseconds/1000.0f)+" seconds.");



		Stopwatch currentWatch = System.Diagnostics.Stopwatch.StartNew();

		this.CurrentFolder = scanFolderRecursively(rootFolder);

		currentWatch.Stop();
		long elapsedMs = currentWatch.ElapsedMilliseconds;

		UnityEngine.Debug.Log ("Found "+this.Data.Count+" #files in "+(elapsedMs/1000.0f)+" seconds.");
		UnityEngine.Debug.Log ("Max depth found are "+this.MaxDepth);
		UnityEngine.Debug.Log ("--------------------------------------------");

		//Lets analyse Scanned:

		//Update each folder local sizes.
		foreach(TaggedFile currentFile in this.DataFiles)
		{
			currentFile.Folder.LocalSize += currentFile.Size;
		}

		for(int i=this.MaxDepth; i>=this.MinDepth ; i--)
		{
			foreach(TaggedFolder currentTopFolder in  this.DepthDict[i])
			{
				// My size is 1)my files:
				currentTopFolder.TotalSize = currentTopFolder.LocalSize;

				// 2) + all my children folder sizes.
				foreach(TaggedFolder currentChildFolder in currentTopFolder.Folders )
					currentTopFolder.TotalSize += currentChildFolder.TotalSize;
			}
		}


		refresh( this.CurrentFolder );

		/*
		List<long,string> SortedReverseResult = new List<long,string>();
		foreach( KeyValuePair<string, long> kvp in this.Statistic_Extension ){}
		*/

/*
		List<KeyValuePair<string, long>> myList = this.Statistic_Extension.ToList();
		myList.Sort((firstPair,nextPair) =>
			{
        		return firstPair.Value.CompareTo(nextPair.Value);
    		}
    	);
*/


		//foreach( KeyValuePair<string, long> kvp in this.Statistic_Extension )
		//foreach( KeyValuePair<string, long> kvp in myList )
		//{
		//	UnityEngine.Debug.Log (		kvp.Key+" = "+kvp.Value );
		//}

		/*
		foreach( KeyValuePair<string, long> kvp in this.Statistic_Extension )
		{
			UnityEngine.Debug.Log (		kvp.Key+" = "+kvp.Value );
		}
		*/

	}


	public void OnGUI()
	{

		this.rootFolder = GUI.TextField( new Rect(0f,0f,Screen.width, 32f), this.rootFolder );


		if ( GUI.Button( new Rect(0f,32f,Screen.width, 32f), "-* SCAN *-" ) )
		{

			this.scanFolder();
		}

		if(this.MAXFOLDER!=0)
			this.progression = this.currentFolderCount / this.MAXFOLDER ;
		GUI.Button( new Rect(0f,32f*2,Screen.width*this.progression, 32f), "Progress" );

	}


	public void refresh(TaggedFolder _RootFolder)
	{
		this.CurrentFolder = _RootFolder;

		GameObject oldROOT = GameObject.Find("ROOT") as GameObject;
		Destroy(oldROOT);

		GameObject ROOT = new GameObject("ROOT");

		long total = 0;
		foreach(TaggedFolder currentChildFolder in  _RootFolder.Folders)
			total += currentChildFolder.TotalSize;
		float SumAngle=0.0f;
		foreach(TaggedFolder currentChildFolder in  _RootFolder.Folders)
		{
			float ratio = currentChildFolder.TotalSize /  (float)total;
			SumAngle += ratio*360.0f;
			GameObject currentPie = createSlicedPie( currentChildFolder.FolderName , ratio*360.0f );
			//currentPie.transform.Rotate(  SumAngle, 0.0f,0.0f);
			
			currentPie.transform.RotateAround( new Vector3( SumAngle, 0.0f,0.0f), Vector3.up, SumAngle);
			currentPie.transform.position = Vector3.zero;
			currentPie.transform.parent = ROOT.transform;
			UnityEngine.Debug.Log (currentChildFolder.FolderName+" "+ (ratio*360.0f).ToString());
			
			PieInteractionScript comp = currentPie.AddComponent<PieInteractionScript>() as PieInteractionScript;
			comp.toolTipText = currentChildFolder.FolderName+ " "+Mathf.RoundToInt(ratio*100.0f) +"%" ;
			comp.Folder = currentChildFolder;
		}


	}

	TaggedFolder scanFolderRecursively( string _Path )
	{
		TaggedFolder currentFolder = new TaggedFolder(_Path);
		this.Data.Add ( currentFolder );

		if( currentFolder.Depth > this.MaxDepth )
			this.MaxDepth = currentFolder.Depth;

		if(! this.DepthDict.ContainsKey( currentFolder.Depth ) )
			this.DepthDict[ currentFolder.Depth ] = new List<TaggedFolder>();

		this.DepthDict[ currentFolder.Depth ].Add( currentFolder );


		//string searchPattern = "*";

		string[] files = new string[]{};
		try
		{
			files = System.IO.Directory.GetFiles(_Path); 

		}
		catch (System.Exception e)
		{
		    UnityEngine.Debug.LogWarning("Exception caught ="+e.Message );
		    //return;
		}
		//Fail on
		//DirectoryNotFoundException: 
		//Directory 'W:\PJPMMP\50_Departments\00_Prod\90_Envoi_Externes\To_PRANA\2013-02-05_AssetBreakdown_XML_Scripts-Olivier_S004-Prelight_S005-Prelight_S010-Prelight\orenouard-alkymia-c637e9bb314f\Mel\Installation - PuppetMaster 1.1 for Maya\PuppetMaster\RESOURCES\clips\xsiWoman_walkcycle' not found.

		foreach (string currentFilePath in files)
		{
			FileInfo newFile = new FileInfo(currentFilePath);
			//long s1 = f.Length;

			//this.Data.Add( newFile ) ;

			if(!Statistic_Extension.ContainsKey(newFile.Extension))
				Statistic_Extension[ newFile.Extension ] = 0;
			//Statistic_Extension[ newFile.Extension ] ++;
			Statistic_Extension[ newFile.Extension ] += newFile.Length ;

			TaggedFile currentFile = new TaggedFile(currentFilePath, newFile.Extension, newFile.Length, currentFolder);
			currentFolder.Files.Add(currentFile);
			this.DataFiles.Add(currentFile);

		}

		string[] folders = System.IO.Directory.GetDirectories(_Path); 
		foreach (string currentDirPath in folders)
		{
			this.currentFolderCount ++;
			currentFolder.Folders.Add( scanFolderRecursively(currentDirPath) );
		}
		return currentFolder;
	}







	#region Chart3D

	void createStackBar()
	{
		int i=0;
		float radius=100f;

		float ScaleY=100f;

		//find max.
		float MAX_VALUE=0.0f;
		foreach( KeyValuePair<string, long> kvp in this.Statistic_Extension )
		{
			if((float)kvp.Value > MAX_VALUE)
				MAX_VALUE = (float)kvp.Value;
		}

		//find sum
		float SUM_VALUE=0.0f;
		foreach( KeyValuePair<string, long> kvp in this.Statistic_Extension )
			SUM_VALUE += (float) kvp.Value;

		//purge negligeables.
		List<string> DeadList= new List<string>();
		foreach( KeyValuePair<string, long> kvp in this.Statistic_Extension )
		{
			if(    ( (float)kvp.Value ) < 0.1f*MAX_VALUE )
				DeadList.Add(kvp.Key);
		}
		foreach( string currentKey in DeadList )
			this.Statistic_Extension.Remove(currentKey);

		foreach( KeyValuePair<string, long> kvp in this.Statistic_Extension )
		{
			float ratio = i / (float) this.Statistic_Extension.Count;

			GameObject currentBar = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Cube);
			currentBar.name = kvp.Key;
			currentBar.transform.position = new Vector3( Mathf.Cos( 2*Mathf.PI*ratio ) ,0.0f, Mathf.Sin( 2*Mathf.PI*ratio ) );
			currentBar.transform.position *= radius;
			float valueAsRatio = kvp.Value / MAX_VALUE ;
			currentBar.transform.localScale = new Vector3( 1.0f,ScaleY*valueAsRatio,1.0f);	//ScaleY in Mo.

			i++;
			//UnityEngine.Debug.Log (		kvp.Key+" = "+kvp.Value );


			valueAsRatio = kvp.Value / SUM_VALUE ;
			GameObject currentSlice = createSlicedPie("toto"+valueAsRatio*360f, valueAsRatio*360f);
			currentSlice.transform.RotateAround( currentSlice.transform.up, valueAsRatio*360f);
		}
	}



	GameObject createSlicedPie(string _ObjectName, float _Angle)
	{
		GameObject result = new GameObject(_ObjectName);
		MeshFilter meshFilter = (MeshFilter) result.AddComponent(typeof(MeshFilter));
		meshFilter.mesh = CreateSliceMesh(_Angle);

		MeshCollider mc = result.AddComponent<MeshCollider>() as MeshCollider;
		mc.isTrigger = true;

		MeshRenderer renderer = result.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		renderer.material.shader = Shader.Find ("Diffuse");
		Texture2D tex = new Texture2D(1, 1);
		tex.SetPixel(0, 0, Color.grey);
		tex.Apply();
		renderer.material.mainTexture = tex;
		renderer.material.color = Color.grey;
		return result;
	}


	private Mesh CreateSliceMesh(float angle)	// angle is given as degree.
	{
		//int Sample = 5 ;	//the number of "slices"
		//int Sample = Mathf.RoundToInt(  (angle/360.0f) * 10  ) ;
		int Sample = Mathf.RoundToInt(  (angle/360.0f) * 20  ) ;

		Mesh m = new Mesh();
		m.name = "SliceMesh";
		
		List<Vector3> ThePoints = new List<Vector3>();
		List<Vector3> TheBottomPoints = new List<Vector3>();
		List<Vector3> TheTopPoints = new List<Vector3>();

		List<int> TriIndex = new List<int>();
		List<Vector2> UVList = new List<Vector2>();

		Vector3 Center = new Vector3(0.0f, 0.0f, 0.0f);
		Vector3 Side = new Vector3(1.0f, 0.0f, 0.0f);

		TheBottomPoints.Add ( Center );
		TheBottomPoints.Add ( Side );

		TheTopPoints.Add ( Center + Vector3.up );
		TheTopPoints.Add ( Side + Vector3.up );

		//UVList.Add( new Vector2( Center.x, Center.z) );
		//UVList.Add( new Vector2(Side.x, Side.z) );

		int INDEX=2;
		int OFFSET= 2 + Sample  ;//delta betwwen index of bottom cap and top cap.

		for(int i=1; i<= Sample ;i++)
		{
			float ratio = i / (float)Sample ;
			Vector3 newOne = new Vector3( Mathf.Cos( angle *ratio* Mathf.Deg2Rad ), 0.0f, Mathf.Sin( angle *ratio* Mathf.Deg2Rad) ); 
			TheBottomPoints.Add ( newOne );
			//previous = newOne;

			TheTopPoints.Add ( newOne + Vector3.up );

			//BOTTOM FACE
			TriIndex.Add ( 0 );		//Center
			TriIndex.Add ( INDEX-1 );		//PrevousOne
			TriIndex.Add ( INDEX );			//current

			//TOP FACE
			TriIndex.Add ( INDEX + OFFSET);			//current
			TriIndex.Add ( INDEX-1  + OFFSET);		//PrevousOne
			TriIndex.Add ( 0 + OFFSET );		//Center

			//SIDE1 FACE
			TriIndex.Add ( INDEX-1 + OFFSET);			//current
			TriIndex.Add ( INDEX );			//current
			TriIndex.Add ( INDEX-1 );		//PrevousOne

			//SIDE2 FACE
			TriIndex.Add ( INDEX );			//current
			TriIndex.Add ( INDEX-1  + OFFSET);		//PrevousOne
			TriIndex.Add ( INDEX + OFFSET);			//current

			INDEX++;

			UVList.Add( new Vector2( newOne.x, newOne.z ) );
			
			//HELP debug
			//GameObject gizmo = new GameObject(i.ToString());
			//gizmo.transform.position = newOne;	
			//addCustomIcon(gizmo);

		}

		//Start & End sides.
		//Start
		TriIndex.Add ( OFFSET );
		TriIndex.Add ( 1 );
		TriIndex.Add ( 0 );

		TriIndex.Add ( OFFSET );
		TriIndex.Add ( OFFSET+1 );
		TriIndex.Add ( 1 );


		TriIndex.Add ( 0 );
		TriIndex.Add ( OFFSET -1 );
		TriIndex.Add ( OFFSET*2 -1 );

		TriIndex.Add ( 0 );
		TriIndex.Add ( OFFSET*2-1 );
		TriIndex.Add ( OFFSET );


		TheBottomPoints.AddRange(TheTopPoints);
		ThePoints = TheBottomPoints;
		m.vertices = ThePoints.ToArray();
		
		/*
		m.uv = new Vector2[] {
			new Vector2 (0, 0),
			new Vector2 (0, 1),
			new Vector2(1, 1),
			
			new Vector2 (0, 0),
			new Vector2 (0, 1),
			new Vector2(1, 1)
		};
		*/

		
		m.triangles = TriIndex.ToArray();
		
		m.RecalculateNormals();
		return m;
	}


	#endregion

	/*
	void addCustomIcon(GameObject _theGameObject)
	{
		Texture2D icon = (Texture2D) Resources.Load ("CustomIcon");
		UnityEngine.Debug.Log (icon.width);

		var editorGUIUtilityType = typeof(EditorGUIUtility);
		var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
		var args = new object [] {_theGameObject, icon};
		editorGUIUtilityType.InvokeMember("SetIconForObject", bindingFlags, null, null, args);
		//editorGUIUtilityType.InvokeMember("ForceReloadInspectors", bindingFlags, null,null,null);
	}
	*/


}
