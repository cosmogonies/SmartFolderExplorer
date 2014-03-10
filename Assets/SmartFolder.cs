using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SmartFolder
{


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

		public TaggedFolder Parent;	//the parent .
		
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

}

