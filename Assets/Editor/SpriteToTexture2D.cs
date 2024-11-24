using UnityEngine;
using UnityEditor;

public static class SpriteToTexture2D
{
    [MenuItem("Custom/SpriteToTexture2D")]
    public static void ChangeSpriteToTexture2D()
    {
        Sprite sprite = Selection.activeObject as Sprite;
        var targetTex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.ARGB32, false);
        var pixels = sprite.texture.GetPixels(
            (int)sprite.textureRect.x,
            (int)sprite.textureRect.y,
            (int)sprite.textureRect.width,
            (int)sprite.textureRect.height);
        targetTex.SetPixels(pixels);
        targetTex.Apply();
        AssetDatabase.CreateAsset(targetTex, "Assets/Art/Texture/" + sprite.name+".asset");
        AssetDatabase.SaveAssets();
    }
}
