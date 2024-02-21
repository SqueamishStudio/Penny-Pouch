using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Security.Cryptography.X509Certificates;

public class TransactionContentManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI noteText;
    [SerializeField] private TextMeshProUGUI balanceText;

    private DateTimeOffset storeTime;

    public void SetAmountText(string text, string extra, float money)
    {
        amount.text = extra + " $" + text;

        balanceText.text = "$" + money.ToString("F2");
    }

    // Create coroutine to set the time text
    private IEnumerator UpdateTime()
    {
        yield return new WaitForSeconds(10);
        SetDateText(storeTime);
    }


    public void SetNoteText(string note)
    {
        noteText.text = note;
    }

    public void SetDateText(DateTimeOffset date)
    {
        dateText.text = TimeConversion.TimeAgo(date.DateTime);
        storeTime = date;

        StartCoroutine(UpdateTime());
    }
}

public static class TimeConversion
{
    public static string TimeAgo(this DateTime dateTime)
    {
        Debug.Log(dateTime);

        string result = string.Empty;
        var timeSpan = DateTime.Now.Subtract(dateTime);

        if (timeSpan <= TimeSpan.FromSeconds(60))
        {
            result = string.Format("{0} seconds ago", timeSpan.Seconds);
        }
        else if (timeSpan <= TimeSpan.FromMinutes(60))
        {
            result = timeSpan.Minutes > 1 ?
                String.Format("about {0} minutes ago", timeSpan.Minutes) :
                "about a minute ago";
        }
        else if (timeSpan <= TimeSpan.FromHours(24))
        {
            result = timeSpan.Hours > 1 ?
                String.Format("about {0} hours ago", timeSpan.Hours) :
                "about an hour ago";
        }
        else if (timeSpan <= TimeSpan.FromDays(30))
        {
            result = timeSpan.Days > 1 ?
                String.Format("about {0} days ago", timeSpan.Days) :
                "yesterday";
        }
        else if (timeSpan <= TimeSpan.FromDays(365))
        {
            result = timeSpan.Days > 30 ?
                String.Format("about {0} months ago", timeSpan.Days / 30) :
                "about a month ago";
        }
        else
        {
            result = timeSpan.Days > 365 ?
                String.Format("about {0} years ago", timeSpan.Days / 365) :
                "about a year ago";
        }

        return result;
    }
}