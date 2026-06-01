using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.Animation;

public enum Duck_Type
{
    duck = 1,
    duck_king = 2
}

public class Duck : MonoBehaviour
{
    public AudioClip soundFall;
    public AudioClip soundBumb;
    public AudioClip soundTalk;
    public GameObject white_circle;

    public Duck_Type type;
    public int score;
    public int hp = 1;
    public float speed = 6;

    bool isLeft = false;
    int status = 0;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
        speed = Random.Range(speed, speed + 5);
    }

    void Update()
    {
        Vector3 pos = transform.localPosition;

        if (status == 0)
        {
            float y_ = Time.deltaTime * 3;

            if (pos.y > 3)
            {
                y_ = -Time.deltaTime * 3;
            }

            if (transform.localPosition.x < 12 && !isLeft)
            {
                transform.localPosition =
                    new Vector3(pos.x + Time.deltaTime * speed,
                                pos.y + y_,
                                pos.z);

                transform.localScale =
                    new Vector3(
                        Mathf.Abs(originalScale.x),
                        originalScale.y,
                        originalScale.z);

                isLeft = false;
            }
            else if (transform.localPosition.x > 12 && !isLeft)
            {
                randomFly();
            }
            else if (transform.localPosition.x > -12 && isLeft)
            {
                transform.localPosition =
                    new Vector3(pos.x - Time.deltaTime * speed,
                                pos.y + y_,
                                pos.z);

                transform.localScale =
                    new Vector3(
                        -Mathf.Abs(originalScale.x),
                        originalScale.y,
                        originalScale.z);

                isLeft = true;
            }
            else if (transform.localPosition.x < -12 && isLeft)
            {
                randomFly();
            }
        }
        else if (status == 2)
        {
            if (pos.y > -8)
            {
                transform.localPosition =
                    new Vector3(
                        pos.x,
                        pos.y - Time.deltaTime * speed * 1.5f,
                        pos.z);
            }
            else
            {
                status = 3;

                DisableSpriteSkins();

                gameObject.SetActive(false);
            }
        }
    }

    private void DisableSpriteSkins()
    {
        SpriteSkin[] skins = GetComponentsInChildren<SpriteSkin>(true);

        foreach (SpriteSkin skin in skins)
        {
            Destroy(skin);
        }
    }

    public void randomFly()
    {
        if (Random.Range(0, 10) > 5)
        {
            GetComponent<AudioSource>().clip = soundTalk;
            GetComponent<AudioSource>().Play();
        }

        if (Random.Range(0, 10) > 5)
        {
            transform.localPosition =
                new Vector3(
                    -15,
                    Random.Range(-5, 2),
                    0);

            isLeft = false;
        }
        else
        {
            transform.localPosition =
                new Vector3(
                    15,
                    Random.Range(-5, 2),
                    0);

            isLeft = true;
        }
    }

    public void fall()
    {
        status = 2;

        GetComponent<AudioSource>().clip = soundFall;
        GetComponent<AudioSource>().Play();

        GetComponent<Animator>().Play("fall");
    }

    public void endHit()
    {
        Debug.Log("END HIT");

        GetComponent<Animator>().Play("fall");

        if (hp <= 0)
        {
            fall();
        }
        else
        {
            status = 0;
            GetComponent<Animator>().Play("fly");
        }
    }

    public void onDamge(int damage)
    {
        GetComponent<AudioSource>().clip = soundBumb;
        GetComponent<AudioSource>().Play();

        status = 1;
        hp -= damage;

        GetComponent<Animator>().Play("hit");
    }
}