﻿using UnityEngine;
using System;

[ExecuteInEditMode]
public class DeferredFogEffect : MonoBehaviour {

	[SerializeField] private Shader _deferredFog;
	
	[NonSerialized] private Material _fogMaterial;
	[NonSerialized] private Camera _deferredCamera;
	[NonSerialized] private Vector3[] _frustumCorners;
	[NonSerialized] private Vector4[] _vectorArray;

	[ImageEffectOpaque]
	private void OnRenderImage (RenderTexture source, RenderTexture destination) {
		if (_fogMaterial == null) {
			_deferredCamera = GetComponent<Camera>();
			_frustumCorners = new Vector3[4];
			_vectorArray = new Vector4[4];
			_fogMaterial = new Material(_deferredFog);
		}
		_deferredCamera.CalculateFrustumCorners(
			new Rect(0f, 0f, 1f, 1f),
			_deferredCamera.farClipPlane,
			_deferredCamera.stereoActiveEye,
			_frustumCorners
		);

		_vectorArray[0] = _frustumCorners[0];
		_vectorArray[1] = _frustumCorners[3];
		_vectorArray[2] = _frustumCorners[1];
		_vectorArray[3] = _frustumCorners[2];
		_fogMaterial.SetVectorArray("_FrustumCorners", _vectorArray);

		Graphics.Blit(source, destination, _fogMaterial);
	}
}