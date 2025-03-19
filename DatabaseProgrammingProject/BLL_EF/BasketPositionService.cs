using BBL.DTOModels;
using BBL.ServiceInterfaces;
using DAL;
using Model;
namespace BLL_EF
{
    public class BasketPositionService : IBasketPositionService
    {
        private readonly AppDbContext _context;

        public BasketPositionService(AppDbContext context)
        {
            _context = context;
        }

        public void AddToBasket(BasketPositionDTO item)
        {
            var basketItem = _context.BasketPositions.FirstOrDefault(b => b.ProductID == item.ProductID);

            if (basketItem != null)
            {
                basketItem.Amount += item.Amount;
            }
            else
            {
                _context.BasketPositions.Add(new BasketPosition
                {
                    ProductID = item.ProductID,
                    Amount = item.Amount
                });
            }
            _context.SaveChanges();
        }


        public void UpdateBasketItems(int ProductID, int quantity)
        {
            var basketItem = _context.BasketPositions.FirstOrDefault(b => b.ProductID == ProductID);
            if (basketItem != null)
            {
                basketItem.Amount = quantity;
                _context.SaveChanges();
            }
        }

        public void RemoveFromBasket(int ProductID)
        {
            var basketItem = _context.BasketPositions.FirstOrDefault(b => b.ProductID == ProductID);
            if (basketItem != null)
            {
                _context.BasketPositions.Remove(basketItem);
                _context.SaveChanges();
            }
        }
    }
}
