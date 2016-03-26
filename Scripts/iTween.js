//iTween Unity VERSION: 1.0.34

/*
Copyright (c)2010 Bob Berkebile(http://www.pixelplacement.com), C# port by Patrick Corkum(http://www.insquare.com)
Permission is hereby granted, free of charge, to any person  obtaining a copy of this software and associated documentation  files (the "Software"), to deal in the Software without  restriction, including without limitation the rights to use,  copy, modify, merge, publish, distribute, sublicense, and/or sell  copies of the Software, and to permit persons to whom the  Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
/* 
 
/*
TERMS OF USE - EASING EQUATIONS
Open source under the BSD License.
Copyright (c)2001 Robert Penner
All rights reserved.
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
Neither the name of the author nor the names of contributors may be used to endorse or promote products derived from this software without specific prior written permission.
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

/*
##########################################################
#
# VARS
# 
##########################################################
*/
//Statics:
static var registers : Array = new Array();
static var params : Array = new Array();

//Pubs:
public var tweenType: String;
public var id : int = 0;
public var inProgress : boolean;

//Privs:
private var kinematic : boolean;

/*
##########################################################
#
# PUB VARS
#
##########################################################
*/
public static var rotateDefaults : Hashtable = {"time":1,"delay":0,"transition":"easeInOutCubic"};
public static var stabDefaults : Hashtable = {"volume":1,"pitch":1,"delay":0};
public static var shakeDefaults : Hashtable = {"time":1,"delay":0};
public static var audioDefaults : Hashtable = {"time":1,"delay":0,"volume":1,"pitch":1,"transition":"linear"};
public static var scaleDefaults : Hashtable = {"time":1,"delay":0,"transition":"easeInOutCubic"};
public static var fadeDefaults : Hashtable = {"time":1,"delay":0,"transition":"linear"};
public static var moveDefaults : Hashtable = {"time":1,"delay":0,"transition":"easeInOutCubic","lookSpeed":8};
public static var moveBezierDefaults : Hashtable = {"time":1,"delay":0,"transition":"easeInOutCubic","lookSpeed":8};
public static var punchPositionDefaults : Hashtable = {"time":1,"delay":0};
public static var punchScaleDefaults : Hashtable = {"time":1,"delay":0};
public static var punchRotationDefaults : Hashtable = {"time":1,"delay":0};
public static var colorDefaults : Hashtable = {"time":1,"delay":0,"transition":"linear"};
public static var lookToUpdateDefaults : Hashtable = {"lookSpeed":3};
public static var moveToUpdateDefaults : Hashtable = {"time":.05};
public static var orientToPath=false;
/*
##########################################################
#
# TRANSITIONS LIST
#
##########################################################
*/
private var TRANSITIONS : Hashtable = {"easeInQuad":easeInQuad, "easeOutQuad":easeOutQuad,"easeInOutQuad":easeInOutQuad, "easeInCubic":easeInCubic, "easeOutCubic":easeOutCubic, "easeInOutCubic":easeInOutCubic, "easeInQuart":easeInQuart, "easeOutQuart":easeOutQuart, "easeInOutQuart":easeInOutQuart, "easeInQuint":easeInQuint, "easeOutQuint":easeOutQuint, "easeInOutQuint":easeInOutQuint, "easeInSine":easeInSine, "easeOutSine":easeOutSine, "easeInOutSine":easeInOutSine, "easeInExpo":easeInExpo, "easeOutExpo":easeOutExpo, "easeInOutExpo":easeInOutExpo, "easeInCirc":easeInCirc, "easeOutCirc":easeOutCirc, "easeInOutCirc":easeInOutCirc, "linear":linear, "spring":spring, "bounce":bounce, "easeInBack":easeInBack, "easeOutBack":easeOutBack, "easeInOutBack":easeInOutBack}; 

/*
##########################################################
# 
# checkForConflicts METHOD
# 
##########################################################
*/
private function checkForConflicts(type:String):void{
	var scripts = GetComponents (iTween);
	for (var script : iTween in scripts) {
		if(script.inProgress && script.tweenType==type){
			Destroy(script);
			script.StopCoroutine(script.tweenType);
		}
	}
}

/*
##########################################################
#
# stop METHOD
#
##########################################################
*/
static function stop(obj: GameObject){
	var scripts = obj.GetComponents (iTween);
	for (var script : iTween in scripts) {
		Destroy(script);
		script.StopCoroutine(script.tweenType);
	}
}

/*
##########################################################
#
# stopType METHOD 
#
##########################################################
*/
static function stopType(obj: GameObject, stopType: String){
	var scripts = obj.GetComponents (iTween);
	for (var script : iTween in scripts) {
		var currentType : String = script.tweenType;
		if(currentType.Substring(0,stopType.length)==stopType){
			Destroy(script);
			script.StopCoroutine(script.tweenType);
		}
	}
}

/*
##########################################################
#
# tweenCount METHOD
#
##########################################################
*/
static function tweenCount(obj: GameObject) : int{
	var scripts = obj.GetComponents (iTween);
	return scripts.Length;
}

/*
##########################################################
#
# findRegister METHOD
#
##########################################################
*/
private function findRegister(){
	for(var i : int = 0; i < registers.length; i++){
		if(registers[i]==gameObject){
			id =  i;
		}
	}
}

/*
##########################################################
#
# init METHOD
#
##########################################################
*/
static function init(target: GameObject,args: Hashtable):void{
	registers.push(target);
	params.push(args);
	target.AddComponent ("iTween");
}

/*
##########################################################
#
# lookToUpdate CONTINUOUSLY CALLABLE METHOD
#
##########################################################
*/
static function lookToUpdate(target: GameObject, args: Hashtable):void{
	var startRotation : Vector3 = target.transform.eulerAngles;
	var lookTarget : Vector3;
	var lookSpeed : float;
	var isLocal : boolean;
	var targetRotation : Quaternion;
	
	if(args["isLocal"]==null){
		isLocal=true;
	}
	
	//Define target:
	lookTarget = args["target"];
	
	//Define speed:
	if(args["lookSpeed"]==null){
		lookSpeed = lookToUpdateDefaults["lookSpeed"];
	}else{
		lookSpeed = args["lookSpeed"];
	}
	
	//Apply look to target:
	switch(isLocal){
		case true:
			targetRotation = Quaternion.LookRotation(lookTarget - target.transform.localPosition, Vector3.up);
			break;
		case false:
			targetRotation = Quaternion.LookRotation(lookTarget - target.transform.position, Vector3.up);
			break;	
	}
	target.transform.rotation = Quaternion.Slerp(target.transform.rotation,targetRotation,Time.deltaTime*lookSpeed);
	
	//Brute force axis back to previous value if user wants single axis usage:
	if(args["axis"] !=null){
		switch(args["axis"]){
			case "x":
				switch(isLocal){
					case true:
						target.transform.localEulerAngles.y = startRotation.y;
						target.transform.localEulerAngles.z = startRotation.z;
						break;
					case false:
						target.transform.eulerAngles.y = startRotation.y;
						target.transform.eulerAngles.z = startRotation.z;
						break;	
				}
				break;
				
			case "y":
				switch(isLocal){
					case true:
						target.transform.localEulerAngles.x = startRotation.x;
						target.transform.localEulerAngles.z = startRotation.z;
						break;
					case false:
						target.transform.eulerAngles.x = startRotation.x;
						target.transform.eulerAngles.z = startRotation.z;
						break;	
				}
				break;
								
			case "z":
				switch(isLocal){
					case true:
						target.transform.localEulerAngles.y = startRotation.y;
						target.transform.localEulerAngles.x = startRotation.x;
						break;
					case false:
						target.transform.eulerAngles.y = startRotation.y;
						target.transform.eulerAngles.z = startRotation.x;
						break;	
				}
				break;
		}
	}
}

/*
##########################################################
#
# lookToUpdateWorld CONTINUOUSLY CALLABLE METHOD
#
##########################################################
*/
static function lookToUpdateWorld(target: GameObject, args: Hashtable):void{
	args["isLocal"]=false;
	lookToUpdate(target,args);
}

/*
##########################################################
#
# moveToUpdate CONTINUOUSLY CALLABLE METHOD
#
##########################################################
*/
static function moveToUpdate(target: GameObject, args: Hashtable):void{
	var tempVelocityX : float = 0.0;
	var tempVelocityY : float = 0.0;
	var tempVelocityZ : float = 0.0;
	var smoothTime : float;
	var positions : Vector3;
	var isLocal : boolean;
	
	if(args["isLocal"]==null){
		isLocal=true;
	}
	
	if(args["time"]==null){
		smoothTime=moveToUpdateDefaults["time"];
	}else{
		smoothTime=args["time"];
	}
	
	if(args["position"]==null){
		if(args["x"]==null){
			switch (isLocal){
				case true:
					positions.x=target.transform.localPosition.x;
					break;
				case false: 
					positions.x=target.transform.position.x;
					break;			
			}
		}else{
			positions.x=args["x"];
		}
		
		if(args["y"]==null){
			switch (isLocal){
				case true:
					positions.y=target.transform.localPosition.y;
					break;
				case false: 
					positions.y=target.transform.position.y;
					break;			
			}
		}else{
			positions.y=args["y"];
		}
		
		if(args["z"]==null){
			switch (isLocal){
				case true:
					positions.z=target.transform.localPosition.z;
					break;
				case false: 
					positions.z=target.transform.position.z;
					break;			
			}
		}else{
			positions.z=args["z"];
		}
	}else{
		positions= args["position"];
	}
	
	var newPlots : Vector3;
	switch (isLocal){
		case true:
			newPlots.x = Mathf.SmoothDamp(target.transform.localPosition.x, positions.x, tempVelocityX, smoothTime);
			newPlots.y = Mathf.SmoothDamp(target.transform.localPosition.y, positions.y, tempVelocityY, smoothTime);
			newPlots.z = Mathf.SmoothDamp(target.transform.localPosition.z, positions.z, tempVelocityZ, smoothTime);
			target.transform.localPosition=newPlots;
			break;
		case false:
			newPlots.x = Mathf.SmoothDamp(target.transform.position.x, positions.x, tempVelocityX, smoothTime);
			newPlots.y = Mathf.SmoothDamp(target.transform.position.y, positions.y, tempVelocityY, smoothTime);
			newPlots.z = Mathf.SmoothDamp(target.transform.position.z, positions.z, tempVelocityZ, smoothTime);
			target.transform.position=newPlots;
			break;	
	}
}

/*
##########################################################
#
# moveToUpdateWorld CONTINUOUSLY CALLABLE METHOD
#
##########################################################
*/
static function moveToUpdateWorld(target: GameObject, args: Hashtable):void{
	args["isLocal"]=false;
	moveToUpdate(target,args);
}

/*
##########################################################
#
# STATIC REGISTERS
#
##########################################################
*/

//### BEGIN CLEANED REGISTERS ###

//moveBy static register:
static function moveBy(target: GameObject,args: Hashtable):void{
	args["type"] = "move";
	args["method"] = "by";
	args["isLocal"] = true;	
	init(target,args);
}

//moveByWorld static register:
static function moveByWorld(target: GameObject,args: Hashtable):void{
	args["type"] = "move";
	args["method"] = "by";
	args["isLocal"] = false;	
	init(target,args);
}

//moveAdd static register:
static function moveAdd(target: GameObject,args: Hashtable):void{
	args["type"] = "move";
	args["method"] = "by";
	args["isLocal"] = true;	
	init(target,args);
}

