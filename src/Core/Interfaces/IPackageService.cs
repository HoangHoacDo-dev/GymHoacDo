using Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces;

public interface IPackageService
{
    Task<IEnumerable<PackageDto>> GetPackagesAsync();
    Task<int> AddPackageAsync(PackageRequest request);
    Task<bool> UpdatePackageAsync(int id, PackageRequest request);
    Task<bool> DeletePackageAsync(int id);
}