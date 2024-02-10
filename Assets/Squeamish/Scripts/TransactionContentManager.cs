using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TransactionContentManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI noteText;

    public float storedMoney;

    public void SetAmountText(string text, string extra, float money)
    {
        amount.text = extra + " $" + text;

        DateTime date = DateTime.Now;
        dateText.text = date.ToString();

        storedMoney = money;
    }

    public void SetNoteText(string note)
    {
        noteText.text = note;
    }
}
