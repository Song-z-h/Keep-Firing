using UnityEngine;
using UnityEngine.Rendering;

public class TogglePipelineSettings : MonoBehaviour
{
    public RenderPipelineAsset exampleAssetA;
    RecordData data;

    void Start()
    {
         data = SaveSystem.LoadGameState();
        if (data.isEvilMode)
        {
            //evil mode
            GraphicsSettings.renderPipelineAsset = exampleAssetA;
        }
        else
        {
            //normal mode
            GraphicsSettings.renderPipelineAsset = null;
        }
    }
    
    public void ChangePipelineSettings()
    {
        data = SaveSystem.LoadGameState();
        if (GraphicsSettings.renderPipelineAsset == null)
        {
            //evil settings
            data.isEvilMode = true;
            GraphicsSettings.renderPipelineAsset = exampleAssetA;
        }
        else
        {
            data.isEvilMode = false;
            GraphicsSettings.renderPipelineAsset = null;
        }
        SaveSystem.SaveGameState(data);
        //Debug.Log(GraphicsSettings.renderPipelineAsset);
    }
}
