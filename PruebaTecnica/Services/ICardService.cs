using PruebaTecnica.Helpers;
using PruebaTecnica.Models;

namespace PruebaTecnica.Services
{
    public interface ICardService
    {
        Task<Response<Card>> Create(Card card);
        Task<Response<PaginatedList<Card>>> GetAll(int pageIndex = 1, int pageSize = 10, string customerName = null);
        Task<Response<Card>> GetById(int id);
    }
}
