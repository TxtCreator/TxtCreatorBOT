using System.ComponentModel.DataAnnotations;

namespace TxtCreatorBOT.Database.Models;

public class UserModel
{
    [Key]
    public ulong UserId { get; set; }
    public long Warns { get; set; }
}