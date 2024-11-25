using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class TilemapDebugger : EditorWindow
{
    private Tilemap tilemap;
    private TileBase tileToReplace;
    private Vector3Int tilePosition = Vector3Int.zero;

    [MenuItem("Tools/Tilemap Debugger")]
    public static void ShowWindow()
    {
        GetWindow<TilemapDebugger>("Tilemap Debugger");
    }

    private void OnGUI()
    {
        GUILayout.Label("Tilemap Debugger", EditorStyles.boldLabel);
        
        tilemap = (Tilemap)EditorGUILayout.ObjectField("Target Tilemap", tilemap, typeof(Tilemap), true);
        
        tileToReplace = (TileBase)EditorGUILayout.ObjectField("Tile to Replace", tileToReplace, typeof(TileBase), false);
        
        GUILayout.Space(10);
        GUILayout.Label("Replace Single Tile", EditorStyles.boldLabel);

        tilePosition = EditorGUILayout.Vector3IntField("Tile Position", tilePosition);

        if (GUILayout.Button("Replace Tile at Position"))
        {
            if (tilemap != null && tileToReplace != null)
            {
                TileUpdater tileUpdater = new TileUpdater
                {
                    tilemap = tilemap,
                    updatedTile = tileToReplace
                };

                tileUpdater.UpdateTile(tilePosition);
            }
            else
            {
                Debug.LogWarning("Please assign both a Tilemap and a Tile to Replace!");
            }
        }
        
        GUILayout.Space(10);
        GUILayout.Label("Replace All Tiles", EditorStyles.boldLabel);

        if (GUILayout.Button("Replace All Tiles"))
        {
            if (tilemap != null && tileToReplace != null)
            {
                TileUpdater tileUpdater = new TileUpdater
                {
                    tilemap = tilemap,
                    updatedTile = tileToReplace
                };

                tileUpdater.UpdateAllTiles();
            }
            else
            {
                Debug.LogWarning("Please assign both a Tilemap and a Tile to Replace!");
            }
        }
    }
}