//moveAddWorld static register:
static function moveAddWorld(target: GameObject,args: Hashtable):void{
	args["type"] = "move";
	args["method"] = "by";
	args["isLocal"] = false;	
	init(target,args);
}

//moveTo static register:
static function moveTo(target: GameObject,args: Hashtable):void{
	args["type"] = "move";
	args["method"] = "to";
	args["isLocal"] = true;	
	init(target,args);
}

//moveToWorld static register:
static function moveToWorld(target: GameObject,args: Hashtable):void{
	args["type"] = "move";
	args["method"] = "to";
	args["isLocal"] = false;	
	init(target,args);
}

//moveFrom static register:
static function moveFrom(target: GameObject,args: Hashtable):void{
	args["type"] = "move";
	args["method"] = "from";
	args["isLocal"] = true;	
	init(target,args);
}

//moveFromWorld static register:
static function moveFromWorld(target: GameObject,args: Hashtable):void{
	args["type"] = "move";
	args["method"] = "from";
	args["isLocal"] = false;	
	init(target,args);
}

//moveToBezier static register:
static function moveToBezier(target: GameObject,args: Hashtable):void{
	args["type"] = "moveBezier";
	args["method"] = "to";
	args["isLocal"] = true;
	init(target,args);
}

//moveToBezierWorld static register:
static function moveToBezierWorld(target: GameObject,args: Hashtable):void{
	args["type"] = "moveBezier";
	args["method"] = "to";
	args["isLocal"] = false;
	init(target,args);
}

//rotateBy static register:
static function rotateBy(target: GameObject,args: Hashtable):void{
	args["type"] = "rotate";
	args["method"] = "by";
	args["isLocal"] = true;	
	init(target,args);
}

//rotateByWorld static register:
static function rotateByWorld(target: GameObject,args: Hashtable):void{
	args["type"] = "rotate";
	args["method"] = "by";
	args["isLocal"] = false;	
	init(target,args);
}

//rotateAdd static register:
static function rotateAdd(target: GameObject,args: Hashtable):void{
	args["type"] = "rotate";
	args["method"] = "add";
	args["isLocal"] = true;	
	init(target,args);
}

//rotateAddWorld static register:
static function rotateAddWorld(target: GameObject,args: Hashtable):void{
	args["type"] = "rotate";
	args["method"] = "add";
	args["isLocal"] = false;	
	init(target,args);
}

//rotateTo static register:
static function rotateTo(target: GameObject,args: Hashtable):void{
	args["type"] = "rotate";
	args["method"] = "to";
	args["isLocal"] = true;	
	init(target,args);
}

//rotateToWorld static register:
static function rotateToWorld(target: GameObject,args: Hashtable):void{
	args["type"] = "rotate";
	args["method"] = "to";
	args["isLocal"] = false;	
	init(target,args);
}

//rotateFrom static register:
static function rotateFrom(target: GameObject,args: Hashtable):void{
	args["type"] = "rotate";
	args["method"] = "from";
	args["isLocal"] = true;	
	init(target,args);
}

//rotateFromWorld static register:
static function rotateFromWorld(target: GameObject,args: Hashtable):void{
	args["type"] = "rotate";
	args["method"] = "from";
	args["isLocal"] = false;	
	init(target,args);
}

//lookTo static register:
static function lookTo(target: GameObject, args: Hashtable):void{
	args["type"] = "look";
	args["method"] = "to";
	args["isLocal"] = true;	
	init(target,args);
}

//lookToWorld static register
static function lookToWorld(target: GameObject, args: Hashtable):void{
	args["type"] = "look";
	args["method"] = "to";
	args["isLocal"] = false;	
	init(target,args);
}

//lookFrom static register:
static function lookFrom(target: GameObject, args: Hashtable):void{
	args["type"] = "look";
	args["method"] = "from";
	args["isLocal"] = true;	
	init(target,args);
}

//lookFromWorld static register
static function lookFromWorld(target: GameObject, args: Hashtable):void{
	args["type"] = "look";
	args["method"] = "from";
	args["isLocal"] = false;	
	init(target,args);
}

//punchRotation static register:
static function punchRotation(target: GameObject,args: Hashtable):void{	
	args["type"] = "punchRotation";
	args["isLocal"] = true;	
	init(target,args);		
}

//punchRotationWorld static register:
static function punchRotationWorld(target: GameObject,args: Hashtable):void{	
	args["type"] = "punchRotation";
	args["isLocal"] = false;	
	init(target,args);		
}

//punchPosition static register:
static function punchPosition(target: GameObject,args: Hashtable):void{
	args["type"] = "punchPosition";
	args["isLocal"] = true;
	init(target,args);	
}

//punchPositionWorld static register:
static function punchPositionWorld(target: GameObject,args: Hashtable):void{
	args["type"] = "punchPosition";
	args["isLocal"] = false;
	init(target,args);	
}

//punchScale static register:
static function punchScale(target: GameObject,args: Hashtable):void{
	args["type"] = "punchScale";
	init(target,args);	
}

//scaleTo static register:
static function scaleTo(target: GameObject,args: Hashtable):void{
	args["type"] = "scale";
	args["method"] = "to";
	init(target,args);
}

//scaleFrom static register:
static function scaleFrom(target: GameObject,args: Hashtable):void{
	args["type"] = "scale";
	args["method"] = "from";
	init(target,args);
}

//scaleBy static register:
static function scaleBy(target: GameObject,args: Hashtable):void{
	args["type"] = "scale";
	args["method"] = "by";
	init(target,args);
}

//scaleAdd static register:
static function scaleAdd(target: GameObject,args: Hashtable):void{
	args["type"] = "scale";
	args["method"] = "add";
	init(target,args);
}

//shake static register:
static function shake(target: GameObject,args: Hashtable):void{
	args["isLocal"] = true;	
	args.Add("type","shake");
	init(target,args);
}

//shakeWorld static register:
static function shakeWorld(target: GameObject,args: Hashtable):void{
	args["isLocal"] = false;	
	args.Add("type","shake");
	init(target,args);
}

//fadeTo static register:
static function fadeTo(target: GameObject,args: Hashtable):void{
	args["type"] = "fade";
	args["method"] = "to";
	init(target,args);
}

//fadeFrom static register:
static function fadeFrom(target: GameObject,args: Hashtable):void{
	args["type"] = "fade";
	args["method"] = "from";
	init(target,args);
}

//colorTo static register:
static function colorTo(target: GameObject,args: Hashtable):void{
	args["type"] = "colorSet";
	args["method"] = "to";
	init(target,args);
}

//colorFrom static register:
static function colorFrom(target: GameObject,args: Hashtable):void{
	args["type"] = "colorSet";
	args["method"] = "from";
	init(target,args);
}
//### END CLEANED REGISTERS ###

//### BEGIN LEGACY REGISTERS ###
//Stab static register:
static function stab(target: GameObject,args: Hashtable):void{
	args.Add("type","stab");
	init(target,args);
}

//Audio to static register:
static function audioTo(target: GameObject,args: Hashtable):void{
	args.Add("type","audioTo");
	init(target,args);
}

//Audio from static register:
static function audioFrom(target: GameObject,args: Hashtable):void{
	var destinationVolume : float;
	var destinationPitch : float;
	
	if(args["audioSource"]==null){
		args["audioSource"]=target.audio;
	}
	if(args["volume"]==null){
		args.Add("volume",audioDefaults["volume"]);
	}
	if(args["pitch"]==null){
		args.Add("pitch",audioDefaults["pitch"]);
	}
	
	var sound : AudioSource = args["audioSource"];
	
	destinationVolume = sound.volume;
	destinationPitch = sound.pitch;
	
	sound.volume=args["volume"];
	sound.pitch=args["pitch"];
	
	args["volume"]=destinationVolume;
	args["pitch"]=destinationPitch;
	
	args.Add("type","audioTo");
	init(target,args);
}

//### END LEGACY REGISTERS ###

/*
##########################################################
#
# Start METHOD
#
##########################################################
*/
function Start(){
	findRegister();
	var args : Hashtable = params[id];
	registers.RemoveAt(id);
	params.RemoveAt(id);
	tweenType=args["type"];

	switch(args["type"]){
		case "look":
			StartCoroutine("look",args);
			break;
			look({}); //fake call to avoid warnings
	
		case "move":
			enableKinematic();
			StartCoroutine("move",args);
			break;
			move({}); //fake call to avoid editor warnings 
			
		case "moveBezier":
			enableKinematic();
			StartCoroutine("moveBezier",args);
			break;
			moveBezier({}); //fake call to avoid editor warnings 
		
		case "rotate":
			enableKinematic();
			StartCoroutine("rotate",args);
			break;
			rotate({}); //fake call to avoid editor warnings 
			
		case "punchRotation":
			enableKinematic();
			StartCoroutine("punchRotation",args);
			break;
			punchRotation({}); //fake call to avoid editor warnings 
		
		case "punchPosition":
			enableKinematic();
			StartCoroutine("punchPosition",args);
			break;
			punchPosition({}); //fake call to avoid editor warnings 
			
		case "punchScale":
			enableKinematic();
			StartCoroutine("punchScale",args);
			break;
			punchScale({}); //fake call to avoid editor warnings 
									
		case "scale":
			enableKinematic();
			StartCoroutine("scale",args);
			break;
			scale({}); //fake call to avoid editor warnings 
		
		case "fade":
			StartCoroutine("fade",args);
			break;
			fade({}); //fake call to avoid editor warnings 
		
		case "colorSet":
			StartCoroutine("colorSet",args);
			break;
			colorSet({}); //fake call to avoid editor warnings 
					
		case "shake":
			enableKinematic();
			StartCoroutine("shake",args);
			break;
			shake({}); //fake call to avoid editor warnings 
			
		case "audioTo":
			StartCoroutine("audioTo",args);
			break;
			audioTo({}); //fake call to avoid editor warnings 
			
		case "stab":
			StartCoroutine("stab",args);
			break;
			stab({}); //fake call to avoid editor warnings 
			
		default:
			break;
	}
}

/*
##########################################################
#
# isKinematic METHOD
#
##########################################################
*/
//Set isKinematic to avoid physics issues:
private function enableKinematic(){
	if(gameObject.GetComponent(Rigidbody)){
		if(!rigidbody.isKinematic){
			rigidbody.isKinematic=true;
			kinematic = true;
		}
	}	
}

/*
##########################################################
#
# OnDisable METHOD
#
##########################################################
*/
//When tween is destroyed or deactivated:
function OnDisable(){
	//kinematic restoration
	if(kinematic){
		rigidbody.isKinematic=false;
	}
}

/*
##########################################################
#
# tweenStart METHOD
#
##########################################################
*/
//When tween starts:
private function tweenStart(args : Hashtable):void{
	if(args["onStartTarget"]==null){
		args.Add("onStartTarget",gameObject);
	}
	if(args["onStart"]){
		var target : GameObject = args["onStartTarget"];
		target.SendMessage(args["onStart"], args["onStartParams"], SendMessageOptions.DontRequireReceiver);
	}
}

/*
##########################################################
#
# tweenUpdate METHOD
#
##########################################################
*/
//When tween updated:
private function tweenUpdate(args : Hashtable):void{
	if(args["onUpdateTarget"]==null){
		args.Add("onUpdateTarget",gameObject);
	}
	if(args["onUpdate"]){
		var target : GameObject = args["onUpdateTarget"];	
		target.SendMessage(args["onUpdate"], args["onUpdateParams"], SendMessageOptions.DontRequireReceiver);
	}
}

