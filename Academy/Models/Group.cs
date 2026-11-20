using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    [Table("Groups")] // Указываем точное имя таблицы в БД
    public class Group
    {
        [Key]
        [Column("group_id")]
        public int group_id { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        [Column("group_name")]
        [Display(Name = "Группа")]
        public string? group_name { get; set; }

        [Column("direction", TypeName = "TINYINT")]
        [ForeignKey(nameof(Direction))]
        public byte direction { get; set; }

        [Column("weekdays", TypeName = "TINYINT")]
        [Display(Name = "Учебные дни")]
        public byte? learning_days { get; set; }

        [Column("start_time", TypeName = "TIME(0)")]
        [Display(Name = "Время")]
        public TimeOnly? start_time { get; set; }

        [Column("start_date", TypeName = "DATE")]
        [Display(Name = "Дата")]
        public DateOnly? start_date { get; set; }

        // Навигационное свойство: связь «многие к одному» к классу Direction
        [Display(Name = "Направление")]
        public Direction? Direction { get; set; }
    }
}
