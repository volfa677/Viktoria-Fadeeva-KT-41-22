﻿namespace ViktoriaFadeevaKT_41_22.Models.DTO
{
    public class LoadDto
    {
        public int DisciplineId { get; set; }
        public int TeacherId { get; set; }
        public int Hours { get; set; }
    }
    public class TeacherNameDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}