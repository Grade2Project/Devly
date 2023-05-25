using System.ComponentModel.DataAnnotations.Schema;

namespace Devly.Database.Models;

public class UsersFavoriteVacancy
{
    public string UserLogin { get; set; }

    [ForeignKey(nameof(UserLogin))] public User User { get; set; }

    public int VacancyId { get; set; }

    [ForeignKey(nameof(VacancyId))] public Vacancy Vacancy { get; set; }
}