namespace JobDrop.Application.DTOs.Requests;

public record CalculateCommuteRequest(
    Guid OriginPointId,
    Guid DestinationPointId,
    List<string> Modes);
