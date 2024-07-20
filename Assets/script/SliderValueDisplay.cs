using UnityEngine;
using UnityEngine.UI;

public class SliderValueDisplay : MonoBehaviour
{
    public Slider slider; // InspectorでSliderをアタッチします
    public Text valueText; // InspectorでTextをアタッチします

    void Start()
    {
        // Sliderの初期値を表示します
        DisplaySliderValue(slider.value);

        // Sliderの値が変更された時のイベントを設定します
        slider.onValueChanged.AddListener(delegate { DisplaySliderValue(slider.value); });
    }

    // Sliderの値を表示する関数
    void DisplaySliderValue(float sliderValue)
    {
        valueText.text = "Sliderの値: " + sliderValue.ToString("F2"); // 小数点以下2桁まで表示
    }
}