/*
##########################################################
#
# tweenComplete METHOD
#
##########################################################
*/
//When tween is done:
private function tweenComplete(args : Hashtable):void{
	if(args["onCompleteTarget"]==null){
		args.Add("onCompleteTarget",gameObject);
	}
	if(args["onComplete"]){
		var target : GameObject = args["onCompleteTarget"];	
		target.SendMessage(args["onComplete"], args["onCompleteParams"], SendMessageOptions.DontRequireReceiver);
	}
}

/*
##########################################################
#
# audioTo APPLICATION
#
##########################################################
*/
private function audioTo(args:Hashtable) {
	//construct args:
	if(args["time"]==null){
		args.Add("time",audioDefaults["time"]);
	}
	if(args["delay"]==null){
		args.Add("delay",audioDefaults["delay"]);
	}
	if(args["transition"]==null){
		args.Add("transition",audioDefaults["transition"]);
	}
	if(args["audioSource"]==null){
		args.Add("audioSource",audio);
	}
	if(args["volume"]==null){
		args.Add("volume",stabDefaults["volume"]);
	}
	if(args["pitch"]==null){
		args.Add("pitch",stabDefaults["pitch"]);
	}
	
	//delay:
	var delay : float = args["delay"];
	if(delay > 0){
		yield WaitForSeconds (args["delay"]);
	}
	
	//Look for conflicts:
	checkForConflicts(tweenType);
	inProgress=true;
	tweenStart(args);
	
	//define object:
	var obj : Transform = gameObject.transform;
	var sound : AudioSource = args["audioSource"];
	var startV : float = sound.volume;
	var startP : float = sound.pitch;
	
	//define targets:
	var endV : float = args["volume"];
	var endP : float = args["pitch"];
	var easing : String = args["transition"];
	var runTime : float = args["time"];
	
	var easingFunc = easeInQuad;
	easingFunc = TRANSITIONS[easing];
	
	//run tween:
	for (var i:float = 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
		sound.volume =easingFunc(startV,endV,i);
		sound.pitch =easingFunc(startP,endP,i);
		tweenUpdate(args);
		yield;
	}
	
	sound.volume = endV;
	sound.pitch = endP;
	tweenComplete(args);
	Destroy (this);
}

/*
##########################################################
#
# stab APPLICATION
#
##########################################################
*/
function stab(args:Hashtable){
	if(!gameObject.GetComponent(AudioSource)){
		gameObject.AddComponent ("AudioSource");
		audio.playOnAwake=false;
	}
	
	//construct args:
	if(args["delay"]==null){
		args.Add("delay",stabDefaults["delay"]);
	}
	if(args["volume"]==null){
		args.Add("volume",stabDefaults["volume"]);
	}
	if(args["pitch"]==null){
		args.Add("pitch",stabDefaults["pitch"]);
	}
	if(args["clip"]==null){
		args.Add("clip",gameObject.audio.clip);
	}
	if(args["onCompleteTarget"]==null){
		args.Add("onCompleteTarget",gameObject);
	}
	
	//delay:
	var delay : float = args["delay"];
	if(delay > 0){
		yield WaitForSeconds (args["delay"]);
	}
	
	inProgress=true;
	tweenStart(args);
	
	//target:
	
	var obj = gameObject.audio;
	obj.clip=args["clip"];
	obj.volume=args["volume"];
	obj.pitch=args["pitch"];
	obj.PlayOneShot(args["clip"]);
	
	//handle completion differently due to audio usage:
	if(args["onComplete"]){
		var pitchArg : float = args["pitch"];
		yield WaitForSeconds(obj.clip.length/pitchArg);
		var target : GameObject = args["onCompleteTarget"];
		target.SendMessage(args["onComplete"], args["onCompleteParams"], SendMessageOptions.DontRequireReceiver);
		Destroy (this);
	}else{
		Destroy (this);	
	}
}

/*
##########################################################
#
# fade APPLICATION
#
##########################################################
*/
function fade(args:Hashtable){
	var method : String = args["method"];
	var runTime : float;
	var delay : float;
	var easing : String;
	var easingFunc = linear;
	var handleChildren : boolean;
	var ignoreCallbacks : boolean;
	var hasGuiTexture : boolean;
	var hasRenderer : boolean;
	var initValue : float;
	var start : float;
	var end : float;
	
	//conflicts?:
	checkForConflicts(tweenType);	
	
	//guiTexture?
	if(guiTexture){
		hasGuiTexture=true;
	}
	
	//hasRenderer?
	if(renderer){
		hasRenderer=true;
	}
	
	//targets:
	if(args["alpha"]==null){
		end=0;
	}else{
		end = args["alpha"];
	}
	
	//specific method setups:
	switch (method){
		case "to":
			if(hasGuiTexture){
				start = guiTexture.color.a;
			}else if(hasRenderer){
				start = renderer.material.color.a;
			}
			break;
		case "from":
			start = end;
			if(hasGuiTexture){
				end = guiTexture.color.a;
				guiTexture.color.a=start;
			}else if(hasRenderer){
				end = renderer.material.color.a;
				renderer.material.color.a=start;
			}
			break;
	}	
	
	//set initvalue:
	if(hasGuiTexture){
		initValue =  guiTexture.color.a;
	}else if(hasRenderer){
		initValue =  renderer.material.color.a;
	}
	
	//if this was a recrsive child, ignore callbacks:
	if(args["ignoreCallbacks"] == null){
		ignoreCallbacks=false;
	}else{
		ignoreCallbacks=true;
	}
	
	//children?
	if(args["includeChildren"] == null){
		handleChildren=true;
	}else{
		handleChildren=false;
	}
	
	//handle children:
	if(handleChildren && transform.childCount>0){
		for (var child : Transform in transform) {
			//Remove child callbacks to avoid repetative callbacks!
			args["ignoreCallbacks"]=true;
			
			switch (method){
				case "to":
					iTween.fadeTo(child.gameObject,args);
					break;
				
				case "from":
					iTween.fadeFrom(child.gameObject,args);
					break;
			}
		}
	}
	
	//duration:
	if(args["time"]==null){
		runTime = fadeDefaults["time"];
	}else{
		runTime = args["time"];
	}
	
	//delay:
	if(args["delay"]==null){
		delay = fadeDefaults["delay"];
	}else{
		delay = args["delay"];
	}
	
	//ease type:
	if(args["transition"]==null){
		easing = fadeDefaults["transition"];
	}else{
		easing = args["transition"];	
	}
	easingFunc = TRANSITIONS[easing];
		
	//delay application:
	if(delay > 0){
		yield WaitForSeconds(delay);
	}
		
	//notify start:
	inProgress=true;
	tweenStart(args);
	
	var caluclatedValue : float;

	//run tween:
	for (var i:float = 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
		caluclatedValue = easingFunc(start,end,i);
		if(hasGuiTexture){
			guiTexture.color.a = caluclatedValue;
		}else if(hasRenderer){
			renderer.material.color.a = caluclatedValue;
		}
		tweenUpdate(args);
		yield;	
	}
	
	//dial in values to ensure precision:
	if(hasGuiTexture){
		guiTexture.color.a = end;
	}else if(hasRenderer){
		renderer.material.color.a = end;
	}

	//if this was a recrsive child, ignore callbacks:
	if(ignoreCallbacks){
		args["onComplete"]="";
		args["onCompleteParams"]="";
		args["onStart"]="";
		args["onStartParams"]="";
	}
	
	//notify complete:
	tweenComplete(args);
	
	//handle loop request:
	switch(args["loopType"]){
		case "loop":
			args["includeChildren"]=false;
			
			if(hasGuiTexture){
				guiTexture.color.a = initValue;
			}else if(hasRenderer){
				renderer.material.color.a = initValue;
			}
						
			switch (method){
				case "to":
					iTween.fadeTo(gameObject,args);
					break;
				case "from":
					iTween.fadeFrom(gameObject,args);
					break;
			}
			
			break;
		case "pingPong":
			args["includeChildren"]=false;
		
			args["alpha"] = initValue;
			iTween.fadeTo(gameObject,args);
			break;
		default:
			Destroy (this);
			break;	
	}	
}

/*
##########################################################
#
# colorSet APPLICATION
#
##########################################################
*/
function colorSet(args:Hashtable){
	var method : String = args["method"];
	var runTime : float;
	var delay : float;
	var easing : String;
	var easingFunc = linear;
	var handleChildren : boolean;
	var ignoreCallbacks : boolean;
	var objectType : String;
	var initValue : Color;
	var start : Color;
	var end : Color;
	
	//conflicts?:
	checkForConflicts(tweenType);
	
	//objectType?
	if(guiTexture){
		objectType="guiTexture";
	}else if(light){
		objectType="light";
	}else if(renderer){
		objectType="renderer";
	}
	
	//set initvalue:
	if(objectType == "guiTexture"){
		initValue =  guiTexture.color;
	}else if(objectType == "light"){
		initValue = light.color;
	}else if(objectType == "renderer"){
		initValue = renderer.material.color ;
	}
		
	//targets:
	if(args["color"]==null){
		end = new Color();
		if(args["r"]==null){
			if(objectType == "guiTexture"){
				end.r = guiTexture.color.r;
			}else if(objectType == "light"){
				end.r = light.color.r;
			}else if(objectType == "renderer"){
				end.r = renderer.material.color.r;
			}
		}else{
			end.r=args["r"];
		}
		
		if(args["g"]==null){
			if(objectType == "guiTexture"){
				end.g = guiTexture.color.g;
			}else if(objectType == "light"){
				end.g = light.color.g;
			}else if(objectType == "renderer"){
				end.g = renderer.material.color.g;
			}
		}else{
			end.g=args["g"];
		}
		
		if(args["b"]==null){
			if(objectType == "guiTexture"){
				end.b = guiTexture.color.b;
			}else if(objectType == "light"){
				end.b = light.color.b;
			}else if(objectType == "renderer"){
				end.b = renderer.material.color.b;
			}
		}else{
			end.b=args["b"];
		}
	}else{
		end = args["color"];
	}	
	
	/*
	//set alpha value:
	if(objectType == "guiTexture"){
		end.a = guiTexture.color.a;
	}else if(objectType == "light"){
		end.a = light.color.a;
	}else if(objectType == "renderer"){
		end.a = renderer.material.color.a;
	}
	*/
	
	if(args["a"]==null){
		if(objectType == "guiTexture"){
			end.a = guiTexture.color.a;
		}else if(objectType == "light"){
			end.a = light.color.a;
		}else if(objectType == "renderer"){
			end.a = renderer.material.color.a;
		}
	}else{
		end.a=args["a"];
	}
	
	//specific method setups:
	switch (method){
		case "to":
			if(objectType == "guiTexture"){
				start = guiTexture.color;
			}else if(objectType == "light"){
				start = light.color;
			}else if(objectType == "renderer"){
				start = renderer.material.color;
			}
			break;
		case "from":
			start = end;			
			if(objectType == "guiTexture"){
				end = guiTexture.color;
				guiTexture.color=start;
			}else if(objectType == "light"){
				end = light.color;
				light.color=start;
			}else if(objectType == "renderer"){
				end = renderer.material.color;
				renderer.material.color=start;
			}
			break;
	}
	
	//if this was a recrsive child, ignore callbacks:
	if(args["ignoreCallbacks"] == null){
		ignoreCallbacks=false;
	}else{
		ignoreCallbacks=true;
	}
	
	//children?
	if(args["includeChildren"] == null){
		handleChildren=true;
	}else{
		handleChildren=false;
	}
	
	//handle children:
	if(handleChildren && transform.childCount>0){
		for (var child : Transform in transform) {
			//Remove child callbacks to avoid repetative callbacks!
			args["ignoreCallbacks"]=true;
			
			switch (method){
				case "to":
					iTween.colorTo(child.gameObject,args);
					break;
				
				case "from":
					iTween.colorFrom(child.gameObject,args);
					break;
			}
		}
	}
	
	//duration:
	if(args["time"]==null){
		runTime = colorDefaults["time"];
	}else{
		runTime = args["time"];
	}
	
	//delay:
	if(args["delay"]==null){
		delay = colorDefaults["delay"];
	}else{
		delay = args["delay"];
	}
	
	//ease type:
	if(args["transition"]==null){
		easing = colorDefaults["transition"];
	}else{
		easing = args["transition"];	
	}
	easingFunc = TRANSITIONS[easing];
		
	//delay application:
	if(delay > 0){
		yield WaitForSeconds(delay);
	}
				
	//notify start:
	inProgress=true;
	tweenStart(args);
	
	var caluclatedValues : Color;
	
	//run tween:
	for (var i:float = 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
		caluclatedValues = Color(easingFunc(start.r,end.r,i),easingFunc(start.g,end.g,i),easingFunc(start.b,end.b,i));
		if(objectType == "guiTexture"){
			guiTexture.color = caluclatedValues;
		}else if(objectType == "light"){
			light.color = caluclatedValues;
		}else if(objectType == "renderer"){
			renderer.material.color = caluclatedValues;
		}
		tweenUpdate(args);
		yield;	
	}
	
	//dial in values to ensure precision:
	if(objectType == "guiTexture"){
		guiTexture.color = end;
	}else if(objectType == "light"){
		light.color = end;
	}else if(objectType == "renderer"){
		renderer.material.color = end;
	}

	//if this was a recrsive child, ignore callbacks:
	if(ignoreCallbacks){
		args["onComplete"]="";
		args["onCompleteParams"]="";
		args["onStart"]="";
		args["onStartParams"]="";
	}
	
	//notify complete:
	tweenComplete(args);
	
	//handle loop request:
	switch(args["loopType"]){
		case "loop":
			args["includeChildren"]=false;
						
			if(objectType == "guiTexture"){
				guiTexture.color = initValue;
			}else if(objectType == "light"){
				light.color = initValue;
			}else if(objectType == "renderer"){
				renderer.material.color = initValue;
			}
						
			switch (method){
				case "to":
					iTween.colorTo(gameObject,args);
					break;
				case "from":
					iTween.colorFrom(gameObject,args);
					break;
			}
			
			break;
		case "pingPong":
			args["includeChildren"]=false;
			args["color"] = start;			
			iTween.colorTo(gameObject,args);
			break;
		default:
			Destroy (this);
			break;	
	}	
}

