using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Server.Databases.Mongo.Core
{
    public interface IEntity
    {
        [BsonId]
        ObjectId InternalId { get; set; }
    }
}
