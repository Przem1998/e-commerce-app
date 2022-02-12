using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }
}