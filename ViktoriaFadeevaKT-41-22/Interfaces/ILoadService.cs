﻿using ViktoriaFadeevaKT_41_22.Filters.LoadFilters;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ViktoriaFadeevaKT_41_22.Interfaces
{
    public interface ILoadService
    {
        Task<List<LoadFilter>> GetLoadsAsync(string teacherFirstName = null, string teacherLastName = null, string departmentName = null, string disciplineName = null);
        Task<LoadFilter> AddLoadAsync(int teacherId, int disciplineId, int hours);
        Task<LoadFilter> UpdateLoadAsync(int loadId, int teacherId, int disciplineId, int hours);
    }
}