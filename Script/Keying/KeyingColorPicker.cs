using UnityEngine;
using UnityEngine.UI;
using Klak.Video;

public class KeyingColorPicker : MonoBehaviour {

    public ColorPicker picker;

    [Header("Keying Material")]
    public Material keyingMaterial;
    //public string propertyName = "_KeyCgCo";

    [Header("Optional - Default Setting")]
    public Color defaultColor = Color.green;
    public Button btn_resetDefault;

    // Use this to prevent OnValueChanged getting called before PlayerPrefsX.GetColor
    bool isPlayerPrefsLoaded;

    void OnEnable()
    {
        // Set up listener for the picker
        picker.onValueChanged.AddListener(color =>
        {
            HandleValueChanged(color);
        }
        );

        // If btn_resetDefault is assigned, add a callback function for resetting value
        if (btn_resetDefault)
            btn_resetDefault.onClick.AddListener(SetToDefaultValue);
    }

    void OnDisable()
    {
        picker.onValueChanged.RemoveAllListeners();

        if (btn_resetDefault)
            btn_resetDefault.onClick.RemoveAllListeners();
    }

    void Start () {

        // Load saved color
        Color savedColor = PlayerPrefsX.GetColor("_KeyCgCo");
        isPlayerPrefsLoaded = true;

        picker.CurrentColor = savedColor;

    }
	
	void Update () {
		
	}

    void HandleValueChanged(Color _color)
    {
        if (isPlayerPrefsLoaded == false)
            return;

        var ycgco = ProcAmp.RGB2YCgCo(_color);
        // Set Key color to the material
        keyingMaterial.SetVector("_KeyCgCo", new Vector2(ycgco.y, ycgco.z));
        // Save color with PlayerPrefsX
        PlayerPrefsX.SetColor("_KeyCgCo", _color);
        //Debug.Log("Picker OnValueChanged");
    }

    public void SetToDefaultValue()
    {
        picker.CurrentColor = defaultColor;
    }
}
