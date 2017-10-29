using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DreamStateTarget
{
	None=0,
	Vase,
	Mug,
	Teapot,
}

public enum DreamStateContext
{
	None=0,
	VaseCar,
	TeapotCar,
	MugCar,
	VaseBuilding,
	TeapotBuilding,
	MugBuilding,
}

public enum DreamStateDream
{
	None=0,
	Building,
	//Shoe,
	//Hat,
	//Dress,
	IPad,
	Car,
}

public enum DreamStateMaterial
{
	None=0,
	Bricks,
	Fur,
	IconArray,
	Orange,
	FineWeave,
	Marble,
	Moss,
}

	
public class DreamState
{
	static public DreamStateTarget Target { get; set; }
	static public DreamStateDream Dream{ get; set; }
	static public DreamStateMaterial Material { get; set; }
}

public class PassiveDreaming : MonoBehaviour 
{
	//static DateTime s_LastIteration = new DateTime(0);
	static Dictionary<DreamStateTarget, DateTime> s_LastTargetTime=new Dictionary<DreamStateTarget, DateTime>();
	static TimeSpan s_PassiveTimeOffset = new TimeSpan(0,0,0,2);
	static long s_PassiveTimeIncrement = new TimeSpan(0,0,6).Ticks;
	static Dictionary<DreamStateTarget, GameObject> s_mTargets = new Dictionary<DreamStateTarget, GameObject>();
	static Dictionary<string, GameObject> s_mContext = new Dictionary<string, GameObject>();


	//static DreamStateTarget s_PriorTarget = DreamStateTarget.None;
	//static Shader s_SolidShader=null;

	// Use this for initialization
	void Start () 
	{
		/*
		const string ShaderName="Standard";
		s_SolidShader=Shader.Find(ShaderName); 
		if (null==s_SolidShader)
			print("Solid Shader not found.");
		*/
		
		DreamState.Target=DreamStateTarget.None; 
		DreamState.Dream=DreamStateDream.None;
		DreamState.Material=DreamStateMaterial.None;

		// cache the targets
		foreach (DreamStateTarget target in Enum.GetValues(typeof(DreamStateTarget)))
		{
			string strTarget=target.ToString();
			if(0==strTarget.CompareTo(DreamStateTarget.None.ToString()))
				continue;
			GameObject go = GameObject.Find(strTarget);
			s_mTargets[target]=go;
			s_mTargets[target].SetActive(false); 
			s_LastTargetTime[target]= DateTime.Now + new TimeSpan(((int)target) * s_PassiveTimeOffset.Ticks);
		}
		// cache the context GOs
		foreach (DreamStateContext context in Enum.GetValues(typeof(DreamStateContext)))
		{
			string strTarget=context.ToString(); 
			if(0==strTarget.CompareTo(DreamStateTarget.None.ToString()))
				continue;
			GameObject go = GameObject.Find(strTarget);
			s_mContext[context.ToString()]=go;
			s_mContext[context.ToString()].SetActive(false); 
		}
	}

	void IncrementRandomly(DreamStateTarget target)
	{
		int nStart=(int)s_PassiveTimeIncrement;
		int nEnd=(int)(1.6f*s_PassiveTimeIncrement);
		long nRandom=UnityEngine.Random.Range(nStart,nEnd); 
		if(s_LastTargetTime[target].Ticks + nRandom < DateTime.Now.Ticks)
		{
			//int nDST=Enum.GetNames(typeof(DreamStateTarget)).Length; 
			int nDSD=Enum.GetNames(typeof(DreamStateDream)).Length; 
			int nDSM=Enum.GetNames(typeof(DreamStateMaterial)).Length;  
			DreamState.Dream=(DreamStateDream)UnityEngine.Random.Range(1,nDSD);
			DreamState.Material=(DreamStateMaterial)UnityEngine.Random.Range(1,nDSM);
			s_LastTargetTime[target]=DateTime.Now;
			string strMaterial=DreamState.Material.ToString();
			//print("Updated "+target.ToString()+" to dream: " + DreamState.Dream.ToString() + " with material: "+ strMaterial);
			string strMaterialPath= "Materials/"+strMaterial;
			Material material = Resources.Load(strMaterialPath, typeof(Material)) as Material; 
			GameObject targetGo=s_mTargets[target];
			MeshRenderer meshRenderer = targetGo.GetComponent<MeshRenderer>();
			meshRenderer.material=material;
			meshRenderer.shadowCastingMode= UnityEngine.Rendering.ShadowCastingMode.On;
			meshRenderer.receiveShadows=true;
			targetGo.SetActive(true);

			// hide all contexts
			foreach(DreamStateDream dream in Enum.GetValues(typeof(DreamStateDream)))
			{
				string strKey = target.ToString() + dream.ToString();
				if(s_mContext.ContainsKey(strKey))
					s_mContext[strKey].SetActive(false); 
			}

			// toggle dream's context
			string strNewContextKey = target.ToString() + DreamState.Dream.ToString();
			if (s_mContext.ContainsKey(strNewContextKey))
			{
				GameObject contextGo=s_mContext[strNewContextKey];
				contextGo.SetActive(true);
				print("Updated "+target.ToString()+" to context: " + strNewContextKey );
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
		IncrementRandomly(DreamStateTarget.Vase);
		IncrementRandomly(DreamStateTarget.Mug);
		IncrementRandomly(DreamStateTarget.Teapot);
	}
}

public class ActiveDreaming : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{

	}

	// Update is called once per frame
	void Update () 
	{

	}
}

