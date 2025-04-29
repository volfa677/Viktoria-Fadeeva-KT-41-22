using Microsoft.EntityFrameworkCore;
using ViktoriaFadeevaKT_41_22.Database;
using ViktoriaFadeevaKT_41_22.Filters.DepartmentFilters;
using ViktoriaFadeevaKT_41_22.Models;
using ViktoriaFadeevaKT_41_22.Interfaces;
using ViktoriaFadeevaKT_41_22.Models.DTO;
using System.Globalization;
using Microsoft.IdentityModel.Tokens;


namespace ViktoriaFadeevaKT_41_22.Services.DepartmentServices
{
    public class DepartmentService : IDepartmentService
    {
        private readonly TeacherDbContext _dbcontext;

        public DepartmentService(TeacherDbContext context)
        {
            _dbcontext = context;
        }

        public async Task<List<DepartmentFilter>> GetDepartmentsAsync(DateTime? foundedAfter = null, int? minTeacherCount = null)
        {
            var query = _dbcontext.Departments
                .Include(d => d.Head)
                .Include(d => d.Teachers)
                .AsQueryable();

            if (minTeacherCount.HasValue)
            {
                query = query.Where(d => d.Teachers.Count >= minTeacherCount.Value);
            }

            var departments = await query.Select(d => new DepartmentFilter
            {
                Id = d.Id,
                Name = d.Name,
                Head = d.Head != null ? $"{d.Head.FirstName} {d.Head.LastName}" : "Нет заведующего",
                TeacherCount = d.Teachers.Count
            }).ToListAsync();

            return departments;
        }

        public async Task AddDepartmentAsync(Department department)
        {
            if (!department.IsValidName())
            {
                throw new ArgumentException("Department name must contain 'Department' or 'Кафедра'.");
            }

            _dbcontext.Departments.Add(department);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<Department> UpdateDepartmentAsync(Department department)
        {
            if (!department.IsValidName())
            {
                throw new ArgumentException("Department name must contain 'Department' or 'Кафедра'.");
            }

            var existingDepartment = await _dbcontext.Departments.FindAsync(department.Id);
            if (existingDepartment == null)
            {
                return null;
            }

            existingDepartment.Name = department.Name;
            existingDepartment.HeadId = department.HeadId;

            await _dbcontext.SaveChangesAsync();
            return existingDepartment;
        }

        public async Task<bool> DeleteDepartmentAsync(int departmentId)
        {
            var department = await _dbcontext.Departments
                .Include(d => d.Teachers)
                .FirstOrDefaultAsync(d => d.Id == departmentId);

            if (department == null)
            {
                return false;
            }


            department.HeadId = null;
            await _dbcontext.SaveChangesAsync();


            _dbcontext.Teachers.RemoveRange(department.Teachers);
            await _dbcontext.SaveChangesAsync();


            _dbcontext.Departments.Remove(department);
            await _dbcontext.SaveChangesAsync();

            return true;
        }

        public async Task<Dictionary<string, List<string>>> GetDepartmentDegreesWithTeachers(string departmentName)
        {
            if (string.IsNullOrEmpty(departmentName))
            {
                throw new ArgumentException("Название кафедры обязательно для заполнения.", nameof(departmentName));
            }

            var result = await (from department in _dbcontext.Departments
                                join teacher in _dbcontext.Teachers on department.Id equals teacher.DepartmentId
                                join degree in _dbcontext.Degrees on teacher.DegreeId equals degree.Id
                                where department.Name == departmentName
                                group new { teacher.LastName, teacher.FirstName } by degree.Name into g
                                select new
                                {
                                    DegreeName = g.Key,
                                    Teachers = g.Select(t => $"{t.LastName} {t.FirstName} ").ToList()
                                })
                               .ToDictionaryAsync(x => x.DegreeName, x => x.Teachers);

            return result;
        }



             public async Task<List<string>> Zashita1(
           string disciplineName,
         int? minHours = null,
          int? maxHours = null)
          {
            if (string.IsNullOrEmpty(disciplineName))
          {
            throw new ArgumentException("Имя дисциплины обязательно для заполнения.", nameof(disciplineName));
         }

          return await (from department in _dbcontext.Departments
                      join teacher in _dbcontext.Teachers on department.Id equals teacher.DepartmentId
                    join load in _dbcontext.Loads on teacher.Id equals load.TeacherId
                  join discipline in _dbcontext.Disciplines on load.DisciplineId equals discipline.Id
                where discipline.Name.Contains(disciplineName) &&
                    (!minHours.HasValue || load.Hours >= minHours.Value) &&
                  (!maxHours.HasValue || load.Hours <= maxHours.Value)
              select department.Name)
                .Distinct()
              .ToListAsync();
         }






    }

}