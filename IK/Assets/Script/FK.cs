using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FK : MonoBehaviour {
    public Bone StartBone;
    public Bone EndBone;
    public Bone[] Bones;

	// Use this for initialization
	void Start () {
		
	}
	
    [ExecuteInEditMode]
	// Update is called once per frame
	void Update () {
        this.transform.position = EndBone.transform.position;
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(StartBone.transform.position, EndBone.transform.position);

    }
    private void OnDrawGizmos()
    {
        float axisLength = 3;
        Color color;
        ColorUtility.TryParseHtmlString("#66CCFF", out color);
        Gizmos.color = color;
        Gizmos.DrawLine(transform.position + Vector3.left * axisLength, transform.position + Vector3.right * axisLength);
        Gizmos.DrawLine(transform.position + Vector3.forward * axisLength, transform.position + Vector3.back * axisLength);
        Gizmos.DrawLine(transform.position + Vector3.up * axisLength, transform.position + Vector3.down * axisLength);
    }
}
