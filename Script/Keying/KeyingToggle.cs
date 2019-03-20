using UnityEngine;
using UnityEngine.UI;

public class KeyingToggle : MonoBehaviour {

    public Toggle tgl;

    [Header("Keying Material")]
    public Material keyingMaterial;
    public string propertyName = "_KEYING";

    [Header("Optional - Default Setting")]
    public bool defaultValue;
    public Button btn_resetDefault;

    // Use this to prevent OnValueChanged getting called before PlayerPrefs.GetBool
    bool isPlayerPrefsLoaded;

    void OnEnable()
    {
        tgl.onValueChanged.AddListener(HandleValueChanged);

        // If btn_resetDefault is assigned, add a callback function for resetting value
        if (btn_resetDefault)
            btn_resetDefault.onClick.AddListener(SetToDefaultValue);
    }

    void OnDisable()
    {
        tgl.onValueChanged.RemoveAllListeners();

        if (btn_resetDefault)
            btn_resetDefault.onClick.RemoveAllListeners();
    }

    void Start () {

        // Load saved bool from PlayerPrefs
        if (PlayerPrefs.GetInt(propertyName) == 1)
            tgl.isOn = true;
        else
            tgl.isOn = false;

        isPlayerPrefsLoaded = true;

    }
	
	void Update () {
		
	}

    void HandleValueChanged(bool _isOn)
    {
        if (isPlayerPrefsLoaded == false)
            return;

        if (_isOn)
        {
            keyingMaterial.EnableKeyword(propertyName);
            PlayerPrefs.SetInt(propertyName, 1);
        }           
        else{
            keyingMaterial.DisableKeyword(propertyName);
            PlayerPrefs.SetInt(propertyName, 0);
        }

    }

    public void SetToDefaultValue()
    {
        tgl.isOn = defaultValue;
    }
}
