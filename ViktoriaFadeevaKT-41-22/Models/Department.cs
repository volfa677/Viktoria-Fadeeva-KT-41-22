namespace ViktoriaFadeevaKT_41_22.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? HeadId { get; set; }
        public Teacher? Head { get; set; }
        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    }
}
