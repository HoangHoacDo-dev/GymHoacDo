using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class PackageService : IPackageService
{
    private readonly AppDbContext _context;

    public PackageService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PackageDto>> GetPackagesAsync()
    {
        // Kéo toàn bộ danh sách gói tập từ Database ra
        return await _context.Packages
            .Select(p => new PackageDto
            {
                Id = p.Id,
                Name = p.Name,
                DurationInDays = p.DurationInDays,
                Price = p.Price
            })
            .ToListAsync();
    }
 
    public async Task<int> AddPackageAsync(PackageRequest request)
    {
        var package = new Package
        {
            Name = request.Name,
            DurationInDays = request.DurationInDays,
            Price = request.Price
        };

        _context.Packages.Add(package);
        await _context.SaveChangesAsync();
        return package.Id;
    }


    public async Task<bool> UpdatePackageAsync(int id, PackageRequest request)
    {
        var package = await _context.Packages.FindAsync(id);
        if (package == null) return false;

        package.Name = request.Name;
        package.DurationInDays = request.DurationInDays;
        package.Price = request.Price;

        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<bool> DeletePackageAsync(int id)
    {
        var package = await _context.Packages.FindAsync(id);
        if (package == null) return false;

        // BẢO VỆ: Kiểm tra xem có ai mua gói này chưa?
        var isUsed = await _context.Subscriptions.AnyAsync(s => s.PackageId == id);
        if (isUsed)
        {
            throw new InvalidOperationException("Không thể xóa gói tập đã có hội viên đăng ký. Hãy ẩn nó đi thay vì xóa!");
        }

        _context.Packages.Remove(package);
        await _context.SaveChangesAsync();
        return true;
    }
}
