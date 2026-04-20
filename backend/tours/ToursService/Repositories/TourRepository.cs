using Microsoft.EntityFrameworkCore;
using ToursService.Data;
using ToursService.Models;
using ToursService.Repositories.Interfaces;

namespace ToursService.Repositories
{
    public class TourRepository : ITourRepository
    {
        private readonly ToursDbContext _context;

        public TourRepository(ToursDbContext context)
        {
            _context = context;
        }

        public Tour GetById(long id)
        {
            return _context.Tours
                .Include(t => t.KeyPoints)
                .Include(t => t.Durations)
                .FirstOrDefault(t => t.Id == id);
        }

        public List<Tour> GetByAuthorId(long authorId)
        {
            return _context.Tours
                .Where(t => t.AuthorId == authorId)
                .Include(t => t.KeyPoints)
                .ToList();
        }

        public Tour Create(Tour tour)
        {
            _context.Tours.Add(tour);
            _context.SaveChanges();
            return tour;
        }

        public Tour Update(Tour tour)
        {
            _context.Tours.Update(tour);
            _context.SaveChanges();
            return tour;
        }

        public void Delete(long id)
        {
            var tour = GetById(id);
            if (tour != null)
            {
                _context.Tours.Remove(tour);
                _context.SaveChanges();
            }
        }
    }
}