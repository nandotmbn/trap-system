
using System.Text.Json.Serialization;
using Domain.Models;
using Domain.Types;

namespace Infrastructure.Interfaces;

[JsonConverter(typeof(JsonStringEnumConverter<SubstationAttributes>))]
public enum SubstationAttributes
{
  Name,
  Address,
  CreatedAt,
  UpdatedAt
};

public interface ISubstation
{
  IQueryable<Substation> Query(string? search, string? name, string? address);
  Task<SubstationResponse> GetSubstation(Guid substationId);
  Task<SubstationsResponse> GetSubstations(string? search, string? name, string? address, SortType sortType = SortType.Asc, SubstationAttributes sortAttribute = SubstationAttributes.Name, int page = 1, int limit = 10);
  Task<CountResponse> CountSubstations(string? search, string? name, string? address);

  Task<Substation> Create(SubstationRequest request);
  Task<Substation> Update(Guid substationId, SubstationRequest request);
  Task<Substation> Delete(Guid substationId);
  Task<SubstationResponse> CreateSubstation(SubstationRequest request);
  Task<SubstationResponse> UpdateSubstation(Guid substationId, SubstationRequest request);
  Task<SubstationResponse> DeleteSubstation(Guid substationId);
}
