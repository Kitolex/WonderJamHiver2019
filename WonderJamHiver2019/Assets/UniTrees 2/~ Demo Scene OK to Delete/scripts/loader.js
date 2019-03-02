#pragma strict

var loadImage : Texture2D;
 
 

function Start () {
 
}

function Update () {
 if (Application.CanStreamedLevelBeLoaded(1)) {
	Application.LoadLevel(1);
 	}
 }
 

function OnGUI() {
	GUI.Box(Rect((Screen.width/2)-128,(Screen.height/2)-128,256,256),loadImage);
	
	GUI.Label( Rect(0,0,128,32),"" + (Application.GetStreamProgressForLevel(1) * 100) + "%" );
}