/*
##########################################################
#
# shake APPLICATION
#
##########################################################
*/
function shake(args:Hashtable){
	var runTime : float;
	var delay : float;
	var isLocal : boolean;
	var start : Vector3;
	var targetValues : Vector3;
	var impact : boolean=true;
	
	//duration:
	if(args["time"]==null){
		runTime = shakeDefaults["time"];
	}else{
		runTime = args["time"];
	}
	
	//delay:
	if(args["delay"]==null){
		delay = shakeDefaults["delay"];
	}else{
		delay = args["delay"];
	}
	
	//local?
	isLocal= args["isLocal"]; 
	
	//obj:
	var obj : Transform = gameObject.transform;
	
	//delay application:
	if(delay > 0){
		yield WaitForSeconds(delay);
	}
	
	//conflicts?:
	checkForConflicts(tweenType);
	
	//notify start:
	inProgress=true;
	tweenStart(args);
		
	//log start values:	
	switch (isLocal){
		case true:
			start=obj.localPosition;
			break;
		case false:	
			start=obj.position;
			break;
	}
		
	//coordiantes:
	if(args["position"] != null){
		targetValues=args["position"];
	}else if(args["amount"] != null){
		targetValues=args["amount"];
	}else{
		if(args["x"] == null){
			targetValues.x=0;
		}else{
			targetValues.x=args["x"];
		}
		if(args["y"] == null){
			targetValues.y=0;
		}else{
			targetValues.y=args["y"];
		}
		if(args["z"] == null){
			targetValues.z=0;
		}else{
			targetValues.z=args["z"];
		}
	}
	
	//run tween:
	var calculatedValues : Vector3;
	//var diminishedValues : Vector3=targetValues;
	
	var initDimX : float = targetValues.x;
	var initDimY : float = targetValues.y;
	var initDimZ : float = targetValues.z;
	
	var dimX : float;
	var dimY : float;
	var dimZ : float;

	for (var i:float = 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
		if(impact){
			impact = false;	
			switch (isLocal){
				case true:
					obj.Translate(Vector3(targetValues.x,targetValues.y,targetValues.z),Space.Self);
					break;
				case false:	
					obj.Translate(Vector3(targetValues.x,targetValues.y,targetValues.z),Space.World);
					break;
			}
		}
		
		if(!impact){
			switch (isLocal){
				case true:
					obj.localPosition=start;
					break;
				case false:	
					obj.position=start;
					break;
			}
			calculatedValues.x = Random.Range(-dimX, dimX);
			calculatedValues.y = Random.Range(-dimY, dimY);
			calculatedValues.z = Random.Range(-dimZ, dimZ);
			
			switch (isLocal){
				case true:
					obj.Translate(calculatedValues,Space.Self);
					break;
				case false:	
					obj.Translate(calculatedValues,Space.World);
					break;
			}
		}
		
		//Diminish shake values		
		dimX = initDimX * (1-i);
		dimY = initDimY * (1-i);
		dimZ = initDimZ * (1-i);
		tweenUpdate(args);
		yield;
	}
	
	//dial in values to ensure precision
	switch (isLocal){
		case true:
			obj.localPosition=start;
			break;
		case false:	
			obj.position=start;
			break;
	}
	tweenComplete(args);
	Destroy (this);
}

/*
##########################################################
#
# punchRotation APPLICATION
#
##########################################################
*/
private function punchRotation(args:Hashtable) {
	//vars:
	var start : Vector3;
	var startRotation : Vector3;
	var end : Vector3;
	var method : String = args["method"];
	var runTime : float;
	var delay : float;
	var easing : String;
	var easingFunc = linear;
	var isLocal : boolean;
	var calculatedValues : Vector3;
	var prevValues : Vector3;
	
	//duration:
	if(args["time"]==null){
		runTime = punchRotationDefaults["time"];
	}else{
		runTime = args["time"];
	}
	
	//delay:
	if(args["delay"]==null){
		delay = punchRotationDefaults["delay"];
	}else{
		delay = args["delay"];
	}
	
	/*
	//ease type:
	if(args["transition"]==null){
		easing = punchRotationDefaults["transition"];
	}else{
		easing = args["transition"];	
	}
	easingFunc = TRANSITIONS[easing];
	*/
	
	//local?
	isLocal= args["isLocal"]; 
	
	//obj:
	var obj : Transform = gameObject.transform;
	
	//delay application:
	if(delay > 0){
		yield WaitForSeconds(delay);
	}
	
	//targets:
	var targetValues : Vector3;
	if(args["rotation"] != null){
		targetValues=args["rotation"];
	}else if(args["amount"] != null){
		targetValues=args["amount"];
	}else{
		if(args["x"] == null){
			targetValues.x=0;
		}else{
			targetValues.x=args["x"];
		}
		if(args["y"] == null){
			targetValues.y=0;
		}else{
			targetValues.y=args["y"];
		}
		if(args["z"] == null){
			targetValues.z=0;
		}else{
			targetValues.z=args["z"];
		}
	}
	
	//conflicts?:
	checkForConflicts(tweenType);
	
	//notify start:
	inProgress=true;
	tweenStart(args);
		
	//log start values:	
	switch (isLocal){
		case true:
			startRotation=obj.localEulerAngles;
			break;
		case false:	
			startRotation=obj.eulerAngles;
			break;
	}

	//run tween:
	for (var i:float = 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
		if(targetValues.x>0){
			calculatedValues.x = punch(targetValues.x,i);
		}else if(targetValues.x<0){
			calculatedValues.x=-punch(Mathf.Abs(targetValues.x),i);	
		}
		if(targetValues.y>0){
			calculatedValues.y=punch(targetValues.y,i);
		}else if(targetValues.y<0){
			calculatedValues.y=-punch(Mathf.Abs(targetValues.y),i);	
		}
		if(targetValues.z>0){
			calculatedValues.z=punch(targetValues.z,i);
		}else if(targetValues.z<0){
			calculatedValues.z=-punch(Mathf.Abs(targetValues.z),i);	
		}
		
		switch (isLocal){
			case true:
				obj.Rotate(calculatedValues-prevValues,Space.Self);
				break;
			case false:
				obj.Rotate(calculatedValues-prevValues,Space.World);
				break;
		}		
		
		prevValues = calculatedValues;
		tweenUpdate(args);
		yield;
	}
	
	//dial in values to ensure precision:
	switch (isLocal){
			case true:
				obj.localEulerAngles=startRotation;
				break;
			case false:	
				obj.eulerAngles=startRotation;
				break;
		}
	
	
	//notify complete:
	tweenComplete(args);
	Destroy (this);
}

/*
##########################################################
#
# punchPosition APPLICATION
#
##########################################################
*/
private function punchPosition(args:Hashtable) {
	//vars:
	var start : Vector3;
	var startPosition : Vector3;
	var end : Vector3;
	var method : String = args["method"];
	var runTime : float;
	var delay : float;
	var isLocal : boolean;
	var changedPosition : Vector3;	
	var calculatedValues : Vector3;
	var prevValues : Vector3;
	
	//duration:
	if(args["time"]==null){
		runTime = punchPositionDefaults["time"];
	}else{
		runTime = args["time"];
	}
	
	//delay:
	if(args["delay"]==null){
		delay = punchPositionDefaults["delay"];
	}else{
		delay = args["delay"];
	}
	
	//local?
	isLocal= args["isLocal"]; 
	
	//obj:
	var obj : Transform = gameObject.transform;
		
	//targets:
	var targetValues : Vector3;
	if(args["position"] != null){
		targetValues=args["position"];
	}else if(args["amount"] != null){
		targetValues=args["amount"];
	}else{
		if(args["x"] == null){
			targetValues.x=0;
		}else{
			targetValues.x=args["x"];
		}
		if(args["y"] == null){
			targetValues.y=0;
		}else{
			targetValues.y=args["y"];
		}
		if(args["z"] == null){
			targetValues.z=0;
		}else{
			targetValues.z=args["z"];
		}
	}
	
	//delay application:
	if(delay > 0){
		yield WaitForSeconds(delay);
	}
	
	//conflicts?:
	checkForConflicts(tweenType);
	
	//notify start:
	inProgress=true;
	tweenStart(args);
		
	//log start values:	
	switch (isLocal){
		case true:
			startPosition=obj.localPosition;
			break;
		case false:	
			startPosition=obj.position;
			break;
	}

	//run tween:
	for (var i:float= 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
		if(targetValues.x>0){
			calculatedValues.x = punch(targetValues.x,i);
		}else if(targetValues.x<0){
			calculatedValues.x=-punch(Mathf.Abs(targetValues.x),i);	
		}
		if(targetValues.y>0){
			calculatedValues.y=punch(targetValues.y,i);
		}else if(targetValues.y<0){
			calculatedValues.y=-punch(Mathf.Abs(targetValues.y),i);	
		}
		if(targetValues.z>0){
			calculatedValues.z=punch(targetValues.z,i);
		}else if(targetValues.z<0){
			calculatedValues.z=-punch(Mathf.Abs(targetValues.z),i);	
		}

		switch (isLocal){
			case true:
				obj.Translate(calculatedValues-prevValues,Space.Self);
				break;
			case false:
				obj.Translate(calculatedValues-prevValues,Space.World);
				break;
		}		
		
		prevValues = calculatedValues;
		tweenUpdate(args);
		yield;
	}
	
	//dial in values to ensure precision:
	switch (isLocal){
			case true:
				obj.localPosition=startPosition;
				break;
			case false:	
				obj.position=startPosition;
				break;
		}
	
	
	//notify complete:
	tweenComplete(args);
	Destroy (this);
}

