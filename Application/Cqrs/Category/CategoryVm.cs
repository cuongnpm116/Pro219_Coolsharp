namespace Application.Cqrs.Category;

public class CategoryVm
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }

    public CategoryVm()
    {
    }

    public CategoryVm(Guid categoryId, string categoryName)
    {
        CategoryId = categoryId;
        CategoryName = categoryName;
    }
}
