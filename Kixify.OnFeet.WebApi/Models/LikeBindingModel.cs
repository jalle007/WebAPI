using Kixify.OnFeet.Dal.Entity;

namespace Kixify.OnFeet.WebApi.Models
{
    public class LikeBindingModel
    {
        public long ImageId { get; set; }
        public long UserId { get; set; }
        public Platform Platform { get; set; }

        public bool Like { get; set; }
    }
}
