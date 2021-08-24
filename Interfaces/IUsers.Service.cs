
using CommerceApi.Data;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IUsuariosService
    {
        Task<User> DoLogin(string email, string senha);

    }
}
