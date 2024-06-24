using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class UISkinBuy : MonoBehaviour
{
    [SerializeField] private Button buyBtn;
    [SerializeField] private Button equipBtn;
    [SerializeField] private GameObject equipedIcon;
    [SerializeField] private string productID;

    [Space]
    [SerializeField] private string equipID = "equip_skin";
    [SerializeField] private int equipIndex;

    private void Start()
    {
        PlayerPrefs.SetInt("skin_0", 1);
        PlayerPrefs.SetInt("location_0", 1);
        CheckIsBuyed();
         
        buyBtn.onClick.AddListener(() =>
        {
            TryBuyProduct(productID);
            CheckIsBuyed();
        });

        equipBtn.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt(equipID, equipIndex);
            CheckIsBuyed();
        });
    }

    private void CheckIsBuyed()
    {
        bool isBuyed = PlayerPrefs.GetInt(productID, 0) == 1;

        buyBtn.gameObject.SetActive(!isBuyed);
        equipBtn.gameObject.SetActive(isBuyed);
        equipedIcon.SetActive(isBuyed && PlayerPrefs.GetInt(equipID, 0) == equipIndex);
    }

    private void TryBuyProduct(string stringId)
    {
        if (!PurchaseManager.Instance.IsInitialized())
        {
            Debug.Log("IAP is not initialized.");
            PokupkaChoPoChem.Instance.ShowFailed();
            return;
        }

        Product product = PurchaseManager.Instance._storeController.products.WithID(stringId);

        PokupkaChoPoChem.Instance.ShowLoading();

        if (product != null && product.availableToPurchase)
        {
            Debug.Log($"Purchasing product asynchronously: '{product.definition.id}'");
            PurchaseManager.Instance._storeController.InitiatePurchase(product);
        }
        else
        {
            Debug.Log($"Could not initiate purchase for product ID: {stringId}. It might not be available for purchase.");
            PokupkaChoPoChem.Instance.ShowFailed();
        }
    }
}
