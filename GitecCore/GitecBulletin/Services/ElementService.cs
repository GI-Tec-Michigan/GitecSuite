using Gitec.ExceptionHandling;
using Gitec.GitecBulletin.Data;
using Gitec.GitecBulletin.Enums;
using Gitec.GitecBulletin.Models;

namespace Gitec.GitecBulletin.Services;

public class ElementService
{
    private readonly GitecBulletinDbContext _dbContext;
    public ElementService(GitecBulletinDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    private void SaveChanges() => _dbContext.SaveChanges();
    
    private static T EnsureEntityExists<T>(IQueryable<T> query, string entityName, object key) where T : class =>
        query.FirstOrDefault() ?? throw new EntityNotFoundException($"{entityName} '{key}' not found.");
    
    public IQueryable<Element> GetElements(bool includeArchived = false) =>
        _dbContext.Elements.Where(e => includeArchived || !e.IsArchived);

    public Element GetElement(string title) =>
        EnsureEntityExists(_dbContext.Elements.Where(e => e.Title == title), "Element", title);

    public Element GetElement(Guid id) =>
        EnsureEntityExists(_dbContext.Elements.Where(e => e.Uid == id), "Element", id);


    public Element CreateElement(Element element)
    {
        _dbContext.Elements.Add(element);
        element.Type = ElementType.Markdown; // Default type if not set
        SaveChanges();
        return element;
    }
    
    public Element CreateElement(string title)
    {
        return CreateElement(new Element(title));
    }

    public Element UpdateElement(Element element)
    {
        _dbContext.Elements.Update(element);
        SaveChanges();
        return element;
    }

    public void ToggleElementArchiveStatus(Element element, bool isArchived)
    {
        element.IsArchived = isArchived;
        _dbContext.Elements.Update(element);
        SaveChanges();
    }
}