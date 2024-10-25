using System.Net;
using WalksAPI.Models.Domain;

namespace WalksAPI.Interfaces.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
