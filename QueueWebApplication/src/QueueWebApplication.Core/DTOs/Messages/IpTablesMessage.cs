using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QueueWebApplication.Core.DTOs.Messages;

public record InitialMessage(Dictionary<int, IPAddress> Accepts)
{
	public readonly string Event = "Initial";
}

public record EventMessage(Action Action, [property: JsonConverter(typeof(IpAddressConverter))]IPAddress InboundAddress, int TargetPort)
{
	public readonly string Event = "event";
}

public enum Action
{
	Allow,
	Revoke,
}

public sealed class IpAddressConverter : JsonConverter<IPAddress>
{
	public override IPAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> IPAddress.Parse(reader.GetString()!);

	public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value.ToString());
}
