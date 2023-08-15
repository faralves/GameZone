using GameZone.Core.Communication;

namespace GameZone.News.WebApp.Models.DTO.Response
{
    public class UsuarioLoginDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public double ExpiresIn { get; set; }
        public UsuarioTokenDTO UsuarioToken { get; set; }
        public ResponseResult ResponseResult { get; set; }
    }
}
