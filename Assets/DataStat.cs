using UnityEngine;
using System.Collections;

public class DataStat 
{

	float[] Datas;

	public DataStat(float[] _Datas)
	{
		this.Datas = _Datas;
	}


	float getMin()
	{
		float MIN_VALUE= float.MaxValue;
		foreach(float current in this.Datas )
		{
			if( current < MIN_VALUE)
				MIN_VALUE = current;
		}
		return MIN_VALUE;
	}


	float getMax()
	{
		
		float MAX_VALUE=float.MinValue;
		foreach(float current in this.Datas )
		{
			if( current > MAX_VALUE)
				MAX_VALUE = current;
		}
		
		return MAX_VALUE;

		
	}


}
