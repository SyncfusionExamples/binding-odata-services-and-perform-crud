using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ODataServiceProject.Models;

namespace ODataServiceProject.Controllers
{

    [Route("api/[controller]")]
    public class OrdersController : ODataController
    {
        private OrdersDetailsContext _db;
        public OrdersController(OrdersDetailsContext context)
        {
            _db = context;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()

        {
            return Ok(_db.Orders);
        }
        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] Orders book)
        {
            _db.Orders.Add(book);
            _db.SaveChanges();

            return Created(book);
        }
        [EnableQuery]
        public async Task<IActionResult> Patch([FromODataUri] long key, [FromBody] Delta<Orders> book)
        {
            var entity = await _db.Orders.FindAsync(key);
            book.Patch(entity);
            await _db.SaveChangesAsync();
            return Updated(entity);
        }
        [EnableQuery]
        public long Delete([FromODataUri] long key)
        {
            var od = _db.Orders.Find(key);
            _db.Orders.Remove(od);
            _db.SaveChanges();
            return key;
        }
    }
}
