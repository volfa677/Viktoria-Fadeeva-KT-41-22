using System.ComponentModel.DataAnnotations;

namespace ViktoriaFadeevaKT_41_22.Models.DTO
{
    public class UpdateDepartmentDto
    {

        [Required]
        public string Name { get; set; }

        public int? HeadId { get; set; }
    }
}