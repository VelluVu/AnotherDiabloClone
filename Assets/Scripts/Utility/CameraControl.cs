using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simppeli Kameraa seuraa vaan pelaajaa..
/// </summary>
public class CameraControl : MonoBehaviour {

    public Transform player;
    bool isShaking;
    public float smoothTime = 0.3F;
    Vector3 velocity = Vector3.zero;

    [Header ( "CameraShake Variables" )]
    public float _duration;
    public float _magnitude;

    private void OnEnable ( )
    {
        Player.playerTakeDamageEvent += ShakeOnImpact;
    }

    private void OnDisable ( )
    {
        Player.playerTakeDamageEvent -= ShakeOnImpact;
    }

    private void LateUpdate ( )
    {
        if ( !isShaking )
        {

            Vector3 targetPosition = new Vector3 ( player.position.x , player.position.y, transform.position.z );
            transform.position = Vector3.SmoothDamp ( transform.position, targetPosition, ref velocity, smoothTime );

        }
    }

    public void ShakeOnImpact( float damage)
    {
        StartCoroutine ( Shake ( _duration, _magnitude ) );
    }

    public IEnumerator Shake ( float duration, float magnitude )
    {

        float elapsed = 0.0f;
        while ( elapsed < duration )
        {
            isShaking = true;
            float x = Random.Range ( -1f, 1f ) * magnitude;
            float y = Random.Range ( -1f, 1f ) * magnitude;

            transform.position = new Vector3 ( transform.position.x + x, transform.position.y + y, transform.position.z );

            elapsed += Time.deltaTime;

            yield return null;
        }
        isShaking = false;

    }
}
