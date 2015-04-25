using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.IO;

public class CLS_FolderTool
{
	//MAX PATH LENGTH  260 caractères.

	//Forbidden caracters  \ / ? : * " > < |


	private List<FileInfo> Files;

	public List<string> scanDirectory( string _Path )
	{
		if(! System.IO.Directory.Exists(_Path) )
		{
			Debug.LogError ("Path does NOT exist ="+_Path);
			return null;//TODO: return custom exception
		}
		else
		{
			List<string> result = new List<string>();

			result = ChildrenOf(_Path);

			Debug.Log ("Found #"+result.Count);
			return result;
		}

	}



	public List<string> ChildrenOf(string _Path) {
		List<string> result = new List<string>();
		AddChildren(_Path, result);
		return result;
	}
	
	private void AddChildren(string _Path, List<string> list) 
	{
		string[] currentFolderFiles = new string[]{};
		currentFolderFiles = System.IO.Directory.GetFiles(_Path); 

		//foreach (string child in currentFolderFiles.) 
		for (int i = 0; i < currentFolderFiles.Length; i++) 
		{

			list.Add(currentFolderFiles[i]);
			//AddChildren(currentFolderFiles[i], list);
		}

		foreach (string subDirectory in Directory.GetDirectories( _Path ))
		{
			AddChildren( subDirectory, list ); 
		}





	}


	/*
	static IEnumerable<T> DepthFirstTreeTraversal<T>(T root, Func<T, IEnumerable<T>> children)      
	{
		var stack = new Stack<T>();
		stack.Push(root);
		while(stack.Count != 0)
		{
			var current = stack.Pop();
			// If you don't care about maintaining child order then remove the Reverse.
			foreach(var child in children(current).Reverse())
				stack.Push(child);
			yield return current;
		}
	}
*/

/*

	private void scanDirectoryRecursively( string _Path  )
	{
		string[] currentFolderFiles = new string[]{};
		try
		{
			currentFolderFiles = System.IO.Directory.GetFiles(_Path); 
		}
		catch (System.Exception e)
		{
			UnityEngine.Debug.LogWarning("Exception caught ="+e.Message );
			//return;
		}
		//Fail on DirectoryNotFoundException: 
		
		foreach (string currentFilePath in currentFolderFiles)
		{
			FileInfo newFile = new FileInfo(currentFilePath);
			Files.Add(newFile);
		}
		foreach (string subDirectory in Directory.GetDirectories( _Path ))
		{
			scanDirectoryRecursively( subDirectory ); 
		}
	}
*/







	
	
	
}
