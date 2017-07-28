using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public UnitController UC
    {
        get { return uc; }
        set
        {
            uc = value;
            Reset();
        }
    }
    private UnitController uc;
    private Transform _transform;
    // Use this for initialization
    void Reset()
    {
        if (UC != null)
            _transform = UC.transform;
    }
    void FixedUpdate()
    {
        if (UC != null)
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(_transform.position.x, _transform.position.y, -10), Time.deltaTime * 3f);
    }
}
