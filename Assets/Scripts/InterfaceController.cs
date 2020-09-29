using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour
{
    public static InterfaceController instance;

    public Slider highMetrSlider;
    public Slider rightSlider;
    public Slider leftSlider;
    public Text rightText;
    public Text leftText;
    public Text highMetrText;

    public Image imageAngleArrow;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Устанавливает значение слайдеров интерфейса
    /// </summary>
    /// <param name="leftForce">Значение мощности левого двигателя</param>
    /// <param name="rightForce">Значение мощности правого двигателя</param>
    /// <param name="high">Высота объекта</param>
    public void FillSliders (float leftForce, float rightForce, float high)
    {
        leftSlider.value = leftForce;
        rightSlider.value = rightForce;
        highMetrSlider.value = high;
        leftText.text = $"{leftForce} Вт";
        rightText.text = $"{rightForce} Вт";
        highMetrText.text = $"{high:#.#} м";
    }

    /// <summary>
    /// Изменение стрелки измерителя угла наклона НЛО
    /// </summary>
    /// <param name="angleUFO">Угол наклона НЛО</param>
    public void ChangeAngleArrow(Quaternion angleUFO)
    {
        imageAngleArrow.transform.rotation = angleUFO;
    }

    /// <summary>
    /// Максимальная высота полета НЛО устанавливается в датчик
    /// </summary>
    /// <param name="maxHigh">Максимальная высота</param>
    public void SetMaxHigh(float maxHigh)
    {
        highMetrSlider.maxValue = maxHigh;
    }

}
