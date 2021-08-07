using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaBackground : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 lastCameraPostion;
    private Vector3 deltaMovement;
    private float textureUnitSizeX;
    private float offsetPostionX;

    private Sprite sprite;
    private Texture2D texture;

    [SerializeField]
    public Vector2 parallaxEffectMult;
    

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPostion = cameraTransform.position;
        sprite = GetComponent<SpriteRenderer>().sprite;
        texture = sprite.texture;
        textureUnitSizeX = (texture.width / sprite.pixelsPerUnit) * transform.localScale.x;
        
    }

    private void FixedUpdate()
    {
        deltaMovement = cameraTransform.position - lastCameraPostion;
        transform.position += new Vector3( deltaMovement.x * parallaxEffectMult.x,deltaMovement.y * parallaxEffectMult.y);
        lastCameraPostion = cameraTransform.position;

        if(Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            offsetPostionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPostionX, transform.position.y);

        }
    }
}
