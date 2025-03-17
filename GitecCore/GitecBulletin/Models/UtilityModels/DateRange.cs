using System.ComponentModel.DataAnnotations;

namespace Gitec.GitecBulletin.Models.UtilityModels;

public class DateRange
{
    [Key]
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}