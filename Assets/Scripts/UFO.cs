using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UFO : MonoBehaviour
{
    public Rigidbody LeftLegUFORigidbody;
    public Rigidbody RightLegUFORigidbody;
    public float speed = 15f;
    public float rotationMultiplier = 0.8f;
    public float forceExplosion;
    public ParticleSystem explosionParticle;
    public float maxHighFly = 55f;

    public FonMusic fonMusicPrefab;
    public AudioSource[] audioSources;


    bool isDeath;

    Rigidbody[] allRigidbodyUFO;
    Collider[] allCollidersUFO;

    private void Awake()
    {
        // Создание объекта фоновой музыки
        if (FonMusic.instance == null) Instantiate(fonMusicPrefab);
    }
    void Start()
    {

        // поиск физики для разрыва на куски
        allRigidbodyUFO = GetComponentsInChildren<Rigidbody>(); 
        allCollidersUFO = GetComponentsInChildren<Collider>();

        audioSources = GetComponents<AudioSource>();

        // Отключение коллайдеров у дочерних объектов
        SwitchEnabledColliders(); 
        isDeath = false;
        InterfaceController.instance.SetMaxHigh(maxHighFly);
    }

    private void Update()
    {
        // Выход
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        // Самоподрыв
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDeath) Explosion();

    }
    void FixedUpdate()
    {

        // вектора направления и силы
        Vector3 minForce = Vector3.up * speed * rotationMultiplier;
        Vector3 maxForce = Vector3.up * speed;

        // Силы двигателей
        Vector3 leftForce = Vector3.zero;
        Vector3 rightForce = Vector3.zero;

        // Движение НЛО, проверка состояния и высоты
        if (!isDeath && transform.position.y < maxHighFly)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                leftForce = maxForce;
                rightForce = maxForce;
            }
            if (Input.GetKey(KeyCode.A))
            {
                leftForce = minForce;
                rightForce = maxForce;
            }
            if (Input.GetKey(KeyCode.D))
            {
                leftForce = maxForce;
                rightForce = minForce;
            }

            LeftLegUFORigidbody.AddRelativeForce(leftForce);
            RightLegUFORigidbody.AddRelativeForce(rightForce);

        }

        // Звук движения
        if (leftForce.y + rightForce.y > 0 && !audioSources[0].isPlaying) audioSources[0].Play();
        else if (leftForce.y + rightForce.y == 0) audioSources[0].Stop();

        // Передача данных интерфейсу
        InterfaceController.instance.FillSliders(leftForce.y, rightForce.y, transform.position.y);
        InterfaceController.instance.ChangeAngleArrow(transform.rotation);

    }

    private void OnCollisionEnter(Collision collision)
    {
        // Столкновение с препятствием
        if (collision.gameObject.CompareTag("Enemy") && !isDeath) Explosion();

        // Прохождение уровня
        if (collision.gameObject.CompareTag("Friend") && !isDeath) SceneLoader.instance.LoadNextScene();
    }

    // Взрыв НЛО
    void Explosion ()
    {

        isDeath = true;
        audioSources[1].Play();
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        explosionParticle.Play();

        // Включение физики
        foreach (var i in allRigidbodyUFO)
        {
            i.isKinematic = false;
        }
        SwitchEnabledColliders();

        // рестарт через 3 секунды
        StartCoroutine(RestartLevel());
    }

    // Включает и выключает коллайдеры НЛО
    void SwitchEnabledColliders ()
    {
        foreach (var i in allCollidersUFO)
        {
            if (i != allCollidersUFO[0]) i.enabled = !i.enabled;
        }
    }

    /// <summary>
    /// Рестарт сцены через 3 секунды
    /// </summary>
    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(3f);
        SceneLoader.instance.RestartScene();
    }
}
