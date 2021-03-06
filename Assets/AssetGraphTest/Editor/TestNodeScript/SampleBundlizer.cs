using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Collections.Generic;

public class SampleBundlizer : AssetGraph.BundlizerBase {
	public override void In (string groupkey, List<AssetGraph.AssetInfo> source, string recommendedBundleOutputDir) {

		var textureAssetPath = source[0].assetPath;
		var textureAssetType = source[0].assetType;

		var mainResourceTexture = AssetDatabase.LoadAssetAtPath(textureAssetPath, textureAssetType) as Texture2D;

		if (mainResourceTexture) Debug.Log("SampleBundlizer_0:loaded:" + textureAssetPath);
		else Debug.LogError("SampleBundlizer_0:failed to load:" + textureAssetPath);

		// load other resources.
		var subResources = new List<UnityEngine.Object>();

		uint crc = 0;
		/**
			generate AssetBundle for iOS
		*/
		var targetPath = Path.Combine(recommendedBundleOutputDir, "bundle.assetbundle");
		
		try {
			BuildPipeline.BuildAssetBundle(
				mainResourceTexture,
				subResources.ToArray(),
				targetPath,
				out crc,
				BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,
				BuildTarget.iOS
			);
		} catch (Exception e) {
			Debug.Log("SampleBundlizer_0:e:" + e);
		}

		if (File.Exists(targetPath)) {
			Debug.Log("SampleBundlizer_0:generated recommendedBundleOutputDir:" + targetPath);
		} else {
			Debug.LogError("SampleBundlizer_0:asset bundle was not generated! recommendedBundleOutputDir:" + targetPath);
		}

	}
}