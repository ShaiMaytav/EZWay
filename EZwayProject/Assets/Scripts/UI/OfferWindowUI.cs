using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OfferWindowUI : MonoBehaviour
{
    public TMP_Text OfferText;
    public string CurrentOfferLink;

    public void CallCurrentLink()
    {
        GameManager.Instance.CallLink(CurrentOfferLink);
    }
}
