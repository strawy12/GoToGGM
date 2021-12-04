using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;
    private ParticleSystem currentParticle;
    private Transform targetTransform;

    private bool isPlay;
    private bool isDestroy;

    private void Update()
    {
        if (!isPlay) return;

        Vector3 targetPos = targetTransform.position;
        targetPos.z = 0f;
       currentParticle.transform.position = targetPos;
    }

    private void ChangeParticle(int particleIndex)
    {
        if(currentParticle != null)
        {
            Destroy(currentParticle.gameObject);
            isDestroy = true;
        }

        currentParticle = Instantiate(particles[particleIndex]);
    }

    private void SetTarget(Transform target)
    {
        targetTransform = target;
    }

    private void PlayParticle(float delay)
    {
        isPlay = true;
        currentParticle.gameObject.SetActive(true);
        currentParticle.Play();
        Invoke("StopParticle", delay);
    }

    private void StopParticle()
    {
        if(isDestroy)
        {
            isDestroy = false;
            return;
        } 
        Destroy(currentParticle.gameObject);
        currentParticle = null;
        isPlay = false;
    }

    public void PlayParticle(int particleIndex, float delay, Transform target)
    {
        GameManager.Inst.Particle.SetTarget(target);
        GameManager.Inst.Particle.ChangeParticle(particleIndex);
        GameManager.Inst.Particle.PlayParticle(delay);
    }
}