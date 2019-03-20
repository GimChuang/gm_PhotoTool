using UnityEngine;
using UnityEngine.UI;

public class KeyingSilder : MonoBehaviour {

    public Slider slider;
    public Text txt_value;

    [Header("Slider Setting")]
    public float minVal;
    public float maxVal;
    public bool wholeNumber;

    [Header("Keying Material")]
    public Material keyingMaterial;
    public string propertyName;

    [Header("Optional - Default Setting")]
    public float defaultValue;
    public Button btn_resetDefault;

    // Use this to prevent OnValueChanged getting called before PlayerPrefs.GetFloat
    bool isPlayerPrefsLoaded;

    void OnEnable()
    {
        // add a callback function to the slider
        slider.onValueChanged.AddListener(HandleValueChanged);

        // If btn_resetDefault is assigned, add a callback function for resetting value
        if (btn_resetDefault)
            btn_resetDefault.onClick.AddListener(SetToDefaultValue);
    }

    void OnDisable()
    {
        slider.onValueChanged.RemoveAllListeners();

        if (btn_resetDefault)
            btn_resetDefault.onClick.RemoveAllListeners();
    }

    void Start () {

        // Slider settings
        slider.minValue = minVal;
        slider.maxValue = maxVal;
        slider.wholeNumbers = wholeNumber;

        // Load saved value from PlayerPrefs
        slider.value = PlayerPrefs.GetFloat(propertyName);
        isPlayerPrefsLoaded = true;

        // Update text to display value
        if (txt_value)
            txt_value.text = slider.value.ToString();

        // If the keyingMaterial is not null, set float automatically in Start()
        if (keyingMaterial)
            keyingMaterial.SetFloat(propertyName, slider.value);

    }
	
	void Update () {
		
	}

    public void SetToDefaultValue()
    {
        slider.value = defaultValue;
        /*
        // If the keyingMaterial is not null, set float automatically
        if (keyingMaterial)
            keyingMaterial.SetFloat(propertyName, slider.value);

        // Update text to display value
        if (txt_value)
            txt_value.text = slider.value.ToString();

        // Save to playerPrefs
        PlayerPrefs.SetFloat(propertyName, slider.value);
        */
    }

    void HandleValueChanged(float _val)
    {
        if (isPlayerPrefsLoaded == false)
            return;

        if (keyingMaterial)
            keyingMaterial.SetFloat(propertyName, slider.value);

        // Update text to display value
        if (txt_value)
            txt_value.text = _val.ToString();

        // Save to playerPrefs
        PlayerPrefs.SetFloat(propertyName, _val);

    }
}