/*
##########################################################
#
# punchScale APPLICATION
#
##########################################################
*/
private function punchScale(args:Hashtable) {
	//vars:
	var start : Vector3;
	var startScale : Vector3;
	var end : Vector3;
	var method : String = args["method"];
	var runTime : float;
	var delay : float;
	var changedScale : Vector3;	
	var calculatedValues : Vector3;
	var prevValues : Vector3;
	
	//duration:
	if(args["time"]==null){
		runTime = punchScaleDefaults["time"];
	}else{
		runTime = args["time"];
	}
	
	//delay:
	if(args["delay"]==null){
		delay = punchScaleDefaults["delay"];
	}else{
		delay = args["delay"];
	}
	
	//obj:
	var obj : Transform = gameObject.transform;
		
	//targets:
	var targetValues : Vector3;
	if(args["scale"] != null){
		targetValues=args["position"];
	}else if(args["amount"] != null){
		targetValues=args["amount"];
	}else{
		if(args["x"] == null){
			targetValues.x=0;
		}else{
			targetValues.x=args["x"];
		}
		if(args["y"] == null){
			targetValues.y=0;
		}else{
			targetValues.y=args["y"];
		}
		if(args["z"] == null){
			targetValues.z=0;
		}else{
			targetValues.z=args["z"];
		}
	}
	
	//delay application:
	if(delay > 0){
		yield WaitForSeconds(delay);
	}
	
	//conflicts?:
	checkForConflicts(tweenType);
	
	//notify start:
	inProgress=true;
	tweenStart(args);
		
	//log start values:	
	startScale=obj.localScale;
	
	//run tween:
	for (var i:float = 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
		if(targetValues.x>0){
			calculatedValues.x = punch(targetValues.x,i);
		}else if(targetValues.x<0){
			calculatedValues.x=-punch(Mathf.Abs(targetValues.x),i);	
		}
		if(targetValues.y>0){
			calculatedValues.y=punch(targetValues.y,i);
		}else if(targetValues.y<0){
			calculatedValues.y=-punch(Mathf.Abs(targetValues.y),i);	
		}
		if(targetValues.z>0){
			calculatedValues.z=punch(targetValues.z,i);
		}else if(targetValues.z<0){
			calculatedValues.z=-punch(Mathf.Abs(targetValues.z),i);	
		}
		
		obj.localScale=calculatedValues+startScale;
		tweenUpdate(args);
		yield;
	}
	
	//dial in values to ensure precision:
	obj.localScale=startScale;
	
	//notify complete:
	tweenComplete(args);
	Destroy (this);
}

/*
##########################################################
#
# move APPLICATION
#
##########################################################
*/
private function move(args:Hashtable){	
	//vars:
	var start : Vector3;
	var startPosition : Vector3;
	var end : Vector3;
	var method : String = args["method"];
	var runTime : float;
	var delay : float;
	var easing : String;
	var easingFunc = linear;
	var isLocal : boolean;
	var lookType : String;
	var lookAt : Vector3;
	var lookAtTransform : Transform;
	var lookSpeed : float;
	var lookValue : Vector3;
	var targetValues : Vector3;
		
	//conflicts?:
	checkForConflicts(tweenType);
	
	//look setup:
	if(args["lookAt"] != null){
		lookAt = args["lookAt"];
		lookType = "Vector3";
		orientToPath=false;
	}else if(args["lookAtTransform"] != null){
		lookAtTransform=args["lookAtTransform"];
		lookType = "Transform";
		orientToPath=false;
	}
		
	//look speed?
	if(args["lookSpeed"] != null){
		lookSpeed  = args["lookSpeed"];
	}else{
		lookSpeed = moveDefaults["lookSpeed"];
	}
	
	//duration:
	if(args["time"]==null){
		runTime = moveDefaults["time"];
	}else{
		runTime = args["time"];
	}
	
	//delay:
	if(args["delay"]==null){
		delay = moveDefaults["delay"];
	}else{
		delay = args["delay"];
	}
	
	//ease type:
	if(args["transition"]==null){
		easing = moveDefaults["transition"];
	}else{
		easing = args["transition"];	
	}
	easingFunc = TRANSITIONS[easing];
	
	//local?
	isLocal= args["isLocal"]; 
	
	//obj:
	var obj : Transform = gameObject.transform;
		
	//targets #1 sloppy:
	if(args["position"] != null){
		targetValues=args["position"];
	}else if(args["amount"] != null){
		targetValues=args["amount"];
	}else{
		if(args["x"] == null){
			switch (isLocal){
				case true:
					targetValues.x=obj.localPosition.x;
					break;
				case false:
					targetValues.x=obj.position.x;
					break;
			}
			if(method == "add" || method == "by"){
				targetValues.x=0;
			}
		}else{
			targetValues.x=args["x"];
		}
		if(args["y"] == null){
			switch (isLocal){
				case true:
					targetValues.y=obj.localPosition.y;
					break;
				case false:
					targetValues.y=obj.position.y;
					break;
			}
			if(method == "add" || method == "by"){
				targetValues.y=0;
			}
		}else{
			targetValues.y=args["y"];
		}
		if(args["z"] == null){
			switch (isLocal){
				case true:
					targetValues.z=obj.localPosition.z;
					break;
				case false:
					targetValues.z=obj.position.z;
					break;
			}
			if(method == "add" || method == "by"){
				targetValues.z=0;
			}
		}else{
			targetValues.z=args["z"];
		}
	}
	
	//specific method setups #1 sloppy:
	switch (method){
		case "to":
			switch(isLocal){
				case true:
					start = obj.localPosition;
					break;
				case false:
					start = obj.position;
					break;
			}
			end = targetValues;
			break;
		case "from":
			start = targetValues;
			switch(isLocal){
				case true:
					end = obj.localPosition;
					obj.localPosition=start;
										
					switch(lookType){
						case("Vector3"):
							lookValue=lookAt;
							break;
						case("Transform"):
							lookValue=lookAtTransform.localPosition;
							break;
					}
												
					if(lookType){
						iTween.lookTo(gameObject,{"time":0, "axis":args["axis"], "target":lookValue});
					}
					
					break;
				case false:
					end = obj.position;
					obj.position=start;
					
					switch(lookType){
						case("Vector3"):
							lookValue=lookAt;
							break;
						case("Transform"):
							lookValue=lookAtTransform.position;
							break;
					}
												
					if(lookType){
						iTween.lookToWorld(gameObject,{"time":0, "axis":args["axis"], "target":lookValue});
					}
					
					break;
			}
			break;
		case "by":
			start = Vector3.zero;
			end = targetValues;
			break;
	}
	
	//delay application:
	if(delay > 0){
		yield WaitForSeconds(delay);
	}
	
	//targets #2 sloppy:
	if(args["position"] != null){
		targetValues=args["position"];
	}else if(args["amount"] != null){
		targetValues=args["amount"];
	}else{
		if(args["x"] == null){
			switch (isLocal){
				case true:
					targetValues.x=obj.localPosition.x;
					break;
				case false:
					targetValues.x=obj.position.x;
					break;
			}
			if(method == "add" || method == "by"){
				targetValues.x=0;
			}
		}else{
			targetValues.x=args["x"];
		}
		if(args["y"] == null){
			switch (isLocal){
				case true:
					targetValues.y=obj.localPosition.y;
					break;
				case false:
					targetValues.y=obj.position.y;
					break;
			}
			if(method == "add" || method == "by"){
				targetValues.y=0;
			}
		}else{
			targetValues.y=args["y"];
		}
		if(args["z"] == null){
			switch (isLocal){
				case true:
					targetValues.z=obj.localPosition.z;
					break;
				case false:
					targetValues.z=obj.position.z;
					break;
			}
			if(method == "add" || method == "by"){
				targetValues.z=0;
			}
		}else{
			targetValues.z=args["z"];
		}
	}
	
	//specific method setups #2 sloppy:
	switch (method){
		case "to":
			switch(isLocal){
				case true:
					start = obj.localPosition;
					break;
				case false:
					start = obj.position;
					break;
			}
			end = targetValues;
			break;
		case "from":
			start = targetValues;
			switch(isLocal){
				case true:
					end = obj.localPosition;
					obj.localPosition=start;
										
					switch(lookType){
						case("Vector3"):
							lookValue=lookAt;
							break;
						case("Transform"):
							lookValue=lookAtTransform.localPosition;
							break;
					}
												
					if(lookType){
						iTween.lookTo(gameObject,{"time":0, "axis":args["axis"], "target":lookValue});
					}
					
					break;
				case false:
					end = obj.position;
					obj.position=start;
					
					switch(lookType){
						case("Vector3"):
							lookValue=lookAt;
							break;
						case("Transform"):
							lookValue=lookAtTransform.position;
							break;
					}
												
					if(lookType){
						iTween.lookToWorld(gameObject,{"time":0, "axis":args["axis"], "target":lookValue});
					}
					
					break;
			}
			break;
		case "by":
			start = Vector3.zero;
			end = targetValues;
			break;
	}	
							
	//notify start:
	inProgress=true;
	tweenStart(args);
	
	//log start position:
	switch (isLocal){
		case true:
			startPosition=obj.localPosition;
			break;
		case false:
			startPosition=obj.position;
			break;
	}	
	
	//run tween:
	switch (method){
		case "to":
			switch(isLocal){
				case true:
					for (var i:int  = 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
						obj.localPosition=Vector3(easingFunc(start.x,end.x,i),easingFunc(start.y,end.y,i),easingFunc(start.z,end.z,i));
						
						switch(lookType){
							case("Vector3"):
								lookValue=lookAt;
								break;
							case("Transform"):
								lookValue=lookAtTransform.localPosition;
								break;
						}
												
						if(lookType && i <.99 && i>.01){
							iTween.lookToUpdate(gameObject,{"target":lookValue , "lookSpeed":lookSpeed, "axis":args["axis"]});	
						}
						
						tweenUpdate(args);
						yield;
					}
					obj.localPosition=end;
					break;
				case false:
					for (i = 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
						obj.position=Vector3(easingFunc(start.x,end.x,i),easingFunc(start.y,end.y,i),easingFunc(start.z,end.z,i));
						
						switch(lookType){
							case("Vector3"):
								lookValue=lookAt;
								break;
							case("Transform"):
								lookValue=lookAtTransform.position;
								break;
						}
												
						if(lookType && i <.99 && i>.01){
							iTween.lookToUpdateWorld(gameObject,{"target":lookValue , "lookSpeed":lookSpeed, "axis":args["axis"]});	
						}
						
						tweenUpdate(args);
						yield;	
					}
					obj.position=end;
					break;			
			}
			break;
		case "from":
			switch(isLocal){
				case true:
					obj.localPosition=end;
					for (i = 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
						obj.localPosition=Vector3(easingFunc(start.x,end.x,i),easingFunc(start.y,end.y,i),easingFunc(start.z,end.z,i));
						
						switch(lookType){
							case("Vector3"):
								lookValue=lookAt;
								break;
							case("Transform"):
								lookValue=lookAtTransform.localPosition;
								break;
						}
												
						if(lookType && i <.99 && i>.01){
							iTween.lookToUpdate(gameObject,{"target":lookValue, "lookSpeed":lookSpeed, "axis":args["axis"]});	
						}
						
						tweenUpdate(args);
						yield;
					}
					obj.localPosition=end;
					break;
				case false:
					obj.position=end;
					for (i = 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
						obj.position=Vector3(easingFunc(start.x,end.x,i),easingFunc(start.y,end.y,i),easingFunc(start.z,end.z,i));
						
						switch(lookType){
							case("Vector3"):
								lookValue=lookAt;
								break;
							case("Transform"):
								lookValue=lookAtTransform.position;
								break;
						}
												
						if(lookType && i <.99 && i>.01){
							iTween.lookToUpdateWorld(gameObject,{"target":lookValue, "lookSpeed":lookSpeed, "axis":args["axis"]});	
						}
						
						tweenUpdate(args);
						yield;	
					}
					obj.position=end;
					break;			
			}
			break;
		case "by":
			var calculated : Vector3;
			var percent : Vector3;
			var calculatedValues : Vector3;
			var prevValues : Vector3;
			for (i = 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
				calculatedValues=Vector3(easingFunc(start.x,end.x,i),easingFunc(start.y,end.y,i),easingFunc(start.z,end.z,i));
				switch (isLocal){
					case true:
						obj.Translate(calculatedValues-prevValues,Space.Self);
						
						switch(lookType){
							case("Vector3"):
								lookValue=lookAt;
								break;
							case("Transform"):
								lookValue=lookAtTransform.localPosition;
								break;
						}
												
						if(lookType && i <.99 && i>.01){
							iTween.lookToUpdate(gameObject,{"target":lookValue , "lookSpeed":lookSpeed, "axis":args["axis"]});	
						}
						
						break;
					case false:
						obj.Translate(calculatedValues-prevValues,Space.World);
						
						switch(lookType){
							case("Vector3"):
								lookValue=lookAt;
								break;
							case("Transform"):
								lookValue=lookAtTransform.position;
								break;
						}
												
						if(lookType && i <.99 && i>.01){
							iTween.lookToUpdateWorld(gameObject,{"target":lookValue , "lookSpeed":lookSpeed, "axis":args["axis"]});	
						}
						
						break;
				}
				prevValues=calculatedValues;	
				tweenUpdate(args);			
				yield;
			}
			break;	
	}
		
	//lock in values to ensure precision:
	//May revisit and add this to ensure 100% accurate results
	
	//notify complete:
	tweenComplete(args);
	
	//handle loop request:
	switch(args["loopType"]){
		case "loop":
			switch (isLocal){
				case true:
					args["position"]=obj.localPosition;
					obj.localPosition=startPosition;
					iTween.moveTo(gameObject,args);
					break;
				case false:
					args["position"]=obj.position;
					obj.position=startPosition;
					iTween.moveToWorld(gameObject,args);
					break;
			}
			break;
		case "pingPong":
			switch (isLocal){
				case true:
					args["position"]=startPosition;
					iTween.moveTo(gameObject,args);
					break;
				case false:
					args["position"]=startPosition;
					iTween.moveToWorld(gameObject,args);
					break;
			}
			break;
		default:
			Destroy (this);
			break;	
	}	
}

