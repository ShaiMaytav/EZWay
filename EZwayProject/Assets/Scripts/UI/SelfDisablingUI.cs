using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelfDisablingUI : MonoBehaviour
{
    [SerializeField] private float disapearTime = 2;
    public UnityEvent onDisabled;

    private void OnEnable()
    {
        StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(disapearTime);
        onDisabled.Invoke();
        gameObject.SetActive(false);
    }
}
