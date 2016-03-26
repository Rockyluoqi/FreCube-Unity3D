#pragma strict
var AnchorPoint:GameObject;
private var rateWasGameObject:GameObject;//被点中物体//
private var mouseButtonDownhitPoint:Vector3;//获取点击碰撞点//
private var mouseButtonhitPoint:Vector3;//获取按键碰撞点//
private var isChangParent=false;//是否已经改变父对象//
private var mouseDownPoint:Vector3;//鼠标点击位置//
private var mouseButtonPoint:Vector3;//鼠标拖动位置//
private var collideNormal:Vector3;//碰撞法线//
private var isReset=false;//是否复原//
function Start () {
}
function Update () {
	if(!AnchorPoint.GetComponent(iTween)&&isChangParent&&!isReset){//复位//
		var gos1 : GameObject[];
			gos1 = GameObject.FindGameObjectsWithTag("Cube");
		for (var go1 : GameObject in gos1) {
					go1.transform.parent=this.transform;
			}
		AnchorPoint.transform.rotation = Quaternion.identity;
		isReset=true;
		collideNormal=Vector3(0,0,0);
		isChangParent=false;
	}
	if(Input.GetMouseButtonDown(0)){
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);    
      	var hit : RaycastHit;    
        if (Physics.Raycast (ray, hit))     			//发出碰撞射线//
        {    
        	rateWasGameObject=hit.collider.gameObject;//获取碰撞物体//
        	mouseButtonDownhitPoint=hit.point;	//获取鼠标点击碰撞点//
        	mouseDownPoint=Input.mousePosition;	//获取鼠标点击位置//
        	collideNormal=hit.normal;//获取碰撞法线//
        } 
	}
	if(Input.GetMouseButtonUp(0)){//当鼠标弹起碰撞物体为空//
		rateWasGameObject=null;
	}
	if(Input.GetMouseButton(0)&&rateWasGameObject!=null){
		var ray1 = Camera.main.ScreenPointToRay (Input.mousePosition);    
      	var hit1 : RaycastHit;  
        if (Physics.Raycast (ray1, hit1))     			//发出碰撞射线//
        {    
        	mouseButtonhitPoint=hit1.point;		//获取碰撞点//
        }
        if(Mathf.Abs(collideNormal.z-0)>0.5){//前后两面//
        	if((Mathf.Abs(mouseButtonhitPoint.x-mouseButtonDownhitPoint.x)>0.5||Mathf.Abs(mouseButtonhitPoint.y-mouseButtonDownhitPoint.y)>0.5)&&!isChangParent){//拖动鼠标超过0.5//
        		var gos : GameObject[];
				gos = GameObject.FindGameObjectsWithTag("Cube");
				for (var go : GameObject in gos) {
					if(Mathf.Abs(mouseButtonhitPoint.x-mouseButtonDownhitPoint.x)>0.5){
						if(Mathf.Abs(go.transform.position.y-rateWasGameObject.transform.position.y)<0.2){
							go.transform.parent=AnchorPoint.transform;
						}
					}
					else if(Mathf.Abs(mouseButtonhitPoint.y-mouseButtonDownhitPoint.y)>0.5){
						if(Mathf.Abs(go.transform.position.x-rateWasGameObject.transform.position.x)<0.2){
							go.transform.parent=AnchorPoint.transform;
						}
					}
				}
				mouseButtonPoint=Input.mousePosition;//获取鼠标最后位置//
				if(Mathf.Abs(mouseButtonhitPoint.x-mouseButtonDownhitPoint.x)>0.5&&!AnchorPoint.GetComponent(iTween)){
        			iTween.rotateBy(AnchorPoint,{"y":-0.25*Mathf.Sign(mouseButtonPoint.x-mouseDownPoint.x),"time":0.5});
        		}
        		else if(Mathf.Abs(mouseButtonhitPoint.y-mouseButtonDownhitPoint.y)>0.5&&!AnchorPoint.GetComponent(iTween)){
        			iTween.rotateBy(AnchorPoint,{"x":-0.25*Mathf.Sign(mouseButtonPoint.y-mouseDownPoint.y)*collideNormal.z,"time":0.5});
        		}
        		isChangParent=true;
        		isReset=false;
        	}
        }
        if(Mathf.Abs(collideNormal.x-0)>0.5){//左右两面//
        	if((Mathf.Abs(mouseButtonhitPoint.z-mouseButtonDownhitPoint.z)>0.5||Mathf.Abs(mouseButtonhitPoint.y-mouseButtonDownhitPoint.y)>0.5)&&!isChangParent){//拖动鼠标超过0.5//
        		var gos2 : GameObject[];
				gos2 = GameObject.FindGameObjectsWithTag("Cube");
				for (var go2 : GameObject in gos2) {
					if(Mathf.Abs(mouseButtonhitPoint.z-mouseButtonDownhitPoint.z)>0.5){
						if(Mathf.Abs(go2.transform.position.y-rateWasGameObject.transform.position.y)<0.2){
							go2.transform.parent=AnchorPoint.transform;
						}
					}
					else if(Mathf.Abs(mouseButtonhitPoint.y-mouseButtonDownhitPoint.y)>0.5){
						if(Mathf.Abs(go2.transform.position.z-rateWasGameObject.transform.position.z)<0.2){
							go2.transform.parent=AnchorPoint.transform;
						}
					}
				}
				mouseButtonPoint=Input.mousePosition;//获取鼠标最后位置//
				if(Mathf.Abs(mouseButtonhitPoint.z-mouseButtonDownhitPoint.z)>0.5&&!AnchorPoint.GetComponent(iTween)){
        			iTween.rotateBy(AnchorPoint,{"y":-0.25*Mathf.Sign(mouseButtonPoint.x-mouseDownPoint.x),"time":0.5});
        		}
        		else if(Mathf.Abs(mouseButtonhitPoint.y-mouseButtonDownhitPoint.y)>0.5&&!AnchorPoint.GetComponent(iTween)){
        			iTween.rotateBy(AnchorPoint,{"z":0.25*Mathf.Sign(mouseButtonPoint.y-mouseDownPoint.y)*collideNormal.x,"time":0.5});
        		}
        		isChangParent=true;
        		isReset=false;
        	}
        }
        if(Mathf.Abs(collideNormal.y-0)>0.5){//上下两面//
        	if((Mathf.Abs(mouseButtonhitPoint.z-mouseButtonDownhitPoint.z)>0.5||Mathf.Abs(mouseButtonhitPoint.x-mouseButtonDownhitPoint.x)>0.5)&&!isChangParent){//拖动鼠标超过0.5//
        		var gos3 : GameObject[];
				gos3 = GameObject.FindGameObjectsWithTag("Cube");
				for (var go3 : GameObject in gos3) {
					if(Mathf.Abs(mouseButtonhitPoint.z-mouseButtonDownhitPoint.z)>0.5){
						if(Mathf.Abs(go3.transform.position.x-rateWasGameObject.transform.position.x)<0.2){
							go3.transform.parent=AnchorPoint.transform;
						}
					}
					else if(Mathf.Abs(mouseButtonhitPoint.x-mouseButtonDownhitPoint.x)>0.5){
						if(Mathf.Abs(go3.transform.position.z-rateWasGameObject.transform.position.z)<0.2){
							go3.transform.parent=AnchorPoint.transform;
						}
					}
				}
				mouseButtonPoint=Input.mousePosition;//获取鼠标最后位置//
				if(Mathf.Abs(mouseButtonhitPoint.z-mouseButtonDownhitPoint.z)>0.5&&!AnchorPoint.GetComponent(iTween)){
        			iTween.rotateBy(AnchorPoint,{"x":0.25*Mathf.Sign(mouseButtonhitPoint.z-mouseButtonDownhitPoint.z)*collideNormal.y,"time":0.5});
        		}
        		else if(Mathf.Abs(mouseButtonhitPoint.x-mouseButtonDownhitPoint.x)>0.5&&!AnchorPoint.GetComponent(iTween)){
        			iTween.rotateBy(AnchorPoint,{"z":-0.25*Mathf.Sign(mouseButtonhitPoint.x-mouseButtonDownhitPoint.x)*collideNormal.y,"time":0.5});
        		}
        		isChangParent=true;
        		isReset=false;
        	}
        }
	}
}