using UnityEngine;
using System.Collections;

public class MyAnimation {
	
	private float AnimSpeed;
	private float timeTillSwap;
	private int curAnimFrame;
	private Texture2D[] textures;
	private Renderer renderer;
	private bool paused;
	
	public MyAnimation( Renderer toRender, Texture2D[] inTextures, float inAnimSpeed )
	{
		AnimSpeed = inAnimSpeed;
		timeTillSwap = inAnimSpeed;
		curAnimFrame = 0;
		textures = inTextures;
		renderer = toRender;
		renderer.material.mainTexture = textures[curAnimFrame];
		paused = true;
	}
	
	//just make it work
	public void Update()
	{
		if(paused)
		{
			return;
		}
		timeTillSwap -= Time.deltaTime*1000.0f;
		if(timeTillSwap < 0)
		{
			curAnimFrame++;
			if(curAnimFrame >= textures.Length)
			{
				curAnimFrame = 0;
			}
			timeTillSwap += AnimSpeed;
			renderer.material.mainTexture = textures[curAnimFrame];
		}
	}
	
	public void Pause()
	{
		paused = true;
	}
	
	public void Play()
	{
		paused = false;
		renderer.material.mainTexture = textures[curAnimFrame];
	}
}
