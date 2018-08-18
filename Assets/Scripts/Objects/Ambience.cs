using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class Ambience
{
	[XmlAttribute("Name")] 
	public string Name;
	[XmlAttribute("TexturesPath")] 
	public string TexPath;

	public Color SunColor;
	public float MaxSunStrength;
	public float SunLightExponent;
	public Color AmbienceSkyColor;
	public Color AmbienceGroundColor;
	public Color AmbienceMinimum;
	public Color SpecularReflectionsColor;
	public Color DarkVegetationColor;
	public Color LightVegetationColor;
	public Vector3 SunLightVector;
	public float SunRotationVector;
	public Vector4 ScreenGamma;
	public int ApplyGammaToSky;
	public Color TextureColorFilter;
	public float TextureColorFilterStrength;
	public float RelativeCarColorFilterStrength;

	/*use_colormap
	6# 2#7 #4 
	0->0 28->9 73->88 120->228 164->255 255->255   # good: 0->0 23->8 68->86 118->204 164->255 255->255 ###0->0 23->8 68->86 118->204 164->255 255->255 #0->0 22->15 48->48 82->169 94->227 125->251 255->255 #0->0 28->2 119->255 255->255    # 43 150 50 150 53 150       # "stretch detail" screen filter  #5 10 13 # additive screen brightness #1	 # use old-school screen brightening?
	0->0 28->9 73->88 120->228 164->255 255->255                   # good: 0->0 23->8 68->86 118->204 164->255 255->255 #0->0 22->15 48->48 82->169 94->227 125->251 255->255 #0->0 28->2 114->255 255->255
	0->0 28->9 73->88 120->228 164->255 255->255                   # good: 0->0 23->8 68->86 118->204 164->255 255->255 #0->0 22->15 48->48 82->169 94->227 125->251 255->255 #0->0 28->2 112->255 255->255	 # fog color for looking into normal scenery
	*/

	public Color FogColorScenery;
	public Vector2 FogAmountScenery;
	public Color FogColorSunHaze;
	public Vector2 FogAmountSunHaze;
	public string EnvironmentSound;
	public Vector3 SunFlareVector;
	public Color SunFlareColor;
	public Vector2 SunCoronaSize;
	public Color ObjectShadowColor;
	public float MaxShadowAlpha;
	//min, max, fresnel
	public Vector3 Reflections;
	public bool TurnOnHeadlights;
	public Color LensflaresColor;
	public Color BackgroundColor;
	public float EnvmappingRotationAngle;
	public bool ForcceMusicOff;
}
