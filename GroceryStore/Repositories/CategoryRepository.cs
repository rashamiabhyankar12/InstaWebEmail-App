using Microsoft.EntityFrameworkCore;

namespace GroceryStore.Repositories;

public interface ICategoryRepository
{
    Task AddSubCategory(Subcategory category);

    Task AddCategory(Category category);
    Task UpdateCategory(Category category);
    Task UpdateSubCategory(Subcategory category);
    Task<Category?> GetCategoryById(int id);

    Task<Subcategory?> GetSubCategoryById(int id);
    Task DeleteCategory(Category category);
    Task DeleteSubCategory(Subcategory category);
    Task<IEnumerable<Category>> GetCategories();
    Task<IEnumerable<Subcategory>> GetSubCategories();
}
public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddCategory(Category subcategory)
    {
        _context.Categories.Add(subcategory);
        await _context.SaveChangesAsync();
    }
    public async Task AddSubCategory(Subcategory subcategory)
    {
        _context.Subcategories.Add(subcategory);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateCategory(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateSubCategory(Subcategory category)
    {
        _context.Subcategories.Update(category);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteCategory(Category category)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteSubCategory(Subcategory category)
    {
        _context.Subcategories.Remove(category);
        await _context.SaveChangesAsync();
    }
    public async Task<Category?> GetCategoryById(int id)
    {
        return await _context.Categories.FindAsync(id);
    }
    public async Task<Subcategory?> GetSubCategoryById(int id)
    {
        return await _context.Subcategories.FindAsync(id);
    }
    public async Task<IEnumerable<Category>> GetCategories()
    {
        return await _context.Categories.ToListAsync();
    }
    public async Task<IEnumerable<Subcategory>> GetSubCategories()
    {
        return await _context.Subcategories.ToListAsync();
    }

}
