namespace GameZone.News.WebApp.Models.DTO.Response
{
    public class UsuarioTokenDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<UsuarioClaimDTO> Claims { get; set; }
    }
}
