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
        _dbContext.Database.EnsureCreated();
    }
    private void SaveChanges() => _dbContext.SaveChanges();
    
    public IQueryable<Element> GetElements(bool includeArchived = false) =>
        _dbContext.Elements.Where(e => includeArchived || !e.IsArchived);

    public Element GetElement(string title) =>
        _dbContext.Elements.FirstOrDefault(e => e.Title == title) ?? throw new EntityNotFoundException("Element");

    public Element GetElement(Guid id) =>
        _dbContext.Elements.FirstOrDefault(e => e.Uid == id) ?? throw new EntityNotFoundException("Element");


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