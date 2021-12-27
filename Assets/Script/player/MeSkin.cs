using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MeSkin : MonoBehaviour
{
    public Material spritesDefault;
    
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        ChangeSkin();
    }

    public void ChangeSkin()
    {
        // Debug.Log("restarted");
        RecordData data = SaveSystem.LoadGameState();
        sr = GetComponent<SpriteRenderer>();
        int s = (int)data.weaponIndex;
        //s = 1;
        switch (s)
        {
            case 1:
                sr.sprite = Resources.Load<Sprite>("Skins/Skin1/skin1");
                break;
            case 2:
                sr.sprite = Resources.Load<Sprite>("Skins/Skin2/skin2");
                break;

            default:
                sr.sprite = Resources.Load<Sprite>("Sprite/Me");
                break;
        }
        if (GraphicsSettings.renderPipelineAsset == null)
        {
            //lower setting
            sr.material = spritesDefault;

        }
        else
        {
            //hyper setting
            switch (s)
            {
                case 1:
                    sr.material = Resources.Load<Material>("Skins/Skin1/Shader_Graphs_skin1_shader");
                    break;

                default:
                    sr.material = spritesDefault;
                    break;
            }


        }
    }
    
}