/*
##########################################################
#
# rotate APPLICATION
#
##########################################################
*/
private function rotate(args:Hashtable){	
	//vars:
	var start : Vector3;
	var startRotation : Vector3;
	var end : Vector3;
	var method : String = args["method"];
	var runTime : float;
	var delay : float;
	var easing : String;
	var easingFunc = linear;
	var isLocal : boolean;
	var calculated : Vector3;
	var percent : Vector3;
	var calculatedValues : Vector3;
	var prevValues : Vector3;
	
	//conflicts?:
	checkForConflicts(tweenType);
	
	//duration:
	if(args["time"]==null){
		runTime = rotateDefaults["time"];
	}else{
		runTime = args["time"];
	}
	
	//delay:
	if(args["delay"]==null){
		delay = rotateDefaults["delay"];
	}else{
		delay = args["delay"];
	}
	
	//ease type:
	if(args["transition"]==null){
		easing = rotateDefaults["transition"];
	}else{
		easing = args["transition"];	
	}
	easingFunc = TRANSITIONS[easing];
	
	//local?
	isLocal= args["isLocal"]; 
	
	//obj:
	var obj : Transform = gameObject.transform;
	
	//targets:
	var targetValues : Vector3;
	if(args["rotation"] != null){
		targetValues=args["rotation"];
	}else if(args["amount"] != null){
		targetValues=args["amount"];
	}else{
		if(args["x"] == null){
			switch (isLocal){
				case true:
					targetValues.x=obj.localEulerAngles.x;
					break;
				case false:
					targetValues.x=obj.eulerAngles.x;
					break;
			}
			if(method == "add" || method == "by"){
				targetValues.x=0;
			}
		}else{
			targetValues.x=args["x"];
		}
		if(args["y"] == null){
			switch (isLocal){
				case true:
					targetValues.y=obj.localEulerAngles.y;
					break;
				case false:
					targetValues.y=obj.eulerAngles.y;
					break;
			}
			if(method == "add" || method == "by"){
				targetValues.y=0;
			}
		}else{
			targetValues.y=args["y"];
		}
		if(args["z"] == null){
			switch (isLocal){
				case true:
					targetValues.z=obj.localEulerAngles.z;
					break;
				case false:
					targetValues.z=obj.eulerAngles.z;
					break;
			}
			if(method == "add" || method == "by"){
				targetValues.z=0;
			}
		}else{
			targetValues.z=args["z"];
		}
	}
				
	//specific method setups:
	switch (method){
		case "to":
			switch(isLocal){
				case true:
					start = obj.localEulerAngles;
					startRotation=obj.localEulerAngles;
					break;
				case false:
					start = obj.eulerAngles;
					startRotation=obj.eulerAngles;
					break;
			}
			end =Vector3(clerp(start.x,targetValues.x,1), clerp(start.y,targetValues.y,1), clerp(start.z,targetValues.z,1));
			break;
			
		case "from":
			start = targetValues;
			var tempHold : Vector3;
			switch(isLocal){
				case true:
					tempHold = obj.localEulerAngles;
					obj.Rotate(obj.localEulerAngles-targetValues,Space.Self);
					start = obj.localEulerAngles;
					end =Vector3(clerp(start.x,tempHold.x,1), clerp(start.y,tempHold.y,1), clerp(start.z,tempHold.z,1));
					break;
				case false:
					tempHold = obj.localEulerAngles;
					obj.Rotate(obj.localEulerAngles-targetValues,Space.World);
					start = obj.localEulerAngles;
					end =Vector3(clerp(start.x,tempHold.x,1), clerp(start.y,tempHold.y,1), clerp(start.z,tempHold.z,1));
					break;
			}
			startRotation = tempHold;
			break;
			
		case "add":
			start = Vector3.zero;
			end = targetValues;
			break;
			
		case "by":
			start = Vector3.zero;
			end = targetValues*360;
			break;
	}
	
	//delay application:
	if(delay > 0){
		yield WaitForSeconds(delay);
	}
		
	//notify start:
	inProgress=true;
	tweenStart(args);
		
	//run tween:
	switch (method){
		case "to":
			switch(isLocal){
				case true:
					for (var i:float = 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
						obj.localEulerAngles=Vector3(easingFunc(start.x,end.x,i),easingFunc(start.y,end.y,i),easingFunc(start.z,end.z,i));
						tweenUpdate(args);
						yield;
					}
					obj.localEulerAngles=end;
					break;
				case false:
					for (i= 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
						obj.eulerAngles=Vector3(easingFunc(start.x,end.x,i),easingFunc(start.y,end.y,i),easingFunc(start.z,end.z,i));
						tweenUpdate(args);
						yield;	
					}
					obj.eulerAngles=end;
					break;			
			}
			break;
			
		case "from":
			switch(isLocal){
				case true:
					obj.localEulerAngles=end;
					for (i= 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
						obj.localEulerAngles=Vector3(easingFunc(start.x,end.x,i),easingFunc(start.y,end.y,i),easingFunc(start.z,end.z,i));
						tweenUpdate(args);
						yield;
					}
					obj.localEulerAngles=end;
					break;
				case false:
					obj.eulerAngles=end;
					for (i=0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
						obj.eulerAngles=Vector3(easingFunc(start.x,end.x,i),easingFunc(start.y,end.y,i),easingFunc(start.z,end.z,i));
						tweenUpdate(args);
						yield;	
					}
					obj.eulerAngles=end;
					break;			
			}
			break;
			
		case "add":
			for (i=0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
				calculatedValues=Vector3(easingFunc(start.x,end.x,i),easingFunc(start.y,end.y,i),easingFunc(start.z,end.z,i));
				switch (isLocal){
					case true:
						obj.Rotate(calculatedValues-prevValues,Space.Self);
						break;
					case false:
						obj.Rotate(calculatedValues-prevValues,Space.World);
						break;
				}
				prevValues=calculatedValues;	
				tweenUpdate(args);			
				yield;
			}
			break;	
			
		case "by":
			for (i=0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
				calculatedValues=Vector3(easingFunc(start.x,end.x,i),easingFunc(start.y,end.y,i),easingFunc(start.z,end.z,i));
				switch (isLocal){
					case true:
						obj.Rotate(calculatedValues-prevValues,Space.Self);
						break;
					case false:
						obj.Rotate(calculatedValues-prevValues,Space.World);
						break;
				}
				prevValues=calculatedValues;
				tweenUpdate(args);				
				yield;
			}
			break;	
	}
	
	//lock in values to ensure precision:
	//May revisit and add this to ensure 100% accurate results
		
	//notify complete:
	tweenComplete(args);
	
	//handle loop request:
	switch(args["loopType"]){
		case "loop":
			//reset rotation:
			switch(isLocal){
				case true:
					obj.localEulerAngles=startRotation;
					break;
				case false:
					obj.eulerAngles=startRotation;
					break;		
				}
				
			//replay:
			switch (method){
				case "to":
					switch(isLocal){
						case true:
							iTween.rotateTo(gameObject,args);
							break;
						case false:
							iTween.rotateToWorld(gameObject,args);
							break;		
						}
					break;	
							
				case "from":
					switch(isLocal){
						case true:
							iTween.rotateFrom(gameObject,args);
							break;
						case false:
							iTween.rotateFromWorld(gameObject,args);
							break;		
						}
					break;
					
				case "add":		
					switch(isLocal){
						case true:
							iTween.rotateAdd(gameObject,args);
							break;
						case false:
							iTween.rotateAddWorld(gameObject,args);
							break;		
						}
					break;
					
				case "by":		
					switch(isLocal){
						case true:
							iTween.rotateBy(gameObject,args);
							break;
						case false:
							iTween.rotateByWorld(gameObject,args);
							break;		
						}
					break;
			}
			break;
			
		case "pingPong":
			//flip targets and replay:
			switch (method){
				case "to":
					args["rotation"]=start;
					switch(isLocal){
						case true:
							iTween.rotateTo(gameObject,args);
							break;
							
						case false:
							iTween.rotateToWorld(gameObject,args);
							break;
					}
					break;	
							
				case "from":
					args["rotation"]=start;
					switch(isLocal){
						case true:
							iTween.rotateTo(gameObject,args);
							break;
							
						case false:
							iTween.rotateToWorld(gameObject,args);
							break;
					}
					break;
					
				case "add":		
					targetValues *= -1;
					args["rotation"]=targetValues;
					switch(isLocal){
						case true:
							iTween.rotateAdd(gameObject,args);
							break;
						case false:
							iTween.rotateAddWorld(gameObject,args);
							break;		
						}
					break;
					
				case "by":		
					targetValues *= -1;
					args["rotation"]=targetValues;
					switch(isLocal){
						case true:
							iTween.rotateBy(gameObject,args);
							break;
						case false:
							iTween.rotateByWorld(gameObject,args);
							break;		
						}
					break;
			}
			break;
			
		default:
			Destroy (this);
			break;	
	}	
}

