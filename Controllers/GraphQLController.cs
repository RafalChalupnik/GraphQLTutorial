using System.Threading.Tasks;
using GraphQL;
using GraphQL.NewtonsoftJson;
using GraphQL.Types;
using GraphQLTutorial.DataAccess;
using GraphQLTutorial.GraphQL;
using Microsoft.AspNetCore.Mvc;

namespace GraphQLTutorial.Controllers
{
    [Route("graphql")]
    [ApiController]
    public class GraphQLController : Controller
    {
        private readonly ApplicationDbContext _db;

        public GraphQLController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            var inputs = query.Variables.ToInputs();

            var schema = new Schema
            {
                Query = new AuthorQuery(_db)
            };

            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.Query = query.Query;
                _.OperationName = query.OperationName;
                _.Inputs = inputs;
            });

            if(result.Errors?.Count > 0)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}