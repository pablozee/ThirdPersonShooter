using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        //  hurtAudioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        // hurtAudioSource.Play();


        Debug.Log("Player took " + amount + " damage.");

        if (currentHealth <= 0)
        {
            // Game over
            Debug.Log("Game over. Player died.");
            Die();
        }
    }

    void Die()
    {
    }
}