/*
##########################################################
#
# look APPLICATION
#
##########################################################
*/
function look(args: Hashtable){
	var isLocal : boolean = args["isLocal"];
	var delay : float;
	var startRotation : Vector3;
	var endRotation : Vector3;
	var lookPoint : Vector3;
	var method : String = args["method"];
	
	//delay:
	if(args["delay"]==null){
		delay = rotateDefaults["delay"];
	}else{
		delay = args["delay"];
	}
	
	/*
	//Don't think this was neccesary
	//delay application:
	if(delay > 0){
		yield WaitForSeconds(delay);
		args.Remove("delay");
	}
	*/
	
	//look point:
	if(args["transform"] == null){
		lookPoint = args["target"];
	}else{
		var trans : Transform = args["transform"];
		lookPoint = trans.position;
	}
	
	//obj:
	var obj : Transform = gameObject.transform;
	
	//startRotation:
	switch (isLocal){
		case true:
			startRotation=obj.transform.localEulerAngles;
			break;
		case false:
			startRotation=obj.transform.eulerAngles;
			break;
	}
	
	//endRotation:
	obj.transform.LookAt(lookPoint);
	switch (isLocal){
		case true:
			endRotation=obj.transform.localEulerAngles;
			obj.transform.localEulerAngles = startRotation;
			break;
		case false:
			endRotation=obj.transform.eulerAngles;
			obj.transform.eulerAngles = startRotation;
			break;
	}
	
	//Brute force axis back to previous value if user wants single axis usage:
	if(args["axis"] !=null){
		switch(args["axis"]){
			case "x":
				endRotation.y=startRotation.y;
				endRotation.z=startRotation.z;
				break;
			case "y":
				endRotation.x=startRotation.x;
				endRotation.z=startRotation.z;
				break;
			case "z":
				endRotation.x=startRotation.x;
				endRotation.y=startRotation.y;
				break;
		}
	}
	
	args["rotation"] = endRotation;
	
	switch (isLocal){
		case true:
			switch(method){
				case "to":
					iTween.rotateTo(obj.gameObject,args);
					break;
					
				case "from":
					iTween.rotateFrom(obj.gameObject,args);
					break;
			}
			break;
			
		case false:
			switch(method){
				case "to":
					iTween.rotateToWorld(obj.gameObject,args);
					break;
					
				case "from":
					iTween.rotateFromWorld(obj.gameObject,args);
					break;
			}
	}
	Destroy(this);
}

/*
##########################################################
#
# moveBezier APPLICATION
#
##########################################################
*/

//moveBezier application:
private function moveBezier(args:Hashtable){
	//vars:
	var isLocal : boolean;
	var startPosition : Vector3;
	var method : String = args["method"];
	var runTime : float;
	var delay : float;
	var easing : String;
	var easingFunc = linear;
	var lookAxis : String;
	
	//lock look to an axis?
	if(args["axis"] != null){
		lookAxis = args["axis"];
	}else{
		lookAxis = "";
	}
	
	//HANDLE 'METHOD' TO ALLOW FROM!
		
	//construct args:
	if(args["lookSpeed"]==null){
		args.Add("lookSpeed",moveBezierDefaults["lookSpeed"]);
	}
	if(args["orientToPath"] == null){
		args.Add("orientToPath",true);
	}
	if(args["looped"]==null){
		args["looped"]=false;
	}
	
	//duration:
	if(args["time"]==null){
		runTime = moveDefaults["time"];
	}else{
		runTime = args["time"];
	}
	
	//delay:
	if(args["delay"]==null){
		delay = moveDefaults["delay"];
	}else{
		delay = args["delay"];
	}
	
	//ease type:
	if(args["transition"]==null){
		easing = moveDefaults["transition"];
	}else{
		easing = args["transition"];	
	}
	easingFunc = TRANSITIONS[easing];
	
	//local?
	isLocal= args["isLocal"]; 
	
	//obj:
	var obj : Transform = gameObject.transform;
	
	//delay application:
	if(delay > 0){
		yield WaitForSeconds(delay);
	}
	
	//conflicts?:
	checkForConflicts(tweenType);
	
	//notify start:
	inProgress=true;
	tweenStart(args);
	
	//log start values:	
	switch (isLocal){
		case true:
			startPosition=obj.localPosition;
			break;
		case false:	
			startPosition=obj.position;
			break;
	}
	
	//init bezier
	var n : float;
	var segments : float;
	var beziers : Array = Array(args["bezier"]);
	var _beziersPI: Array;
	var props : Hashtable = {};
	var i : float;
	var ii : float;
	var b : Object;
	var p : DictionaryEntry;
	var start : Vector3;
	
	if (args["isLocal"]) {
		beziers.splice(0,0, obj.localPosition);
		start=obj.localPosition;
	}else{
		beziers.splice(0,0, obj.position);
		start=obj.position;
	}

	_beziersPI = ParseBeziers(beziers,args["looped"]);
	var iNumPoints : int = _beziersPI.length;
	
	//run tween:
	for (i = 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
		var virtTimePart : float = easingFunc(0, 1, i);
		var iCurAxisPoint : int;
		
		if (virtTimePart <= 0){
			iCurAxisPoint = 0;
		}else if (virtTimePart >= 1){
			iCurAxisPoint = iNumPoints - 1;
		}else{
			iCurAxisPoint = Mathf.Floor(iNumPoints * virtTimePart);
		}

		var timeFract : float = iNumPoints * virtTimePart - iCurAxisPoint;
		var bpi : BezierPointInfo = _beziersPI[iCurAxisPoint];

		var newVector : Vector3 = bpi.starting + timeFract * (2 * (1 - timeFract) * (bpi.intermediate - bpi.starting) + timeFract * (bpi.end - bpi.starting));
		
		var lookSpeed : float = args["lookSpeed"];
		
		//orientToPath - cutting off outer ends of curve percentage to avoid lookAt jitters:
		if(args["orientToPath"] && i <.99 && i>.01){
			switch(isLocal){
				case true:
					iTween.lookToUpdate(gameObject,{"target":newVector,"lookSpeed":lookSpeed,"axis":lookAxis});
					break;
				case false:	
					iTween.lookToUpdateWorld(gameObject,{"target":newVector,"lookSpeed":lookSpeed,"axis":lookAxis});
					break;
			}
		}
		
		//look at target:
		if(args["lookAt"] != null){
			args["orientToPath"] = false;
			var lookTarget : Vector3 = args["lookAt"];
			switch(isLocal){
				case true:
					iTween.lookToUpdateWorld(gameObject,{"target":lookTarget,"lookSpeed":lookSpeed,"axis":lookAxis});
					break;
				case false:	
					iTween.lookToUpdateWorld(gameObject,{"target":lookTarget,"lookSpeed":lookSpeed,"axis":lookAxis});
					break;
			}
		}
		
		//look at transform:
		if(args["lookAtTransform"] != null){
			args["orientToPath"] = false;
			var lookTransform : Transform = args["lookAtTransform"];
			switch(isLocal){
				case true:
					iTween.lookToUpdate(gameObject,{"target":lookTransform.position,"lookSpeed":lookSpeed,"axis":lookAxis});
					break;
				case false:	
					iTween.lookToUpdateWorld(gameObject,{"target":lookTransform.position,"lookSpeed":lookSpeed,"axis":lookAxis});
					break;
			}
		}

		//move object				
		if (args["isLocal"]){
			obj.localPosition = newVector;
		}else{
			obj.position = newVector; 
		}		
		tweenUpdate(args);
		yield;
	}
	
	//ensure end position is accurate:
	var bpiEnd : BezierPointInfo = _beziersPI[_beziersPI.Count - 1];
	switch(isLocal){
		case true:
			obj.transform.localPosition = bpiEnd.end;
			break;
		case false:	
			obj.transform.position = bpiEnd.end;
			break;
	}
	
	//handle loop request:
	switch(args["loopType"]){
		case "loop":
			switch(isLocal){
				case true:
					obj.localPosition = start;
					iTween.moveToBezier(gameObject,args);
					break;
				case false:	
					obj.position = start; 
					iTween.moveToBezierWorld(gameObject,args);
					break;
			}
			break;
		case "pingPong":
			beziers.Reverse();
			args["bezier"]=beziers;
			args["looped"]=true;
			switch(isLocal){
				case true:
					iTween.moveToBezier(gameObject,args);
					break;
				case false:	
					iTween.moveToBezierWorld(gameObject,args);
					break;
			}
			break;
		default:
			Destroy (this);
			break;			
	}
	
	tweenComplete(args);
}

