namespace CustomerWebApp.Components.Carts;

public class CartState
{
    public event Action OnChange;
    private int _quantity;

    public int Quantity
    {
        get => _quantity;
        set
        {
            _quantity = value;
            NotifyStateChanged();
        }
    }
    public void DecreaseQuantity()
    {
        if (_quantity > 0)
        {
            _quantity--;
            NotifyStateChanged();
        }
    }
    public void DecreaseQuantity(int count)
    {
        if (_quantity > 0)
        {
            _quantity = Math.Max(0, _quantity - count);
            NotifyStateChanged();
        }
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
public class SelectedProductState
{
    public List<Guid> SelectedProductDetailIds { get; private set; } = new List<Guid>();

    public void AddSelectedProductDetail(Guid productDetailId)
    {
        SelectedProductDetailIds.Add(productDetailId);
    }

    public void Clear()
    {
        SelectedProductDetailIds.Clear();
    }
}
