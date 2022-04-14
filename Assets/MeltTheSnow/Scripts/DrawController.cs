using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawController : MonoBehaviour
{
    [SerializeField] private Vector3 _cameraOffset;
    [SerializeField] private float _cameraMoveDamping;
    [SerializeField] private Bounds _cameraBounds;

    [SerializeField] private ParticleSystem _drawerParticleSystem;
    [SerializeField] private Transform _plane;
    

    private Vector3 _cameraTargetPos;
    

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        _cameraTargetPos = Camera.main.transform.position;
    }

    void Update()
    {
        bool mouseDown = Input.GetMouseButton(0);

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(_plane.up, _plane.position);

        if (plane.Raycast(ray, out float enter))
        {
            var point = ray.GetPoint(enter);
            _drawerParticleSystem.transform.position = point;

            if (mouseDown)
            {
                _cameraTargetPos = _cameraBounds.Contains(point) ? point : _cameraBounds.ClosestPoint(point);
            }
        }
        
        float t = Time.deltaTime * _cameraMoveDamping;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, _cameraTargetPos + _cameraOffset, t);

        if (Input.GetMouseButtonDown(0))
        {
            _drawerParticleSystem.Play();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _drawerParticleSystem.Stop();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(_cameraBounds.center, _cameraBounds.size);
    }
}
