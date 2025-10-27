using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Million.DTO.Entities
{
    public class Property
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("codeInternal")]
        public string CodeInternal { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonElement("idOwner")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OwnerId { get; set; }

        [BsonIgnore]
        public Owner Owner { get; set; }

        [BsonIgnore]
        public List<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();

        [BsonIgnore]
        public List<PropertyTrace> PropertyTraces { get; set; } = new List<PropertyTrace>();
    }
}
