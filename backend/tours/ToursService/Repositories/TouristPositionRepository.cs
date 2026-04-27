using ToursService.Data;
using ToursService.Models;
using ToursService.Repositories.Interfaces;

namespace ToursService.Repositories
{
    public class TouristPositionRepository : ITouristPositionRepository
    {
        private readonly ToursDbContext _context;

        public TouristPositionRepository(ToursDbContext context)
        {
            _context = context;
        }

        public TouristPosition GetByTouristId(long touristId)
        {
            return _context.TouristPositions.FirstOrDefault(p => p.TouristId == touristId);
        }

        public TouristPosition Create(TouristPosition position)
        {
            _context.TouristPositions.Add(position);
            _context.SaveChanges();
            return position;
        }

        public TouristPosition Update(TouristPosition position)
        {
            _context.TouristPositions.Update(position);
            _context.SaveChanges();
            return position;
        }
    }
}
