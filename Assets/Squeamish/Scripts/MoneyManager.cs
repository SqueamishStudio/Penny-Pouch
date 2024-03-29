using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Unity.VisualScripting;

public class Transaction
{
    public float balance = 0;
    public float amount = 0;
    public string note = "";
    public DateTimeOffset date;

    public Transaction()
    {
        this.date = DateTimeOffset.Now;
    } 
}


public class MoneyManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI MainMoney;
    [SerializeField] TextMeshProUGUI AmountMoney;
    [SerializeField] TextMeshProUGUI NoteText;

    [SerializeField] GameObject error_object;
    [SerializeField] GameObject maxMoney_obj;

    [SerializeField] GameObject TransactionUIPrefab_NOTE;
    [SerializeField] GameObject TransactionUIPrefab_NOTELESS;

    private GameObject TransUI;
    [SerializeField] GameObject TransactionContentFilter;

    [SerializeField] private List<Transaction> TransactionHistory = new List<Transaction>();

    private float Balance;

    private FileDataHandler fileDataHandler;

    private void Start()
    {
        var path = Application.persistentDataPath;
        fileDataHandler = new FileDataHandler(path);
        
        var x = fileDataHandler.Load("[]");
        Debug.Log("Loaded: " + x);
        TransactionHistory = TransactionsFromJson(x);

        // Load the transaction history
        for (int i = 0; i < TransactionHistory.Count; i++)
        {
            Transaction transaction = TransactionHistory[i];
            Debug.Log(transaction.date);
            if (transaction != null)
            {
                CreateTransactionUI(transaction);
            }
        }

        if (TransactionHistory.Count > 0) // If there are transactions, set the balance to the last transaction's balance
        {
            Balance = TransactionHistory[TransactionHistory.Count - 1].balance;
        } else // If there are no transactions, set the balance to 0
        {
            Balance = 0f;
        }

        SetText();
    }

    public void Save()
    {
        Transaction transaction = new Transaction();
        float Amount = GetAmount();
        if (Amount > 0 & !MaximumRangeMoney(transaction.amount))
        {
            Balance += Amount;
            transaction.balance = Balance;
            transaction.amount = Amount;
            transaction.note = NoteText.text;
            
            SetText();
            ClearUi();

            CreateTransactionUI(transaction);
            SaveTransaction(transaction);
        } else
        {
            ShowError();
        }
    }

    public void Spend()
    {
        Transaction transaction = new Transaction();
        float Amount = GetAmount();
        if (Amount > 0 & !MaximumRangeMoney(-transaction.amount))
        {
            Balance -= Amount;
            transaction.balance = Balance;
            transaction.amount = -Amount;
            transaction.note = NoteText.text;

            SetText();
            ClearUi();

            CreateTransactionUI(transaction);
            SaveTransaction(transaction);
        }
        else
        {
            ShowError();
        }
    }

    private void ShowError()
    {
        error_object.SetActive(true);
    }

    private float GetAmount()
    {
        float Amount = 0;
        var str = AmountMoney.text.Trim();
        str = Regex.Replace(str, @"[^\d.,]", "");

        bool success = float.TryParse(str, out Amount);
            if (!success)
            {
                Debug.Log("Failed to parse AmountMoney.text to a float");
                Amount = 0;
            }
            else
            {
                Amount = float.Parse(str);
            }

        return Amount;
    }

    private void SetText()
    {
        MainMoney.text = "$" + Balance.ToString("F2");
    }

    private void ClearUi()
    {
        AmountMoney.text = "";
    }

    private void CreateTransactionUI(Transaction transaction)
    {
        if (transaction.note.Length > 1)
        {
            TransUI = Instantiate(TransactionUIPrefab_NOTE);

            TransUI.GetComponent<TransactionContentManager>().SetNoteText(transaction.note);
        }
        else
        {
            TransUI = Instantiate(TransactionUIPrefab_NOTELESS);
        }

        


        TransUI.transform.SetParent(TransactionContentFilter.transform);

        TransUI.transform.SetAsFirstSibling();

        TransUI.GetComponent<TransactionContentManager>().SetAmountText
            (Mathf.Abs(transaction.amount).ToString("F2"), 
                       (transaction.amount < 0) ? "-" : "+", transaction.balance);
        
        TransUI.GetComponent<TransactionContentManager>().SetDateText(transaction.date);

        TransUI.transform.localScale = new Vector3(1, 1, 1);
    }

    private void SaveTransaction(Transaction transaction)
    {
        TransactionHistory.Add(transaction);
        fileDataHandler.Save(TransactionsToJson());
    }

    private string TransactionsToJson()
    {
        return JsonConvert.SerializeObject(TransactionHistory);
    }

    private List<Transaction> TransactionsFromJson(string json)
    {
        return JsonConvert.DeserializeObject<List<Transaction>>(json);
    }

    private bool MaximumRangeMoney(float amount)
    {
        if (Balance + amount < -100000)
        {
            maxMoney_obj.SetActive(true);
            return true;
        }
        else if (Balance + amount > 100000)
        {
            maxMoney_obj.SetActive(true);
            return true;
        }

        return false;
    }

    public void DELETEDATA()
    {
        fileDataHandler.Delete();
    }

    public void QUITAPPLICATION()
    {
        Application.Quit();
    }
}