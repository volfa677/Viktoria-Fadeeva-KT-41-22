using Microsoft.EntityFrameworkCore;
using ViktoriaFadeevaKT_41_22.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViktoriaFadeevaKT_41_22.Filters.DisciplineFilters;
using ViktoriaFadeevaKT_41_22.Interfaces;
using ViktoriaFadeevaKT_41_22.Models.DTO;
using ViktoriaFadeevaKT_41_22.Models;

namespace ViktoriaFadeevaKT_41_22.Services.DisciplineServices
{
    public class DisciplineService : IDisciplineService
    {
        private readonly TeacherDbContext _dbcontext;

        public DisciplineService(TeacherDbContext context)
        {
            _dbcontext = context;
        }

        public async Task<List<DisciplineFilter>> GetDisciplinesAsync(string firstName = null, string lastName = null, int? minHours = null, int? maxHours = null)
        {
            var query = _dbcontext.Disciplines
                .Include(d => d.Loads)
                .ThenInclude(l => l.Teacher)
                .AsQueryable();

            if (!string.IsNullOrEmpty(firstName))
            {
                query = query.Where(d => d.Loads.Any(l => l.Teacher.FirstName.Contains(firstName)));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                query = query.Where(d => d.Loads.Any(l => l.Teacher.LastName.Contains(lastName)));
            }

            if (minHours.HasValue)
            {
                query = query.Where(d => d.Loads.Sum(l => l.Hours) >= minHours.Value);
            }

            if (maxHours.HasValue)
            {
                query = query.Where(d => d.Loads.Sum(l => l.Hours) <= maxHours.Value);
            }

            var disciplines = await query.Select(d => new DisciplineFilter
            {
                Id = d.Id,
                Name = d.Name,
                TotalHours = d.Loads.Sum(l => l.Hours),
                Teachers = d.Loads
                    .Select(l => $"{l.Teacher.FirstName} {l.Teacher.LastName}")
                    .Distinct()
                    .ToList()
            }).ToListAsync();

            return disciplines;
        }

        public async Task<Discipline> GetDisciplineByIdAsync(int id)
        {
            return await _dbcontext.Disciplines
                .Include(d => d.Loads)
                .ThenInclude(l => l.Teacher)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Discipline> AddDisciplineAsync(DisciplineDto disciplineDto)
        {
            var discipline = new Discipline
            {
                Name = disciplineDto.Name
            };

            _dbcontext.Disciplines.Add(discipline);
            await _dbcontext.SaveChangesAsync();
            return discipline;
        }

        public async Task<Discipline> UpdateDisciplineAsync(int id, DisciplineDto disciplineDto)
        {
            var existingDiscipline = await _dbcontext.Disciplines.FindAsync(id);
            if (existingDiscipline == null)
            {
                throw new KeyNotFoundException("Discipline not found");
            }

            existingDiscipline.Name = disciplineDto.Name;

            await _dbcontext.SaveChangesAsync();
            return existingDiscipline;
        }

        public async Task<bool> DeleteDisciplineAsync(int id)
        {
            var discipline = await _dbcontext.Disciplines.FindAsync(id);
            if (discipline == null)
            {
                return false;
            }

            _dbcontext.Disciplines.Remove(discipline);
            await _dbcontext.SaveChangesAsync();
            return true;
        }
    }
}