using UnityEngine;
using System.Collections;

public class CubeTracking : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Tracking()
    {
        PXCMSenseManager sm = PXCMSenseManager.CreateInstance();
        sm.EnableHand();
        PXCMHandModule handModule = sm.QueryHand();
        PXCMHandConfiguration handConfig = handModule.CreateActiveConfiguration();
        //handConfig.EnableJointSpeed(...);
    }
}
