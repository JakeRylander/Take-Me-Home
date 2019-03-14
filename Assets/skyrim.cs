using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class skyrim : MonoBehaviour {
	
	public VideoPlayer player; 
	public RawImage image;

     RenderTexture text;
 
     void Start () {
 
         text = new RenderTexture((int)player.clip.width, (int)player.clip.height, 0);
 
         player.targetTexture = text;
         image.texture = text;
 
         Vector3 scale = image.transform.localScale;
 
         scale.y = player.clip.height / (float)player.clip.width * scale.y;
 
         image.transform.localScale = scale;
 
     }
}
