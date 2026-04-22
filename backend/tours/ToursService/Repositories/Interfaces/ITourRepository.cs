using ToursService.Models;

namespace ToursService.Repositories.Interfaces
{
    public interface ITourRepository
    {
        Tour GetById(long id);
        List<Tour> GetByAuthorId(long authorId);
        Tour Create(Tour tour);
        Tour Update(Tour tour);
        void Delete(long id);
        List<Tour> GetAll();
    }
}