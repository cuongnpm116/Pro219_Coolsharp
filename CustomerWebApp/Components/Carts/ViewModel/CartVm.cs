namespace CustomerWebApp.Components.Carts.ViewModel
{
    public class CartVm
    {
        public Guid CustomerId { get; set; }
        public List<CartItemVm> ListCart { get; set; } = new();
        public decimal TotalPayment
        {
            get
            {
                return ListCart.Where(item => item.IsChecked).Sum(item => item.AmountOfMoney);
            }

        }
    }
}
