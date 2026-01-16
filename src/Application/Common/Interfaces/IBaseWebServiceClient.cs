namespace FoodStorageApi.Application.Common.Interfaces;

public interface IBaseWebServiceClient
{
  Task<string> GetAsync(string endpoint, CancellationToken cancellationToken = default);
  Task<string> PostAsync(string endpoint, string content, CancellationToken cancellationToken = default);
  Task<string> PutAsync(string endpoint, string content, CancellationToken cancellationToken = default);
  Task<string> DeleteAsync(string endpoint, CancellationToken cancellationToken = default);
}