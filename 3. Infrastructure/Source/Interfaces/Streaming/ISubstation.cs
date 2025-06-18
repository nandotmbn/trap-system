
using Domain.Models;
using Domain.Types;

namespace Infrastructure.Interfaces
{
  public interface ISubstation
  {
    Task<Substation> Create(SubstationRequest request);
    Task<Substation> Update(Guid substationId, SubstationRequest request);
    Task<Substation> Delete(Guid substationId);
    Task<SubstationResponse> CreateSubstation(SubstationRequest request);
    Task<SubstationResponse> UpdateSubstation(Guid substationId, SubstationRequest request);
    Task<SubstationResponse> DeleteSubstation(Guid substationId);
  }
}