using System;
using UnityEditor.Animations;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    private Transform _player;
    private float _moveSpeed;
    private bool _hasStopped = false;
    private RuntimeAnimatorController _idleController;
    private PlayerSpotlight _playerSpotlight;

    public void Initialize(Transform playerTransform, float speed, RuntimeAnimatorController idleController)
    {
        _player = playerTransform;
        _moveSpeed = speed;
        _idleController = idleController;
        
        _playerSpotlight = FindObjectOfType<PlayerSpotlight>();
    }

    void Update()
    {
        if (!_player || _hasStopped) return;

        // Calculate direction to player
        Vector3 directionToPlayer = (_player.position - transform.position).normalized;
        
        // Set Y to 0 to ensure the object moves on the X and Z plane only
        directionToPlayer.y = 0;

        // Move towards player (X and Z axis only)
        transform.position += directionToPlayer * (_moveSpeed * Time.deltaTime);

        // Make the object face the player (only rotate on Y axis)
        Vector3 targetDirection = new Vector3(directionToPlayer.x, 0, directionToPlayer.z);
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _moveSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") && !_hasStopped)
        {
            _hasStopped = true;
            var animator = gameObject.GetComponent<Animator>();
            if (animator)
            {
                animator.runtimeAnimatorController = _idleController;
            }
            
            if (_playerSpotlight != null)
            {
                _playerSpotlight.FreezePlayerWithSpotlight();
            }
            
            Debug.LogWarning("Stopped chasing player and triggered spotlight");
        }
    }
}