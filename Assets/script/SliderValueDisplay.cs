using UnityEngine;
using UnityEngine.UI;

public class SliderValueDisplay : MonoBehaviour
{
    public Slider slider; // Inspector��Slider���A�^�b�`���܂�
    public Text valueText; // Inspector��Text���A�^�b�`���܂�

    void Start()
    {
        // Slider�̏����l��\�����܂�
        DisplaySliderValue(slider.value);

        // Slider�̒l���ύX���ꂽ���̃C�x���g��ݒ肵�܂�
        slider.onValueChanged.AddListener(delegate { DisplaySliderValue(slider.value); });
    }

    // Slider�̒l��\������֐�
    void DisplaySliderValue(float sliderValue)
    {
        valueText.text = "Slider�̒l: " + sliderValue.ToString("F2"); // �����_�ȉ�2���܂ŕ\��
    }
}