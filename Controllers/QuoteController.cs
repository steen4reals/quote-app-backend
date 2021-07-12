using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;


[ApiController]
[Route("Quotes")]
public class QuoteController : ControllerBase
{
    private readonly IRepository<Quote> _quoteRepository;

    public QuoteController(IRepository<Quote> quoteRepository)
    {
        _quoteRepository = quoteRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(string search = "", int limit = 100, int page = 1)
    {
        //from controller base, checks if model state is valid
        Console.WriteLine(ModelState.IsValid);
        try
        {
            var quoteResult = await _quoteRepository.Search(search, limit, page);
            return Ok(quoteResult);

        }
        catch (Exception)
        {
            if (limit < 0 || page <= 0)
            {
                return BadRequest($"Sorry, the {(page <= 0 ? "page" : "limit")} entered is not valid.\nTry entering a positive number.");
            }
            return NotFound("Sorry, there are no quotes in the database.\nTry posting some books.");
        }
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        try
        {
            var returnedQuote = await _quoteRepository.Get(id);
            return Ok(returnedQuote);
        }
        catch (Exception)
        {
            return NotFound($"Sorry, quote of id {id} cannot be fetched, since it does not exist.\nAre you sure the id is correct?");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        try
        {
            _quoteRepository.Delete(id);
            return Ok();
        }
        catch (Exception)
        {
            return BadRequest($"Sorry, book of id {id} cannot be deleted, since it does not exit.\nAre you sure the id is correct?");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] Quote quote)
    {
        try
        {
            var updatedQuote = await _quoteRepository.Update(new Quote { Id = id, Wisdom = quote.Wisdom, Author = quote.Author });
            return Ok(updatedQuote);
        }
        catch (Exception)
        {
            return BadRequest($"Sorry, quote of id {id} cannot be updated, since it does not exist.\nAre you sure the id is correct?");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] Quote quote)
    {
        //from controller base, checks if model state is valid
        Console.WriteLine(ModelState.IsValid);
        try
        {
            var insertedQuote = await _quoteRepository.Insert(quote);
            return Created($"/quote/{insertedQuote.Id}", insertedQuote);
        }
        catch (Exception)
        {
            return BadRequest($"Sorry, cannot insert new quote.\nAre you sure the book is valid?");
        }
    }
}
