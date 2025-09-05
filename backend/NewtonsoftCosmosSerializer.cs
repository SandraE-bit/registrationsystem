using System.IO;
using System.Text;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

public class NewtonsoftCosmosSerializer : CosmosSerializer
{
    private static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);
    private readonly JsonSerializer serializer;

    public NewtonsoftCosmosSerializer()
    {
        this.serializer = JsonSerializer.Create(
            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
        );
    }

    public override T FromStream<T>(Stream stream)
    {
        using var sr = new StreamReader(stream);
        using var jsonTextReader = new JsonTextReader(sr);
        return serializer.Deserialize<T>(jsonTextReader)!;
    }

    public override Stream ToStream<T>(T input)
    {
        var stream = new MemoryStream();
        using var writer = new StreamWriter(stream, DefaultEncoding, 1024, leaveOpen: true);
        using var jsonWriter = new JsonTextWriter(writer);
        serializer.Serialize(jsonWriter, input);
        jsonWriter.Flush();
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}
