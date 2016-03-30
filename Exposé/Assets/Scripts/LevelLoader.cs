using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour
{
    public string filePath;
    private string line;
    private Dictionary<string, Transform> prefabs;

    // List of possible prefabs
    public Transform player;
    public Transform tree;
    public Transform photoAlbum;

    // Use this for initialization
    void Start()
    {
        prefabs = new Dictionary<string, Transform>();
        prefabs.Add("player", player);
        prefabs.Add("tree", tree);
        prefabs.Add("photoAlbum", player);

        line = "";
        System.IO.StreamReader file = new System.IO.StreamReader(filePath);
        while ((line = file.ReadLine()) != null)
        {
            // think about the breakdown here, for example, if things are put into
            // empty gameObject folders when they are instantiated...
            if(line.StartsWith("Prefab:"))
            {
                // Cut "Prefab:" and split at ";"
                string[] properties = line.Substring(6).Split(';');

                Vector3[] vectors = new Vector3[properties.Length-1];
                for(int i = 0; i < vectors.Length; i++)
                {
                    string[] values = new string[3];
                    if(properties[i+1].StartsWith("Position:"))
                    {
                        // '9' used becasue of 'Position:' is 9 chars
                        values = properties[i+1].Substring(9).Split(',');
                    }
                    else if(properties[i+1].StartsWith("Rotation:"))
                    {
                        // '9' used becasue of 'Rotation:' is 9 chars
                        values = properties[i + 1].Substring(9).Split(',');
                    }
                    else if(properties[i+1].StartsWith("Scale:"))
                    {
                        // '9' used becasue of 'Scale:' is 6 chars
                        values = properties[i + 1].Substring(6).Split(',');
                    }
                    vectors[i] = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
                }
                // Read the colorname and remove leading or trailing spaces
                string prefabName = properties[0].Trim();

                Transform gameObj = Instantiate(prefabs[prefabName],vectors[0],Quaternion.identity) as Transform;
                gameObj.rotation = Quaternion.Euler(vectors[1]);   //new Vector3(vectors[1].x,vectors[1].y,vectors[1].z);
                gameObj.localScale = vectors[2];
            }
        }

        file.Close();
    }

    // Update is called once per frame
    void Update()
    {

    }
}