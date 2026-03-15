using System.Collections;
using System.Net.NetworkInformation;
using UnityEditor.SceneManagement;
using UnityEngine;

public class RingAttack : MonoBehaviour
{
    private int stonesRemaining = 0;

    [Header("Ring Settings")]
    public GameObject stonePrefab;
    public float radius = 5f;
    public int stoneCount = 12; 
    public float stayDuration = 1f; // How long stones stay after jutting up
    public float collisionWindowAfterJut = 0.2f;
    public float damage = 12f;

    [Header("Randomization")]
    public float minScale = 0.8f;
    public float maxScale = 1.4f;
    public float minVerticalOffset = -0.3f;
    public float maxVerticalOffset = 0.5f;

    [Header("Stone Materials")]
    public Material[] stoneMaterials;

    [Header("Attack Warning Indicator")]
    public GameObject warningPrefab;
    public float warningDuration = 1f;

    [Header("Jut Settings")]
    public float jutDuration = 0.3f;
    public float jutStartDepth = 2f; // Depth at which stones spawn

    public void Activate()
    {
        StartCoroutine(ShowWarningThenFire());
    }

    private IEnumerator ShowWarningThenFire()
    {
        yield return StartCoroutine(ShowWarning());
        SpawnRing();
    }

    private IEnumerator ShowWarning()
    {
        Vector3 warningPos = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        GameObject warning = Instantiate(warningPrefab, warningPos, Quaternion.identity);
        warning.transform.localScale = 0.25f * radius * Vector3.one;

        Material mat = warning.GetComponent<Renderer>().material;
        float elapsed = 0f;

        while (elapsed < warningDuration)
        {
            elapsed += Time.deltaTime;
            float pulse = Mathf.PingPong(elapsed * 2f, 2.5f);
            mat.SetFloat("_Opacity", pulse);
            yield return null;
        }

        Destroy(warning);
    }

    private void SpawnRing()
    {
        stonesRemaining = stoneCount;

        for (int i = 0; i < stoneCount; i++)
        {
            float angle = i * (360f / stoneCount);
            float rad = angle * Mathf.Deg2Rad;

            float x = Mathf.Cos(rad) * radius;
            float z = Mathf.Sin(rad) * radius;
            float y = Random.Range(minVerticalOffset, maxVerticalOffset);

            Vector3 spawnPos = transform.position + new Vector3(x, y, z);

            Quaternion baseRot = Quaternion.LookRotation(new Vector3((x * 1.25f), 0, (z * 1.25f)).normalized);
            Quaternion randomRot = baseRot * Quaternion.Euler(
                Random.Range(0f, 15f),
                Random.Range(-20f, 20f),
                Random.Range(0f, 15f)
            );

            GameObject stone = Instantiate(stonePrefab, spawnPos, randomRot, transform);

            float scale = Random.Range(minScale, maxScale);
            stone.transform.localScale = Vector3.one * scale;

            if (stoneMaterials.Length > 0)
            {
                Renderer rend = stone.GetComponent<Renderer>();
                if (rend != null)
                    rend.material = stoneMaterials[Random.Range(0, (stoneMaterials.Length - 1))];
            }

            // Coroutine called for each stone, plays jutting animation and handles removal as well
            StartCoroutine(JutUp(stone, spawnPos));
        }
    }

    private IEnumerator JutUp(GameObject stone, Vector3 targetPos)
    {
        // Get collider to control when the player can be affected by the rock
        Collider col = stone.GetComponent<Collider>();

        // Jut up
        Vector3 startPos = targetPos + Vector3.down * jutStartDepth;
        stone.transform.position = startPos;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / jutDuration;
            stone.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        stone.transform.position = targetPos;

        // Small window after jut where collision is still active
        // Rock then loses collision and remains above ground for a little longer
        yield return new WaitForSeconds(collisionWindowAfterJut);
        if (col != null) col.enabled = false;
        yield return new WaitForSeconds(stayDuration - collisionWindowAfterJut);

        // Descend back into ground
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / jutDuration;
            stone.transform.position = Vector3.Lerp(targetPos, startPos, t);
            yield return null;
        }

        // Destroy the stone and decrement the stone counter
        Destroy(stone);
        stonesRemaining -= 1;

        // Once all stones are gone, destroy the ring spawner itself
        if (stonesRemaining <= 0)
            Destroy(gameObject);
    }
}