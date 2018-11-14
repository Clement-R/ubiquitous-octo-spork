using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingScore : MonoBehaviour {

    public float speed = 1f;

    private TextMeshPro _textMesh;
    private Transform _player;

    void Start () {
        _textMesh = GetComponent<TextMeshPro>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(RiseAndFade());
    }

    private void Update()
    {
        transform.LookAt(_player.position, Vector3.up);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));
    }

    IEnumerator RiseAndFade()
    {
        float t = Time.time;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + new Vector3(0f, 1f, 0f);
        float completion = 0f;
        while (completion < 1f)
        {
            completion = (Time.time - t) * speed;
            transform.position = Vector3.Lerp(startPosition, endPosition, completion);
            _textMesh.color = new Color(_textMesh.color.r, _textMesh.color.g, _textMesh.color.b, 1 - completion);
            yield return null;
        }

        yield return null;   
    }
}
