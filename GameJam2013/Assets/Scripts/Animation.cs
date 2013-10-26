using UnityEngine;
using System.Collections;

public class MyAnimation {
	
	private float AnimSpeed;
	private float timeTillSwap;
	private int curAnimFrame;
	private Texture2D[] textures;
	private Renderer renderer;
	private bool paused;
	private bool loop;
	public bool done;
	public int reverse;
	
	public MyAnimation( Renderer toRender, Texture2D[] inTextures, float inAnimSpeed, bool inLoop )
	{
		AnimSpeed = inAnimSpeed;
		timeTillSwap = inAnimSpeed;
		curAnimFrame = 0;
		textures = inTextures;
		renderer = toRender;
		renderer.material.mainTexture = textures[curAnimFrame];
		paused = true;
		loop = inLoop;
		reverse = 1;
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
			curAnimFrame += 1*reverse;
			if((curAnimFrame >= textures.Length) || (curAnimFrame < 0))
			{
				if(loop)
				{
					curAnimFrame -= textures.Length*reverse;
				}
				else
				{
					//undo last
					curAnimFrame -= 1*reverse;
					this.done = true;
					this.paused = true;
				}
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
		reverse = 1;
		curAnimFrame = 0;
		renderer.material.mainTexture = textures[curAnimFrame];
	}
	
	public void PlayReverse()
	{
		paused = false;
		reverse = -1;
		curAnimFrame = textures.Length-1;
		renderer.material.mainTexture = textures[curAnimFrame];
	}
}
