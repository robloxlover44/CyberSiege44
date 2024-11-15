﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace TDTK {

	public class TDE {
		
		public static bool IsPrefabOrPrefabInstance(GameObject obj){
			PrefabType type=PrefabUtility.GetPrefabType(obj);
			return type==PrefabType.Prefab || type==PrefabType.PrefabInstance;
		}
		public static bool IsPrefab(GameObject obj){
			return obj==null ? false : PrefabUtility.GetPrefabType(obj)==PrefabType.Prefab;
		}
		
		
		
		//~ public static int GetTowerIndex(int prefabID){
			//~ for(int i=0; i<towerDB.towerList.Count; i++){
				//~ if(towerDB.towerList[i].prefabID==prefabID) return (i+1);
			//~ }
			//~ return 0;
		//~ }
		//~ public static int GetCreepIndex(int prefabID){
			//~ for(int i=0; i<creepDB.creepList.Count; i++){
				//~ if(creepDB.creepList[i].prefabID==prefabID) return (i+1);
			//~ }
			//~ return 0;
		//~ }
		//~ public static int GetAbilityIndex(int prefabID){
			//~ for(int i=0; i<abilityDB.abilityList.Count; i++){
				//~ if(abilityDB.abilityList[i].prefabID==prefabID) return (i+1);
			//~ }
			//~ return 0;
		//~ }
		//~ public static int GetPerkIndex(int prefabID){
			//~ for(int i=0; i<perkDB.perkList.Count; i++){
				//~ if(perkDB.perkList[i].prefabID==prefabID) return (i+1);
			//~ }
			//~ return 0;
		//~ }
		
		
		
		public static DamageTableDB damageTableDB;
		public static RscDB rscDB;
		public static TowerDB towerDB;
		public static CreepDB creepDB;
		public static AbilityDB abilityDB;
		public static PerkDB perkDB;
		public static EffectDB effectDB;
		
		public static GUIStyle headerS;
		public static GUIStyle foldoutS;
		public static GUIStyle conflictS;
		
		private static bool init=false;
		public static void Init(){
			if(init) return;
			
			init=true;	//Debug.Log(" - Init Editor - ");
			
			damageTableDB=DamageTableDB.Init();
			rscDB=RscDB.Init();
			towerDB=TowerDB.Init();
			creepDB=CreepDB.Init();
			abilityDB=AbilityDB.Init();
			perkDB=PerkDB.Init();
			effectDB=EffectDB.Init();
			
			DamageTableDB.UpdateLabel();
			TowerDB.UpdateLabel();
			CreepDB.UpdateLabel();
			AbilityDB.UpdateLabel();
			PerkDB.UpdateLabel();
			EffectDB.UpdateLabel();
		}
		
		private static bool initUIStyle=false;
		public static void InitGUIStyle(){
			if(initUIStyle) return;
			
			initUIStyle=true;
			
			headerS=new GUIStyle("Label");
			headerS.fontStyle=FontStyle.Bold;
			
			foldoutS=new GUIStyle("foldout");
			foldoutS.fontStyle=FontStyle.Bold;
			foldoutS.normal.textColor = Color.black;
			
			conflictS=new GUIStyle("Label");
			conflictS.normal.textColor = Color.red;
		}
		
		
		public static GUIContent[] SetupContL(string[] label, string[] tooltip){
			GUIContent[] contL=new GUIContent[label.Length];
			for(int i=0; i<contL.Length; i++) contL[i]=new GUIContent(label[i], tooltip[i]);
			return contL;
		}
		
		public static void Label(float x, float y, float width, float height, string lb, string tooltip="", GUIStyle style=null){
			if(style==null) EditorGUI.LabelField(new Rect(x, y, width, height), new GUIContent(lb, tooltip));
			else EditorGUI.LabelField(new Rect(x, y, width, height), new GUIContent(lb, tooltip), style);
		}
		
		
		public static int GenerateNewID(List<int> list, int ID=0){
			while(list.Contains(ID)) ID+=1;
			return ID;
		}
		
		
		public static string[] GetArmorLabel(){ return DamageTableDB.armorlb; }
		public static string[] GetDamageLabel(){ return DamageTableDB.damagelb; }
		
		
		public static void SetDirty(){ 
			EditorUtility.SetDirty(rscDB);
			EditorUtility.SetDirty(damageTableDB);
			EditorUtility.SetDirty(towerDB);
			EditorUtility.SetDirty(creepDB);
			//~ EditorUtility.SetDirty(fpsWeaponDB);
			EditorUtility.SetDirty(abilityDB);
			EditorUtility.SetDirty(perkDB);
			EditorUtility.SetDirty(effectDB);
			
			for(int i=0; i<towerDB.towerList.Count; i++) EditorUtility.SetDirty(towerDB.towerList[i]);
			for(int i=0; i<creepDB.creepList.Count; i++) EditorUtility.SetDirty(creepDB.creepList[i]);
			//~ for(int i=0; i<fpsWeaponDB.weaponList.Count; i++) EditorUtility.SetDirty(fpsWeaponDB.weaponList[i]);
		}
		
		
		
		//static int spaceX=120; static int spaceY=18; static int width=150; static int widthS=40; static int height=16;	
		public static float DrawBasicInfo(float startX, float startY, TDItem item){
			int spaceX=120; int spaceY=18; int width=150; int height=16;
			TDE.DrawSprite(new Rect(startX, startY, 60, 60), item.icon);	startX+=65;
			
			TDE.Label(startX, startY+=5, width, height, "Name:", "The item name to be displayed in game");
			item.name=EditorGUI.TextField(new Rect(startX+spaceX-65, startY, width, height), item.name);
			
			TDE.Label(startX, startY+=spaceY, width, height, "Icon:", "The item icon to be displayed in game, must be a sprite");
			item.icon=(Sprite)EditorGUI.ObjectField(new Rect(startX+spaceX-65, startY, width, height), item.icon, typeof(Sprite), false);
			
			TDE.Label(startX, startY+=spaceY, width, height, "PrefabID: "+item.prefabID.ToString());
			//TDE.Label(startX+spaceX-65, startY, width, height, item.prefabID.ToString(), "");
			
			return startY+spaceY;
		}
		public static float DrawBasicInfo(float startX, float startY, Unit unit){
			int spaceX=120; int spaceY=18; int width=150; int height=16;
			TDE.DrawSprite(new Rect(startX, startY, 60, 60), unit.icon);	startX+=65;
			
			TDE.Label(startX, startY+=5, width, height, "Name:", "The item name to be displayed in game");
			unit.unitName=EditorGUI.TextField(new Rect(startX+spaceX-65, startY, width, height), unit.unitName);
			
			TDE.Label(startX, startY+=spaceY, width, height, "Icon:", "The item icon to be displayed in game, must be a sprite");
			unit.icon=(Sprite)EditorGUI.ObjectField(new Rect(startX+spaceX-65, startY, width, height), unit.icon, typeof(Sprite), false);
			
			TDE.Label(startX, startY+=spaceY, width, height, "Prefab:", "The prefab object of the unit\nClick this to highlight it in the ProjectTab");
			EditorGUI.ObjectField(new Rect(startX+spaceX-65, startY, width, height), unit.gameObject, typeof(GameObject), false);
			
			return startY+spaceY*2;
		}
		
		
		
		
		
		
		
		
		
		public static bool DrawSprite(Rect rect, Sprite sprite, string tooltip="", bool drawBox=true){
			if(drawBox) GUI.Box(rect, new GUIContent("", tooltip));
			
			if(sprite!=null){
				Texture t = sprite.texture;
				Rect tr = sprite.textureRect;
				Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height );
				
				rect.x+=2;
				rect.y+=2;
				rect.width-=4;
				rect.height-=4;
				GUI.DrawTextureWithTexCoords(rect, t, r);
			}
			
			//if(addXButton){
			//	rect.width=12;	rect.height=12;
			//	bool flag=GUI.Button(rect, "X", GetXButtonStyle());
			//	return flag;
			//}
			
			return false;
		}
		
		//a guiStyle used to draw the button to delete sprite icon on TowerDB, CreepDB and ResourceDB Editor
		//private static GUIStyle xButtonStyle;
		//public static GUIStyle GetXButtonStyle(){
		//	if(xButtonStyle==null){
		//		xButtonStyle=new GUIStyle("Button");
		//		xButtonStyle.alignment=TextAnchor.MiddleCenter;
		//		xButtonStyle.padding=new RectOffset(0, 0, 0, 0);
		//	}
		//	return xButtonStyle;
		//}
		
	}

}