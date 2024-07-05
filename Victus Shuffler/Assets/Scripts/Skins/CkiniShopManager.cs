using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class CkiniShopManager : MonoBehaviour, IDetailedStoreListener
{
    private static CkiniShopManager _instance;
    public IStoreController _storeController;
    private IExtensionProvider _storeExtensionProvider;

    public static CkiniShopManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject iapManager = new GameObject("PurchaseManager");
                DontDestroyOnLoad(iapManager);
                _instance = iapManager.AddComponent<CkiniShopManager>();
            }
            return _instance;
        }
    }

    private void OnEnable()
    {
        _instance = this;
    }

    private void OnDisable()
    {
        _instance = null;
    }

    private void Start()
    {
        InitializePurchasing();
        DontDestroyOnLoad(gameObject);
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
            return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct("skin_1", ProductType.NonConsumable);
        builder.AddProduct("skin_2", ProductType.NonConsumable);
        builder.AddProduct("location_1", ProductType.NonConsumable);
        builder.AddProduct("location_2", ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public bool IsInitialized()
    {
        return _storeController != null && _storeExtensionProvider != null;
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        _storeController = controller;
        _storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason reason, string msg)
    {
        Debug.Log($"IAP Initialization Failed: {reason.ToString()} - {msg}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        switch (args.purchasedProduct.definition.id)
        {
            case "skin_1":
                InterfaysSPokupkoy.Instance.ShowSuccess();
                PlayerPrefs.SetInt("skin_1", 1);
                break;
            case "skin_2":
                InterfaysSPokupkoy.Instance.ShowSuccess();
                PlayerPrefs.SetInt("skin_2", 1);
                break;
            case "location_1":
                InterfaysSPokupkoy.Instance.ShowSuccess();
                PlayerPrefs.SetInt("location_1", 1);
                break;
            case "location_2":
                InterfaysSPokupkoy.Instance.ShowSuccess();
                PlayerPrefs.SetInt("location_2", 1);
                break;
            default:
                Debug.Log($"Unexpected product ID: {args.purchasedProduct.definition.id}");
                InterfaysSPokupkoy.Instance.ShowFailed();
                break;
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        InterfaysSPokupkoy.Instance.ShowFailed();
        Debug.Log($"Purchase of {product.definition.id} failed due to {failureReason}");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log($"IAP Initialization Failed: {error.ToString()}");
    }

    public void ReactivatePurchases()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer ||
            Application.isEditor)
        {
            Debug.Log("Starting purchase restoration...");

            var apple = _storeExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result, error) =>
            {
                if (result)
                {
                    Debug.Log("Purchases successfully restored.");
                }
                else
                {
                    Debug.Log($"Failed to restore purchases. Error: {error}");
                }
            });
        }
        else
        {
            Debug.Log("Restore purchases is not supported on this platform.");
        }

        MagazinManeger.Instance.UpdateMagazin();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        InterfaysSPokupkoy.Instance.ShowFailed();
        Debug.Log($"Purchase of {product.definition.id} failed due to {failureDescription}");
    }
}
