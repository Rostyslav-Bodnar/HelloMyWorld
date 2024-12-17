using TMPro;
using UnityEngine;

public class StorageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TreeAmountText;
    [SerializeField] private TextMeshProUGUI StoneAmountText;
    [SerializeField] private TextMeshProUGUI FuelAmountText;
    [SerializeField] private TextMeshProUGUI MoneyAmountText;
    [SerializeField] private TextMeshProUGUI FoodAmountText;

    public Storage storage;
    private void Start()
    {
        storage = new Storage();
        TreeAmountText.text = storage.trees.ToString();
        StoneAmountText.text = storage.stones.ToString();
        FuelAmountText.text = storage.Fuel.ToString();
        MoneyAmountText.text = storage.money.ToString();
        FoodAmountText.text = storage.food.ToString();
    }
}
