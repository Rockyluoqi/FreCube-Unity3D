/*******************************************************************************

INTEL CORPORATION PROPRIETARY INFORMATION
This software is supplied under the terms of a license agreement or nondisclosure
agreement with Intel Corporation and may not be copied or disclosed except in
accordance with the terms of that agreement
Copyright(c) 2014 Intel Corporation. All Rights Reserved.

*******************************************************************************/
using UnityEngine;
using System.Collections;

public class TextWindow : MonoBehaviour {
	public int NumLines=20;
	public Color color=Color.yellow;

	// Use this for initialization
	void Start () {
		string lines="";
		for (int i=0;i<NumLines;i++)
			lines+="\n";
		guiText.text=lines;
		guiText.material.color=color;
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void WriteLn(string text) {
		string content=guiText.text;
		guiText.text=content.Substring(content.IndexOf("\n")+1)+text+"\n";
	}
}
