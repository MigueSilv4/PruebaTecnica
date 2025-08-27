using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Data;
using PruebaTecnica.Helpers;
using PruebaTecnica.Models;

namespace PruebaTecnica.Services
{
    public class CardService : ICardService
    {
        private readonly AppDbContext _db;
        public CardService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Response<Card>> Create(Card card)
        {
            var response = new Response<Card>();
            try
            {
                if (await _db.Cards.AnyAsync(c => c.CardNumber == card.CardNumber))
                {
                    response.Message = "El número de tarjeta ya existe.";
                    response.Success = false;
                    return response;
                }
                _db.Cards.Add(card);
                await _db.SaveChangesAsync();
                response.Data = card;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Ocurrió un error interno en el servidor.";
                response.Success = false;
                return response;
            }
        }


        public async Task<Response<PaginatedList<Card>>> GetAll(int pageIndex = 1, int pageSize = 10, string customerName = null)
        {
            var response = new Response<PaginatedList<Card>>();
            try
            {
                var query = _db.Cards.AsQueryable();

                if (!string.IsNullOrEmpty(customerName))
                {
                    query = query.Where(c => c.CustomerName.Contains(customerName));
                }

                var paginatedList = await query.ToPaginatedListAsync(pageIndex, pageSize);

                response.Data = paginatedList;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Ocurrió un error interno en el servidor.";
                response.Success = false;
                return response;
            }
        }

        public async Task<Response<Card>> GetById(int id)
        {
            var response = new Response<Card>();
            try
            {
                var card = await _db.Cards.FirstOrDefaultAsync(c => c.Id == id);
                if (card is null)
                {
                    response.Message = "No se encontró la tarjeta con el ID especificado.";
                    response.Success = false;
                    return response;
                }
                response.Data = card;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Ocurrió un error interno en el servidor.";
                response.Success = false;
                return response;
            }
        }
    }
}
