using Microsoft.AspNetCore.Mvc;

namespace GameZone.Api.Controllers.Base
{
    public interface IGenericControllerActions<T, Y, R>
    {
        [HttpGet]
        Task<IActionResult> GetAll();
        [HttpGet]
        Task<IActionResult> GetById(Y id);
        [HttpPost]
        Task<IActionResult> Create(T entity);
        [HttpPut]
        Task<IActionResult> Update(int id, R entity);
        [HttpDelete]
        Task<IActionResult> Delete(Y id);
    }
}
