using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera _camera;
    private float _lastCameraX;
    private float _cameraHalfWidth;
    [SerializeField] private ParallaxLayer[] parallaxLayers;

    private void Awake()
    {
        _camera = Camera.main;
        _cameraHalfWidth = _camera.orthographicSize * _camera.aspect;
        CalImgLen();
    }

    private void FixedUpdate()
    {
        float currentCameraX = _camera.transform.position.x;
        float distanceToMove = _camera.transform.position.x - _lastCameraX;
        _lastCameraX = currentCameraX;
        
        float cameraEdgeLeft = currentCameraX - _cameraHalfWidth;
        float cameraEdgeRight = currentCameraX + _cameraHalfWidth;

        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.Move(distanceToMove);
            layer.LoopBackground(cameraEdgeLeft, cameraEdgeRight);
        }
    }

    private void CalImgLen()
    {
        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.CalculateImageWidth();
        }
    }
}