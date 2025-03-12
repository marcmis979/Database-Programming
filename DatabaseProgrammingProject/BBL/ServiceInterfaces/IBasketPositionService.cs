using BBL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBL.ServiceInterfaces
{
    public interface IBasketPositionService
    {
        void AddToBasket(BasketPositionDTO item);
        void UpdateBasketItems(int productId, int quantity);
        void RemoveFromBasket(int productId);
    }
}
