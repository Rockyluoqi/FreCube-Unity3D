var renderOverlay : DisplayTextureFullScreen;


function Start() {

	renderOverlay = GetComponent(DisplayTextureFullScreen);
	renderOverlay.setStartColor(Color.white);
	renderOverlay.setDelay(2.0);
}

function Update () {

	if (renderOverlay.GUIColor.a > 0) {
		renderOverlay.AlphaDown(Time.deltaTime);
	} 
}
