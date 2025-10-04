using Myshop.Models;

namespace Myshop.DAL;

public interface IItemRepository
{
    Task<IEnumerable<Item>> GetAll();
    Task<Item?> GetItemById(int id);
    Task Create(Item item);
    Task Update(Item item);
    Task<bool> Delete(int id);
}