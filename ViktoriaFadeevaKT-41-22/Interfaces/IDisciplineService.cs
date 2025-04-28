using ViktoriaFadeevaKT_41_22.Filters.DisciplineFilters;
using ViktoriaFadeevaKT_41_22.Models;
using ViktoriaFadeevaKT_41_22.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ViktoriaFadeevaKT_41_22.Interfaces
{
    public interface IDisciplineService
    {
        Task<List<DisciplineFilter>> GetDisciplinesAsync(string firstName = null, string lastName = null, int? minHours = null, int? maxHours = null);
        Task<Discipline> GetDisciplineByIdAsync(int id);
        Task<Discipline> AddDisciplineAsync(DisciplineDto disciplineDto);
        Task<Discipline> UpdateDisciplineAsync(int id, DisciplineDto disciplineDto);
        Task<bool> DeleteDisciplineAsync(int id);
    }
}