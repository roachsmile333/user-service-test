using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace UserService.Models.User
{
    public class UserDto : IUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [NotNull]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public BsonDateTime CreatedDate { get; set; }
    }
}
