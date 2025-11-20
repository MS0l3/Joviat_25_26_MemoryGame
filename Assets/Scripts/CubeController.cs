using UnityEngine;
using UnityEngine.UI;

public class CubeController : MonoBehaviour
{
    public MeshRenderer cubeRenderer; // Assigna el MeshRenderer del cub
    public Toggle toggle;             // Assigna el Toggle
    public Slider slider;             // Assigna el Slider

    private void Start()
    {
        // Assegurem que el toggle comenci sincronitzat amb l'estat del renderer
        toggle.isOn = cubeRenderer.enabled;

        // Afegim listeners als components UI
        toggle.onValueChanged.AddListener(OnToggleChanged);
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        // Mostra o amaga el cub
        cubeRenderer.enabled = isOn;
    }

    private void OnSliderChanged(float value)
    {
        // Fa rotar el cub segons el valor del slider (0 a 360 graus)
        cubeRenderer.transform.rotation = Quaternion.Euler(0, value * 360f, 0);
    }
}