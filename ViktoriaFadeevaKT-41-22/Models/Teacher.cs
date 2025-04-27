namespace ViktoriaFadeevaKT_41_22.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public int? AcademicDegreeId { get; set; }
        public AcademicDegree? AcademicDegree { get; set; }

        public int? PositionId { get; set; }
        public Position? Position { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public Department? ManagedDepartment { get; set; }

        public ICollection<Load> Loads { get; set; } = new List<Load>();
    }
}
