using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Million.DTO.Entities;

public class PropertyImage
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("idPropertyImage")]
    public string IdPropertyImage { get; set; }

    [BsonElement("idProperty")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdProperty { get; set; }

    [BsonElement("file")]
    public string File { get; set; }

    [BsonElement("enabled")]
    public bool Enabled { get; set; }
}