/*
##########################################################
#
# scale APPLICATION
#
##########################################################
*/
private function scale(args:Hashtable){
	//vars:
	var start : Vector3;
	var startScale : Vector3;
	var end : Vector3;
	var method : String = args["method"];
	var runTime : float;
	var delay : float;
	var easing : String;
	var easingFunc = linear;
	var targetValues : Vector3;
	var calculatedValues : Vector3;
	
	//conflicts?:
	checkForConflicts(tweenType);
	
	//duration:
	if(args["time"]==null){
		runTime = scaleDefaults["time"];
	}else{
		runTime = args["time"];
	}
	
	//delay:
	if(args["delay"]==null){
		delay = scaleDefaults["delay"];
	}else{
		delay = args["delay"];
	}
	
	//ease type:
	if(args["transition"]==null){
		easing = scaleDefaults["transition"];
	}else{
		easing = args["transition"];	
	}
	easingFunc = TRANSITIONS[easing];
	
	//obj:
	var obj : Transform = gameObject.transform;
	
	//targets:
	if(args["scale"] != null){
		targetValues=args["scale"];
	}else if(args["amount"] != null){
		targetValues=args["amount"];
	}else{
		if(args["x"] == null){
			if(method == "by"){
				targetValues.x=1;
			}else if(method == "add"){
				targetValues.x=0;
			}else{
				targetValues.x=obj.localScale.x;
			}
		}else{
			targetValues.x=args["x"];
		}
		if(args["y"] == null){
			if(method == "by"){
				targetValues.y=1;
			}else if(method == "add"){
				targetValues.y=0;
			}else{
				targetValues.y=obj.localScale.y;
			}
		}else{
			targetValues.y=args["y"];
		}
		if(args["z"] == null){
			if(method == "by"){
				targetValues.z=1;
			}else if(method == "add"){
				targetValues.z=0;
			}else{
				targetValues.z=obj.localScale.z;
			}
		}else{
			targetValues.z=args["z"];
		}
	}
							
	//log start position:
	startScale=obj.localScale;				
	
	//specific method setups:
	switch (method){
		case "to":
			start = obj.localScale;
			end = targetValues;
			break;
			
		case "from":
			end = obj.localScale;
			obj.localScale=targetValues;
			start = obj.localScale;
			break;
			
		case "by":
			start = obj.localScale;
			end = Vector3(obj.localScale.x*targetValues.x,obj.localScale.y*targetValues.y,obj.localScale.z*targetValues.z);
			break;
			
		case "add":
			start = obj.localScale;
			end = Vector3(obj.localScale.x+targetValues.x,obj.localScale.y+targetValues.y,obj.localScale.z+targetValues.z);
			break;
	}
		
	//delay application:
	if(delay > 0){
		yield WaitForSeconds(delay);
	}
		
	//notify start:
	inProgress=true;
	tweenStart(args);
		
	//run tween:
	switch (method){
		case "to":
			for (var i:float= 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
				obj.localScale=Vector3(easingFunc(start.x,end.x,i),easingFunc(start.y,end.y,i),easingFunc(start.z,end.z,i));
				tweenUpdate(args);
				yield;
			}
			obj.localScale=end;		
			break;
			
		case "from":
			obj.localScale=end;
			for (i= 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
				obj.localScale=Vector3(easingFunc(start.x,end.x,i),easingFunc(start.y,end.y,i),easingFunc(start.z,end.z,i));
				tweenUpdate(args);
				yield;
			}
			obj.localScale=end;
			break;
			
		case "by":
			for (i= 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
				calculatedValues=Vector3(easingFunc(start.x,end.x,i),easingFunc(start.y,end.y,i),easingFunc(start.z,end.z,i));
				obj.localScale=calculatedValues;
				tweenUpdate(args);
				yield;
			}
			obj.localScale=end;
			break;	
			
		case "add":
			for (i= 0.0; i < 1.0; i += Time.deltaTime*(1/runTime)) {
				calculatedValues=Vector3(easingFunc(start.x,end.x,i),easingFunc(start.y,end.y,i),easingFunc(start.z,end.z,i));
				obj.localScale=calculatedValues;
				tweenUpdate(args);
				yield;
			}
			obj.localScale=end;
			break;
	}
	
	//notify complete:
	tweenComplete(args);
	
	//handle loop request:
	switch(args["loopType"]){
		case "loop":
			//reset scale:
			obj.localScale=startScale;
				
			//replay:
			switch (method){
				case "to":
					iTween.scaleTo(gameObject,args);
					break;	
							
				case "from":
					iTween.scaleFrom(gameObject,args);
					break;
					
				case "add":		
					iTween.scaleAdd(gameObject,args);
					break;
					
				case "by":		
					iTween.scaleBy(gameObject,args);
					break;
			}
			break;
			
		case "pingPong":
			//flip targets and replay:
			switch (method){
				case "to":
					args["scale"]=start;
					iTween.scaleTo(gameObject,args);
					break;	
							
				case "from":
					args["scale"]=start;
					iTween.scaleTo(gameObject,args);
					break;
					
				case "add":		
					targetValues *= -1;
					args["scale"]=targetValues;
					iTween.scaleAdd(gameObject,args);
					break;
					
				case "by":				
					targetValues = start;
					args["scale"]=targetValues;
					iTween.scaleTo(gameObject,args);
					break;
			}
			break;
			
		default:
			Destroy (this);
			break;	
	}
}

/*
##########################################################
#
# EASING CURVES
#
##########################################################
*/
private function linear(start : float, end : float, value : float) : float{
	return Mathf.Lerp(start, end, value);
}

private function clerp(start : float, end : float, value : float) : float {
	var min = 0.0;
	var max = 360.0;
	var half = Mathf.Abs((max - min)/2.0);
	var retval = 0.0;
	var diff = 0.0;

	if((end - start) < -half){
		diff = ((max - start)+end)*value;
		retval =  start+diff;
	}
	else if((end - start) > half){
		diff = -((max - end)+start)*value;
		retval =  start+diff;
	}
	else retval =  start+(end-start)*value;
	return retval;
}

private function spring(start : float, end : float, value : float) : float{
	value = Mathf.Clamp01(value);
	value = (Mathf.Sin(value * Mathf.PI * (0.2 + 2.5 * value * value * value)) * Mathf.Pow(1 - value, 2.2) + value) * (1 + (1.2 * (1 - value)));
	return start + (end - start) * value;
}

private function easeInQuad(start : float, end : float, value : float) : float {
	value /= 1;
	end -= start;
	return end*value*value + start;
}

private function easeOutQuad(start : float, end : float, value : float) : float{
	value /= 1;
	end -= start;
	return -end * value*(value-2) + start;
}

private function easeInOutQuad(start : float, end : float, value : float) : float {
	value /= .5;
	end -= start;
	if (value < 1) return end/2*value*value + start;
	value--;
	return -end/2 * (value*(value-2) - 1) + start;
};

private function easeInCubic (start : float, end : float, value : float) : float {
	value /= 1;
	end -= start;
	return end*value*value*value + start;
};

private function easeOutCubic (start : float, end : float, value : float) : float {
	value /= 1;
	value--;
	end -= start;
	return end*(value*value*value + 1) + start;
};

private function easeInOutCubic (start : float, end : float, value : float) : float {
	value /= .5;
	end -= start;
	if (value < 1) return end/2*value*value*value + start;
	value -= 2;
	return end/2*(value*value*value + 2) + start;
};

private function easeInQuart (start : float, end : float, value : float) : float {
	value /= 1;
	end -= start;
	return end*value*value*value*value + start;
};

private function easeOutQuart(start : float, end : float, value : float) : float {
	value /= 1;
	value--;
	end -= start;
	return -end * (value*value*value*value - 1) + start;
};

private function easeInOutQuart(start : float, end : float, value : float) : float {
	value /= .5;
	end -= start;
	if (value < 1) return end/2*value*value*value*value + start;
	value -= 2;
	return -end/2 * (value*value*value*value - 2) + start;
};

private function easeInQuint(start : float, end : float, value : float) : float {
	value /= 1;
	end -= start;
	return end*value*value*value*value*value + start;
};

private function easeOutQuint(start : float, end : float, value : float) : float {
	value /= 1;
	value--;
	end -= start;
	return end*(value*value*value*value*value + 1) + start;
};

private function easeInOutQuint(start : float, end : float, value : float) : float {
	value /= .5;
	end -= start;
	if (value < 1) return end/2*value*value*value*value*value + start;
	value -= 2;
	return end/2*(value*value*value*value*value + 2) + start;
};

private function easeInSine(start : float, end : float, value : float) : float {
	end -= start;
	return -end * Mathf.Cos(value/1 * (Mathf.PI/2)) + end + start;
};

private function easeOutSine(start : float, end : float, value : float) : float {
	end -= start;
	return end * Mathf.Sin(value/1 * (Mathf.PI/2)) + start;
};

private function easeInOutSine(start : float, end : float, value : float) : float {
	end -= start;
	return -end/2 * (Mathf.Cos(Mathf.PI*value/1) - 1) + start;
};

private function easeInExpo(start : float, end : float, value : float) : float {
	end -= start;
	return end * Mathf.Pow(2, 10 * (value/1 - 1) ) + start;
};

private function easeOutExpo(start : float, end : float, value : float) : float {
	end -= start;
	return end * ( -Mathf.Pow( 2, -10 * value/1 ) + 1 ) + start;
};

private function easeInOutExpo(start : float, end : float, value : float) : float {
	value /= .5;
	end -= start;
	if (value < 1) return end/2 * Mathf.Pow( 2, 10 * (value - 1) ) + start;
	value--;
	return end/2 * ( -Mathf.Pow( 2, -10 * value) + 2 ) + start;
};

private function easeInCirc(start : float, end : float, value : float) : float {
	value /= 1;
	end -= start;
	return -end * (Mathf.Sqrt(1 - value*value) - 1) + start;
};

private function easeOutCirc(start : float, end : float, value : float) : float {
	value /= 1;
	value--;
	end -= start;
	return end * Mathf.Sqrt(1 - value*value) + start;
};

private function easeInOutCirc(start : float, end : float, value : float) : float {
	value /= .5;
	end -= start;
	if (value < 1) return -end/2 * (Mathf.Sqrt(1 - value*value) - 1) + start;
	value -= 2;
	return end/2 * (Mathf.Sqrt(1 - value*value) + 1) + start;
};

private function bounce (start : float, end : float, value : float) : float {
	value /= 1;
	end -= start;
	if (value < (1/2.75)) {
		return end*(7.5625*value*value) + start;
	} else if (value < (2/2.75)) {
		value-=(1.5/2.75);
		return end*(7.5625*(value)*value + .75) + start;
	} else if (value < (2.5/2.75)) {
		value-=(2.25/2.75);
		return end*(7.5625*(value)*value + .9375) + start;
	} else {
		value-=(2.625/2.75);
		return end*(7.5625*(value)*value + .984375) + start;
	}
}
var s:float=0;
function easeInBack (start : float, end : float, value : float) : float {
	end -= start;
	value/=1;
	s = 1.70158;
	return end*(value)*value*((s+1)*value - s) + start;
}

function easeOutBack (start : float, end : float, value : float) : float {
	s = 1.70158;
	end -= start;
	value=(value/1)-1;
	return end*((value)*value*((s+1)*value + s) + 1) + start;
}

function easeInOutBack (start : float, end : float, value : float) : float {
	s = 1.70158; 
	end -= start;
	value/=.5;
	if ((value) < 1) {
		s*=(1.525);
		return end/2*(value*value*(((s)+1)*value - s)) + start;
	}
	value-=2;
	s*=(1.525);
	return end/2*((value)*value*(((s)+1)*value + s) + 2) + start;
}

function punch (amplitude : float, value: float) : float {	
	var start=0;
	var end=0;
	var s : float;
	
	if (value==0){
		return start; 
	}
	
	value /= 1;
	
	if (value==1){
		return start+end;  
	}
	
	var period=1*.3;
	
	if (amplitude < Mathf.Abs(end)) { 
		amplitude=end; 
		s=period/4;
	}else {
		s = period/(2*Mathf.PI) * Mathf.Asin (end/amplitude);
		return (amplitude*Mathf.Pow(2,-10*value) * Mathf.Sin((value*1-s)*(2*Mathf.PI)/period) + end + start);
	}
}

/*
##########################################################
#
# BezierPointInfo HELPER
#
##########################################################
*/
class BezierPointInfo
{
	public var starting : Vector3;
	public var intermediate : Vector3;
	public var end: Vector3;
}

/*
##########################################################
#
# ParseBeziers HELPER
#
##########################################################
*/
private function ParseBeziers(points: Array, wasLoop:boolean) : Array{
	if(wasLoop){
		points.Shift();
	}
	
	var returnPoints : Array= new Array();

	if (points.Count > 2){
		var iCurPoint : int;
		
		for (iCurPoint = 0; iCurPoint < points.Count - 1; iCurPoint++){
			var curPoint : Vector3 = points[iCurPoint];
			var curSetofPoints: BezierPointInfo = new BezierPointInfo();
			curSetofPoints.starting = curPoint;
			if (iCurPoint == 0){
				var p1 : Vector3 = points[1];
				var p2 : Vector3 = points[2];
				curSetofPoints.intermediate = p1 - ((p2 - curPoint) / 4);					
			}else{
				var bpiint: BezierPointInfo = returnPoints[iCurPoint - 1];
				curSetofPoints.intermediate = 2 * curPoint - bpiint.intermediate;
			}
			curSetofPoints.end = points[iCurPoint + 1];
			returnPoints.push(curSetofPoints);
		}
	}else{
		var curSetofPoints2: BezierPointInfo = new BezierPointInfo();
		curSetofPoints2.starting = points[0];
		curSetofPoints2.end = points[1];
		curSetofPoints2.intermediate = ((curSetofPoints2.starting + curSetofPoints2.end) / 2);
		
		returnPoints.push(curSetofPoints2);
	}
	return returnPoints;
}