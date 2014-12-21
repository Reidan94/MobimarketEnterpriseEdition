using System.Linq;

namespace Mobimarket.BL
{
  public class OrderManager : Manager
  {
    public static void UpdateOrderIncome(int id)
    {
      var order = mobiMarket.Orders.FirstOrDefault(x => x.Id == id);
      var enterprise = order.Branch.Enterprise;
      enterprise.AddIncome(order.TotalCost);
    }

    public static void ConfirmOrder(int id)
    {
      var order = mobiMarket.Orders.FirstOrDefault(ord => ord.Id == id);
      order.Finished = true;
      mobiMarket.SaveChangesAsync();
    }
  }
}