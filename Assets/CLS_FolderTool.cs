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






    string[] findTagInFile(System.IO.FileInfo _File)
    {
        List<string> tags = new List<string>();






        return tags.ToArray();
    }

    public string[] findTagInFilePath(string _FilePath)
    {
        List<string> tags = new List<string>();

        List<char> Separators = new List<char>();
        Separators.Add(':');
        Separators.Add('\\');     
        Separators.Add('/');
        Separators.Add('.');
        Separators.Add(' ');
        Separators.Add('_');
        Separators.Add('-');

        List<char> currentTag = new List<char>();

        //Debug.Log("From =" + _FilePath);

        for (int i = 0; i < _FilePath.Length; i++)
        {
            char currentCharacter = _FilePath[i];

            if (Separators.Contains(currentCharacter))
            {
                //Splitting!
                if(currentTag.Count>0)
                {
                    string newTag = new string(currentTag.ToArray());
                    newTag = newTag.ToLower();

                    //Removing blacklisted tags.
                    if ((newTag == "img") || (newTag == "vid") || (newTag == "dsc"))
                    {
                        currentTag.Clear();
                        continue;   
                    }
                     
                    
                    if (checkIfTagIsADate(newTag))
                    {
                        /*
                        if (!tags.Contains(newTag.Substring(0, 4)))
                            tags.Add(newTag.Substring(0,4)); //adding year
                        if (!tags.Contains(newTag.Substring(4, 2)))
                            tags.Add(newTag.Substring(4,2)); //adding month
                        if (!tags.Contains(newTag.Substring(6, 2)))
                            tags.Add(newTag.Substring(6,2)); //adding day
                        */
                      
                    }
                    else
                    {
                        if (!tags.Contains(newTag))
                            tags.Add(newTag);
                    }

                    currentTag.Clear();

                }

            }
            else 
            {
                currentTag.Add(currentCharacter);
            }
        }

        //updateModificationDate(_FilePath, tags.ToArray() );


        //Debug.Log("Found =" + System.String.Join(".", tags.ToArray()) );

        return tags.ToArray();
    }


    bool checkIfTagIsADate(string _currentTag)
    {
        //format: 20150416
        if (_currentTag.Length==8)
        {
            if (_currentTag.StartsWith("20") || _currentTag.StartsWith("19"))   //only last century is supported (previously photographs were NOT invented, nor exif)
            {

                if ((_currentTag[4] == '0') || (_currentTag[4] == '1')) //only tweleve months
                {
                    return true;
                }
            }
        }

        return false;
    }

    void updateModificationDate(string _FilePath, string[] _Tags)
    {
        //System.IO.FileInfo theFile = new FileInfo(_FilePath);

        System.DateTime theTime;

        theTime = new System.DateTime(1981, 10, 28);

        System.IO.File.SetLastWriteTime(_FilePath, theTime);

        //File.SetLastWriteTime(path, DateTime.Now);
        
    }

    //string[] tagifyDate(string _thedateAsString)
    //{
    

    //}

}
